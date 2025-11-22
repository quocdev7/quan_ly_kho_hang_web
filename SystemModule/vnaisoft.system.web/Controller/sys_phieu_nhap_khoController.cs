using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
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
    public partial class sys_phieu_nhap_khoController : BaseAuthenticationController
    {
        public sys_phieu_nhap_kho_repo repo;
        public AppSettings _appsetting;

        public sys_phieu_nhap_khoController(IUserService userService, MongoDBContext context, IOptions<AppSettings> appsetting) : base(userService)
        {
            repo = new sys_phieu_nhap_kho_repo(context);
            _appsetting = appsetting.Value;
        }

        public IActionResult getListUse()
        {
            var result = repo._context.sys_phieu_nhap_kho_col.AsQueryable()
                .Where(d => d.status_del == 1).
                 Select(d => new
                 {
                     id = d.id,
                     name = d.ma,
                 }).ToList();
            return Json(result);
        }
        //public async Task<IActionResult> get_list_mat_hang_cua_phieu_nhap([FromBody] JObject json)
        //{
        //    var id_don_hang = json.GetValue("id_don_hang").ToString();
        //    var id = json.GetValue("id").ToList();
        //    var ids = String.Join(",", id);
        //    var list_id = ids.Split(",").ToList();
        //    var queryTableDetail = repo._context.sys_phieu_nhap_kho_chi_tiets.AsQueryable();
        //    var list_mat_hang = repo.FindAllDetailMatHang(queryTableDetail).AsQueryable().Where(q => list_id.Contains(q.id_phieu_nhap_kho)).ToList();
        //    var list_mat_hang_group = list_mat_hang.GroupBy(q => q.id_mat_hang).Select(q => new sys_phieu_nhap_kho_chi_tiet_ref_model
        //    {
        //        id_mat_hang = q.Key,
        //        don_gia = q.First().don_gia,
        //        so_luong = q.Sum(d => d.so_luong),
        //        noi_dung = q.First().ma_mat_hang,
        //        ten_mat_hang = q.First().ten_mat_hang,
        //        thanh_tien = q.Sum(d => d.so_luong) * q.First().don_gia,
        //    }).ToList();
        //    //model.id_mat_hangs = string.Join(',', model.list_mat_hang.Select(q => q.db.id_mat_hang).ToList());
        //    var don_hang = repo._context.sys_don_hang_bans.AsQueryable().Where(d => d.id == id_don_hang).Where(d => d.status_del == 1).SingleOrDefault();
        //    var van_chuyen = new sys_phieu_nhap_kho_chi_tiet_ref_model();
        //    if (don_hang.tien_van_chuyen != null)
        //    {
        //        van_chuyen.id_mat_hang = "-1";
        //        van_chuyen.don_gia = don_hang.tien_van_chuyen + (don_hang.tien_van_chuyen * don_hang.tien_vat_van_chuyen) / 100;
        //        van_chuyen.so_luong = 1;
        //        van_chuyen.noi_dung = "Tiền vận chuyển";
        //        van_chuyen.thanh_tien = (van_chuyen.so_luong ?? 0) * (van_chuyen.don_gia ?? 0);
        //        list_mat_hang_group.Add(van_chuyen);
        //    }
        //    var model = new
        //    {
        //        list_mat_hang = list_mat_hang,
        //        doi_tuong = don_hang,
        //    };
        //    return Json(model);
        //}

        public async Task<IActionResult> get_list_mat_hang_ban([FromBody] JObject json)
        {
            var id = json.GetValue("id").ToString();

            var list_mat_hang = new List<sys_phieu_nhap_kho_model>();
            var list_mat_hang_ignore = new List<sys_phieu_nhap_kho_model>();
            try
            {
                list_mat_hang_ignore = JsonConvert.DeserializeObject<List<sys_phieu_nhap_kho_model>>(json.GetValue("list_mat_hang").ToString());
            }
            catch
            {

            }
            var queryTable = repo._context.sys_phieu_nhap_kho_col.AsQueryable().Where(q => q.status_del == 1);
            list_mat_hang = repo.FindAll(queryTable).ToList();
            return Json(list_mat_hang);
        }
        public async Task<IActionResult> get_code([FromBody] JObject json)
        {
            var code = repo.getCode();
            return Json(code);
        }

        [HttpPost]
        public async Task<IActionResult> create([FromBody] JObject json)
        {
            var model = JsonConvert.DeserializeObject<sys_phieu_nhap_kho_model>(json.GetValue("data").ToString());
            var check = checkModelStateCreate(model);
            if (!check)
            {
                return generateError();
            }
            model.db.ma = repo.getCode();
            model.db.id = model.db.ma;
            model.db.status_del = 1;
            model.db.nguoi_cap_nhat = getUserId();
            model.db.ngay_nhap = model.db.ngay_nhap.Value.AddMinutes(1);
            model.db.ngay_cap_nhat = DateTime.Now;
            await repo.insert(model);

            return Json(model);
        }
        public async Task<IActionResult> update_status_del([FromBody] JObject json)
        {
            var id = json.GetValue("id").ToString();
            var status_del = int.Parse(json.GetValue("status_del").ToString());
            await repo.update_status_del(id, getUserId(), status_del);
            return Json("");
        }
        [HttpPost]
        public async Task<IActionResult> edit([FromBody] JObject json)
        {
            var model = JsonConvert.DeserializeObject<sys_phieu_nhap_kho_model>(json.GetValue("data").ToString());
            var check = checkModelStateEdit(model);
            if (!check)
            {
                return generateError();
            }
            model.db.nguoi_cap_nhat = getUserId();
            model.db.ngay_cap_nhat = DateTime.Now;
            model.db.ngay_nhap = model.db.ngay_nhap.Value;


            await repo.update(model);

            return Json(model);
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
                var tu_ngay = dictionary["tu_ngay"].ToString();
                var tu_ngay_t = Convert.ToDateTime(tu_ngay, System.Globalization.CultureInfo.InvariantCulture);
                var den_ngay = dictionary["den_ngay"].ToString();
                var den_ngay_t = Convert.ToDateTime(den_ngay, System.Globalization.CultureInfo.InvariantCulture);
                tu_ngay_t = new DateTime(tu_ngay_t.Year, tu_ngay_t.Month, tu_ngay_t.Day, 0, 0, 0);
                den_ngay_t = new DateTime(den_ngay_t.Year, den_ngay_t.Month, den_ngay_t.Day, 23, 59, 59);
                var id_loai_nhap = dictionary["id_loai_nhap"];

                //var query = repo.FindAll()
                var queryTable = repo._context.sys_phieu_nhap_kho_col.AsQueryable().Where(d => d.status_del == status_del)
                      //.Where(d => d.nguoi_cap_nhat == getUserId())
                      //.Where(d => d.loai == 1)
                      .Where(d => d.id_loai_nhap == id_loai_nhap || id_loai_nhap == "-1")
                       .Where(d => d.ngay_nhap >= tu_ngay_t && d.ngay_nhap <= den_ngay_t)
                       .Where(d => d.ma.ToLower().Contains(search) || d.ghi_chu.ToLower().Contains(search)
                       || d.ten.ToLower().Contains(search) || d.ten_khong_dau.ToLower().Contains(search)
                       //|| (d.id_don_hang_mua.ToLower() ?? "").Contains(search) || (d.id_don_hang_ban.ToLower() ?? "").Contains(search)
                       || search == "")
                       ;

                var count = queryTable.Count();
                queryTable = queryTable.OrderByDescending(d => d.ma);
                var dataList = await Task.Run(() => repo.FindAll(queryTable.Skip(param.Start).Take(param.Length)).ToList());
                dataList.ForEach(t =>
                {
                    if (t.db.id_don_hang_ban != null)
                    {
                        t.ma_don_hang = repo._context.sys_don_hang_ban_col.AsQueryable().Where(q => q.id == t.db.id_don_hang_ban).Select(q => q.ma).SingleOrDefault();
                    }
                    if (t.db.id_don_hang_mua != null)
                    {
                        t.ma_don_hang = repo._context.sys_don_hang_mua_col.AsQueryable().Where(q => q.id == t.db.id_don_hang_mua).Select(q => q.ma).SingleOrDefault();
                    }
                    t.db.ngay_nhap = DateTime.Parse(t.db.ngay_nhap.Value.ToString());
                    t.ngay_nhap = t.db.ngay_nhap.Value.ToString("dd/MM/yyyy");
                });
                DTResult<sys_phieu_nhap_kho_model> result = new DTResult<sys_phieu_nhap_kho_model>
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
