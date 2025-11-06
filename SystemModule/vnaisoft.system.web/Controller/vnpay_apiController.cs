using dotAPNS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using vnaisoft.common.BaseClass;
using vnaisoft.common.common;
using vnaisoft.common.Helpers;
using vnaisoft.common.Services;
using vnaisoft.DataBase.Helper;
using vnaisoft.DataBase.Mongodb;
using vnaisoft.system.data.DataAccess;
using vnaisoft.system.data.Models;

namespace vnaisoft.system.web.Controller
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
