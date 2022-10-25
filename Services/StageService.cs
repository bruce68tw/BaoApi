using BaoApi.Models;
using Base.Services;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace BaoApi.Services
{
    public class StageService
    {
        public async Task<byte[]> GetBatchImageAsync(string baoId)
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
            return await GetZipImageAsync(sql, baoId);
        }

        public async Task<byte[]> GetStepImageAsync(string baoId)
        {
            //get one baoStage
            var sql = @"
select s.Id, s.FileName, s.AppHint
from dbo.BaoStage s
join dbo.BaoAttend t on t.BaoId=@BaoId and t.UserId=@UserId
join dbo.Bao b on b.Id=@BaoId and b.IsBatch=0
where s.BaoId=@BaoId
and s.Sort+1=t.NowLevel
";
            return await GetZipImageAsync(sql, baoId);
        }

        /// <summary>
        /// get image zip file
        /// </summary>
        /// <param name="sql">sql for read BaoStage</param>
        /// <param name="rows">BaoStage rows</param>
        /// <returns></returns>
        private async Task<byte[]> GetZipImageAsync(string sql, string baoId)
        {
            //1.read BaoStage table
            var args = new List<object>() {
                "BaoId", baoId,
                "UserId", _Fun.UserId(),
            };
            var rows = await _Db.GetModelsA<StageImageDto>(sql, args);
            if (rows == null)
                return null;

            //2.create zip file in stream (simple syntax)
            using var ms = new MemoryStream();
            using (var zip = new ZipArchive(ms, ZipArchiveMode.Create, true))
            {
                for (var i = 0; i < rows.Count; i++)
                {
                    //image name in zip is like 0_xxx.jpg for sorting
                    var row = rows[i];
                    var rowId = row.Id;
                    var ext = "." + _File.GetFileExt(row.FileName);
                    var path = $"{_Xp.DirStageImage()}FileName_{rowId}{ext}";
                    var preZero = "";
                    //3.如果檔案不存在則檔名前面加00
                    if (!File.Exists(path))
                    {
                        path = _Xp.NoImagePath;
                        preZero = "00";
                    }
                    var hint = row.AppHint.Trim();
                    if (hint != "")
                        rowId += "_" + hint;
                    //4.寫入 zip, ex: 1_xxx_.png
                    zip.CreateEntryFromFile(path, $"{i + 1}_{preZero}{rowId}_{ext}");
                }
            }

            //response stream, fileDownloadName not work
            //ms.Position = 0;
            return ms.ToArray();
        }

        /// <summary>
        /// reply batch stage
        /// </summary>
        /// <param name="baoId"></param>
        /// <param name="reply"></param>
        /// <returns>0(fail), 1(ok)</returns>
        public async Task<string> ReplyBatchAsync(string baoId, string reply)
        {
            //write user reply first
            var result = "0";   //initial return value
            var userId = _Fun.UserId();
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

            //read stage.Answer for compare
            sql = @"
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
            args = new List<object>() {
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
            //set BaoAttend.AttendStatus
            sql = @"
update dbo.BaoAttend set
    AttendStatus='9'
where BaoId=@BaoId
and UserId=@UserId
";
            await db.ExecSqlA(sql, args);
            result = "1";

        lab_exit:
            await db.DisposeAsync();
            return result;
        }

    }//class
}
