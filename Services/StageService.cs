using BaoApi.Models;
using BaoLib.Enums;
using Base.Services;
using Newtonsoft.Json.Linq;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace BaoApi.Services
{
    public class StageService
    {
        //BaoStage包含答案欄位, 所以讀資料要小心, 以下為安全欄位清單, table as=s, 字尾不含逗號
        private const string StageSafeFids = "s.Id, s.BaoId, s.Name, s.FileName, s.AppHint, s.Sort";

        //判斷用戶是否參加的 sql, 包含參數@UserId
        private const string UserAttend = @"
exists (
    select BaoId 
    from dbo.BaoAttend
    where BaoId=s.BaoId
    and UserId=@UserId
)";

        //傳入參數 for sql
        private List<object> BaoAndUserArgs(string baoId, string? userId = null)
        {
            userId ??= _Fun.UserId();
            return [
                "BaoId", baoId,
                "UserId", userId,
            ];
        }

        /// <summary>
        /// 傳回某個Bao的全部關卡資料, 包含答題狀態, 會判斷是否參加、ReplyType=Batch/AnyStep
        /// </summary>
        /// <param name="baoId"></param>
        /// <returns></returns>
        public async Task<JArray?> GetRowsForBatchAnyA(string baoId)
        {
            //如果StageReplyStatus有對應資料會一併傳回
            var sql = $@"
select {StageSafeFids}, us.*
from dbo.BaoStage s
join dbo.Bao b on s.BaoId=b.Id and b.ReplyType in ('{ReplyTypeEstr.Batch}','{ReplyTypeEstr.AnyStep}')
left join dbo.StageReplyStatus us on s.Id=us.StageId and us.UserId=@UserId
where s.BaoId=@BaoId
and {UserAttend}
order by s.Sort
";
            return await _Db.GetRowsA(sql, BaoAndUserArgs(baoId));
        }

        /// <summary>
        /// sql for 目前關卡, for ReplyType=Step, 判斷是否參加, Bao.ReplyType=Step
        /// 包含參數 @BaoId, @UserId
        /// 用在讀取資料、下載圖檔
        /// </summary>
        /// <returns>sql string</returns>
        private string NowStepSql()
        {
            return $@"
select {StageSafeFids}
from dbo.BaoStage s
join dbo.BaoAttend t on t.BaoId=@BaoId and t.UserId=@UserId
join dbo.Bao b on b.Id=@BaoId and b.ReplyType='{ReplyTypeEstr.Step}'
where s.BaoId=@BaoId
and s.Sort+1=t.NowLevel
";
        }

        private string StepAnySql()
        {
            return $@"
select {StageSafeFids}, b.StageCount
from dbo.BaoStage s
join dbo.Bao b on s.BaoId=b.Id and b.ReplyType='{ReplyTypeEstr.AnyStep}'
where s.Id=@StageId
and {UserAttend}
";
        }

        /// <summary>
        /// (用於答題)傳回一筆stage資料 for Step/AnyStep, 判斷用戶是否參加
        /// </summary>
        /// <param name="stageId"></param>
        /// <returns></returns>
        public async Task<JObject?> GetRowForStepAnyA(string stageId)
        {
            //如果StageReplyStatus有對應資料會一併傳回
            return await _Db.GetRowA(StepAnySql(), ["StageId", stageId, "UserId", _Fun.UserId()]);
        }

        /// <summary>
        /// 找下一個解答的關卡 for ReplyType=Step, 判斷是否參加
        /// </summary>
        /// <param name="baoId"></param>
        /// <returns></returns>
        public async Task<JObject?> GetNowStepRowA(string baoId)
        {
            return await _Db.GetRowA(NowStepSql(), BaoAndUserArgs(baoId));
        }

        /// <summary>
        /// 讀取目前關卡的圖檔, 將欄位資訊寫入下載的檔名, 前端解析
        /// 檔名為 sort+1,stageId,custHint
        /// </summary>
        /// <param name="baoId"></param>
        /// <returns></returns>
        public async Task<byte[]?> GetBatchImageA(string baoId)
        {
            //get baoStage list
            var sql = $@"
select {StageSafeFids}
from dbo.BaoStage s
where s.BaoId=@BaoId
and {UserAttend}
order by s.Sort
";
            return await GetZipImageA(sql, BaoAndUserArgs(baoId), true);
        }

        /// <summary>
        /// 讀取目前關卡的圖檔, 將欄位資訊寫入下載的檔名, 前端解析
        /// 檔名為 stageId
        /// </summary>
        /// <param name="baoId"></param>
        /// <returns></returns>
        public async Task<byte[]?> GetNowStepImageA(string baoId)
        {
            return await GetZipImageA(NowStepSql(), BaoAndUserArgs(baoId), false);
        }

        /// <summary>
        /// 讀取目前關卡的圖檔, 將欄位資訊寫入下載的檔名, 前端解析
        /// 檔名為 stageId
        /// </summary>
        /// <param name="baoId"></param>
        /// <returns></returns>
        public async Task<byte[]?> GetAnyStepImageA(string stageId)
        {
            return await GetZipImageA(StepAnySql(), ["StageId", stageId, "UserId", _Fun.UserId()], false);
        }

        /// <summary>
        /// get image zip file, 圖檔名稱(底線分隔): Sort+1,StageId,Hint
        /// </summary>
        /// <param name="sql">sql for read BaoStage</param>
        /// <param name="rows">BaoStage rows</param>
        /// <param name="isBatch">if true 會使用合併檔名</param>
        /// <returns></returns>
        private async Task<byte[]?> GetZipImageA(string sql, List<object> args, bool isBatch)
        {
            //1.read BaoStage table
            var rows = await _Db.GetModelsA<StageImageDto>(sql, args);
            if (rows == null) return null;

            //2.create zip file in stream (simple syntax)
            using var ms = new MemoryStream();
            using (var zip = new ZipArchive(ms, ZipArchiveMode.Create, true))
            {
                for (var i = 0; i < rows.Count; i++)
                {
                    //image name in zip is like 0_xxx.jpg for sorting
                    var row = rows[i];
                    var rowId = row.Id;
                    var sort = row.Sort;
                    var ext = "." + _File.GetFileExt(row.FileName);
                    var path = $"{_Xp.DirStageImage()}FileName_{rowId}{ext}";
                    var preZero = "";
                    //3.如果檔案不存在則使用空白圖檔, 檔名前面加00(前端判斷)
                    if (!File.Exists(path))
                    {
                        path = _Fun.NoImagePath;
                        preZero = "00";
                    }

                    var name = $"{preZero}{rowId}.{ext}";
                    if (isBatch)
                    {
                        name = $"{sort + 1}_{name}";
                        var hint = row.AppHint.Trim();
                        if (hint != "") 
                            name = $"{name}_{hint}";
                    }

                    //4.寫入 zip, ex: 1_[00]xxx_.png
                    zip.CreateEntryFromFile(path, name);
                }
            }

            //response stream, fileDownloadName not work
            //ms.Position = 0;
            return ms.ToArray();
        }

        private async Task<bool> AddReplyA(string baoId, string stageId, string userId, string reply, Db db)
        {
            var sql = @"
insert dbo.StageReplyLog(Id, StageId, UserId, Reply, Created)
values(@Id, @StageId, @UserId, @Reply, @Created)
";
            var args = new List<object>() {
                "Id", _Str.NewId(),
                "StageId", stageId,
                "UserId", userId,
                "Reply", reply,
                "Created", _Date.NowDbStr()
            };

            return (await db.ExecSqlA(sql, args) == 1);
        }

        //設定遊戲為完成
        private async Task<bool> SetAttendFinishA(string baoId, string userId, Db db)
        {
            var sql = $@"
update dbo.BaoAttend set
    AttendStatus='{AttendStatusEstr.Finish}'
where BaoId=@BaoId
and UserId='{userId}'
";
            return (await db.ExecSqlA(sql, ["BaoId", baoId]) == 1);
        }

        /// <summary>
        /// reply a stpe stage
        /// </summary>
        /// <param name="baoId"></param>
        /// <param name="reply"></param>
        /// <returns>ReplyStageStatusEstr, 0(fail), 1(ok), L(答錯&鎖定), F(完成尋寶)</returns>
        public async Task<string> ReplyOneA(string baoId, string stageId, string reply)
        {
            //const string Wrong = "0";
            //const string Right = "1";
            //const string Lock = "-1";

            var result = ReplyStageStatusEstr.Wrong;   //initial return value
            var userId = _Fun.UserId();
            var db = new Db();
            //await db.BeginTranA();

            //get Bao.StageCount, BaoStage.Id, Answer
            //如果是Step, 必須判斷是否為目前進行的關卡
            var sql = @$"
select b.ReplyType, b.StageMaxError, b.StageCount, s.Answer, s.Sort
from dbo.BaoStage s
join dbo.BaoAttend a on s.BaoId=a.BaoId
join dbo.Bao b on a.BaoId=b.Id
where s.BaoId=@BaoId
and s.Id=@StageId
and a.UserId='{userId}'
and (b.ReplyType='{ReplyTypeEstr.AnyStep}' or s.Sort+1=a.NowLevel)
";
            //join dbo.BaoAttend a on s.BaoId = a.BaoId and s.Sort + 1 = a.NowLevel
            var args2bs = new List<object>() { "BaoId", baoId, "StageId", stageId };
            var row = await db.GetRowA(sql, args2bs);
            if (row == null)
                goto lab_exit;

            //write user reply first, 答題內容使用AES加密
            var stageCount = Convert.ToInt32(row["StageCount"]);
            var stageSort = Convert.ToInt32(row["Sort"]);
            reply = reply.Trim();
            var reply2 = _Str.AesEncode(reply);
            if (!await AddReplyA(baoId, stageId, userId, reply2, db))
                goto lab_exit;

            //case of 答題錯誤
            //compare reply & answer(Md5加密)
            var argStage = new List<object>() { "StageId", stageId };
            int rowCount;
            if (row!["Answer"]!.ToString() != _Str.Md5(reply))
            {
                var stageMaxError = Convert.ToInt16(row["StageMaxError"]!);
                if (stageMaxError > 0)
                {
                    var replyCount = await db.GetIntA($@"
select ErrorCount
from dbo.StageReplyStatus
where StageId=@StageId 
and UserId='{userId}'", argStage);
                    var overError = (replyCount >= stageMaxError);
                    result = overError ? ReplyStageStatusEstr.Lock : ReplyStageStatusEstr.Wrong;

                    //update/insert StageReplyStatus
                    if (overError)
                    {
                        //lock
                        rowCount = await db.ExecSqlA($@"
update dbo.StageReplyStatus 
set ReplyStatus='{ReplyStageStatusEstr.Lock}', 
    ErrorCount={replyCount}
where UserId='{userId}'
and StageId=@StageId
", argStage);
                        if (rowCount == 0)
                        {
                            await db.ExecSqlA($@"
insert into dbo.StageReplyStatus(UserId, StageId, ReplyStatus, ErrorCount) 
values('{userId}', @StageId, '{ReplyStageStatusEstr.Lock}', 1)
", argStage);
                        }
                    }
                    else
                    {
                        //not lock
                        rowCount = await db.ExecSqlA($@"
update dbo.StageReplyStatus 
set ErrorCount=ErrorCount+1
where UserId='{userId}'
and StageId=@StageId
", argStage);
                        if (rowCount == 0)
                        {
                            await db.ExecSqlA($@"
insert into dbo.StageReplyStatus(UserId, StageId, ReplyStatus, ErrorCount) 
values('{userId}', @StageId, '{ReplyStageStatusEstr.Wrong}', 1)
", argStage);
                        }
                    }
                }

                goto lab_exit;
            } //if 答題錯誤

            //=== case of 答題正確 ===
            //設定關卡解題成功
            result = ReplyStageStatusEstr.Right;
            rowCount = await db.ExecSqlA($@"
update dbo.StageReplyStatus 
set ReplyStatus='{ReplyStageStatusEstr.Right}'
where UserId='{userId}'
and StageId=@StageId
", argStage);

            if (rowCount == 0)
            {
                await db.ExecSqlA($@"
insert into dbo.StageReplyStatus(UserId, StageId, ReplyStatus, ErrorCount) 
values('{userId}', @StageId, '{ReplyStageStatusEstr.Right}', 0)
", argStage);
            }

            //如果全部答對, 則設定為尋寶成功, 同時傳回 ReplyBaoStatusEstr.Finish !! (不是 ReplyStageStatusEstr)
            var argBao = new List<object>() { "BaoId", baoId };
            var okCount = await db.GetIntA($@"
select count(*) 
from dbo.StageReplyStatus us
join dbo.BaoStage s on us.StageId=s.Id 
where s.BaoId=@BaoId
and UserId='{userId}'
and us.ReplyStatus='{ReplyStageStatusEstr.Right}'
", argBao);

            if (okCount == stageCount)
            {
                result = ReplyBaoStatusEstr.Finish; //important !!
                if (!await SetAttendFinishA(baoId, userId, db))
                {
                    await db.ExecSqlA($@"
insert into dbo.BaoAttend(UserId,BaoId,AttendStatus,NowLevel,Created) 
values('{userId}',@BaoId, )
set AttendStatus='{AttendStatusEstr.Finish}'
where BaoId=@BaoId
and UserId='{userId}'
", argBao);

                }
            }
            else if(row["ReplyType"]!.ToString() == ReplyTypeEstr.Step)
            {
                //如果為Step, 則update BaoAttend.NowLevel(base 1)
                await db.ExecSqlA($@"
update dbo.BaoAttend 
set NowLevel={stageSort + 2} 
where BaoId=@BaoId
and UserId='{userId}'
", argBao);
            }

        lab_exit:
            /*
            if (result == "1")
                await db.CommitA();
            else
                await db.RollbackA();
            */

            await db.DisposeAsync();
            return result;
        }

        /// <summary>
        /// reply batch stage
        /// </summary>
        /// <param name="baoId"></param>
        /// <param name="reply"></param>
        /// <returns>0(fail), 1(ok)</returns>
        public async Task<string> ReplyAllA(string baoId, string reply)
        {
            //write user reply first
            var result = "0";   //initial return value
            var userId = _Fun.UserId();
            var db = new Db();
            //await db.BeginTranA();

            /* //todo
            if (!await AddReplyA(baoId, userId, reply, db))
                goto lab_exit;
            */

            //read stage.Answer for compare
            var sql = $@"
select s.Answer
from dbo.BaoStage s
where s.BaoId=@BaoId
and {UserAttend}
order by s.Sort
";
            var args = BaoAndUserArgs(baoId, userId);
            var list = await db.GetStrsA(sql, args);
            if (list == null)
                goto lab_exit;

            //compare reply & answer
            var count = list.Count;
            var replys = reply.Split("\n");
            if (count != replys.Length)
                goto lab_exit;

            for (var i = 0; i < count; i++)
            {
                if (list[i] != _Str.Md5(replys[i].Trim()))
                    goto lab_exit;
            }

            //case of right answer
            result = await SetAttendFinishA(baoId, userId, db)
                ? "1" : "0";

        lab_exit:
            /*
            if (result == "1")
                await db.CommitA();
            else
                await db.RollbackA();
            */

            await db.DisposeAsync();
            return result;
        }

    }//class
}
