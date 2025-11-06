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
    public partial class sys_khach_hang_nha_cung_capController : BaseAuthenticationController
    {
        public sys_khach_hang_nha_cung_cap_repo repo;
        public AppSettings _appsetting;

        public sys_khach_hang_nha_cung_capController(IUserService userService, MongoDBContext context, IOptions<AppSettings> appsetting) : base(userService)
        {
            repo = new sys_khach_hang_nha_cung_cap_repo(context);
            _appsetting = appsetting.Value;
        }
        public async Task<IActionResult> get_code([FromBody] JObject json)
        {
            var code = repo.getCode();
            return Json(code);
        }


        [HttpPost]
        public async Task<IActionResult> getListUse([FromBody] JObject json)
        {
            var search = "";
            try
            {
                search = (json.GetValue("search").ToString() ?? "").ToLower();
            }
            catch
            {

            }
            var result = repo._context.sys_khach_hang_nha_cung_cap_col.AsQueryable()
                .Where(d => d.status_del == 1)
                 .Where(t => t.ma.ToLower().Contains(search.ToLower()) || t.ten.ToLower().Contains(search))
                 .Select(d => new
                 {
                     id = d.id,
                     name = "(" + d.ma + ") " + d.ten,
                     ma_so_thue = d.ma_so_thue,
                     dien_thoai = d.dien_thoai,
                     ten = d.ten
                 }).Take(10).ToList();
            return Json(result);
        }
        [HttpPost]
        public async Task<IActionResult> create([FromBody] JObject json)
        {

            var model = JsonConvert.DeserializeObject<sys_khach_hang_nha_cung_cap_model>(json.GetValue("data").ToString());
            var check = checkModelStateCreate(model);
            if (!check)
            {
                return generateError();
            }
            //model.db.id = ObjectId.GenerateNewId().ToString();

            //if (model.db.hinh_thuc == 1)
            //{
            //    model.db.ma_so_thue = model.db.ma_so_thue;
            //    model.db.id = model.db.dien_thoai;
            //    model.db.ma = model.db.dien_thoai;

            //}
            //else
            //{
            //    model.db.id = model.db.ma_so_thue;
            //    model.db.ma = model.db.ma_so_thue;

            //}
            model.db.id = model.db.ma_so_thue;
            model.db.ma = model.db.ma_so_thue;
            model.db.ma_so_thue = model.db.ma_so_thue;
            model.db.status_del = 1;
            model.db.nguoi_cap_nhat = getUserId();
            model.db.ngay_cap_nhat = DateTime.Now;

            await repo.insert(model);


            return Json(model);
        }

        [HttpPost]
        public async Task<IActionResult> edit([FromBody] JObject json)
        {
            var model = JsonConvert.DeserializeObject<sys_khach_hang_nha_cung_cap_model>(json.GetValue("data").ToString());
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
                var hinh_thuc = int.Parse(dictionary["hinh_thuc"]);
                var loai_hinh = int.Parse(dictionary["loai_hinh"]);

                var query = repo.FindAll()

                    .Where(d => d.db.status_del == status_del)

                     .Where(d => d.db.ten.ToLower().Contains(search) || d.db.ma_so_thue.ToLower().Contains(search)
                     || d.db.ten_khong_dau.ToLower().Contains(search) || d.db.dien_thoai.ToLower().Contains(search) || search == "")

                     ;

                if (hinh_thuc == -1)
                {

                }
                //else if (hinh_thuc == 5)
                //{
                //    query = query.Where(q => q.db.laKhachHang == true);
                //}
                //else if (hinh_thuc == 6)
                //{
                //    query = query.Where(q => q.db.laNhaCungCap == true);
                //}
                else
                {
                    query = query.Where(d => d.db.hinh_thuc == hinh_thuc);
                }

                //if (loai_hinh == 1)
                //{
                //    var query_don_hang_ban = repo._context.erp_don_hang_bans.AsQueryable().Select(d => d.id_doi_tuong).Distinct();
                //    query = query.Where(d => query_don_hang_ban.Contains(d.db.id));
                //}
                //else if (loai_hinh == 2)
                //{
                //    var query_don_hang_mua = repo._context.erp_don_hang_muas.AsQueryable().Select(d => d.id_doi_tuong).Distinct();
                //    query = query.Where(d => query_don_hang_mua.Contains(d.db.id));
                //}

                var count = query.Count();

                var dataList = await Task.Run(() => query.OrderByDescending(d => d.db.ngay_cap_nhat).Skip(param.Start).Take(param.Length)
        .ToList());
                dataList.ForEach(q =>
                {
                    //loai
                    //if (q.db.laKhachHang == true)
                    //{
                    //    q.ten_loai = "Khách hàng";
                    //}
                    //if (q.db.laNhaCungCap == true)
                    //{
                    //    q.ten_loai = "Nhà cung cấp";
                    //}
                    //if (q.db.laKhachHang == true && q.db.laNhaCungCap == true)
                    //{
                    //    q.ten_loai = "Khách hàng; Nhà cung cấp";
                    //}

                });
                DTResult<sys_khach_hang_nha_cung_cap_model> result = new DTResult<sys_khach_hang_nha_cung_cap_model>
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
