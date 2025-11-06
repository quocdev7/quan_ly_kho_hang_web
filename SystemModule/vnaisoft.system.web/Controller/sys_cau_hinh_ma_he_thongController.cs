using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using vnaisoft.common.BaseClass;
using vnaisoft.common.common;
using vnaisoft.common.Common;
using vnaisoft.common.Helpers;
using vnaisoft.common.Services;

using vnaisoft.DataBase.Mongodb;
using vnaisoft.DataBase.Mongodb.Collection.system;
using vnaisoft.fireBase.API;
using vnaisoft.system.data.DataAccess;
using vnaisoft.system.data.Models;


namespace vnaisoft.system.web.Controller
{
    public partial class sys_cau_hinh_ma_he_thongController : BaseAuthenticationController
    {
        public sys_cau_hinh_ma_he_thong_repo repo;

        public sys_cau_hinh_ma_he_thongController(IUserService userService, MongoDBContext context) : base(userService)
        {
            repo = new sys_cau_hinh_ma_he_thong_repo(context);
        }




        public async Task<IActionResult> getInitCode([FromBody] JObject json)
        {
            try
            {
                var controller = json.GetValue("controller").ToString();
                var data = repo.FindAll().Where(q => q.db.controller == controller).FirstOrDefault();

                if (data == null)
                {
                    data = new sys_cau_hinh_ma_he_thong_model();
                    data.db.controller = controller;
                    data.db.is_ngay_gio = false;
                    data.db.tien_to = "";
                    data.db.so_chu_so_tu_tang = 4;
                }
                else
                {
                    if (data.db.so_chu_so_tu_tang == 4) { data.so_tu_tang = "0000"; }
                    if (data.db.so_chu_so_tu_tang == 5) { data.so_tu_tang = "00000"; }
                    if (data.db.so_chu_so_tu_tang == 6) { data.so_tu_tang = "000000"; }
                }
                return Json(data);
            }

            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }

        }
        [HttpPost]
        public async Task<IActionResult> create([FromBody] JObject json)
        {
            var model = JsonConvert.DeserializeObject<sys_cau_hinh_ma_he_thong_model>(json.GetValue("data").ToString());
            var check = checkModelStateCreate(model);
            if (!check)
            {
                return generateError();
            }


            if (model.db.id == "0")
                await repo.save(model, 1);
            else
                await repo.save(model, 2);

            model.db.id = ObjectId.GenerateNewId().ToString();
            model.db.nguoi_cap_nhat = getUserId();
            await repo.insert(model);
            return Json(model);
        }

        [HttpPost]
        public async Task<IActionResult> edit([FromBody] JObject json)
        {
            var model = JsonConvert.DeserializeObject<sys_cau_hinh_ma_he_thong_model>(json.GetValue("data").ToString());
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
        [HttpPost]
        public async Task<IActionResult> get_data([FromBody] JObject json)
        {
            var controller = json.GetValue("controller").ToString();
            var result = repo.FindAll().Where(q => q.db.controller == controller).FirstOrDefault();
            return Json(result);
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

                     ;
                var status_del = int.Parse(dictionary["status_del"]);
                query = query.Where(d => d.db.status_del == status_del);

                var count = query.Count();
                var dataList = await Task.Run(() => query.OrderByDescending(d => d.db.id).Skip(param.Start).Take(param.Length)
      .ToList());
                DTResult<sys_cau_hinh_ma_he_thong_model> result = new DTResult<sys_cau_hinh_ma_he_thong_model>
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
