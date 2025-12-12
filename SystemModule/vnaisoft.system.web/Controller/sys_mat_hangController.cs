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
    public partial class sys_mat_hangController : BaseAuthenticationController
    {
        public sys_mat_hang_repo repo;
        public AppSettings _appsetting;

        public sys_mat_hangController(IUserService userService, MongoDBContext context, IOptions<AppSettings> appsetting) : base(userService)
        {
            repo = new sys_mat_hang_repo(context);
            _appsetting = appsetting.Value;
        }

        public IActionResult getListUse([FromBody] JObject json)
        {
            if (json.GetValue("search") == null) return Json(new List<string>());
            var search = json.GetValue("search").ToString().ToLower() ?? "";
            var result = repo._context.sys_mat_hang_col.AsQueryable()
                .Where(d => d.status_del == 1)
                  .Where(d => d.ten.ToLower().Contains(search) || d.ma.ToLower().Contains(search)).
                 Select(d => new
                 {
                     id = d.id,
                     name = "(" + d.ma + ") " + d.ten,
                     id_don_vi_tinh = d.id_don_vi_tinh,
                     id_vat = d.vat,
                 }).Take(10).ToList();
            return Json(result);
        }
        //public async Task<IActionResult> add_mat_hang([FromBody] JObject json)
        //{
        //    var ma = json.GetValue("ma").ToString();
        //    var loai_giao_dich = json.GetValue("loai_giao_dich").ToString();
        //    var id_doi_tuong = json.GetValue("id_doi_tuong").ToString();
        //    var result = 1;
        //    var list_mat_hang = new List<sys_mat_hang_model>();
        //    var list_mat_hang_ignore = new List<erp_don_hang_ban_mat_hang_model>();
        //    try
        //    {
        //        list_mat_hang_ignore = JsonConvert.DeserializeObject<List<erp_don_hang_ban_mat_hang_model>>(json.GetValue("list_mat_hang").ToString());
        //    }
        //    catch
        //    {

        //    }


        //    var list_ignore = list_mat_hang_ignore.Select(q => q.id_mat_hang).ToList();
        //    var queryTable = repo._context.sys_mat_hang_col.AsQueryable().Where(q => q.ma == ma.Trim() || q.ma_vach == ma.Trim()).Where(q => !list_ignore.Contains(q.id));
        //    list_mat_hang = repo.FindAll(queryTable).ToList();
        //    if (id_doi_tuong != "")
        //    {
        //        list_mat_hang.ForEach(t =>
        //        {
        //            var findGiaMua = repo._context.erp_bang_gia_muas.AsQueryable().Where(d => d.id_mat_hang == t.db.id && d.id_nha_cung_cap == id_doi_tuong).OrderByDescending(q => q.ngay_ghi_nhan).FirstOrDefault();
        //            if (findGiaMua != null) t.gia_mua = findGiaMua.don_gia;
        //            if (t.db.id_loai_mat_hang != null)
        //            {
        //                var loai_dinh_khoan_mat_hang = repo._context.erp_loai_mat_hangs.AsQueryable().Where(d => d.id == t.db.id_loai_mat_hang && d.status_del == 1).SingleOrDefault();
        //                if (loai_dinh_khoan_mat_hang.id_loai_dinh_khoan_mat_hang != null)
        //                {
        //                    var ldkmh = repo._context.erp_loai_dinh_khoan_mat_hangs.AsQueryable().Where(d => d.id == loai_dinh_khoan_mat_hang.id_loai_dinh_khoan_mat_hang && d.status_del == 1).SingleOrDefault();
        //                    t.tai_khoan_no = ldkmh.ma_tk_no_tien_mat;
        //                    t.tai_khoan_co = ldkmh.ma_tk_co_tien_mat;
        //                }
        //            }
        //            var cong_ty = repo._context.erp_cong_tys.AsQueryable().SingleOrDefault();
        //            if (cong_ty.ma_so_thue != "")
        //            {
        //                var ma_dt = repo._context.erp_khach_hang_nha_cung_caps.AsQueryable().Where(d => d.dien_thoai == cong_ty.ma_so_thue).Select(d => d.ma).SingleOrDefault();
        //                t.doi_tuong_co = ma_dt;
        //                t.doi_tuong_no = ma_dt;
        //            }
        //            else
        //            {
        //                var ma_dt = repo._context.erp_khach_hang_nha_cung_caps.AsQueryable().Where(d => d.dien_thoai == cong_ty.dien_thoai).Select(d => d.ma).SingleOrDefault();
        //                t.doi_tuong_co = ma_dt;
        //                t.doi_tuong_no = ma_dt;
        //            }
        //        });
        //    }
        //    if (list_mat_hang.Count == 0) result = 1;
        //    else
        //    {
        //        if (list_mat_hang[0].db.status_del == 2)
        //        {

        //            result = 4;
        //            list_mat_hang = new List<sys_mat_hang_model>();
        //        }
        //        else
        //        {
        //            if (loai_giao_dich == "1")
        //            {
        //                if (list_mat_hang[0].db.thuoc_tinh == 6)
        //                {
        //                    result = 2;
        //                    list_mat_hang = new List<sys_mat_hang_model>();
        //                }
        //            }
        //            else
        //            {
        //                if (list_mat_hang[0].db.thuoc_tinh != 6)
        //                {
        //                    result = 3;
        //                    list_mat_hang = new List<sys_mat_hang_model>();
        //                }
        //            }
        //        }

        //    }

        //    var data = new
        //    {
        //        result = result,
        //        list_mat_hang = list_mat_hang
        //    };



        //    return Json(data);
        //}
        //public async Task<IActionResult> add_mat_hang_da_chon([FromBody] JObject json)
        //{
        //    var ma = json.GetValue("ma").ToString();
        //    var loai_giao_dich = json.GetValue("loai_giao_dich").ToString();
        //    var id_doi_tuong = json.GetValue("id_doi_tuong").ToString();
        //    var result = 1;
        //    var list_mat_hang = new List<sys_mat_hang_model>();
        //    //var list_mat_hang_ignore = new List<erp_don_hang_ban_mat_hang_model>();
        //    //try
        //    //{
        //    //    list_mat_hang_ignore = JsonConvert.DeserializeObject<List<erp_don_hang_ban_mat_hang_model>>(json.GetValue("list_mat_hang").ToString());
        //    //}
        //    //catch
        //    //{

        //    //}


        //    //var list_ignore = list_mat_hang_ignore.Select(q => q.id_mat_hang).ToList();
        //    var queryTable = repo._context.sys_mat_hang_col.AsQueryable().Where(q => q.ma == ma.Trim() || q.ma_vach == ma.Trim());
        //    list_mat_hang = repo.FindAll(queryTable).ToList();
        //    if (id_doi_tuong != "")
        //    {
        //        list_mat_hang.ForEach(t =>
        //        {
        //            var findGiaMua = repo._context.erp_bang_gia_muas.AsQueryable().Where(d => d.id_mat_hang == t.db.id && d.id_nha_cung_cap == id_doi_tuong).OrderByDescending(q => q.ngay_ghi_nhan).FirstOrDefault();
        //            if (findGiaMua != null) t.gia_mua = findGiaMua.don_gia;
        //            if (t.db.id_loai_mat_hang != null)
        //            {
        //                var loai_dinh_khoan_mat_hang = repo._context.erp_loai_mat_hangs.AsQueryable().Where(d => d.id == t.db.id_loai_mat_hang && d.status_del == 1).SingleOrDefault();
        //                if (loai_dinh_khoan_mat_hang.id_loai_dinh_khoan_mat_hang != null)
        //                {
        //                    var ldkmh = repo._context.erp_loai_dinh_khoan_mat_hangs.AsQueryable().Where(d => d.id == loai_dinh_khoan_mat_hang.id_loai_dinh_khoan_mat_hang && d.status_del == 1).SingleOrDefault();
        //                    t.tai_khoan_no = ldkmh.ma_tk_no_tien_mat;
        //                    t.tai_khoan_co = ldkmh.ma_tk_co_tien_mat;
        //                }
        //            }
        //            var cong_ty = repo._context.erp_cong_tys.AsQueryable().SingleOrDefault();
        //            if (cong_ty.ma_so_thue != "")
        //            {
        //                var ma_dt = repo._context.erp_khach_hang_nha_cung_caps.AsQueryable().Where(d => d.dien_thoai == cong_ty.ma_so_thue).Select(d => d.ma).SingleOrDefault();
        //                t.doi_tuong_co = ma_dt;
        //                t.doi_tuong_no = ma_dt;
        //            }
        //            else
        //            {
        //                var ma_dt = repo._context.erp_khach_hang_nha_cung_caps.AsQueryable().Where(d => d.dien_thoai == cong_ty.dien_thoai).Select(d => d.ma).SingleOrDefault();
        //                t.doi_tuong_co = ma_dt;
        //                t.doi_tuong_no = ma_dt;
        //            }
        //        });
        //    }
        //    if (list_mat_hang.Count == 0) result = 1;
        //    else
        //    {
        //        if (list_mat_hang[0].db.status_del == 2)
        //        {

        //            result = 4;
        //            list_mat_hang = new List<sys_mat_hang_model>();
        //        }
        //        else
        //        {
        //            if (loai_giao_dich == "1")
        //            {
        //                if (list_mat_hang[0].db.thuoc_tinh == 6)
        //                {
        //                    result = 2;
        //                    list_mat_hang = new List<sys_mat_hang_model>();
        //                }
        //            }
        //            else
        //            {
        //                if (list_mat_hang[0].db.thuoc_tinh != 6)
        //                {
        //                    result = 3;
        //                    list_mat_hang = new List<sys_mat_hang_model>();
        //                }
        //            }
        //        }

        //    }

        //    var data = new
        //    {
        //        result = result,
        //        list_mat_hang = list_mat_hang
        //    };



        //    return Json(data);
        //}

        public async Task<IActionResult> get_code([FromBody] JObject json)
        {
            var code = repo.getCode();
            return Json(code);
        }
        [HttpPost]
        public async Task<IActionResult> create([FromBody] JObject json)
        {

            var model = JsonConvert.DeserializeObject<sys_mat_hang_model>(json.GetValue("data").ToString());
            var check = checkModelStateCreate(model);
            if (!check)
            {
                return generateError();
            }
            model.db.id = model.db.ma;
            model.db.status_del = 1;
            model.db.nguoi_cap_nhat = getUserId();

            model.db.ngay_cap_nhat = DateTime.Now;
            await repo.insert(model);
            //generate_qrcode(model);
            //generate_barcode(model);



            return Json(model);
        }
        [HttpPost]
        public async Task<IActionResult> edit([FromBody] JObject json)
        {
            var model = JsonConvert.DeserializeObject<sys_mat_hang_model>(json.GetValue("data").ToString());
            var check = checkModelStateEdit(model);
            if (!check)
            {
                return generateError();
            }
            model.db.nguoi_cap_nhat = getUserId();
            model.db.ngay_cap_nhat = DateTime.Now;
            await repo.update(model);

            //generate_qrcode(model);
            //generate_barcode(model);

            return Json(model);
        }
        public async Task<IActionResult> update_status_del([FromBody] JObject json)
        {
            var id = json.GetValue("id").ToString();
            var status_del = int.Parse(json.GetValue("status_del").ToString());
            repo.update_status_del(id, getUserId(), status_del);
            return Json("");
        }

        public IActionResult getElementById([FromBody] JObject json)
        {
            var id = json.GetValue("id").ToString();
            var query = repo._context.sys_mat_hang_col.AsQueryable().Where(q => q.id == id);
            var model = repo.FindAll(query).SingleOrDefault();
            //if (model.db.list_dac_tinh != null)
            //{
            //    model.db.list_dac_tinh.ForEach(q =>
            //    {
            //        if (q.code == "4" || q.code == "5")
            //            q.list_dac_tinh_mat_hang = repo._context.erp_dac_tinh_mat_hang_danh_mucs.AsQueryable().Where(d => d.id_dac_tinh == q.id)
            //            .Select(q => new erp_dac_tinh_common_model()
            //            {
            //                id = q.id,
            //                name = q.ten
            //            }).ToList();
            //    });
            //}
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
                var id_loai_mat_hang = dictionary["id_loai_mat_hang"];

                var id_doi_tuong = "";
                try
                {
                    id_doi_tuong = dictionary["id_doi_tuong"];
                }
                catch (Exception e) { }
                ;
                var id_thuoc_tinh = dictionary["id_thuoc_tinh"];
                var status_del = int.Parse(dictionary["status_del"]);
                var kieu_ban = dictionary["kieu_ban"];
                var ignore_ids = dictionary["ignore_ids"];
                var loai_giao_dich = dictionary["loai_giao_dich"];
                var lst_ignore_id = new List<string>();

                if (!String.IsNullOrEmpty(ignore_ids))
                {
                    ignore_ids = ignore_ids.Substring(0, ignore_ids.Length - 1);
                    lst_ignore_id = ignore_ids.Split(",").ToList();

                }

                var queryTable = repo._context.sys_mat_hang_col.AsQueryable().Where(d => d.status_del == status_del)
                      .Where(d => d.id_loai_mat_hang == id_loai_mat_hang || id_loai_mat_hang == "-1")

                      .Where(d => d.ten.ToLower().Contains(search) || d.ma.ToLower().Contains(search)
                      || d.ten_khong_dau.ToLower().Contains(search) || d.ghi_chu.ToLower().Contains(search) || search == "")
                      .Where(d => !lst_ignore_id.Contains(d.id))
                      ;

                if (kieu_ban == "1")
                {
                    queryTable = queryTable.Where(d => d.gia_ban_si != null && d.gia_ban_si > 0);
                }
                else if (kieu_ban == "2")
                {
                    queryTable = queryTable.Where(d => d.gia_ban_le != null && d.gia_ban_le > 0);
                }
                else { }


                var count = queryTable.Count();
                queryTable = queryTable.OrderByDescending(d => d.ma);
                var dataList = await Task.Run(() => repo.FindAll(queryTable.Skip(param.Start).Take(param.Length)).ToList());
                dataList.ForEach(t =>
                {
                    t.ten_loai_mat_hang = repo._context.sys_loai_mat_hang_col.AsQueryable().Where(d => d.id == t.db.id_loai_mat_hang && d.status_del == 1).Select(q => q.ten).FirstOrDefault();
                    t.ma_loai_mat_hang = repo._context.sys_loai_mat_hang_col.AsQueryable().Where(d => d.id == t.db.id_loai_mat_hang && d.status_del == 1).Select(q => q.ma).FirstOrDefault();
                    t.ten_don_vi_tinh = repo._context.sys_don_vi_tinh_col.AsQueryable().Where(d => d.id == t.db.id_don_vi_tinh && d.status_del == 1).Select(q => q.ten).FirstOrDefault();
                    var ton_kho = repo._context.sys_ton_kho_mat_hang_col.AsQueryable().Where(d => d.id_mat_hang == t.db.id).Sum(q => q.so_luong_ton);
                    ton_kho = ton_kho < 0 ? 0 : ton_kho;
                    t.ton_kho = (decimal?)((ton_kho / 1000));
                });

                DTResult<sys_mat_hang_model> result = new DTResult<sys_mat_hang_model>
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
        public async Task<IActionResult> DataHandlerTonKho([FromBody] JObject json)
        {
            try
            {
                var a = Request;
                var param = JsonConvert.DeserializeObject<DTParameters>(json.GetValue("param1").ToString());
                var dictionary = new Dictionary<string, string>();
                dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json.GetValue("data").ToString());

                var search = dictionary["search"].Trim().ToLower();
                var id_loai_mat_hang = dictionary["id_loai_mat_hang"];
                var status_del = int.Parse(dictionary["status_del"]);
                var ignore_ids = dictionary["ignore_ids"];
                var lst_ignore_id = new List<string>();
                if (!String.IsNullOrEmpty(ignore_ids))
                {
                    ignore_ids = ignore_ids.Substring(0, ignore_ids.Length - 1);
                    lst_ignore_id = ignore_ids.Split(",").ToList();

                }
                var queryTable = repo._context.sys_mat_hang_col.AsQueryable().Where(d => d.status_del == status_del)
                      .Where(d => d.id_loai_mat_hang == id_loai_mat_hang || id_loai_mat_hang == "-1")
                      .Where(d => d.ten.ToLower().Contains(search) || d.ma.ToLower().Contains(search)
                      || d.ten_khong_dau.ToLower().Contains(search) || d.ghi_chu.ToLower().Contains(search) || search == "")
                      .Where(d => !lst_ignore_id.Contains(d.id))
                      ;


                var count = queryTable.Count();
                queryTable = queryTable.OrderByDescending(d => d.ma);
                var dataList = await Task.Run(() => repo.FindAll(queryTable.Skip(param.Start).Take(param.Length)).ToList());
                dataList.ForEach(t =>
                {
                    var ton_kho = repo._context.sys_ton_kho_mat_hang_col.AsQueryable().Where(d => d.id_mat_hang == t.db.id).Sum(q => q.so_luong_ton);
                    ton_kho = ton_kho < 0 ? 0 : ton_kho;
                    t.ton_kho = (decimal?)((ton_kho / 1000));
                    t.ten_don_vi_tinh = repo._context.sys_don_vi_tinh_col.AsQueryable().Where(d => d.id == t.db.id_don_vi_tinh && d.status_del == 1).Select(d => d.ten).FirstOrDefault();

                });

                DTResult<sys_mat_hang_model> result = new DTResult<sys_mat_hang_model>
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
