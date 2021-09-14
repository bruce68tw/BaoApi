using Base.Models;
using Base.Services;

namespace BaoApi.Services
{
    public class BaoEdit : XpEdit
    {
        public BaoEdit(string ctrl) : base(ctrl) { }

        override public EditDto GetDto()
        {
            return new EditDto
            {
				Table = "dbo.[CustInput]",
                PkeyFid = "Id",
                Col4 = null,
                Items = new [] 
				{
					new EitemDto { Fid = "Id" },
                    new EitemDto { Fid = "FldCheck", Required = true },
                    new EitemDto { Fid = "FldDate", Required = true },
                    new EitemDto { Fid = "FldDt", Required = true },
                    new EitemDto { Fid = "FldDec", Required = true },
                },
            };
        }

    } //class
}
