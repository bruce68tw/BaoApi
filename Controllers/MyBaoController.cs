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
    public class MyBaoController : BaseCtrl
    {
        [HttpPost]
        public async Task<ContentResult> GetPage([BindRequired] EasyDtDto dt)
        {
            return JsonToCnt(await new MyBaoRead().GetPageA(dt));
        }

        /*
        [HttpPost]
        public ContentResult GetDetail(JObject json)
        {
            var sql = @"
select b.*,
    Corp=u.Name,
    u.IsCorp,
    JoinCount=(
		select count(*) 
		from dbo.BaoAttend
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
