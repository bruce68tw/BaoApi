using BaoApi.Services;
using Base.Services;
using BaseWeb.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BaoApi.Controllers
{
    //[XgProgAuth]
    [ApiController]
    //[Route("api/[controller]")]
    public class HomeController : XpCtrl
    {
        [HttpPost("Home/WhenLogin")]
        public string WhenLogin()
        {
            return "When Login";
        }

        [HttpPost("Home/Login")]
        public IActionResult Login(JObject json)
        {
            var key = _Xp.GetAesKey();
            var userId = _Str.AesDecode(json["info"].ToString(), key, key);
            var token = new JwtSecurityToken(
                claims: new[]
                {
                    new Claim(ClaimTypes.Name, userId),
                },
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: new SigningCredentials(
                    _Xp.GetJwtKey(),
                    SecurityAlgorithms.HmacSha256
                )
            );

            return Ok(new JwtSecurityTokenHandler().WriteToken(token));
            /*
            return Ok(new
            {
            token = new JwtSecurityTokenHandler().WriteToken(token),
            //success = true,
            //message = "登錄成功"
            });
            */
        }

        [Authorize]
        [HttpPost("Home/AfterLogin")]
        public string AfterLogin()
        {
            return "After Login";
        }

        /*
        //return sessionId
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
                return "";
            //return "Id is wrong.";

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
            //var info2 = session.Get<BaseUserDto>(_Fun.BaseUser);
            //Response.Cookies.Append(SessionDefaults.CookieName, session.Id,
            //    new CookieOptions() { Expires = DateTime.Now.AddMinutes(60) });
            return SessionDefaults.CookieName + "=" + session.Id;
            //return "";
        }

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
