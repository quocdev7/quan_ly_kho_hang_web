using Microsoft.AspNetCore.Mvc;
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
using vnaisoft.common.Services;
using vnaisoft.DataBase.Mongodb;
using vnaisoft.DataBase.System;
using vnaisoft.system.data.DataAccess;
using vnaisoft.system.data.Models;

namespace vnaisoft.system.web.Controller
{
    public partial class sys_group_userController : BaseAuthenticationController
    {
        public sys_group_user_repo repo;

        public sys_group_userController(IUserService userService, MongoDBContext context) : base(userService)
        {
            repo = new sys_group_user_repo(context);
        }

        public IActionResult getListUse()
        {

            var result = repo._context.sys_group_user_col.AsQueryable()
                .Where(d => d.status_del == 1).
                 Select(d => new
                 {
                     id = d.id,
                     name = d.name
                 }).ToList();
            return Json(result);
        }
        public async Task<IActionResult> getListItem([FromBody] JObject json)
        {
            var id = json.GetValue("id").ToString();
            var model = repo.FindAllItem().OrderBy(q=>q.type_user).ToList();

            model.ForEach(t =>
            {
                t.isCheck = repo._context.sys_group_user_detail_col.AsQueryable().Where(d => d.id_group_user == id && d.user_id == t.user_id).ToList().Count() > 0;
            });

            return Json(model);
        }
        public async Task<ActionResult> getListRoleFull()
        {
            var UserId = getUserId();
            var user = getUser();
            var model = ListControlller.list;
            model = model
                //.Where(d => (d.type_user ?? 1) == user.loai)
                .ToList();
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
        public async Task<IActionResult> getListRole([FromBody] JObject json)
        {

            var model = repo.FindAllRole(json.GetValue("id").ToString()).ToList();
            return Json(model);
        }

        [HttpPost]
        public async Task<IActionResult> create([FromBody] JObject json)
        {

            var model = JsonConvert.DeserializeObject<sys_group_user_model>(json.GetValue("data").ToString());
            var check = checkModelStateCreate(model);
            if (!check)
            {
                return generateError();
            }

            model.db.status_del = 1;
            model.db.id = ObjectId.GenerateNewId().ToString();
            model.db.nguoi_cap_nhat = getUserId();

            model.db.ngay_cap_nhat = DateTime.Now;
            await repo.insert(model);
            return Json(model);
        }

        [HttpPost]
        public async Task<IActionResult> edit([FromBody] JObject json)
        {
            var model = JsonConvert.DeserializeObject<sys_group_user_model>(json.GetValue("data").ToString());
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


        public async Task<IActionResult> update_status_del([FromBody] JObject json)
        {
            var id = json.GetValue("id").ToString();
            var status_del = int.Parse(json.GetValue("status_del").ToString());
            repo.update_status_del(id, getUserId(), status_del);
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
                var query = repo.FindAll()
                     //.Where(d=>d.db.status_del==1)
                     .Where(d => d.db.name.ToLower().Contains(search) || d.db.note.ToLower().Contains(search))
                     ;
                var status_del = int.Parse(dictionary["status_del"]);
                query = query.Where(d => d.db.status_del == status_del);

                var count = query.Count();
                var dataList = await Task.Run(() => query.Skip(param.Start).Take(param.Length)
        .OrderByDescending(d => d.db.ngay_cap_nhat).ToList());
                //dataList.ForEach(q =>
                //{
                //    q.count_user = repo._context.sys_group_user_details.AsQueryable().Where(d => d.user_id == q.db.nguoi_cap_nhat && d.status_del==1).Count();

                //});
                DTResult<sys_group_user_model> result = new DTResult<sys_group_user_model>
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
