using Base.Models;
using Base.Interfaces;
using BaseApi.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using DocumentFormat.OpenXml.Spreadsheet;

namespace BaoApi.Services
{
    public class MyBaseUserService : IBaseUserSvc
    {
        //get base user info
        public BaseUserDto GetData()
        {
            return new BaseUserDto()
            {
                UserId = _Http.JwtToUserId(),
            };
            //return _Http.JwtToBr();
            /*
            var authStr = _Http.GetRequest().Headers["Authorization"]
                .ToString().Replace("Bearer ", "");
            var token = new JwtSecurityTokenHandler().ReadJwtToken(authStr);
            var userId = token.Claims.First(c => c.Type == ClaimTypes.Name).Value;
            return new BaseUserDto()
            {
                UserId = userId,
            };
            */
        }
    }
}
