using BaoApi.Services;
using Base.Services;
using BaseWeb.Controllers;
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
    public class StageController : XpCtrl
    {
        [HttpPost("Stage/GetIds")]
        public async Task<string> GetBatchIds(JObject json)
        {
            var baoId = json["id"].ToString();
            var sql = @"
select s.Id
from dbo.Stage s
where s.BaoId=@BaoId
and exists (
    select BaoId 
    from dbo.BaoUser
    where BaoId=@BaoId
    and UserId=@UserId
    and Status=1
)
order by s.Sort
";
            var list = await _Db.GetStrsAsync(sql, 
                new List<object>() { "BaoId", json["id"].ToString()}
            );
            return _List.ToStr(list);
        }

        //id: bao.Id
        public FileResult GetBatchImage(string id)
        {
            //get stage id list
            var ids = new List<string>(){ "aa" };

            //create zip file in stream
            using (var ms = new MemoryStream())
            {
                using (var archive = new ZipArchive(ms, ZipArchiveMode.Create, true))
                {
                    foreach(var id2 in ids)
                    {
                        var fileName = id2 + ".png";
                        archive.CreateEntryFromFile(_Xp.Config.DirStageImage + fileName, fileName);
                    }
                }

                ms.Position = 0;
                return File(ms.ToArray(), "application/zip");
            }

            //case of error
            //throw new ErrorException("Can't zip files");
        }

    }//class
}
