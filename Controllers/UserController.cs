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
    public class UserController : ApiCtrl
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
            var row = await _Db.GetJsonA(sql, new List<object>() { "Id", _Xp.Decode(id) });
            return JsonToCnt(row); ;
        }

        //比對認証碼, called by: (1)new user, (2)recover
        [HttpPost]
        public async Task<string> Auth([BindRequired] string data)
        {
            return await new UserService().AuthAsync(data);
        }

        [HttpPost]
        public async Task<string> EmailRecover([BindRequired] string email)
        {
            return await new UserService().EmailRecoverAsync(email);
        }

    }//class
}
