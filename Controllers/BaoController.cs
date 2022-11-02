using BaoApi.Services;
using Base.Models;
using BaseApi.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;

namespace BaoApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]/[action]")]
    public class BaoController : ApiCtrl
    {
        [HttpPost]
        public async Task<ContentResult> GetPage([BindRequired] EasyDtDto dt)
        {
            return JsonToCnt(await new BaoRead().GetPageA(dt));
        }

        [HttpPost]
        public async Task<ContentResult> GetDetail([BindRequired] string id)
        {
            return JsonToCnt(await new BaoService().GetDetailA(id));
        }

        /// <summary>
        /// 參加活動
        /// </summary>
        /// <param name="baoId"></param>
        /// <returns>0(活動未開始),1(成功加入),else(error msg)</returns>
        [HttpPost]
        public async Task<string> Attend(string baoId)
        {
            return await new BaoService().AttendA(baoId);
        }

    }//class
}
