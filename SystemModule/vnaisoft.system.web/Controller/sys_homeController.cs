using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using vnaisoft.common.BaseClass;
using vnaisoft.common.Helpers;
using vnaisoft.common.Models;
using vnaisoft.common.Services;
using vnaisoft.DataBase.commonFunc;
using vnaisoft.DataBase.Mongodb;
using JsonConvert = Newtonsoft.Json.JsonConvert;

namespace vnaisoft.system.web.Controller
{
    public partial class sys_homeController : BaseAuthenticationController
    {
        //private vnaisoftDefautContext context;
        public MongoDBContext _context;
        public AppSettings _appsetting;
        private IHttpContextAccessor _httpContextAccessor;
        public common_mongo_repo _common_repo;
        public sys_homeController(IUserService userService, MongoDBContext context, IOptions<AppSettings> appsetting, IHttpContextAccessor httpContextAccessor) : base(userService)
        {
            _context = context;
            _appsetting = appsetting.Value;
            _httpContextAccessor = httpContextAccessor;
            _common_repo = new common_mongo_repo(context);
        }
        public async Task<ActionResult> checkLogin()
        {
            return generateSuscess();

        }


        [AllowAnonymous]
        public ActionResult downloadtempFileError(string path)
        {
            string Files = path;
            byte[] fileBytes = System.IO.File.ReadAllBytes(Files);
            System.IO.File.WriteAllBytes(Files, fileBytes);
            MemoryStream ms = new MemoryStream(fileBytes);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, "error.txt");
        }
        public async Task<ActionResult> getListRoleFull()
        {
            var UserId = getUserId();
            var model = ListControlller.list;
            model = model.Where(d => (d.type_user ?? 1) == 1).ToList();
            var listdynamic = new List<dynamic>();
            for (int i = 0; i < model.Count; i++)
            {
                var listRole = model[i].list_role
                    .Select(d => new
                    {
                        role = d,
                        controller_name = model[i].translate
                    });
                listdynamic.AddRange(listRole);

            }
            return Json(listdynamic);

        }
        public string GetUrlAction(HttpRequest request)
        {
            string scheme = request.Scheme; // "http" or "https"
            string host = request.Host.ToString(); // "localhost:44399"
            string path = request.Path.ToString(); // "/sys_truong_hoc_index"
            string queryString = request.QueryString.ToString(); // Any query parameters, e.g., "?param1=value1"

            // Construct the full URL
            string fullUrl = $"{scheme}://{host}{path}{queryString}";

            return fullUrl;
        }
        public async Task<ActionResult> getModule([FromBody] JObject json)
        {

            var UserId = getUserId();
            //var typeuser = _context.Users.AsQueryable().Where(t => t.Id == UserId).Select(d => d.type).FirstOrDefault();
            var model = JsonConvert.DeserializeObject<List<ControllerAppModel>>(JsonConvert.SerializeObject(ListControlller.list));
            var groupID = _context.sys_group_user_detail_col.AsQueryable().Where(d => d.user_id == UserId).Select(d => d.id_group_user).ToList();
            //model = model.Where(d => (d.type_user ?? 1) == typeuser).ToList();
            var modelfilerRole = model;

            model.ForEach((menu) =>
            {
                menu.list_role = _context.sys_group_user_role_col.AsQueryable().Where(d => groupID.Contains(d.id_group_user)).ToList().Where(d => d.id_controller_role.Split(";")[0] == menu.controller).Select(d => new ControllerRoleModel
                {
                    id = d.id_controller_role,
                    name = d.role_name,
                    list_controller_action = new List<string>()
                    {
                          d.id_controller_role,
                    }
                }).ToList();
            });

            var controller_names = _context.sys_group_user_role_col.AsQueryable().Where(d => groupID.Contains(d.id_group_user))
          .Select(d => d.controller_name).Distinct().ToList();
            modelfilerRole = modelfilerRole.Where(d => controller_names.Contains(d.translate) || d.is_show_all_user == true)
        .ToList();
            var listdynamic = new List<dynamic>();
            for (int i = 0; i < modelfilerRole.Count; i++)
            {
                var count = 0;

                var countreturn = 0;

                var item = new
                {
                    menu = modelfilerRole[i],
                    badge_approval = count,
                    badge_return = countreturn,
                };
                listdynamic.Add(item);

            }
            return Json(listdynamic);


            //var request = _httpContextAccessor.HttpContext.Request;
            //var path = request.Path.ToString();
            //var parts = path.Split('/');
            //var action = "";
            //if (parts.Length > 1)
            //{
            //    action = parts[1]; // Returns "sys_truong_hoc_index"
            //}


            //var UserId = getUserId();
            //var ma = json.GetValue("ma").ToString();
            //var originalUrl = json.GetValue("originalUrl").ToString();
            ////var typeuser = _context.Users.AsQueryable().Where(t => t.Id == UserId).Select(d => d.type).FirstOrDefault();

            ////var path = Path.Combine(_appsetting.folder_path, "file_upload");
            ////var pathsave = Path.Combine(path, "module.txt");

            ////  System.IO.File.WriteAllText(pathsave, Newtonsoft.Json.JsonConvert.SerializeObject(ListControlller.list.Select(q=>q.controller)));

            ////var data = get_module_he_thong(ma, originalUrl);
            ////var lst = ListControlller.list.Where(q => data.lst_module.Contains(q.controller)).ToList();
            //var model = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ControllerAppModel>>(JsonConvert.SerializeObject(ListControlller.list));



            ////  var model = JsonConvert.DeserializeObject<List<ControllerAppModel>>(JsonConvert.SerializeObject(ListControlller.list));




            //var groupID = _context.sys_group_user_detail_col.AsQueryable().Where(d => d.user_id == UserId).Select(d => d.id_group_user).ToList();
            ////model = model.Where(d => (d.type_user ?? 1) == typeuser).ToList();
            //var modelfilerRole = model;
            //var lst_role = _context.sys_group_user_role_col.AsQueryable().Where(d => groupID.Contains(d.id_group_user) && d.id_module == ma).ToList();
            //model.ForEach((menu) =>
            //{
            //    menu.list_role = lst_role.Where(d => d.id_controller_role.Split(";")[0] == menu.controller).Select(d => new ControllerRoleModel
            //    {
            //        id = d.id_controller_role,
            //        name = d.role_name,
            //        list_controller_action = new List<string>()
            //    {
            //              d.id_controller_role,
            //    }
            //    }).ToList();
            //});

            //var controller_names = _context.sys_group_user_role_col.AsQueryable().Where(d => groupID.Contains(d.id_group_user)).Select(d => d.controller_name).Distinct().ToList();
            //modelfilerRole = modelfilerRole.Where(d => controller_names.Contains(d.translate) || d.is_show_all_user == true)
            //    .ToList();
            //var listdynamic = new List<dynamic>();
            //for (int i = 0; i < modelfilerRole.Count; i++)
            //{
            //    var count = 0;

            //    var countreturn = 0;

            //    var item = new
            //    {
            //        menu = modelfilerRole[i],
            //        badge_approval = count,
            //        badge_return = countreturn,
            //    };
            //    listdynamic.Add(item);

            //}
            ////if(getUser().type==2)
            ////{
            ////    ControllerAppModel controller = new ControllerAppModel();
            ////    var item1 = new
            ////    {
            ////        menu = controller,
            ////        badge_approval = 0,
            ////        badge_return = 0,

            ////    };
            ////    item1.menu.is_show_all_user = true;
            ////    item1.menu.controller = "maintain_san_pham";
            ////    item1.menu.url = "maintain_san_pham_index";
            ////    item1.menu.id = "maintain_san_pham";
            ////    item1.menu.title = "maintain_san_pham";
            ////    item1.menu.translate = "NAV.maintain_san_pham";
            ////    item1.menu.module = "maintain";
            ////    item1.menu.icon = "history";
            ////    listdynamic.Add(item1);
            ////}    


            ////var item1 = new ControllerAppModel
            ////{
            ////    controller
            ////};
            ////var menu = new
            ////{
            ////    menu =,
            ////    badge_approval = 0,
            ////    badge_return = 0,


            ////}

            //var rs = new
            //{
            //    menu = listdynamic,
            //    //ma = data.ma,
            //    //ten = data.ten
            //};
            //return Json(rs);

        }


    }
}
