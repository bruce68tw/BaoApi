using BaoApi.Services;
using Base.Services;
using BaseApi.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BaoApi.Controllers
{
    [ApiController]
    public class HomeController : ApiCtrl
    {
        [HttpPost("Home/WhenLogin")]
        public string WhenLogin()
        {
            //userId same to flutter
            return _Xp.UserIdEncode("650TZLT38A");
        }

        //login and get token
        //info: AES userId
        [HttpPost("Home/Login")]
        public IActionResult Login(JObject json)
        {
            if (_Object.IsEmpty(json["info"]))
                return null;

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
        }

        [Authorize]
        [HttpPost("Home/AfterLogin")]
        public string AfterLogin()
        {
            return "After Login";
        }

    }//class
}
