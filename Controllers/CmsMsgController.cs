using BaoApi.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BaoApi.Controllers
{
    [Authorize]
    [ApiController]
    public class CmsMsgController : XpCmsController
    {
        public CmsMsgController()
        {
            //ProgName = "最新消息維護";
            CmsType = CmsTypeEstr.Msg;
            //DirUpload = _Xp.DirCmsType(CmsType);
        }

    }//class
}