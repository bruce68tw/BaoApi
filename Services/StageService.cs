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
        /// get image zip file, 圖檔名稱(底線分隔): Sort+1,StageId,Hint
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

        private async Task<bool> AddReplyA(string baoId, string stageId, string userId, string reply, Db db)
        {
            var sql = @"
insert dbo.BaoReply(Id, BaoId, StageId, UserId, Reply, Created)
values(@Id, @BaoId, @StageId, @UserId, @Reply, @Created)
";
            var args = new List<object>() {
                "Id", _Str.NewId(),
                "BaoId", baoId,
                "StageId", stageId,
                "UserId", userId,
                "Reply", reply,
                "Created", _Date.NowDbStr()
            };

            return (await db.ExecSqlA(sql, args) == 1);
        }

        private async Task<bool> SetAttendFinishA(string baoId, string userId, Db db)
        {
            var sql = $@"
update dbo.BaoAttend set
    AttendStatus='{AttendStatusEstr.Finish}'
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
        public async Task<string> ReplyStepA(string baoId, string stageId, string reply)
        {
            var result = "0";   //initial return value
            var userId = _Fun.UserId();
            var db = new Db();
            //await db.BeginTranA();

            //get Bao.StageCount, BaoStage.Id, Answer
            //如果是Step, 必須判斷是否為目前進行的關卡
            var sql = @$"
select b.AnswerType, b.MaxError, b.StageCount, s.Answer, s.Sort
from dbo.BaoStage s
join dbo.BaoAttend a on s.BaoId=a.BaoId
join dbo.Bao b on a.BaoId=b.Id
where s.BaoId=@BaoId
and s.Id=@StageId
and a.UserId='{userId}'
and (b.AnswerType='{AnswerTypeEstr.AnyStep}' or s.Sort+1=a.NowLevel)
";
            //join dbo.BaoAttend a on s.BaoId = a.BaoId and s.Sort + 1 = a.NowLevel
            var args = new List<object>() { "BaoId", baoId, "StageId", stageId, "UserId", userId };
            var json = await db.GetRowA(sql, args);
            if (json == null)
                goto lab_exit;

            //write user reply first
            var replyMd5 = _Str.Md5(reply.Trim());
            if (!await AddReplyA(baoId, stageId, userId, replyMd5, db))
                goto lab_exit;

            //case of 答題錯誤
            //compare reply & answer
            if (json["Answer"]!.ToString() != replyMd5)
            {
                var maxError = Convert.ToInt16(json["MaxError"]!);
                if (maxError > 0)
                {
                    var replyCount = await db.GetIntA("select count(*) from dbo.BaoReply where BaoId=@BaoId and StageId=@StageId", args);
                    result = (replyCount >= maxError) ? "-1" : "0";
                }
                goto lab_exit;
            }

            //case of 答題正確
            //Step時, 如果為最後一關, 則設定為答題成功(NowLevel大於關卡數目), 否則update BaoAttend.NowLevel(base 1)
            if (json["AnswerType"]!.ToString() == AnswerTypeEstr.Step)
            {
                var sort = Convert.ToInt32(json["Sort"]);   //base 0
                var stageCount = Convert.ToInt32(json["StageCount"]);
                var status = (sort + 1 >= stageCount)
                    ? await SetAttendFinishA(baoId, userId, db)
                    : (await db.ExecSqlA($"update dbo.BaoAttend set NowLevel={sort + 2} where BaoId='{baoId}' and UserId='{userId}'") == 1);
                result = status ? "1" : "0";
            }
            else
            {

            }

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

            /* //todo
            if (!await AddReplyA(baoId, userId, reply, db))
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
            result = await SetAttendFinishA(baoId, userId, db)
                ? "1" : "0";

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
