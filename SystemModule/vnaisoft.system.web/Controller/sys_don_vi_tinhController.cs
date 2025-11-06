using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using vnaisoft.common.BaseClass;
using vnaisoft.common.common;
using vnaisoft.common.Helpers;
using vnaisoft.common.Services;
using vnaisoft.DataBase.Mongodb;
using vnaisoft.system.data.DataAccess;
using vnaisoft.system.data.Models;

namespace vnaisoft.system.web.Controller
{
    public partial class sys_don_vi_tinhController : BaseAuthenticationController
    {
        public sys_don_vi_tinh_repo repo;
        public AppSettings _appsetting;

        public sys_don_vi_tinhController(IUserService userService, MongoDBContext context, IOptions<AppSettings> appsetting) : base(userService)
        {
            repo = new sys_don_vi_tinh_repo(context);
            _appsetting = appsetting.Value;
        }

        //Lấy mã folder , subfolder 
        public async Task<IActionResult> get_code([FromBody] JObject json)
        {
            var code = repo.getCode();
            return Json(code);
        }


        public IActionResult getListUse()
        {

            var result = repo._context.sys_don_vi_tinh_col.AsQueryable()
                .Where(d => d.status_del == 1).
                 Select(d => new
                 {
                     id = d.id,
                     name = d.ten,
                 }).ToList();
            return Json(result);
        }


        [HttpPost]
        public async Task<IActionResult> create([FromBody] JObject json)
        {

            var model = JsonConvert.DeserializeObject<sys_don_vi_tinh_model>(json.GetValue("data").ToString());
            var check = checkModelStateCreate(model);
            if (!check)
            {
                return generateError();
            }

            model.db.ma = repo.getCode();
            model.db.id = model.db.ma;
            model.db.status_del = 1;
            model.db.nguoi_cap_nhat = getUserId();
            model.db.ngay_cap_nhat = DateTime.Now;

            await repo.insert(model);
            return Json(model);
        }

        [HttpPost]
        public async Task<IActionResult> edit([FromBody] JObject json)
        {
            var model = JsonConvert.DeserializeObject<sys_don_vi_tinh_model>(json.GetValue("data").ToString());
            var check = checkModelStateEdit(model);
            if (!check)
            {
                return generateError();
            }
            model.db.nguoi_cap_nhat = getUserId();
            model.db.ngay_cap_nhat = DateTime.Now;
            await repo.update(model);
            return Json(model);
        }

        public async Task<IActionResult> delete([FromBody] JObject json)
        {
            var id = json.GetValue("id").ToString();
            var status_del = int.Parse(json.GetValue("status_del").ToString());
            repo.delete(id);
            return Json("");
        }


        public async Task<IActionResult> update_status_del([FromBody] JObject json)
        {
            var id = json.GetValue("id").ToString();
            var status_del = int.Parse(json.GetValue("status_del").ToString());
            repo.update_status_del(id, getUserId(), status_del);
            return Json("");
        }


        public async Task<IActionResult> getElementById([FromBody] JObject json)
        {
            var id = json.GetValue("id").ToString();
            var model = await repo.getElementById(id);
            return Json(model);
        }


        [HttpPost]

        public async Task<IActionResult> DataHandler([FromBody] JObject json)
        {
            try
            {
                var a = Request;
                var param = JsonConvert.DeserializeObject<DTParameters>(json.GetValue("param1").ToString());
                var dictionary = new Dictionary<string, string>();
                dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json.GetValue("data").ToString());

                var search = dictionary["search"].Trim().ToLower();
                var status_del = int.Parse(dictionary["status_del"]);

                var query = repo.FindAll()
                    .Where(d => d.db.status_del == status_del)
                     .Where(d => d.db.ten.ToLower().Contains(search) || d.db.ma.ToLower().Contains(search) || d.db.ghi_chu.ToLower().Contains(search));

                //.Where(d => d.db.ten.Contains(search))
                //.Where(d => d.db.ghi_chu.Contains(search))
                //.Where(d => d.db.ma.Contains(search) || search == "")
                //;

                var count = query.Count();
                var dataList = await Task.Run(() => query.OrderByDescending(d => d.db.ma).Skip(param.Start).Take(param.Length)
      .ToList());
                DTResult<sys_don_vi_tinh_model> result = new DTResult<sys_don_vi_tinh_model>
                {
                    start = param.Start,
                    draw = param.Draw,
                    data = dataList,
                    recordsFiltered = count,
                    recordsTotal = count
                };
                return Json(result);
            }

            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }

        }



    }
}
