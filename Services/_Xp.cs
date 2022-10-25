using BaoApi.Models;
using Base.Models;
using Base.Services;
using BaseApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaoApi.Services
{
    #pragma warning disable CA2211 // 非常數欄位不應可見
    public static class _Xp
    {
        //AES & JWT key
        private const string AesKey = "YourAesKey";
        private const string JwtKey = "YourJwtKey";

        //cms type msg
        public const string CmsMsg = "M";

        //from config file
        public static XpConfigDto Config;

        //dir path
        public static string NoImagePath = _Fun.DirRoot + "_image/noImage.jpg";
        public static string DirTemplate = _Fun.Dir("_template");
        //public static string DirStageImage = Config.DirStageImage;
        //public static string DirCms = Config.DirCms;

        //set value
        private static readonly string _aesKey16 = _Str.PreZero(16, AesKey, true);
        private static readonly SymmetricSecurityKey _jwtKey16 =
            new(Encoding.UTF8.GetBytes(_Str.PreZero(16, JwtKey, true)));

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

        public static string Encode(string data)
        {
            return _Str.AesEncode(data, _aesKey16, _aesKey16);
        }

        public static string Decode(string data)
        {
            return _Str.AesDecode(data, _aesKey16, _aesKey16);
        }

        public static SymmetricSecurityKey GetJwtKey()
        {
            return _jwtKey16;
        }

        public static async Task<FileResult> ViewCmsTypeAsync(string fid, string key, string ext, string cmsType)
        {
            return await ViewFileAsync(DirCmsType(cmsType), fid, key, ext);
        }

        private static async Task<FileResult> ViewFileAsync(string dir, string fid, string key, string ext)
        {
            var path = $"{dir}{fid}_{key}.{ext}";
            return await _WebFile.ViewFileA(path, $"{fid}.{ext}");
        }

        //send email for new user auth
        public static async Task EmailNewAuthAsync(JObject user)
        {
            var email = new EmailDto()
            {
                Subject = "新用戶認証信",
                ToUsers = new() { user["Email"].ToString() },
                Body = _Str.ReplaceJson(await _File.ToStrA(_Xp.DirTemplate + "EmailNewAuth.html"), user),
            };
            await _Email.SendByDtoA(email);
        }

        public static async Task EmailRecoverAsync(JObject user)
        {
            var email = new EmailDto()
            {
                Subject = "回復帳號認証信",
                ToUsers = new() { user["Email"].ToString() },
                Body = _Str.ReplaceJson(await _File.ToStrA(_Xp.DirTemplate + "EmailRecover.html"), user),
            };
            await _Email.SendByDtoA(email);
        }
        
    } //class
    #pragma warning restore CA2211 // 非常數欄位不應可見
}
