using BaoApi.Services;
using Base.Services;
using BaseWeb.Controllers;
using BaseWeb.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BaoApi.Controllers
{
    [ApiController]
    public class MySetController : XpCtrl
    {
        //return Id or error msg(0:xxx)
        [HttpPost("MySet/Create")]
        public async Task<string> Create(JObject json)
        {
            var key = _Xp.GetAesKey();
            var row = _Str.ToJson(_Str.AesDecode(json["row"].ToString(), key, key));
            var sql = @"
insert into dbo.UserApp(Id,Name,Phone,Email,Address,Created) values(
@Id,@Name,@Phone,@Email,@Address,@Created
)";
            var newId = _Str.NewId();
            var args = new List<object>()
            {
                "Id", newId,
                "Name", row["Name"].ToString(),
                "Phone", row["Phone"].ToString(),
                "Email", row["Email"].ToString(),
                "Address", row["Address"].ToString(),
                "Created", _Date.NowDbStr(),
            };
            return (await _Db.ExecSqlAsync(sql, args) == 1)
                ? newId 
                : _Str.GetPreError();
        }

        //return error msg if any
        [HttpPost("MySet/Update")]
        public async Task<string> Update(JObject json)
        {
            var key = _Xp.GetAesKey();
            var row = _Str.ToJson(_Str.AesDecode(json["row"].ToString(), key, key));
            var sql = @"
update dbo.UserApp set
    Name=@Name,
    Email=@Email,
    Address=@Address,
    Revised=@Revised
where Id=@Id";
            var args = new List<object>()
            {
                "Name", row["Name"].ToString(),
                "Email", row["Email"].ToString(),
                "Address", row["Address"].ToString(),
                "Revised", _Date.NowDbStr(),
                "Id", _Fun.GetBaseUser().UserId,
            };
            return (await _Db.ExecSqlAsync(sql, args) == 1)
                ? "" 
                : _Fun.SystemError;
        }

        [HttpPost("MySet/GetRow")]
        public async Task<ContentResult> GetRow(JObject json)
        {
            var key = _Xp.GetAesKey();
            var id = _Str.AesDecode(json["id"].ToString(), key, key);
            var sql = "select * from UserApp where Id=@Id";
            var row = await _Db.GetJsonAsync(sql, new List<object>() { "Id", id });
            return JsonToCnt(row); ;
        }

    }//class
}
