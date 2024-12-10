using Base.Services;
using BaseApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;

namespace BaoApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class TestEmailController : BaseCtrl
    {
        /// <summary>
        /// test smtp
        /// </summary>
        /// <param name="userId"></param>
        [HttpPost]
        public async Task Send([BindRequired] string userId)
        {
            if (userId == "aa")
                await _Email.SendRootA("test smtp");
        }

    }//class
}
