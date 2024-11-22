using Base.Services;
using BaseApi.Controllers;
using BaseApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BaoApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class TestJwtController : BaseCtrl
    {
        /// <summary>
        /// login and get token
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>JWT string</returns>
        [HttpPost]
        public string Login([BindRequired] string userId)
        {
            var token = new JwtSecurityToken(
                claims:
                [
                    new Claim(ClaimTypes.Name, userId),
                ],
                expires: DateTime.Now.AddMinutes(_Fun.TimeOut),
                signingCredentials: new SigningCredentials(
                    _Login.GetJwtKey(),
                    SecurityAlgorithms.HmacSha256
                )
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        //testing
        [Authorize]
        [HttpPost]
        public string AfterLogin()
        {
            return $"After Login OK, your userId={_Fun.UserId()}";
        }

    }//class
}
