using Base.Models;
using BaseApi.Controllers;
using BaoApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace BaoApi.Controllers
{
    //CMS base controller, abstract class
    abstract public class XpCmsController : ApiCtrl 
    {
        //public string ProgName;     //program name
        public string CmsType;      //map to CmsTypeEstr
        public string DirUpload;    //upload dir, no right slash

        //read rows with cmsType
        [HttpPost("[controller]/[action]")]
        public async Task<ContentResult> GetPage(DtDto dt)
        {
            return JsonToCnt(await new XpCmsRead(CmsType).GetPageAsync(Ctrl, dt));
        }

        private XpCmsEdit EditService()
        {
            return new XpCmsEdit(Ctrl);
        }

        [HttpPost("[controller]/[action]")]
        public async Task<ContentResult> GetViewJson(JObject json)
        {
            var key = json["id"].ToString();
            return JsonToCnt(await EditService().GetViewJsonAsync(key));
        }

    }//class
}