using Base.Models;
using Base.Services;

namespace BaoApi.Services
{
    public class XpCmsEdit : XpEdit
    {
        public XpCmsEdit(string ctrl) : base(ctrl) { }

        //private string _cmsType;
        override public EditDto GetDto()
        {
            return new EditDto
            {
				Table = "dbo.Cms",
                PkeyFid = "Id",
                ReadSql = @"
select c.*
from dbo.Cms c
where c.Id='{0}'
",
            };
        }

    } //class
}
