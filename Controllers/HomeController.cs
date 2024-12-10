using Base.Models;
using Base.Services;
using BaseApi.Controllers;
using BaseApi.Services;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace BaoApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class HomeController : BaseCtrl
    {
        public string Index()
        {
            return "hello";
        }

        /// <summary>
        /// login and get token
        /// </summary>
        /// <param name="info">info: AES encoded userId</param>
        /// <returns>JObject, {token,attends}</returns>
        [HttpPost]
        public async Task<ContentResult> Login([BindRequired] string userId)
        {
            var db = new Db();
            var status = await _Login.LoginByUidA(userId, db);
            JObject? result;
            if (status == 1)
            {
                //成功: get user attend BaoId list
                var attends = await db.GetRowsA($@"
select BaoId, AttendStatus 
from dbo.BaoAttend 
where UserId=@UserId
", ["UserId", userId]);

                result = new JObject()
                {
                    ["token"] = _Login.GetJwtAuthStr(userId),
                    ["attends"] = attends,
                };
            }
            else
            {
                result = _Json.GetError(status.ToString());
            }

            await db.DisposeAsync();
            return JsonToCnt(result);
        }

        //register in Startup.cs
        [HttpGet]
        public ResultDto Error()
        {
            var msg = HttpContext.Features.Get<IExceptionHandlerFeature>()!
                .Error.Message;
            _Log.Error(msg);
            return new ResultDto()
            {
                ErrorMsg = _Fun.IsDev ? msg : _Fun.SystemError,
            };
        }

    }//class
}
