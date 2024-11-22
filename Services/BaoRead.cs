using BaoLib.Enums;
using Base.Models;
using Base.Services;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace BaoApi.Services
{
    public class BaoRead
    {
        private readonly ReadDto readDto = new()
        {
            ReadSql = $@"
select b.IsMove, b.IsBatch, b.IsMoney, 
    b.StartTime, Corp=c.Name,
    b.Id, b.Name
from dbo.Bao b
join dbo.UserCust c on b.Creator=c.Id
where b.StartTime < cast(getDate() as date)
and b.EndTime > getdate()
and b.Status=1
and b.LaunchStatus='{LaunchStatusEstr.Yes}'
order by b.StartTime desc
",
            /*
            Items = new QitemDto[] {
                new() { Fid = "Name", Op = ItemOpEstr.Like },
            },
            */
        };

        public async Task<JObject?> GetPageA(EasyDtDto easyDto)
        {
            //1.get redis key: BaoList + query condition
            var userId = _Fun.UserId();
            var key = RedisTypeEstr.BaoList + _Str.Md5(_Model.ToJsonStr(easyDto));

            /*
            //2.check redis has data or not
            var value = _Cache.GetStr(userId, key);

            //3.return redis data if existed
            if (value != null) return _Str.ToJson(value);
            */

            //4.read db
            var json = await new CrudReadSvc().GetPageA(readDto, easyDto);

            //5.write redis & return data
            //if (json != null) _Cache.SetStr(userId, key, _Json.ToStr(json));
            return json;
        }

    } //class
}