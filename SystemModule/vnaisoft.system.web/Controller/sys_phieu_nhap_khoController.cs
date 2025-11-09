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
        //public async Task<IActionResult> getPrint([FromBody] JObject json)
        //{
        //    var id = json.GetValue("id").ToString();
        //    var template = repo._context.sys_mau_ins.AsQueryable().Where(t => t.id_loai == "sys_phieu_nhap_kho").FirstOrDefault();
        //    var model = await repo.getElementById(id);
        //    var commonPrint = new Common_print_helper(repo._context);
        //    var templatePrint = commonPrint.generatePrint(template.noi_dung, model, "list_mat_hang");
        //    return Json(new
        //    {
        //        tieu_de = template.ten,
        //        noi_dung = templatePrint
        //    });
        //}
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

        [AllowAnonymous]
        public ActionResult downloadtemp()
        {
            var currentpath = Directory.GetCurrentDirectory();
            string newPath = Path.Combine(currentpath, "wwwroot", "assets", "template");
            if (!Directory.Exists(newPath))
                Directory.CreateDirectory(newPath);

            string Files = newPath + "\\sys_phieu_nhap_kho.xlsx";
            byte[] fileBytes = System.IO.File.ReadAllBytes(Files);
            System.IO.File.WriteAllBytes(Files, fileBytes);
            MemoryStream ms = new MemoryStream(fileBytes);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, "sys_phieu_nhap_kho.xlsx");
        }
        //public async Task<FileStreamResult> exportExcel(string search, int status_del, bool? open, DateTime tu_ngay, DateTime den_ngay, string id_loai_nhap, string id_kho)
        //{

        //    var excel = new ExcelHelper(_appsetting);

        //    search = search ?? "";
        //    search = search.Trim().ToLower();
        //    var tu_ngay_t = Convert.ToDateTime(tu_ngay, System.Globalization.CultureInfo.InvariantCulture);
        //    var den_ngay_t = Convert.ToDateTime(den_ngay, System.Globalization.CultureInfo.InvariantCulture);
        //    tu_ngay_t = new DateTime(tu_ngay_t.Year, tu_ngay_t.Month, tu_ngay_t.Day, 0, 0, 0);
        //    den_ngay_t = new DateTime(den_ngay_t.Year, den_ngay_t.Month, den_ngay_t.Day, 23, 59, 59);

        //    var queryTable = repo._context.sys_phieu_nhap_kho_col.AsQueryable().Where(d => d.status_del == status_del)
        //               //.Where(d => d.nguoi_cap_nhat == getUserId())
        //               .Where(d => d.ma.ToLower().Contains(search) || d.ghi_chu.ToLower().Contains(search)
        //               || d.ten.ToLower().Contains(search) || d.ten_khong_dau.ToLower().Contains(search)
        //               || (d.id_don_hang_mua.ToLower() ?? "").Contains(search) || (d.id_don_hang_ban.ToLower() ?? "").Contains(search) || search == "")
        //               .Where(d => d.ngay_nhap >= tu_ngay_t && d.ngay_nhap <= den_ngay_t)
        //               ;
        //    if (open == true)
        //    {
        //        queryTable = queryTable
        //         .Where(d => d.id_kho == id_kho || id_kho == "-1")
        //          .Where(d => d.id_loai_nhap == id_loai_nhap || id_loai_nhap == "-1")

        //         ;
        //    }
        //    var query = repo.FindAll(queryTable).ToList();

        //    var dataList = query.OrderByDescending(d => d.db.ma).ToList();
        //    dataList.ForEach(t =>
        //    {



        //        t.ngay_cap_nhap_str = t.db.ngay_cap_nhat.Value.ToString("dd/MM/yyyy");

        //        t.ngay_nhap_str = t.db.ngay_nhap.Value.ToString("dd/MM/yyyy");
        //        if (t.db.id_don_hang_ban != null)
        //        {
        //            t.ma_don_hang = repo._context.sys_don_hang_bans.AsQueryable().Where(q => q.id == t.db.id_don_hang_ban).Select(q => q.ma).SingleOrDefault();
        //        }
        //        else if (t.db.id_don_hang_mua != null)
        //        {
        //            t.ma_don_hang = repo._context.sys_don_hang_muas.AsQueryable().Where(q => q.id == t.db.id_don_hang_mua).Select(q => q.ma).SingleOrDefault();
        //        }

        //    });
        //    string[] header = new string[] {
        //        "STT (No.)","Mã","Tên","Ngày nhập","Tên kho","Mã đơn hàng","Loại nhập","Người cập nhật","Ngày cập nhật"
        //    };

        //    string[] listKey = new string[]
        //    {
        //         "db.ma","db.ten","ngay_nhap_str","ten_kho","ma_don_hang","ten_loai_nhap","ten_nguoi_cap_nhat","ngay_cap_nhap_str"
        //    };


        //    return await exportFileExcel(_appsetting, header, listKey, dataList, "sys_phieu_nhap_kho");
        //}

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
            //model.db.loai = 1;
            model.db.status_del = 1;
            model.db.nguoi_cap_nhat = getUserId();
            model.db.ngay_nhap = model.db.ngay_nhap.Value.AddMinutes(1);
            model.db.ngay_cap_nhat = DateTime.Now;
            //if (string.IsNullOrEmpty(model.db.id_don_hang_ban) && string.IsNullOrEmpty(model.db.id_don_hang_mua)) model.db.nguon = 4;
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
        public async Task<IActionResult> getElementByMa([FromBody] JObject json)
        {
            var ma = json.GetValue("ma").ToString();
            var queryTable = repo._context.sys_phieu_nhap_kho_col.AsQueryable();
            var model = repo.FindAll(queryTable).AsQueryable().Where(q => q.db.ma == ma).SingleOrDefault();
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
                    //if (t.db.id_don_hang_ban != null)
                    //{
                    //    t.ma_don_hang = repo._context.sys_don_hang_bans.AsQueryable().Where(q => q.id == t.db.id_don_hang_ban).Select(q => q.ma).SingleOrDefault();
                    //}
                    //if (t.db.id_don_hang_mua != null)
                    //{
                    //    t.ma_don_hang = repo._context.sys_don_hang_muas.AsQueryable().Where(q => q.id == t.db.id_don_hang_mua).Select(q => q.ma).SingleOrDefault();
                    //}
                    t.db.ngay_nhap = DateTime.Parse(t.db.ngay_nhap.Value.ToString());
                    //t.ngay_nhap = date_ngay_nhap.ToString("dd/MM/yyyy");
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
        [AllowAnonymous]
        public ActionResult downloadtempdetail()
        {
            var currentpath = Directory.GetCurrentDirectory();
            string newPath = Path.Combine(currentpath, "wwwroot", "assets", "template");
            if (!Directory.Exists(newPath))
                Directory.CreateDirectory(newPath);

            string Files = newPath + "\\sys_mat_hang_theo_phieu_nhap.xlsx";
            byte[] fileBytes = System.IO.File.ReadAllBytes(Files);
            System.IO.File.WriteAllBytes(Files, fileBytes);
            MemoryStream ms = new MemoryStream(fileBytes);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, "sys_mat_hang_theo_phieu_nhap.xlsx");
        }
        //[HttpPost]
        //public async Task<IActionResult> ImportFromExcel()
        //{
        //    var error = "";
        //    IFormFile file = Request.Form.Files[0];
        //    var name = "PhieuNhapKho";

        //    if (file.Length > 0)
        //    {
        //        var count = 0;
        //        try
        //        {
        //            var list_model = new List<sys_phieu_nhap_kho_model>();
        //            var list_row = handleImportFileSheetName(file, name);
        //            for (int ct = 0; ct < list_row.Count(); ct++)
        //            {
        //                var fileImport = list_row[ct].list_cell.ToList();
        //                var model = new sys_phieu_nhap_kho_model();
        //                var stt = (fileImport[0].value.ToString() ?? "").Trim();
        //                var ma_phieu_nhap_kho = (fileImport[1].value.ToString() ?? "").Trim();
        //                var ma_mat_hang = (fileImport[2].value.ToString() ?? "").Trim();
        //                var so_luong = (fileImport[3].value.ToString() ?? "").Trim();
        //                if (string.IsNullOrEmpty(so_luong))
        //                    so_luong = "0";
        //                var ghi_chu_detail = (fileImport[4].value.ToString() ?? "").Trim();
        //                var ma_loai_nhap = (fileImport[5].value.ToString() ?? "").Trim();
        //                var ma_kho = (fileImport[6].value.ToString() ?? "").Trim();
        //                var ngay_nhap = (fileImport[7].value.ToString() ?? "").Trim();
        //                var ghi_chu = (fileImport[8].value.ToString() ?? "").Trim();
        //                var ma_don_hang = (fileImport[9].value.ToString() ?? "").Trim();
        //                var ma_doi_tuong = (fileImport[10].value.ToString() ?? "").Trim();

        //                model.db.ma = ma_phieu_nhap_kho;
        //                model.db.id_loai_nhap = ma_loai_nhap;
        //                model.ma_kho = ma_kho;
        //                model.ghi_chu_detail = ghi_chu_detail;
        //                model.db.ngay_nhap = DateTime.Parse(ngay_nhap);
        //                model.db.ghi_chu = ghi_chu;
        //                model.ma_mat_hang = ma_mat_hang;
        //                model.so_luong = long.Parse(so_luong);
        //                /// <summary>
        //                /// NTRH : Nhập trả hàng
        //                /// NSX : nhập sản xuất
        //                /// NM : nhập mua
        //                /// NGT : nhập ghi tăng
        //                /// NCK : nhập chuyển kho
        //                /// </summary>
        //                if (model.db.id_loai_nhap == "NM" || model.db.id_loai_nhap == "NTRH")
        //                {
        //                    model.ma_don_hang = ma_don_hang;
        //                }
        //                model.db.id_doi_tuong = ma_doi_tuong;

        //                //user import
        //                //error = CheckErrorImport(model, ct + 1, error);
        //                list_model.Add(model);
        //                count++;
        //            }
        //            var lst_ma = list_model.Select(q => q.db.ma).Distinct().ToList();
        //            //foreach (var ma in lst_ma)
        //            for (int ct = 0; ct < lst_ma.Count(); ct++)
        //            {
        //                var ma = lst_ma[ct];
        //                var model = list_model.Where(q => q.db.ma == ma).FirstOrDefault();
        //                model.list_mat_hang = list_model.Where(q => q.db.ma == ma).Select(d => new sys_phieu_nhap_kho_chi_tiet_model
        //                {
        //                    id_mat_hang = d.ma_mat_hang,
        //                    so_luong = d.so_luong,
        //                    ghi_chu = d.ghi_chu_detail,
        //                }).ToList();
        //                error = CheckErrorImport(model, ct + 1, error);
        //            }
        //            if (string.IsNullOrEmpty(error))
        //            {
        //                //var lst_ma = list_model.Select(q => q.db.ma).Distinct().ToList();
        //                foreach (var ma in lst_ma)
        //                {
        //                    var model = list_model.Where(q => q.db.ma == ma).FirstOrDefault();
        //                    model.list_mat_hang = list_model.Where(q => q.db.ma == ma).Select(d => new sys_phieu_nhap_kho_chi_tiet_model
        //                    {
        //                        id_mat_hang = d.ma_mat_hang,
        //                        so_luong = d.so_luong,
        //                        ghi_chu = d.ghi_chu_detail,
        //                    }).ToList();
        //                    if (string.IsNullOrEmpty(model.db.ma))
        //                    {
        //                        model.db.ma = repo.getCode();
        //                    }
        //                    model.db.id = model.db.ma;
        //                    model.db.id_kho = repo._context.sys_khos.AsQueryable().Where(d => d.ma.Trim().ToLower() == model.ma_kho.Trim().ToLower()).Select(d => d.id).SingleOrDefault();
        //                    if (model.db.id_loai_nhap == "NM")
        //                    {
        //                        model.db.id_don_hang_mua = repo._context.sys_don_hang_muas.AsQueryable().Where(d => d.ma.Trim().ToLower() == model.ma_don_hang.Trim().ToLower()).Select(d => d.id).SingleOrDefault();
        //                        model.db.nguon = 2;
        //                    }
        //                    else if (model.db.id_loai_nhap == "NTRH")
        //                    {
        //                        model.db.id_don_hang_ban = repo._context.sys_don_hang_bans.AsQueryable().Where(d => d.ma.Trim().ToLower() == model.ma_don_hang.Trim().ToLower()).Select(d => d.id).SingleOrDefault();
        //                        model.db.nguon = 1;
        //                    }
        //                    if (string.IsNullOrEmpty(model.db.id_don_hang_ban) && string.IsNullOrEmpty(model.db.id_don_hang_mua))
        //                        model.db.nguon = 4;
        //                    if (model.db.id_doi_tuong == "DTTD")
        //                    {
        //                        var doi_tuong = repo._context.sys_khach_hang_nha_cung_caps.AsQueryable().Where(d => d.ma.Trim().ToLower() == model.db.id_doi_tuong.Trim().ToLower()).SingleOrDefault();
        //                        model.db.hinh_thuc_doi_tuong = 1;
        //                        model.db.ten_doi_tuong = doi_tuong.ten;
        //                        model.db.ma_so_thue = "";
        //                        model.db.dien_thoai = "";
        //                        model.db.email = "";
        //                        model.db.dia_chi_doi_tuong = "";
        //                    }
        //                    else if (model.db.id_doi_tuong != "DTTD" && model.db.id_doi_tuong != "")
        //                    {
        //                        var doi_tuong = repo._context.sys_khach_hang_nha_cung_caps.AsQueryable().Where(d => d.ma.Trim().ToLower() == model.db.id_doi_tuong.Trim().ToLower()).SingleOrDefault();
        //                        model.db.hinh_thuc_doi_tuong = doi_tuong.hinh_thuc;
        //                        model.db.ten_doi_tuong = doi_tuong.ten;
        //                        model.db.ma_so_thue = doi_tuong.ma_so_thue;
        //                        model.db.dien_thoai = doi_tuong.dien_thoai;
        //                        model.db.email = doi_tuong.email;
        //                        model.db.dia_chi_doi_tuong = doi_tuong.dia_chi;
        //                    }
        //                    model.db.nguoi_cap_nhat = getUserId();
        //                    model.db.ngay_cap_nhat = DateTime.Now;
        //                    model.db.status_del = 1;
        //                    await repo.insert_import(model);
        //                }
        //            }
        //            var db_log = new sys_lich_su_import_db();
        //            db_log.id = ObjectId.GenerateNewId().ToString();
        //            db_log.ten = file.FileName;
        //            db_log.error = error;
        //            db_log.controller = "sys_phieu_nhap_kho";
        //            db_log.ngay_cap_nhat = DateTime.Now;
        //            db_log.nguoi_cap_nhat = getUserId();
        //            await repo.add_log(db_log);

        //            if (error == "")
        //            {
        //                return Json("1");
        //            }
        //            else
        //            {
        //                var path_err = get_file_err(name, error, _appsetting.folder_path);

        //                try
        //                {
        //                    var memory = new MemoryStream();
        //                    using (var stream = new FileStream(path_err, FileMode.Open))
        //                    {
        //                        await stream.CopyToAsync(memory);
        //                    }
        //                    memory.Position = 0;

        //                    return Json(path_err);

        //                }
        //                catch (Exception ex)
        //                {
        //                    return Json("-1");
        //                }
        //            }
        //        }
        //        catch
        //        {
        //            var a = count;
        //            return Json("-1");
        //        }
        //    }
        //    else
        //    {
        //        return Json("-1");
        //    }

        //}
        //[HttpPost]
        //public async Task<IActionResult> ImportFromExcelMatHang()
        //{
        //    var error = "";
        //    IFormFile file = Request.Form.Files[0];
        //    var model_master = JsonConvert.DeserializeObject<sys_phieu_nhap_kho_model>(Request.Form["model"]);
        //    var rs = "";

        //    if (file.Length > 0)
        //    {
        //        try
        //        {

        //            var list_row = handleImportFile(file);

        //            for (int ct = 0; ct < list_row.Count(); ct++)
        //            {
        //                var fileImport = list_row[ct].list_cell.ToList();


        //                var model = new sys_phieu_nhap_kho_chi_tiet_model();

        //                var stt = (fileImport[0].value.ToString() ?? "").Trim();
        //                var ma_mat_hang = (fileImport[1].value.ToString() ?? "").Trim();
        //                var so_luong = (fileImport[2].value.ToString() ?? "").Trim();
        //                var ghi_chu = (fileImport[3].value.ToString() ?? "").Trim();

        //                model.ma_mat_hang = ma_mat_hang;
        //                model.db.so_luong = decimal.Parse(so_luong);
        //                model.db.ghi_chu = ghi_chu;


        //                //user import
        //                error = CheckErrorImportMatHang(model, ct + 1, error);
        //                if (!string.IsNullOrEmpty(error))
        //                {

        //                }
        //                else
        //                {
        //                    var mat_hang = repo._context.sys_mat_hangs.AsQueryable().Where(q => q.ma.Trim().ToLower() == model.ma_mat_hang.Trim().ToLower()).FirstOrDefault();
        //                    model.db.id_mat_hang = mat_hang.id;
        //                    model.db.id_don_vi_tinh = mat_hang.id_don_vi_tinh;
        //                    model.ten_mat_hang = mat_hang.ten;
        //                    model.ten_don_vi_tinh = repo._context.sys_don_vi_tinhs.AsQueryable().Where(q => q.id == model.db.id_don_vi_tinh).Select(q => q.ten).FirstOrDefault();
        //                    model_master.list_mat_hang.Add(model);
        //                }

        //            }
        //            var data = new
        //            {
        //                error = error,
        //                list_mat_hang = model_master.list_mat_hang
        //            };
        //            return Json(data);
        //        }
        //        catch (Exception ex)
        //        {
        //            var data = new
        //            {
        //                error = "File không đúng định dạng"

        //            };

        //            return Json(data);
        //        }


        //    }
        //    else
        //    {
        //        var data = new
        //        {
        //            error = "File không đúng định dạng"

        //        };

        //        return Json(data);

        //    }
        //}

    }
}
