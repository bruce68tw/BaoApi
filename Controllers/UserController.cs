using BaoApi.Services;
using Base.Services;
using BaseApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BaoApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class UserController : BaseCtrl
    {
        [HttpPost]
        public async Task<string> Create([BindRequired] JObject row)
        {
            return await new UserService().CreateA(row);
        }

        [HttpPost]
        public async Task<string> Update([BindRequired] JObject row)
        {
            return await new UserService().UpdateA(row);
        }

        [HttpPost]
        public async Task<ContentResult> GetRow([BindRequired] string id)
        {
            var sql = "select * from dbo.UserApp where Id=@Id";
            var row = await _Db.GetRowA(sql, new List<object>() { "Id", _Xp.Decode(id) });
            return JsonToCnt(row); ;
        }

        /// <summary>
        /// 比對認証碼, called by: (1)new user, (2)recover
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ContentResult> Auth([BindRequired] string data)
        {
            return JsonToCnt(await new UserService().AuthA(data));
        }

        [HttpPost]
        public async Task<string> EmailRecover([BindRequired] string email)
        {
            return await new UserService().EmailRecoverA(email);
        }

    }//class
}
