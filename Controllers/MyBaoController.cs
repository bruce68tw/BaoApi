using BaoApi.Services;
using Base.Models;
using Base.Services;
using BaseWeb.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BaoApi.Controllers
{
    [Authorize]
    [ApiController]
    public class MyBaoController : XpCtrl
    {
        [HttpPost("MyBao/GetPage")]
        public async Task<ContentResult> GetPage(EasyDtDto dt)
        {
            return JsonToCnt(await new MyBaoRead().GetPageAsync(Ctrl, dt));
        }

        /*
        [HttpPost("MyBao/GetDetail")]
        public ContentResult GetDetail(JObject json)
        {
            var sql = @"
select b.*,
    Corp=u.Name,
    u.IsCorp,
    JoinCount=(
		select count(*) 
		from dbo.UserBao
		where BaoId=@Id
	)
from dbo.Bao b
join dbo.UserCust u on b.Creator=u.Id
where b.Id=@Id
";
            var row = _Db.GetJson(sql, 
                new List<object>() { "Id", json["id"].ToString()}
            );
            return JsonToCnt(row);
        }
        */

    }//class
}
