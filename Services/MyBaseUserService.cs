using Base.Models;
using Base.Services;
using BaseWeb.Extensions;
using BaseWeb.Services;

namespace BaoApi.Services
{
    public class MyBaseUserService : IBaseUserService
    {
        //get base user info
        public BaseUserDto GetData()
        {
            return _Http.GetSession().Get<BaseUserDto>(_Fun.BaseUser);   //extension method
        }
    }
}
