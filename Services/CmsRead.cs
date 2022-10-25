using Base.Models;
using Base.Services;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace BaoApi.Services
{
    public class CmsRead
    {
        private readonly ReadDto readDto = new()
        {
            ReadSql = @"
select *
from dbo.Cms
where StartTime < getdate()
and EndTime > getdate()
and Status=1
order by Id desc
",
        };

        public async Task<JObject> GetPageA(EasyDtDto easyDto)
        {
            return await new CrudRead().GetPageA(readDto, easyDto); ;
        }

    } //class
}