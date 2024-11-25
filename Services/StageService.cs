using BaoApi.Models;
using BaoLib.Enums;
using Base.Services;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace BaoApi.Services
{
    public class StageService
    {
        public async Task<byte[]?> GetBatchImageA(string baoId)
        {
            //get baoStage list
            var sql = @"
select s.*
from dbo.BaoStage s
where s.BaoId=@BaoId
and exists (
    select BaoId 
    from dbo.BaoAttend
    where BaoId=s.BaoId
    and UserId=@UserId
)
order by s.Sort
";
            return await GetZipImageA(sql, baoId);
        }

        /// <summary>
        /// 讀取目前關卡的圖檔, 將欄位資訊寫入下載的檔名, 前端解析
        /// </summary>
        /// <param name="baoId"></param>
        /// <returns></returns>
        public async Task<byte[]?> GetStepImageA(string baoId)
        {
            //get one baoStage
            var sql = $@"
select s.Id, s.Sort, s.FileName, s.AppHint
from dbo.BaoStage s
join dbo.BaoAttend t on t.BaoId=@BaoId and t.UserId=@UserId
join dbo.Bao b on b.Id=@BaoId and b.AnswerType='{AnswerTypeEstr.Step}'
where s.BaoId=@BaoId
and s.Sort+1=t.NowLevel
";
            return await GetZipImageA(sql, baoId);
        }

        /// <summary>
        /// get image zip file, 圖檔名稱: Sort+1,StageId,Hint
        /// </summary>
        /// <param name="sql">sql for read BaoStage</param>
        /// <param name="rows">BaoStage rows</param>
        /// <returns></returns>
        private async Task<byte[]?> GetZipImageA(string sql, string baoId)
        {
            //1.read BaoStage table
            var args = new List<object>() {
                "BaoId", baoId,
                "UserId", _Fun.UserId(),
            };
            var rows = await _Db.GetModelsA<StageImageDto>(sql, args);
            if (rows == null) return null;

            //2.create zip file in stream (simple syntax)
            using var ms = new MemoryStream();
            using (var zip = new ZipArchive(ms, ZipArchiveMode.Create, true))
            {
                for (var i = 0; i < rows.Count; i++)
                {
                    //image name in zip is like 0_xxx.jpg for sorting
                    var row = rows[i];
                    var rowId = row.Id;
                    var sort = row.Sort;
                    var ext = "." + _File.GetFileExt(row.FileName);
                    var path = $"{_Xp.DirStageImage()}FileName_{rowId}{ext}";
                    var preZero = "";
                    //3.如果檔案不存在則檔名前面加00
                    if (!File.Exists(path))
                    {
                        path = _Fun.NoImagePath;
                        preZero = "00";
                    }
                    var hint = row.AppHint.Trim();
                    if (hint != "") rowId += "_" + hint;

                    //4.寫入 zip, ex: 1_xxx_.png
                    zip.CreateEntryFromFile(path, $"{sort + 1}_{preZero}{rowId}_{ext}");
                }
            }

            //response stream, fileDownloadName not work
            //ms.Position = 0;
            return ms.ToArray();
        }

        private async Task<bool> AddReplyA(string baoId, string userId, string reply, Db db)
        {
            var sql = @"
insert dbo.BaoReply(Id, BaoId, UserId, Reply, Created)
values(@Id, @BaoId, @UserId, @Reply, @Created)
";
            var args = new List<object>() {
                "Id", _Str.NewId(),
                "BaoId", baoId,
                "UserId", userId,
                "Reply", reply,
                "Created", _Date.NowDbStr()
            };

            return (await db.ExecSqlA(sql, args) == 1);
        }

        private async Task<bool> SetReplyRightA(string baoId, string userId, Db db)
        {
            var sql = @"
update dbo.BaoAttend set
    AttendStatus='9'
where BaoId=@BaoId
and UserId=@UserId
";
            var args = new List<object>() {
                "BaoId", baoId,
                "UserId", userId,
            };

            return (await db.ExecSqlA(sql, args) == 1);
        }

        /// <summary>
        /// reply a stpe stage
        /// </summary>
        /// <param name="baoId"></param>
        /// <param name="reply"></param>
        /// <returns>0(fail), 1(ok)</returns>
        public async Task<string> ReplyStepA(string baoId, string reply)
        {
            var result = "0";   //initial return value
            var userId = _Fun.UserId();
            var db = new Db();
            //await db.BeginTranA();

            //get Bao.StageCount, BaoStage.Id, Answer
            var sql = @$"
select b.StageCount, StageId=s.Id, s.Answer, s.Sort
from dbo.BaoStage s
join dbo.BaoAttend a on s.BaoId=a.BaoId and s.Sort+1=a.NowLevel
join dbo.Bao b on a.BaoId=b.Id
where s.BaoId=@BaoId
and a.UserId='{userId}'
";
            var json = await db.GetRowA(sql, new() { "BaoId", baoId });
            if (json == null)
                goto lab_exit;

            //write user reply first
            if (!await AddReplyA(json["StageId"]!.ToString(), userId, reply, db))
                goto lab_exit;

            //compare reply & answer
            if (json["Answer"]!.ToString() != _Str.Md5(reply.Trim()))
                goto lab_exit;

            //如果為最後一關, 則設定為答題成功, 否則update BaoAttend.NowLevel(base 1)
            var sort = Convert.ToInt32(json["Sort"]);   //base 0
            var stageCount = Convert.ToInt32(json["StageCount"]);
            var status = (sort + 1 >= stageCount)
                ? await SetReplyRightA(baoId, userId, db)
                : (await db.ExecSqlA($"update dbo.BaoAttend set NowLevel={sort + 2} where BaoId='{baoId}' and UserId='{userId}'") == 1);
            result = status ? "1" : "0";

        lab_exit:
            /*
            if (result == "1")
                await db.CommitA();
            else
                await db.RollbackA();
            */

            await db.DisposeAsync();
            return result;
        }

        /// <summary>
        /// reply batch stage
        /// </summary>
        /// <param name="baoId"></param>
        /// <param name="reply"></param>
        /// <returns>0(fail), 1(ok)</returns>
        public async Task<string> ReplyBatchA(string baoId, string reply)
        {
            //write user reply first
            var result = "0";   //initial return value
            var userId = _Fun.UserId();
            var db = new Db();
            //await db.BeginTranA();

            if (!await AddReplyA(baoId, userId, reply, db))
                goto lab_exit;
            /*
            var sql = @"
insert dbo.BaoReply(Id, BaoId, UserId, Reply, Created)
values(@Id, @BaoId, @UserId, @Reply, @Created)
";
            var args = new List<object>() {
                "Id", _Str.NewId(),
                "BaoId", baoId,
                "UserId", userId,
                "Reply", reply,
                "Created", _Date.NowDbStr()
            };

            var db = new Db();
            if (await db.ExecSqlA(sql, args) != 1)
                goto lab_exit;
            */

            //read stage.Answer for compare
            var sql = @"
select s.Answer
from dbo.BaoStage s
where s.BaoId=@BaoId
and exists (
    select BaoId 
    from dbo.BaoAttend
    where BaoId=s.BaoId
    and UserId=@UserId
)
order by s.Sort
";
            var args = new List<object>() {
                "BaoId", baoId,
                "UserId", userId,
            };
            var list = await db.GetStrsA(sql, args);
            if (list == null)
                goto lab_exit;

            //compare reply & answer
            var count = list.Count;
            var replys = reply.Split("\n");
            if (count != replys.Length)
                goto lab_exit;

            for (var i = 0; i < count; i++)
            {
                if (list[i] != _Str.Md5(replys[i].Trim()))
                    goto lab_exit;
            }

            //case of right answer
            result = await SetReplyRightA(baoId, userId, db)
                ? "1" : "0";
            /*
            //set BaoAttend.AttendStatus
            sql = @"
update dbo.BaoAttend set
    AttendStatus='9'
where BaoId=@BaoId
and UserId=@UserId
";
            await db.ExecSqlA(sql, args);
            result = "1";
            */

        lab_exit:
            /*
            if (result == "1")
                await db.CommitA();
            else
                await db.RollbackA();
            */

            await db.DisposeAsync();
            return result;
        }

    }//class
}
