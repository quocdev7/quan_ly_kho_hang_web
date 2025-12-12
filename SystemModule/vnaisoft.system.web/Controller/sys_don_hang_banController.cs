using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using quan_ly_kho.common.BaseClass;
using quan_ly_kho.common.Common;
using quan_ly_kho.common.Helpers;
using quan_ly_kho.common.Services;
using quan_ly_kho.DataBase.Mongodb;
using quan_ly_kho.system.data.DataAccess;
using quan_ly_kho.system.data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace quan_ly_kho.system.web.Controller
{
    public partial class sys_don_hang_banController : BaseAuthenticationController
    {
        public sys_don_hang_ban_repo repo;
        //public erp_phieu_xuat_kho_repo repoXK;

        public AppSettings _appsetting;

        public sys_don_hang_banController(IUserService userService, MongoDBContext context, IOptions<AppSettings> appsetting) : base(userService)
        {
            repo = new sys_don_hang_ban_repo(context);
            //repoXK = new erp_phieu_xuat_kho_repo(context);
            _appsetting = appsetting.Value;
        }
        public async Task<IActionResult> get_code([FromBody] JObject json)
        {
            var code = repo.getCode();
            return Json(code);
        }
        public IActionResult getListUse()
        {
            var result = repo._context.sys_don_hang_ban_col.AsQueryable()
                //.Where(d => d.kieu_ban == 1)
                .Where(d => d.status_del == 1).
                 Select(d => new
                 {
                     id = d.id,
                     name = d.ma,
                     //kieu_ban = d.kieu_ban,
                 }).ToList();
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> create([FromBody] JObject json)
        {

            var model = JsonConvert.DeserializeObject<sys_don_hang_ban_model>(json.GetValue("data").ToString());
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
            model.db.ngay_tao = DateTime.Now;
            await repo.insert(model);
            return Json(model);
        }
        public async Task<IActionResult> getElementById([FromBody] JObject json)
        {
            var id = json.GetValue("id").ToString();
            var model = await repo.getElementById(id);
            return Json(model);
        }

        [HttpPost]
        public async Task<IActionResult> edit([FromBody] JObject json)
        {
            var model = JsonConvert.DeserializeObject<sys_don_hang_ban_model>(json.GetValue("data").ToString());
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

                var tu_ngay = dictionary["tu_ngay"].ToString();
                var tu_ngay_dt = Convert.ToDateTime(tu_ngay, System.Globalization.CultureInfo.InvariantCulture);
                var den_ngay = dictionary["den_ngay"].ToString();
                var den_ngay_dt = Convert.ToDateTime(den_ngay, System.Globalization.CultureInfo.InvariantCulture);
                tu_ngay_dt = new DateTime(tu_ngay_dt.Year, tu_ngay_dt.Month, tu_ngay_dt.Day, 0, 0, 0);
                den_ngay_dt = new DateTime(den_ngay_dt.Year, den_ngay_dt.Month, den_ngay_dt.Day, 23, 59, 59);

                var queryTable = repo._context.sys_don_hang_ban_col.AsQueryable().Where(d => d.status_del == status_del)
                   .Where(d => tu_ngay_dt <= d.ngay_dat_hang && den_ngay_dt >= d.ngay_dat_hang)
                   .Where(d => d.ma.ToLower().Contains(search) || d.ghi_chu.ToLower().Contains(search)
                                || d.ten.ToLower().Contains(search) || d.ten_khong_dau.ToLower().Contains(search)
                                || search == "")
                     ;
                var count = queryTable.Count();
                queryTable = queryTable.OrderByDescending(d => d.ma);
                var dataList = await Task.Run(() => repo.FindAll(queryTable.Skip(param.Start).Take(param.Length)).ToList());
                DTResult<sys_don_hang_ban_model> result = new DTResult<sys_don_hang_ban_model>
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
        [HttpPost]
        public async Task<IActionResult> DataHandlerDonHangBan([FromBody] JObject json)
        {
            try
            {
                var a = Request;
                var param = JsonConvert.DeserializeObject<DTParameters>(json.GetValue("param1").ToString());
                var dictionary = new Dictionary<string, string>();
                dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json.GetValue("data").ToString());

                var search = dictionary["search"].Trim().ToLower();
                var status_del = int.Parse(dictionary["status_del"]);
                var nguon = dictionary["nguon"];
                var tu_ngay = dictionary["tu_ngay"].ToString();
                var tu_ngay_dt = Convert.ToDateTime(tu_ngay, System.Globalization.CultureInfo.InvariantCulture);
                var den_ngay = dictionary["den_ngay"].ToString();
                var den_ngay_dt = Convert.ToDateTime(den_ngay, System.Globalization.CultureInfo.InvariantCulture);
                tu_ngay_dt = new DateTime(tu_ngay_dt.Year, tu_ngay_dt.Month, tu_ngay_dt.Day, 0, 0, 0);
                den_ngay_dt = new DateTime(den_ngay_dt.Year, den_ngay_dt.Month, den_ngay_dt.Day, 23, 59, 59);

                var queryTable = repo._context.sys_don_hang_ban_col.AsQueryable().Where(d => d.status_del == status_del)
                    .Where(d => tu_ngay_dt <= d.ngay_dat_hang && den_ngay_dt >= d.ngay_dat_hang)
                   .Where(d => d.ma.ToLower().Contains(search) || d.ghi_chu.ToLower().Contains(search)
                                || d.ten.ToLower().Contains(search) || d.ten_khong_dau.ToLower().Contains(search)
                                || search == "")
                     ;
                var count = queryTable.Count();
                queryTable = queryTable.OrderByDescending(d => d.ma);
                var dataList = await Task.Run(() => repo.FindAll(queryTable.Skip(param.Start).Take(param.Length)).ToList());
                DTResult<sys_don_hang_ban_model> result = new DTResult<sys_don_hang_ban_model>
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
