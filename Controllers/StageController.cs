using BaoApi.Services;
using Base.Services;
using BaseApi.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace BaoApi.Controllers
{
    [Authorize]
    [ApiController]
    public class StageController : ApiCtrl
    {
        //get batch stage images zip file
        //id: bao.Id
        [HttpPost("Stage/GetBatchImage")]
        public async Task<FileResult> GetBatchImage(JObject json)
        {
            //check
            if (_Object.IsEmpty(json["id"]))
                return null;

            //get stage.id list
            var baoId = json["id"].ToString();
            var sql = @"
select s.Id
from dbo.Stage s
where s.BaoId=@BaoId
and exists (
    select BaoId 
    from dbo.Attend
    where BaoId=s.BaoId
    and UserId=@UserId
)
order by s.Sort
";
            var args = new List<object>() {
                "BaoId", baoId,
                "UserId", _Fun.UserId(),
            };
            var list = await _Db.GetStrsAsync(sql, args);
            if (list == null)
                return null;

            //create zip file in stream (simple syntax)
            using var ms = new MemoryStream();
            using (var zip = new ZipArchive(ms, ZipArchiveMode.Create, true))
            {
                for (var i=0; i<list.Count; i++)
                {
                    //image name in zip is like 0_xxx.jpg for sorting
                    var fileName = list[i] + ".jpg";
                    zip.CreateEntryFromFile(_Xp.Config.DirStageImage + fileName, $"{i}_{fileName}");
                }
            }

            //response stream, fileDownloadName not work
            ms.Position = 0;
            return File(ms.ToArray(), "application/zip");

            //case of error
            //throw new ErrorException("Can't zip files");
        }

        //input baoId, reply
        //return: 0(fail), 1(ok)
        [HttpPost("Stage/ReplyBatch")]
        public async Task<string> ReplyBatch(JObject json)
        {
            //check input
            var result = "0";   //initial
            if (_Object.IsEmpty(json["baoId"]) || _Object.IsEmpty(json["reply"]))
                return result;

            //read db below
            var baoId = json["baoId"].ToString();
            var reply = json["reply"].ToString();
            var replys = reply.Split("\n");
            var sql = @"
select s.Answer
from dbo.Stage s
where s.BaoId=@BaoId
and exists (
    select BaoId 
    from dbo.Attend
    where BaoId=s.BaoId
    and UserId=@UserId
)
order by s.Sort
";
            var args = new List<object>() {
                "BaoId", baoId,
                "UserId", _Fun.UserId(),
            };
            var db = new Db();
            var list = await db.GetStrsAsync(sql, args);
            if (list == null)
                goto lab_exit;

            var count = list.Count;
            if (count != replys.Length)
                goto lab_exit;

            for (var i=0; i<count; i++)
            {
                if (list[i] != _Str.Md5(replys[i].Trim()))
                    goto lab_exit;
            }

            //update user reply
            sql = @"
insert dbo.Reply(Id,BaoId,UserId,Reply,Created)
values(@Id,@BaoId,@UserId,@Reply,@Created)
";
            var key = _Xp.GetAesKey(baoId);
            args = new List<object>() {
                "Id", _Str.NewId(),
                "BaoId", baoId,
                "UserId", _Fun.UserId(),
                "Reply", _Str.AesEncode(reply, key, key),   //encrypt by baoId
                "Created", _Date.NowDbStr()
            };

            //set result
            result = (await db.ExecSqlAsync(sql, args) == 1) 
                ? "1" : "0";

        lab_exit:
            await db.DisposeAsync();
            return result;
        }

    }//class
}
