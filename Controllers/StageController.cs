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
        public async Task<string> ReplyAll([BindRequired] string id, [BindRequired] string reply)
        {
            return await new StageService().ReplyAllA(id, reply);
        }

        /// <summary>
        /// step reply
        /// </summary>
        /// <param name="id">BaoStage.Id</param>
        /// <param name="reply"></param>
        /// <returns>0(fail), 1(ok)</returns>
        [HttpPost]
        public async Task<string> ReplyOne([BindRequired] string baoId, [BindRequired] string stageId, [BindRequired] string reply)
        {
            return await new StageService().ReplyOneA(baoId, stageId, reply);
        }

        //讀取某個尋寶的全部關卡, 會判斷用戶是否參加
        [HttpPost]
        public async Task<JArray?> GetRows([BindRequired] string baoId)
        {
            return await new StageService().GetRowsA(baoId);
        }

    }//class
}
