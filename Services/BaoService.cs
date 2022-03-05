using BaoLib.Enums;
using Base.Services;
using Newtonsoft.Json.Linq;
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
        public async Task<JObject> GetDetailAsync(string id)
        {
            if (!await _Str.CheckKeyAsync(id))
                return null;

            //get redis key: BaoDetail + baoId
            var key = RedisTypeEstr.BaoDetail + id;
            var value = await _Redis.GetStrAsync(key);
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
where b.Id='{id}'
";
                row = await _Db.GetJsonAsync(sql);

                //write redis
                await _Redis.SetStrAsync(key, _Json.ToStr(row));
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

    } //class
}
