using Base.Services;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BaoApi.Services
{
    public class HomeService
    {
        /// <summary>
        /// get JWT token & BaoAttend info
        /// </summary>
        /// <param name="encodeId">encoded userId</param>
        /// <returns>JObject</returns>
        public async Task<JObject> LoginAsync(string encodeId)
        {
            var userId = _Xp.Decode(encodeId);
            var token = new JwtSecurityToken(
                claims: new[]
                {
                    new Claim(ClaimTypes.Name, userId),
                },
                signingCredentials: new SigningCredentials(
                    _Xp.GetJwtKey(),
                    SecurityAlgorithms.HmacSha256
                ),
                expires: DateTime.Now.AddMinutes(60)
            );

            //get user attend BaoId list
            var attends = await _Db.GetJsonsAsync($@"
select BaoId, AttendStatus 
from dbo.BaoAttend 
where UserId='{userId}'
");

            return new JObject()
            {
                ["token"] = new JwtSecurityTokenHandler().WriteToken(token),
                ["attends"] = attends,
            };
        }

    } //class
}
