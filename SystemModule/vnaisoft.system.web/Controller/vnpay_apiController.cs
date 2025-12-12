using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using quan_ly_kho.common.BaseClass;
using quan_ly_kho.common.Helpers;
using quan_ly_kho.common.Services;
using quan_ly_kho.DataBase.Mongodb;
using System.Threading.Tasks;

namespace quan_ly_kho.system.web.Controller
{
    public partial class vnpay_apiController : BaseAuthenticationController
    {
        public AppSettings _appsetting;
        public vnpay_apiController(IUserService userService, MongoDBContext context, IOptions<AppSettings> appsetting) : base(userService)
        {
            //repo = new sys_banner_repo(context);
            _appsetting = appsetting.Value;

        }


        [HttpPost]
        public async Task<IActionResult> edit([FromBody] JObject json)
        {

            return Json("");
        }




    }
}
