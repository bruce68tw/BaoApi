using BaoApi.Models;
using Base.Models;
using Base.Services;
using BaseApi.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace BaoApi.Services
{
#pragma warning disable CA2211 // 非常數欄位不應可見
    public static class _Xp
    {
        //AES & JWT key
        private const string AesKey = "YourAesKey";
        public const string JwtKey = "YourJwtKey";

        //cms type msg
        public const string CmsMsg = "M";

        //from config file
        public static XpConfigDto Config = null!;

        //dir path
        //public static string NoImagePath = _Fun.DirRoot + "_image/noImage.jpg";
        public static string DirTemplate = _Fun.Dir("_template");
        //public static string DirStageImage = Config.DirStageImage;
        //public static string DirCms = Config.DirCms;

        //set value
        //private static readonly string _aesKey16 = _Str.PreZero(16, AesKey, true);
        //private static readonly SymmetricSecurityKey _jwtKey16 =
        //    new(Encoding.UTF8.GetBytes(_Str.PreZero(16, JwtKey, true)));

        public static string DirStageImage()
        {
            return Config.DirStageImage;
        }
        public static string DirCms()
        {
            return Config.DirCms;
        }

        public static string DirCmsType(string cmsType)
        {
            return DirCms() + cmsType + _Fun.DirSep;
        }

        /*
        public static string Encode(string data)
        {
            return _Str.AesEncode(data, _aesKey16);
        }

        public static string Decode(string data)
        {
            return _Str.AesDecode(data, _aesKey16);
        }
        */

        /*
        public static SymmetricSecurityKey GetJwtKey()
        {
            return _jwtKey16;
        }
        */

        public static async Task<FileResult?> ViewCmsTypeA(string fid, string key, string ext, string cmsType)
        {
            return await ViewFileA(DirCmsType(cmsType), fid, key, ext);
        }

        private static async Task<FileResult?> ViewFileA(string dir, string fid, string key, string ext)
        {
            var path = $"{dir}{fid}_{key}.{ext}";
            return await _HttpFile.ViewFileA(path, $"{fid}.{ext}");
        }

        //send email for new user auth
        public static async Task EmailNewAuthA(UserAppDto user)
        {
            var email = new EmailDto()
            {
                Subject = "新用戶認証信",
                ToUsers = [user.Email],
                Body = _Str.ReplaceJson(await _File.ToStrA(_Xp.DirTemplate + "EmailNewAuth.html") ?? "", _Model.ToJson(user)),
            };
            await _Email.SendByDtoA(email);
        }

        public static async Task EmailRecoverA(UserAppDto user)
        {
            var email = new EmailDto()
            {
                Subject = "回復帳號認証信",
                ToUsers = [user.Email],
                Body = _Str.ReplaceJson(await _File.ToStrA(_Xp.DirTemplate + "EmailRecover.html") ?? "", _Model.ToJson(user)),
            };
            await _Email.SendByDtoA(email);
        }
        
        /// <summary>
        /// get userId and jwt token
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static JObject GetUidAndToken(string userId)
        {
            return new JObject()
            {
                ["userId"] = userId,
                ["token"] = _Login.GetJwtAuthStr(userId),
            };
        }
    } //class
    #pragma warning restore CA2211 // 非常數欄位不應可見
}
