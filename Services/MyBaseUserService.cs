using Base.Models;
using Base.Services;
using BaseApi.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace BaoApi.Services
{
    public class MyBaseUserService : IBaseUserService
    {
        //get base user info
        public BaseUserDto GetData()
        {
            var authStr = _Http.GetRequest().Headers["Authorization"]
                .ToString().Replace("Bearer ", "");
            var token = new JwtSecurityTokenHandler().ReadJwtToken(authStr);
            var userId = token.Claims.First(c => c.Type == ClaimTypes.Name).Value;
            return new BaseUserDto()
            {
                UserId = userId,
            };
        }
    }
}
