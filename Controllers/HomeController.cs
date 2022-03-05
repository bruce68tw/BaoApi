using BaoApi.Services;
using Base.Models;
using Base.Services;
using BaseApi.Controllers;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;

namespace BaoApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class HomeController : ApiCtrl
    {
        /// <summary>
        /// login and get token
        /// </summary>
        /// <param name="info">info: AES userId</param>
        /// <returns>JObject, {token,baoIds}</returns>
        [HttpPost]
        public async Task<ContentResult> Login([BindRequired] string info)
        {
            return JsonToCnt(await new HomeService().LoginAsync(info));
        }

        //register in Startup.cs
        [HttpGet]
        public async Task<ResultDto> Error()
        {
            var msg = HttpContext.Features.Get<IExceptionHandlerFeature>()
                .Error.Message;
            await _Log.ErrorAsync(msg);
            return new ResultDto()
            {
                ErrorMsg = _Fun.IsDev 
                    ? msg : _Fun.SystemError,
            };
        }

    }//class
}
