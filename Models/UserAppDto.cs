using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Http.HttpResults;
using System;

namespace BaoApi.Models
{
    /// <summary>
    /// 對應 UserApp table
    /// </summary>
    public class UserAppDto
    {
        //constructor
        public UserAppDto()
        {
            //DirStageImage = "";
        }

        //folder of stage image
        public string Id { get; set; } = "";
        public string Name { get; set; } = "";
        public string Phone { get; set; } = "";
        public string Email { get; set; } = "";
        public string Ip { get; set; } = "";
        public string Address { get; set; } = "";
        public string AuthCode { get; set; } = "";
        public DateTime? Created { get; set; }
        public DateTime? Revised { get; set; }
    }
}
