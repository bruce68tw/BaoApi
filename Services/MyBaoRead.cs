using Base.Models;
using Base.Services;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace BaoApi.Services
{
    public class MyBaoRead
    {
        private ReadDto readDto = new ReadDto()
        {
            ReadSql = $@"
select checked=0, isMove=b.IsMove, giftType=b.GiftType, 
    startTime=b.StartTime, corp=c.Name,
    id=b.Id, name=b.Name
from dbo.Bao b
join dbo.UserCust c on b.Creator=c.Id
join dbo.UserBao ub on b.Id=ub.BaoId
where ub.UserId='{_Fun.GetBaseUser().UserId}'
order by ub.Created desc
",
        };

        public async Task<JObject> GetPageAsync(string ctrl, EasyDtDto easyDto)
        {
            return await new CrudRead().GetPageAsync(ctrl, readDto, easyDto);
        }

    } //class
}