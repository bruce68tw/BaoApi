using BaoApi.Services;
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
    public class StageController : ApiCtrl
    {
        /// <summary>
        /// get batch stage images zip file
        /// </summary>
        /// <param name="id">bao.Id</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<FileResult> GetBatchImage([BindRequired] string id)
        {
            var bytes = await new StageService().GetBatchImageAsync(id);
            return File(bytes, "application/zip");
        }

        /// <summary>
        /// get step stage images zip file
        /// </summary>
        /// <param name="id">bao.Id</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<FileResult> GetStepImage([BindRequired] string id)
        {
            var bytes = await new StageService().GetStepImageAsync(id);
            return File(bytes, "application/zip");
        }

        /// <summary>
        /// batch reply
        /// </summary>
        /// <param name="id"></param>
        /// <param name="reply"></param>
        /// <returns>0(fail), 1(ok)</returns>
        [HttpPost]
        public async Task<string> ReplyBatch([BindRequired] string id, [BindRequired] string reply)
        {
            return await new StageService().ReplyBatchAsync(id, reply);
        }

    }//class
}
