using Base.Services;
using BaseApi.Controllers;
using BaseApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

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
        public string LoginByUid([BindRequired] string userId)
        {
            return _Login.GetJwtAuthStr(userId);
        }

        //使用 postMan 測試可檢視詳細錯誤訊息
        [Authorize]
        [HttpPost]
        public string CheckAuth()
        {
            return $"Your userId={_Fun.UserId()}";
        }

    }//class
}
