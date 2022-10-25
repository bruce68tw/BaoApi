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
            return JsonToCnt(await new BaoService().GetDetailAsync(id));
        }

    }//class
}
