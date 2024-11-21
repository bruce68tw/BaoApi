using BaoApi.Services;
using BaoLib.Enums;
using Base.Models;
using BaseApi.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;

namespace BaoApi.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("[controller]/[action]")]
    public class CmsController : BaseCtrl
    {
        [HttpPost]
        public async Task<ContentResult> GetPage([BindRequired] EasyDtDto dt)
        {
            return JsonToCnt(await new CmsRead().GetPageA(dt));
        }

        [HttpPost]
        public async Task<ContentResult> GetDetail([BindRequired] string id)
        {
            return JsonToCnt(await new CmsService().GetDetailA(id));
        }

        [HttpPost]
        public async Task<FileResult?> ViewFile(string id, string ext)
        {
            return await _Xp.ViewCmsTypeA("FileName", id, ext, CmsTypeEstr.Card);
        }

    }//class
}