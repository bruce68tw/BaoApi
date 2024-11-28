using BaoLib.Services;
using Base.Models;
using Base.Services;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace BaoApi.Services
{
    public class MyBaoRead
    {
        private readonly ReadDto readDto = new()
        {
            ReadSql = $@"
select b.*, 
    ReplyTypeName=x.Name,
    PrizeTypeName=x2.Name,
    Corp=c.Name
from dbo.Bao b
join dbo.UserCust c on b.Creator=c.Id
join dbo.BaoAttend a on b.Id=a.BaoId
join dbo.XpCode x on x.Type='{_XpLib.ReplyType}' and b.ReplyType=x.Value
join dbo.XpCode x2 on x2.Type='{_XpLib.PrizeType}' and b.PrizeType=x2.Value
where a.UserId='{_Fun.UserId()}'
order by a.Created desc
",
        };

        public async Task<JObject?> GetPageA(EasyDtDto easyDto)
        {
            return await new CrudReadSvc().GetPageA(readDto, easyDto);
        }

    } //class
}