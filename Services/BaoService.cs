using BaoLib.Enums;
using Base.Services;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BaoApi.Services
{
    public class BaoService
    {
        /// <summary>
        /// get bao detail
        /// </summary>
        /// <param name="id">bao Id</param>
        /// <returns>JObject</returns>
        public async Task<JObject> GetDetailA(string id)
        {
            if (!await _Str.CheckKeyA(id))
                return null;

            //get redis key: BaoDetail + baoId
            var key = RedisTypeEstr.BaoDetail + id;
            var value = await _Redis.GetStrA(key);
            JObject row;
            if (value != null)
            {
                row = _Str.ToJson(value);
            }
            else
            {

                //get from DB, cannot read BaoAttend here, coz Redis 
                var sql = $@"
select b.*,
    Corp=u.Name
from dbo.Bao b
join dbo.UserCust u on b.Creator=u.Id
where b.Id=@Id
";
                row = await _Db.GetJsonA(sql, new() { "Id", id });

                //write redis
                await _Redis.SetStrA(key, _Json.ToStr(row));
            }
            if (row == null)
                return null;

            //re-set Status field if need
            if (row["Status"].ToString() == "1")
            { 
                //set status=0 if cancel attend or not between start/end
                if (!_Date.IsNowInRange(row["StartTime"].ToString(), row["EndTime"].ToString()))
                    row["Status"] = 0;
            }
            return row;
        }

        /// <summary>
        /// 參加活動
        /// </summary>
        /// <param name="baoId"></param>
        /// <returns>0(活動未開始),1(成功加入),else(error msg)</returns>
        public async Task<string> AttendA(string baoId)
        {
            //get Bao row
            var args = new List<object>() { "Id", baoId };
            var db = new Db();
            var row = await db.GetJsonA("select * from dbo.Bao where Id=@Id and Status=1", args);
            if (row == null)
                return "找不到這一筆尋寶資料。";

            //check start/end time
            var now = DateTime.Now;
            var startTime = _Date.CsToDt(row["StartTime"].ToString());
            var endTime = _Date.CsToDt(row["EndTime"].ToString());
            if (now > endTime)
                return "活動已經過期，無法參加。";

            //check BaoAttend existed
            var userId = _Fun.UserId();
            row = await db.GetJsonA($"select UserId from dbo.BaoAttend where UserId='{userId}' and BaoId=@Id", args);
            if (row != null)
                return "您已經參加這個活動，無法再參加。";

            //insert BaoAttend
            var sql = @$"
insert into dbo.BaoAttend(UserId, BaoId, AttendStatus, NowLevel, Created) values
('{userId}', @Id, '1', 1, getdate())
";
            var rowCount = await db.ExecSqlA(sql, args);
            await db.DisposeAsync();

            if (rowCount != 1)
                return "系統發生錯誤，無法參加。";

            //活動是否開始
            return _Date.IsNowInRange(startTime.Value, endTime.Value)
                ? "1" : "0";
        }

    } //class
}
