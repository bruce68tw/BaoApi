using BaoApi.Services;
using Base.Services;
using BaseWeb.Controllers;
using BaseWeb.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace BaoApi.Controllers
{
    public class MySetController : XpCtrl
    {
        //return Id or error msg(0:xxx)
        [HttpPost("MySet/Create")]
        public string Create([FromBody] JObject json)
        {
            var key = _Xp.GetAesKey();
            var row = _Json.StrToJson(_Str.AesDecode(json["row"].ToString(), key, key));
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
            return (_Db.ExecSql(sql, args) == 1)
                ? newId 
                : _Str.GetError();
        }

        //return error msg if any
        [HttpPost("MySet/Update")]
        public string Update([FromBody] JObject json)
        {
            var key = _Xp.GetAesKey();
            var row = _Json.StrToJson(_Str.AesDecode(json["row"].ToString(), key, key));
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
            return (_Db.ExecSql(sql, args) == 1)
                ? "" 
                : _Fun.SystemError;
        }

        [HttpPost("MySet/GetRow")]
        public ContentResult GetRow([FromBody] JObject json)
        {
            var key = _Xp.GetAesKey();
            var id = _Str.AesDecode(json["id"].ToString(), key, key);
            var sql = "select * from UserApp where Id=@Id";
            var row = _Db.GetJson(sql, new List<object>() { "Id", id });
            return JsonToCnt(row); ;
        }

    }//class
}
