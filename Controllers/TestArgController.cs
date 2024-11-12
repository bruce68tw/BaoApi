using Base.Models;
using BaseApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace BaoApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class TestArgController : BaseCtrl
    {

        [HttpPost]
        public string T1(string Id, string Str)
        {
            return $"T1: id={Id}, str={Str}";
        }

        [HttpPost]
        public string T2(IdStrDto data)
        {
            return $"T2: Id={data.Id}, Str={data.Str}";
        }

        [HttpPost]
        public string T3(JObject data)
        {
            return $"T3: Id={data["Id"]}, Str={data["Str"]}";
        }

    }//class
}
