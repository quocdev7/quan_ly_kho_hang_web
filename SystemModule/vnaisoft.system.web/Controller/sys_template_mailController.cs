using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using vnaisoft.common.BaseClass;
using vnaisoft.common.common;
using vnaisoft.common.Services;
using vnaisoft.DataBase.Mongodb;
using vnaisoft.system.data.DataAccess;
using vnaisoft.system.data.Models;

namespace vnaisoft.system.web.Controller
{
    public partial class sys_template_mailController : BaseAuthenticationController
    {
        private sys_template_mail_repo repo;

        public sys_template_mailController(IUserService userService, MongoDBContext context) : base(userService)
        {
            repo = new sys_template_mail_repo(context);
        }

        [HttpPost]
        public async Task<IActionResult> create([FromBody] JObject json)
        {

            var model = JsonConvert.DeserializeObject<sys_template_mail_model>(json.GetValue("data").ToString());
            var check = checkModelStateCreate(model);
            if (!check)
            {
                return generateError();
            }
            model.db.nguoi_tao = getUserId();
            model.db.nguoi_cap_nhat = getUserId();
            model.db.id = Guid.NewGuid().ToString();
            model.db.ngay_tao = DateTime.Now;
            model.db.ngay_cap_nhat = DateTime.Now;
            model.db.status_del = 1;
            await repo.insert(model);
            return Json(model);
        }

        [HttpPost]
        public async Task<IActionResult> edit([FromBody] JObject json)
        {
            var model = JsonConvert.DeserializeObject<sys_template_mail_model>(json.GetValue("data").ToString());
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

        public IActionResult getListUse()
        {
            var result = repo._context.sys_template_mail_col.AsQueryable()
               .Where(d => d.status_del == 1).
                Select(d => new
                {
                    id = d.id,
                    name = d.name,
                    tieu_de = d.name,
                    noi_dung = d.template,
                }).ToList();
            return Json(result);
        }



        public async Task<IActionResult> update_status_del([FromBody] JObject json)
        {
            var id = json.GetValue("id").ToString();
            var status_del = int.Parse(json.GetValue("status_del").ToString());
            await repo.update_status_del(id, getUserId(), status_del);
            return Json("");
        }

        public async Task<IActionResult> delete([FromBody] JObject json)
        {
            var id = json.GetValue("id").ToString();
            await repo.update_status_del(id, getUserId(), 2);
            return Json("");
        }

        public async Task<IActionResult> getElementById(string id)
        {
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
                var id_type = dictionary["id_type"];
                var query = repo.FindAll()
                     .Where(d => d.db.name.ToLower().Contains(search))
                     ;
                var status_del = int.Parse(dictionary["status_del"]);
                query = query.Where(d => d.db.status_del == status_del);

                var count = query.Count();
                var dataList = await Task.Run(() => query.OrderByDescending(d => d.db.nguoi_cap_nhat).Skip(param.Start).Take(param.Length)
       .ToList());
                dataList.ForEach(q =>
                {
                    //q.nguoi_cap_nhat = repo._context.sys_user_col.AsQueryable()
                    //.Where(d => d.id == q.nguoi_cap_nhat).Select(q => q.ho_va_ten).SingleOrDefault();
                });
                DTResult<sys_template_mail_model> result = new DTResult<sys_template_mail_model>
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
