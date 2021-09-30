using BaoApi.Models;
using Base.Services;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BaoApi.Services
{
    public static class _Xp
    {
        //same to bao_app(flutter)
        public static string AesKey = "YourAesKey";

        public static string JwtKey = "YourJwtKey";

        //from config file
        public static XpConfigDto Config;

        public static string GetAesKey()
        {
            return _Str.PreZero(16, AesKey, true);
        }

        public static SymmetricSecurityKey GetJwtKey()
        {
            var key = _Str.PreZero(16, JwtKey, true);
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        }

    } //class
}
