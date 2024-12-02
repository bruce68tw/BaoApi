using BaoLib.Enums;
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
            //讀取全部已參加尋寶, 由前端控制
            ReadSql = $@"
select b.*, 
    bt.AttendStatus,
    ReplyTypeName=x.Name,
    PrizeTypeName=x2.Name,
    Corp=c.Name,
    BaoStatus={_Xp.BaoStatusSql}
from dbo.Bao b
join dbo.UserCust c on b.Creator=c.Id
join dbo.BaoAttend bt on b.Id=bt.BaoId and bt.UserId='{_Fun.UserId()}'
join dbo.XpCode x on x.Type='{_XpLib.ReplyType}' and b.ReplyType=x.Value
join dbo.XpCode x2 on x2.Type='{_XpLib.PrizeType}' and b.PrizeType=x2.Value
order by bt.Created desc
",
        };

        public async Task<JObject?> GetPageA(EasyDtDto easyDto)
        {
            return await new CrudReadSvc().GetPageA(readDto, easyDto);
        }

    } //class
}