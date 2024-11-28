using BaoApi.Services;
using BaseApi.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json.Linq;
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
        /// <param name="baoId">bao.Id</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<FileResult> GetBatchImage([BindRequired] string baoId)
        {
            var bytes = await new StageService().GetBatchImageA(baoId);
            return File(bytes!, "application/zip");
        }

        /// <summary>
        /// get step stage images zip file
        /// </summary>
        /// <param name="baoId">bao.Id</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<FileResult> GetNowStepImage([BindRequired] string baoId)
        {
            var bytes = await new StageService().GetNowStepImageA(baoId);
            return File(bytes!, "application/zip");
        }

        /// <summary>
        /// get step stage images zip file
        /// </summary>
        /// <param name="baoId">bao.Id</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<FileResult> GetAnyStepImage([BindRequired] string stageId)
        {
            var bytes = await new StageService().GetAnyStepImageA(stageId);
            return File(bytes!, "application/zip");
        }

        /// <summary>
        /// batch reply
        /// </summary>
        /// <param name="baoId">Bao.Id</param>
        /// <param name="reply"></param>
        /// <returns>0(fail), 1(ok)</returns>
        [HttpPost]
        public async Task<string> ReplyAll([BindRequired] string baoId, [BindRequired] string reply)
        {
            return await new StageService().ReplyAllA(baoId, reply);
        }

        /// <summary>
        /// step reply
        /// </summary>
        /// <param name="id">BaoStage.Id</param>
        /// <param name="reply"></param>
        /// <returns>0(fail), 1(ok), -1(答錯&鎖定)</returns>
        [HttpPost]
        public async Task<string> ReplyOne([BindRequired] string baoId, [BindRequired] string stageId, [BindRequired] string reply)
        {
            return await new StageService().ReplyOneA(baoId, stageId, reply);
        }

        //讀取某個尋寶的全部關卡 for Batch/AnyStep, 會判斷用戶是否參加
        [HttpPost]
        public async Task<JArray?> GetRowsForBatchAny([BindRequired] string baoId)
        {
            return await new StageService().GetRowsForBatchAnyA(baoId);
        }

        [HttpPost]
        public async Task<JObject?> GetRowForStepAny([BindRequired] string stageId)
        {
            return await new StageService().GetRowForStepAnyA(stageId);
        }

        //找目前的關卡 for ReplyType=Step
        [HttpPost]
        public async Task<JObject?> GetNowStepRow([BindRequired] string baoId)
        {
            return await new StageService().GetNowStepRowA(baoId);
        }
    }//class
}
