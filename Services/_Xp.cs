using BaoApi.Models;
using Base.Services;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BaoApi.Services
{
    public static class _Xp
    {
        //same to bao_app(flutter)
        public const string AesKey = "YourAesKey";

        public const string JwtKey = "YourJwtKey";

        //cms type msg
        public const string CmsMsg = "M";

        //from config file
        public static XpConfigDto Config;

        public static string GetAesKey(string key = AesKey)
        {
            return _Str.PreZero(16, AesKey, true);
        }

        public static string UserIdEncode(string userId)
        {
            var key = GetAesKey();
            return _Str.AesEncode(userId, key, key);
        }

        public static SymmetricSecurityKey GetJwtKey()
        {
            var key = _Str.PreZero(16, JwtKey, true);
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        }

    } //class
}
