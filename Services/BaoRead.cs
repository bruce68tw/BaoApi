using Base.Enums;
using Base.Models;
using Base.Services;
using Newtonsoft.Json.Linq;

namespace BaoApi.Services
{
    public class BaoRead
    {
        private ReadDto readDto = new ReadDto()
        {
            ReadSql = @"
select checked=0, type=1, walk=1, 
    start='2021/5/1', corp='corp 1',
    id=Id, name=Name, corp=Corp
from dbo.Bao
order by Id
",
            Items = new [] {
                new QitemDto { Fid = "Name", Op = ItemOpEstr.Like },
            },
        };

        public JObject GetPage(string ctrl, EasyDtDto easyDto)
        {
            return new CrudRead().GetPage(ctrl, readDto, easyDto);
        }

    } //class
}