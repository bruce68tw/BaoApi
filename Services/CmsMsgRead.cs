using Base.Enums;
using Base.Models;
using Base.Services;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace BaoApi.Services
{
    public class CmsMsgRead
    {
        private readonly ReadDto readDto = new()
        {
            ReadSql = $@"
select Id, Title, Text, StartTime
from dbo.Cms
where CmsType='{_Xp.CmsMsg}'
order by Id desc
",
        };

        public async Task<JObject> GetPageAsync(EasyDtDto easyDto)
        {
            return await new CrudRead().GetPageAsync(readDto, easyDto);
        }

    } //class
}