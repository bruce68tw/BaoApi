using BaoApi.Services;
using Base.Models;
using Base.Services;
using BaseWeb.Controllers;
using BaseWeb.Extensions;
using BaseWeb.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace BaoApi.Controllers
{
    //[XgProgAuth]
    //[ApiController]
    //[Route("api/[controller]")]
    public class HomeController : XpCtrl
    {
        [HttpPost("Home/GetStr")]
        public string GetStr()
        {
            /*
            //var http = _Http.GetHttp();
            var session = _Http.GetSession();
            session.Set("f1", "f1");
            string f1 = session.Get<string>("f1");
            var sessId = session.Id;
            */
            return "String 1";
        }

        [HttpPost("Home/SetSession")]
        public string SetSession([FromBody] JObject json)
        {
            var key = _Xp.GetAesKey();
            var id = _Str.AesDecode(json["key"].ToString(), key, key);
            var sql = @"
select Id,Name
from dbo.UserApp
where Id=@Id
";
            var row = _Db.GetJson(sql, new List<object>() { "Id", id });
            if (row == null)
                return "Id is wrong.";

            #region set base user info
            //var userId = row["UserId"].ToString();
            var userInfo = new BaseUserDto()
            {
                UserId = id,
                UserName = row["Name"].ToString(),
                Locale = _Fun.Config.Locale,
                IsLogin = true,
            };
            #endregion

            //4.set session of base user info
            var session = _Http.GetSession();
            session.Set(_Fun.BaseUser, userInfo);   //extension method
            var info2 = session.Get<BaseUserDto>(_Fun.BaseUser);
            var ssid = session.Id;
            return "";
        }

        /*
        [HttpPost("Home/GetPage0")]
        public ContentResult GetPage0()
        {
            return GetPage("1", 10);
        }

        [HttpPost("Home/GetPage")]
        public ContentResult GetPage([FromBody] JObject json)
        {
            //var msg = (f1 == null) ? "F1 is null" : "F1=" + f1;
            //_Log.Error(msg);

            var rows = new JArray();
            rows.Add(new JObject()
            {
                ["checked"] = 1,
                ["type"] = 0,
                ["walk"] = 1,
                ["name"] = "name 1",
                ["corp"] = "corp 1",
                ["start"] = "2021/5/1",
            });
            rows.Add(new JObject()
            {
                ["checked"] = 0,
                ["type"] = 1,
                ["walk"] = 0,
                ["name"] = "name 2",
                ["corp"] = "corp 2",
                ["start"] = "2021/6/1",
            });
            var result = JObject.FromObject(new
            {
                draw = 1,
                data = rows,
                recordsFiltered = 15,
            });
             
            return JsonToCnt(result); 
        }
        */

    }//class
}
