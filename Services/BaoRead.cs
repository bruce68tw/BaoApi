﻿using Base.Enums;
using Base.Models;
using Base.Services;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace BaoApi.Services
{
    public class BaoRead
    {
        private ReadDto readDto = new ReadDto()
        {
            ReadSql = @"
select checked=0, isMove=b.IsMove, giftType=b.GiftType, 
    startTime=b.StartTime, corp=c.Name,
    id=b.Id, name=b.Name
from dbo.Bao b
join dbo.UserCust c on b.Creator=c.Id
order by b.Id desc
",
            /*
            Items = new [] {
                new QitemDto { Fid = "Name", Op = ItemOpEstr.Like },
            },
            */
        };

        public async Task<JObject> GetPageAsync(string ctrl, EasyDtDto easyDto)
        {
            return await new CrudRead().GetPageAsync(ctrl, readDto, easyDto);
        }

    } //class
}