using BaoApi.Services;
using Base.Models;
using Base.Services;
using BaseApi.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BaoApi.Controllers
{
    [Authorize]
    [ApiController]
    public class BaoController : ApiCtrl
    {
        [HttpPost("Bao/GetPage")]
        public async Task<ContentResult> GetPage(EasyDtDto dt)
        {
            return JsonToCnt(await new BaoRead().GetPageAsync(dt));
        }

        [HttpPost("Bao/GetDetail")]
        public async Task<ContentResult> GetDetail(JObject json)
        {
            var sql = @"
select b.*,
    Corp=u.Name,
    u.IsCorp,
    JoinCount=(
		select count(*) 
		from dbo.Attend
		where BaoId=@Id
	)
from dbo.Bao b
join dbo.UserCust u on b.Creator=u.Id
where b.Id=@Id
";
            var row = await _Db.GetJsonAsync(sql, 
                new List<object>() { "Id", json["id"].ToString()}
            );
            return JsonToCnt(row);
        }

    }//class
}
