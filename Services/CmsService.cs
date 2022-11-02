using Base.Services;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace BaoApi.Services
{
    public class CmsService
    {
        /// <summary>
        /// get cms detail
        /// </summary>
        /// <param name="id">cms Id</param>
        /// <returns>JObject</returns>
        public async Task<JObject> GetDetailA(string id)
        {
            if (!await _Str.CheckKeyA(id))
                return null;

            //get from DB, cannot read BaoAttend here, coz Redis 
            var sql = $@"
select *
from dbo.Cms
where Id=@Id
";
            return await _Db.GetJsonA(sql, new() { "Id", id });
        }

    } //class
}
