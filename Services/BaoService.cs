using Base.Services;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BaoApi.Services
{
    public class BaoService
    {
        public async Task<JObject> GetDetailAsync(string id)
        {
            return await _Db.GetJsonAsync("select * from Bao where Id=@Id", new List<object>() { "Id", id });
        }

    } //class
}
