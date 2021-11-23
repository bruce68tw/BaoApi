﻿using Base.Enums;
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
            ReadSql = @"
select Checked=0, b.IsMove, b.GiftType, 
    b.StartTime, Corp=c.Name,
    b.Id, b.Name
from dbo.Bao b
join dbo.UserCust c on b.Creator=c.Id
order by b.Id desc
",
            /*
            Items = new QitemDto[] {
                new() { Fid = "Name", Op = ItemOpEstr.Like },
            },
            */
        };

        public async Task<JObject> GetPageAsync(EasyDtDto easyDto)
        {
            return await new CrudRead().GetPageAsync(readDto, easyDto);
        }

    } //class
}