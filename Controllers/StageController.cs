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
    public class StageController : BaseCtrl
    {
        /// <summary>
        /// get batch stage images zip file
        /// </summary>
        /// <param name="id">bao.Id</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<FileResult> GetBatchImage([BindRequired] string id)
        {
            var bytes = await new StageService().GetBatchImageA(id);
            return File(bytes!, "application/zip");
        }

        /// <summary>
        /// get step stage images zip file
        /// </summary>
        /// <param name="id">bao.Id</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<FileResult> GetStepImage([BindRequired] string id)
        {
            var bytes = await new StageService().GetStepImageA(id);
            return File(bytes!, "application/zip");
        }

        /// <summary>
        /// batch reply
        /// </summary>
        /// <param name="id">Bao.Id</param>
        /// <param name="reply"></param>
        /// <returns>0(fail), 1(ok)</returns>
        [HttpPost]
        public async Task<string> ReplyBatch([BindRequired] string id, [BindRequired] string reply)
        {
            return await new StageService().ReplyBatchA(id, reply);
        }

        /// <summary>
        /// step reply
        /// </summary>
        /// <param name="id">BaoStage.Id</param>
        /// <param name="reply"></param>
        /// <returns>0(fail), 1(ok)</returns>
        [HttpPost]
        public async Task<string> ReplyStep([BindRequired] string baoId, [BindRequired] string stageId, [BindRequired] string reply)
        {
            return await new StageService().ReplyStepA(baoId, reply);
        }

    }//class
}
