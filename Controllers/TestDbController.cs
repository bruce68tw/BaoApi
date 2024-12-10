using Base.Services;
using BaseApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;

namespace BaoApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class TestDbController : BaseCtrl
    {
        /// <summary>
        /// test db connection
        /// </summary>
        /// <param name="userId"></param>
        [HttpPost]
        public async Task<string?> Read([BindRequired] string userId)
        {
            return (userId == "aa")
                ? await _Db.GetStrA("select top 1 Name from dbo.Bao order by Id")
                : "Not Allowed.";
        }

    }//class
}
