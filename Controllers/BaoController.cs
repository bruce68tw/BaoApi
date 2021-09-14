using BaoApi.Services;
using Base.Models;
using BaseWeb.Controllers;
using BaseWeb.Services;
using Microsoft.AspNetCore.Mvc;

namespace BaoApi.Controllers
{
    //[XgProgAuth]
    //[ApiController]
    //[Route("api/[controller]")]
    public class BaoController : XpCtrl
    {
        [HttpPost("Bao/GetPage")]
        public ContentResult GetPage([FromBody] EasyDtDto dt)
        {
            var ssid = _Http.GetSession().Id;
            return JsonToCnt(new BaoRead().GetPage(Ctrl, dt));
        }

        /*
        private BaoEdit EditService()
        {
            return new BaoEdit(Ctrl);
        }

        [HttpPost]
        public ContentResult GetViewJson(string key)
        {
            return JsonToCnt(EditService().GetViewJson(key));
        }
        */

    }//class
}
