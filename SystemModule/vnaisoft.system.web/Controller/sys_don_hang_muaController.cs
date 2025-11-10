using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    public partial class sys_don_hang_muaController : BaseAuthenticationController
    {
        public sys_don_hang_mua_repo repo;

        public AppSettings _appsetting;

        public sys_don_hang_muaController(IUserService userService, MongoDBContext context, IOptions<AppSettings> appsetting) : base(userService)
        {
            repo = new sys_don_hang_mua_repo(context);
            _appsetting = appsetting.Value;
        }
        public async Task<IActionResult> get_code([FromBody] JObject json)
        {
            var code = repo.getCode();
            return Json(code);
        }
        //public async Task<IActionResult> check_kho()
        //{
        //    var id_kho = "";

        //    id_kho = repo._context.sys_user_col.AsQueryable().Where(q => q.id == getUserId()).Select(q => q.id_kho_nhap).SingleOrDefault();


        //    return Json(id_kho);
        //}

        //[AllowAnonymous]
        //public ActionResult downloadtempdetail()
        //{
        //    var currentpath = Directory.GetCurrentDirectory();
        //    string newPath = Path.Combine(currentpath, "wwwroot", "assets", "template");
        //    if (!Directory.Exists(newPath))
        //        Directory.CreateDirectory(newPath);

        //    string Files = newPath + "\\erp_mat_hang_theo_don_hang_mua.xlsx";
        //    byte[] fileBytes = System.IO.File.ReadAllBytes(Files);
        //    System.IO.File.WriteAllBytes(Files, fileBytes);
        //    MemoryStream ms = new MemoryStream(fileBytes);
        //    return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, "erp_mat_hang_theo_don_hang_mua.xlsx");
        //}
        //[HttpPost]
        //public async Task<IActionResult> ImportFromExcelMatHang()
        //{
        //    var error = "";
        //    IFormFile file = Request.Form.Files[0];
        //    var model_master = JsonConvert.DeserializeObject<sys_don_hang_mua_model>(Request.Form["model"]);
        //    var rs = "";

        //    if (file.Length > 0)
        //    {
        //        try
        //        {

        //            var list_row = handleImportFile(file);

        //            for (int ct = 0; ct < list_row.Count(); ct++)
        //            {
        //                var fileImport = list_row[ct].list_cell.ToList();


        //                var model = new sys_don_hang_mua_mat_hang_model();

        //                var stt = (fileImport[0].value.ToString() ?? "").Trim();
        //                var ma_mat_hang = (fileImport[1].value.ToString() ?? "").Trim();
        //                var so_luong = (fileImport[2].value.ToString() ?? "").Trim();
        //                var don_gia = (fileImport[3].value.ToString() ?? "").Trim();
        //                var chiet_khau = (fileImport[4].value.ToString() ?? "").Trim();
        //                var ghi_chu = (fileImport[5].value.ToString() ?? "").Trim();

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
        //                    var mat_hang = repo._context.erp_mat_hangs.AsQueryable().Where(q => q.ma.Trim().ToLower() == model.ma_mat_hang.Trim().ToLower()).FirstOrDefault();
        //                    model.db.id_mat_hang = mat_hang.id;
        //                    model.db.id_don_vi_tinh = mat_hang.id_don_vi_tinh;
        //                    model.db.vat = mat_hang.vat;
        //                    model.ten_mat_hang = mat_hang.ten;
        //                    model.ten_don_vi_tinh = repo._context.erp_don_vi_tinhs.AsQueryable().Where(q => q.id == model.db.id_don_vi_tinh).Select(q => q.ten).FirstOrDefault();

        //                    model.db.chiet_khau = decimal.Parse(chiet_khau) != 0 ? decimal.Parse(chiet_khau) : mat_hang.ty_le_chiet_khau;
        //                    var don_gia_tu_bgm = repo._context.erp_bang_gia_muas.AsQueryable()
        //                                                        .Where(d => d.id_nha_cung_cap == model_master.db.id_doi_tuong)
        //                                                        .Where(d => d.id_mat_hang == model.db.id_mat_hang)
        //                                                          .Where(d => d.ngay_ghi_nhan <= DateTime.Now)
        //                                                        .Where(d => d.status_del == 1).OrderByDescending(q => q.ngay_ghi_nhan).Select(d => d.don_gia).SingleOrDefault();
        //                    model.db.don_gia = don_gia_tu_bgm == null ? decimal.Parse(don_gia) : don_gia_tu_bgm;
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

        //public async Task<IActionResult> getPrint([FromBody] JObject json)
        //{
        //    var id = json.GetValue("id").ToString();
        //    var template = repo._context.erp_mau_ins.AsQueryable().Where(t => t.id_loai == "sys_don_hang_mua").FirstOrDefault();
        //    var model = await repo.getElementById(id);
        //    var commonPrint = new Common_print_helper(repo._context);
        //    var templatePrint = commonPrint.generatePrint(template.noi_dung, model, "list_mat_hang");
        //    return Json(new
        //    {
        //        tieu_de = template.ten,
        //        noi_dung = templatePrint
        //    });
        //}
        
        public IActionResult getListUse()
        {
            var result = repo._context.sys_don_hang_mua_col.AsQueryable()
                .Where(d => d.status_del == 1).
                 Select(d => new
                 {
                     id = d.id,
                     name = d.ma,
                 }).ToList();
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> create([FromBody] JObject json)
        {

            var model = JsonConvert.DeserializeObject<sys_don_hang_mua_model>(json.GetValue("data").ToString());
            var check = checkModelStateCreate(model);
            if (!check)
            {
                return generateError();
            }
            model.db.ma = repo.getCode();
            model.db.id = model.db.ma;
            model.db.status_del = 1;
            model.db.nguoi_cap_nhat = getUserId();
            model.db.nguoi_tao = getUserId();
            model.db.ngay_cap_nhat = DateTime.Now;
            model.db.ngay_tao = DateTime.Now;
            await repo.insert(model);
            return Json(model);
        }

        [HttpPost]
        public async Task<IActionResult> edit([FromBody] JObject json)
        {
            var model = JsonConvert.DeserializeObject<sys_don_hang_mua_model>(json.GetValue("data").ToString());
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
            await repo.update_status_del(id, getUserId(), status_del);
            return Json("");
        }
        //public async Task<IActionResult> getElementById([FromBody] JObject json)
        //{
        //    var id = json.GetValue("id").ToString();
        //    var model = await repo.getElementById(id);
        //    return Json(model);
        //}
        //public async Task<IActionResult> getElementByIdLog([FromBody] JObject json)
        //{
        //    var id = json.GetValue("id").ToString();
        //    var model = await repo.getElementByIdLog(id);
        //    return Json(model);
        //}
        //public async Task<IActionResult> getElementByIdXuatKho([FromBody] JObject json)
        //{
        //    var id = json.GetValue("id").ToString();
        //    var queryTable = repo._context.sys_don_hang_muas.AsQueryable().Where(q => q.id == id.Trim());
        //    var model = repo.FindAll(queryTable).AsQueryable().Where(q => q.db.id == id).SingleOrDefault();
        //    var queryTableDetail = repo._context.sys_don_hang_mua_mat_hangs.AsQueryable().Where(q => q.id_don_hang == id.Trim());
        //    model.list_mat_hang = repo.FindAllDetail(queryTableDetail).AsQueryable().Where(q => q.db.id_don_hang == id).ToList();
        //    model.id_mat_hangs = string.Join(',', model.list_mat_hang.Select(q => q.db.id_mat_hang).ToList());
        //    return Json(model);
        //}
        
        public async Task<IActionResult> get_list_don_hang_mua([FromBody] JObject json)
        {
            var list_don_hang_mua = new List<sys_don_hang_mua_model>();
            var queryTable = repo._context.sys_don_hang_mua_col.AsQueryable().Where(q => q.status_del == 1);
            list_don_hang_mua = repo.FindAll(queryTable).Where(q => q.db.status_del == 1).ToList();
            return Json(list_don_hang_mua);
        }
        //[HttpPost]
        //public async Task<IActionResult> DataHandlerLog([FromBody] JObject json)
        //{
        //    try
        //    {
        //        var a = Request;
        //        var param = JsonConvert.DeserializeObject<DTParameters>(json.GetValue("param1").ToString());
        //        var dictionary = new Dictionary<string, string>();
        //        dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json.GetValue("data").ToString());
        //        var search = dictionary["search"].Trim().ToLower();
        //        var id_don_hang = dictionary["id_don_hang"];
        //        var queryTable = repo._context.sys_don_hang_mua_logs.AsQueryable()
        //            .Where(d => d.id_don_hang == id_don_hang)
        //            ;
        //        var count = queryTable.Count();
        //        queryTable = queryTable.OrderByDescending(d => d.ma);
        //        var dataList = await Task.Run(() => repo.FindAllLog(queryTable.Skip(param.Start).Take(param.Length)).OrderByDescending(d => d.db.ngay_cap_nhat).ToList());
        //        DTResult<sys_don_hang_mua_log_model> result = new DTResult<sys_don_hang_mua_log_model>
        //        {
        //            start = param.Start,
        //            draw = param.Draw,
        //            data = dataList,
        //            recordsFiltered = count,
        //            recordsTotal = count
        //        };
        //        return Json(result);
        //    }

        //    catch (Exception ex)
        //    {
        //        return Json(new { error = ex.Message });
        //    }

        //}

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
                var id_loai_giao_dich = dictionary["id_loai_giao_dich"];
                // var id_kho = dictionary["id_kho"];
                var id_doi_tuong = dictionary["id_doi_tuong"];
                var status_del = int.Parse(dictionary["status_del"]);
                var is_nhap_du = int.Parse(dictionary["is_nhap_du"]);
                var is_chi_du = int.Parse(dictionary["is_chi_du"]);

                var tu_ngay = dictionary["tu_ngay"].ToString();
                var den_ngay = dictionary["den_ngay"].ToString();
                var tu_ngay_dt = Convert.ToDateTime(tu_ngay, System.Globalization.CultureInfo.InvariantCulture);
                var den_ngay_dt = Convert.ToDateTime(den_ngay, System.Globalization.CultureInfo.InvariantCulture);
                tu_ngay_dt = new DateTime(tu_ngay_dt.Year, tu_ngay_dt.Month, tu_ngay_dt.Day, 0, 0, 0);
                den_ngay_dt = new DateTime(den_ngay_dt.Year, den_ngay_dt.Month, den_ngay_dt.Day, 23, 59, 59);


                //var query = repo.FindAll()
                var queryTable = repo._context.sys_don_hang_mua_col.AsQueryable().Where(d => d.status_del == status_del)
                    .Where(d => tu_ngay_dt <= d.ngay_dat_hang && den_ngay_dt >= d.ngay_dat_hang)
                     .Where(d => d.ma.ToLower().Contains(search) || d.ghi_chu.ToLower().Contains(search)
                     || d.ten.ToLower().Contains(search) || d.ten_khong_dau.ToLower().Contains(search)
                     //|| (d.ma_so_thue.ToLower() ?? "").Contains(search) || (d.dien_thoai.ToLower() ?? "").Contains(search)
                     //|| (d.ten_doi_tuong.ToLower() ?? "").Contains(search) 
                     || search == "")
                     //.Where(q => q.nguoi_cap_nhat == getUserId())
                     ;

                //if (is_nhap_du == 1)
                //{
                //    queryTable = queryTable.Where(q => q.is_nhap_du == true);
                //}
                //if (is_nhap_du == 0)
                //{
                //    queryTable = queryTable.Where(q => q.is_nhap_du != true);
                //}
                //if (is_chi_du == 1)
                //{
                //    queryTable = queryTable.Where(q => q.is_chi_du == true);
                //}
                //if (is_chi_du == 0)
                //{
                //    queryTable = queryTable.Where(q => q.is_chi_du != true);
                //}
                //if (Boolean.Parse(dictionary["open"]) == true)
                //{

                //    queryTable = queryTable
                //            //.Where(d => d.status_del == status_del)
                //            .Where(d => d.loai_giao_dich == int.Parse(id_loai_giao_dich) || id_loai_giao_dich == "-1")
                //            //.Where(d => d.id_kho_nhap == id_kho || id_kho == "-1")
                //            .Where(d => d.hinh_thuc_doi_tuong == int.Parse(id_doi_tuong) || id_doi_tuong == "-1")
                //            ;
                //}

                var count = queryTable.Count();
                queryTable = queryTable.OrderByDescending(d => d.ma);
                var dataList = await Task.Run(() => repo.FindAll(queryTable.Skip(param.Start).Take(param.Length)).ToList());
                DTResult<sys_don_hang_mua_model> result = new DTResult<sys_don_hang_mua_model>
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
        //[HttpPost]
        //public async Task<IActionResult> DataHandlerDonHangMuaHH([FromBody] JObject json)
        //{
        //    try
        //    {
        //        var a = Request;
        //        var param = JsonConvert.DeserializeObject<DTParameters>(json.GetValue("param1").ToString());
        //        var dictionary = new Dictionary<string, string>();
        //        dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json.GetValue("data").ToString());

        //        var search = dictionary["search"].Trim().ToLower();
        //        var id_loai_giao_dich = dictionary["id_loai_giao_dich"];
        //        var id_kho = dictionary["id_kho"];
        //        var id_doi_tuong = dictionary["id_doi_tuong"];
        //        var nguon = dictionary["nguon"];
        //        var status_del = int.Parse(dictionary["status_del"]);
        //        var is_nhap_du = int.Parse(dictionary["is_nhap_du"]);
        //        var is_xuat_du = int.Parse(dictionary["is_xuat_du"]);
        //        var tu_ngay = dictionary["tu_ngay"].ToString();
        //        var den_ngay = dictionary["den_ngay"].ToString();
        //        var tu_ngay_dt = Convert.ToDateTime(tu_ngay, System.Globalization.CultureInfo.InvariantCulture);
        //        var den_ngay_dt = Convert.ToDateTime(den_ngay, System.Globalization.CultureInfo.InvariantCulture);
        //        tu_ngay_dt = new DateTime(tu_ngay_dt.Year, tu_ngay_dt.Month, tu_ngay_dt.Day, 0, 0, 0);
        //        den_ngay_dt = new DateTime(den_ngay_dt.Year, den_ngay_dt.Month, den_ngay_dt.Day, 23, 59, 59);

        //        //var query = repo.FindAll()
        //        var queryTable = repo._context.sys_don_hang_muas.AsQueryable().Where(d => d.status_del == status_del)
        //            .Where(d => tu_ngay_dt <= d.ngay_dat_hang && den_ngay_dt >= d.ngay_dat_hang)
        //             .Where(d => d.loai_giao_dich == int.Parse(id_loai_giao_dich))
        //             .Where(d => d.ma.ToLower().Contains(search) || d.ghi_chu.ToLower().Contains(search)
        //             || d.ten.ToLower().Contains(search) || d.ten_khong_dau.ToLower().Contains(search)
        //             || (d.ma_so_thue.ToLower() ?? "").Contains(search) || (d.dien_thoai.ToLower() ?? "").Contains(search)
        //             || (d.ten_doi_tuong.ToLower() ?? "").Contains(search) || search == "")
        //             //.Where(q => q.nguoi_cap_nhat == getUserId())
        //             ;
        //        //2pn: chọn đơn hàng mua phiếu nhập, 2px: chọn đơn hàng mua phiếu xuất
        //        if (nguon == "2pn")
        //        {
        //            if (is_nhap_du == 0)
        //            {
        //                queryTable = queryTable.Where(d => d.is_nhap_du != true);
        //            }
        //            if (is_nhap_du == 1)
        //            {
        //                queryTable = queryTable.Where(d => d.is_nhap_du == true);
        //            }
        //        }
        //        else if (nguon == "2px")
        //        {
        //            queryTable = queryTable;
        //        }

        //        if (Boolean.Parse(dictionary["open"]) == true)
        //        {

        //            queryTable = queryTable
        //                    //.Where(d => d.status_del == status_del)
        //                    .Where(d => d.id_kho_nhap == id_kho || id_kho == "-1")
        //                    .Where(d => d.hinh_thuc_doi_tuong == int.Parse(id_doi_tuong) || id_doi_tuong == "-1")
        //                    ;
        //        }

        //        var count = queryTable.Count();
        //        queryTable = queryTable.OrderByDescending(d => d.ma);
        //        var dataList = await Task.Run(() => repo.FindAll(queryTable.Skip(param.Start).Take(param.Length)).ToList());
        //        DTResult<sys_don_hang_mua_model> result = new DTResult<sys_don_hang_mua_model>
        //        {
        //            start = param.Start,
        //            draw = param.Draw,
        //            data = dataList,
        //            recordsFiltered = count,
        //            recordsTotal = count
        //        };
        //        return Json(result);
        //    }

        //    catch (Exception ex)
        //    {
        //        return Json(new { error = ex.Message });
        //    }

        //}
        //public async Task<FileStreamResult> exportExcel(string search, int? status_del, bool? open, int? id_loai_giao_dich, int? id_doi_tuong, DateTime tu_ngay, DateTime den_ngay)
        //{
        //    search = search ?? "";
        //    search = search.Trim().ToLower();


        //    var queryTable = repo._context.sys_don_hang_muas.AsQueryable().Where(d => d.status_del == status_del)
        //             .Where(d => d.ma.ToLower().Contains(search) || d.ghi_chu.ToLower().Contains(search)
        //             || d.ten.ToLower().Contains(search) || d.ten_khong_dau.ToLower().Contains(search)
        //             || (d.ma_so_thue.ToLower() ?? "").Contains(search) || (d.dien_thoai.ToLower() ?? "").Contains(search)
        //             || (d.ten_doi_tuong.ToLower() ?? "").Contains(search) || search == "")
        //             //.Where(q => q.nguoi_cap_nhat == getUserId())
        //             ;
        //    var tu_ngay_t = Convert.ToDateTime(tu_ngay, System.Globalization.CultureInfo.InvariantCulture);
        //    var den_ngay_t = Convert.ToDateTime(den_ngay, System.Globalization.CultureInfo.InvariantCulture);
        //    tu_ngay_t = new DateTime(tu_ngay_t.Year, tu_ngay_t.Month, tu_ngay_t.Day, 0, 0, 0);
        //    den_ngay_t = new DateTime(den_ngay_t.Year, den_ngay_t.Month, den_ngay_t.Day, 23, 59, 59);

        //    queryTable = queryTable
        //            .Where(d => tu_ngay_t <= d.ngay_dat_hang && den_ngay_t >= d.ngay_dat_hang)
        //            .Where(d => d.loai_giao_dich == id_loai_giao_dich || id_loai_giao_dich == -1)
        //            .Where(d => d.hinh_thuc_doi_tuong == id_doi_tuong || id_doi_tuong == -1)
        //            ;
        //    var query = repo.FindAll(queryTable).ToList();
        //    var dataList = query.OrderByDescending(d => d.db.ma).ToList();
        //    dataList.ForEach(t =>
        //    {
        //        t.ngay_cap_nhap_str = t.db.ngay_cap_nhat.Value.ToString("dd/MM/yyyy");

        //    });
        //    string[] header = new string[] {
        //        "STT (No.)","Mã đơn hàng","Tên đơn hàng","Loại giao dịch","Hình thức","Mã số thuế","Điện thoại","Tên đối tượng","Email","Địa chỉ","Tổng tiền sau thuế","Người cập nhật","Ngày cập nhật"
        //    };

        //    string[] listKey = new string[]
        //    {
        //       "db.ma","db.ten","loai_giao_dich_str","hinh_thuc_doi_tuong_str","StrExcel_db.ma_so_thue","StrExcel_db.dien_thoai","db.ten_doi_tuong","db.email","db.dia_chi_doi_tuong","Num_db.tong_tien_sau_thue","ten_nguoi_cap_nhat","ngay_cap_nhap_str"
        //    };

        //    return await exportFileExcel(_appsetting, header, listKey, dataList, "sys_don_hang_mua");
        //}
        //public async Task<FileStreamResult> exportExcelDetails()
        //{
        //    var queryTable = repo._context.sys_don_hang_mua_mat_hangs.AsQueryable()
        //             //.Where(d => d.ma.ToLower().Contains(search) || d.ghi_chu.ToLower().Contains(search)
        //             //|| d.ten.ToLower().Contains(search) || d.ten_khong_dau.ToLower().Contains(search)
        //             //|| (d.ma_so_thue.ToLower() ?? "").Contains(search) || (d.dien_thoai.ToLower() ?? "").Contains(search)
        //             //|| (d.ten_doi_tuong.ToLower() ?? "").Contains(search) || search == "")
        //             //.Where(q => q.nguoi_cap_nhat == getUserId())
        //             ;


        //    queryTable = queryTable
        //            ;
        //    var query = repo.FindAllDetail(queryTable).ToList();
        //    var dataList = query.OrderByDescending(d => d.db.ngay_cap_nhat).ToList();
        //    dataList.ForEach(t =>
        //    {
        //        t.ten_nguoi_cap_nhat = repo._context.Users.AsQueryable().Where(d => d.status_del == 1).Where(d => d.Id == t.db.nguoi_cap_nhat).Select(d => d.full_name).SingleOrDefault();
        //        t.ngay_cap_nhap_str = t.db.ngay_cap_nhat.Value.ToString("dd/MM/yyyy");
        //        t.trang_thai_str = t.db.status_del == 1 ? "Sử dụng" : "Hủy";
        //    });
        //    string[] header = new string[] {
        //        "STT (No.)","Mã đơn hàng","Ngày mua hàng","Mã mặt hàng","Tên mặt hàng","Số lượng","Đơn vị tính","Đơn giá","Chiết khấu","Thành tiền sau chiết khấu","VAT (%)","Tiền thuế","Thành tiền sau thuế","Ghi chú","Trạng thái","Người cập nhật","Ngày cập nhật"
        //    };

        //    string[] listKey = new string[]
        //    {
        //       "db.id_don_hang","db.ngay_mua_hang","ma_mat_hang","ten_mat_hang","Num_db.so_luong","ten_don_vi_tinh","Num_db.don_gia","Num_db.chiet_khau","Num_db.thanh_tien_chiet_khau","Num_db.vat","Num_db.tien_vat","Num_db.thanh_tien_sau_thue","db.ghi_chu","trang_thai_str","ten_nguoi_cap_nhat","ngay_cap_nhap_str"
        //    };

        //    return await exportFileExcel(_appsetting, header, listKey, dataList, "sys_don_hang_mua_chi_tiet");
        //}
        //[HttpPost]
        //public async Task<IActionResult> ImportFromExcel()
        //{
        //    var error = "";
        //    IFormFile file = Request.Form.Files[0];
        //    var name = "DonHangMua";

        //    if (file.Length > 0)
        //    {
        //        try
        //        {
        //            var list_row = handleImportFileSheetName(file, name);
        //            List<sys_don_hang_mua_model> list_import = new List<sys_don_hang_mua_model>();
        //            for (int ct = 0; ct < list_row.Count(); ct++)
        //            {
        //                var fileImport = list_row[ct].list_cell.ToList();

        //                var model = new sys_don_hang_mua_model();

        //                var stt = (fileImport[0].value.ToString() ?? "").Trim();
        //                var ma_don_hang = (fileImport[1].value.ToString() ?? "").Trim();
        //                var ma_mat_hang = (fileImport[2].value.ToString() ?? "").Trim();
        //                var so_luong = (fileImport[3].value.ToString() ?? "").Trim();
        //                var don_gia = (fileImport[4].value.ToString() ?? "").Trim();
        //                var chiet_khau = (fileImport[5].value.ToString() ?? "").Trim();
        //                var ghi_chu_chi_tiet = (fileImport[6].value.ToString() ?? "").Trim();
        //                var hang_hoa = (fileImport[7].value.ToString() ?? "").Trim();
        //                var ma_doi_tuong = (fileImport[8].value.ToString() ?? "").Trim();
        //                var phuong_thuc_thanh_toan = (fileImport[9].value.ToString() ?? "").Trim();
        //                var so_tai_khoan_ngan_hang_chuyen = (fileImport[10].value.ToString() ?? "").Trim();

        //                var ngay_dat_hang = (fileImport[11].value.ToString() ?? "").Trim();
        //                var so_ngay_du_kien_giao = (fileImport[12].value.ToString() ?? "").Trim();
        //                var chi_phi_van_chuyen = (fileImport[13].value.ToString() ?? "").Trim();
        //                var thue_van_chuyen = (fileImport[14].value.ToString() ?? "").Trim();
        //                var is_da_chi = (fileImport[15].value.ToString() ?? "").Trim();
        //                var is_da_nhap = (fileImport[16].value.ToString() ?? "").Trim();
        //                var ghi_chu = (fileImport[17].value.ToString() ?? "").Trim();
        //                var vat = (fileImport[18].value.ToString() ?? "").Trim();

        //                model.vat = vat;
        //                model.db.ma = ma_don_hang;
        //                model.ma_mat_hang = ma_mat_hang;
        //                model.so_luong = decimal.Parse(so_luong);
        //                model.don_gia = decimal.Parse(don_gia);
        //                if (chiet_khau == "")
        //                {
        //                    model.chiet_khau = null;
        //                }
        //                else
        //                {
        //                    model.chiet_khau = decimal.Parse(chiet_khau);
        //                }
        //                model.ghi_chu_chi_tiet = ghi_chu_chi_tiet;
        //                model.db.loai_giao_dich = int.Parse(hang_hoa);
        //                model.db.phuong_thuc_thanh_toan = int.Parse(phuong_thuc_thanh_toan);
        //                model.db.so_tai_khoan = so_tai_khoan_ngan_hang_chuyen;
        //                model.ma_doi_tuong = ma_doi_tuong;
        //                if (model.ma_doi_tuong == null || model.ma_doi_tuong == "")
        //                {
        //                    model.db.id_doi_tuong = "DTTD";
        //                }
        //                else
        //                {
        //                    model.db.id_doi_tuong = ma_doi_tuong;

        //                }
        //                if (model.db.phuong_thuc_thanh_toan == 2)
        //                {
        //                    var ngan_hang_chuyen = repo._context.erp_tai_khoan_ngan_hangs.AsQueryable().
        //                  Where(q => q.so_tai_khoan == so_tai_khoan_ngan_hang_chuyen).Select(q => new
        //                  {
        //                      id = q.id,
        //                      so_tai_khoan = q.so_tai_khoan,
        //                  }).SingleOrDefault();
        //                    if (ngan_hang_chuyen != null)
        //                    {
        //                        model.db.id_tai_khoan_ngan_hang = ngan_hang_chuyen.id;
        //                        model.db.so_tai_khoan = ngan_hang_chuyen.so_tai_khoan;
        //                    }
        //                }
        //                if (ngay_dat_hang == "")
        //                {
        //                    model.db.ngay_dat_hang = null;
        //                }
        //                else
        //                {
        //                    model.db.ngay_dat_hang = DateTime.Parse(ngay_dat_hang);
        //                }
        //                model.db.so_ngay_du_kien = int.Parse(so_ngay_du_kien_giao);
        //                model.db.vat_van_chuyen = thue_van_chuyen;
        //                model.db.tien_vat_van_chuyen = (decimal.Parse(chi_phi_van_chuyen) * int.Parse(thue_van_chuyen)) / 100;
        //                model.db.tien_van_chuyen = decimal.Parse(chi_phi_van_chuyen);
        //                model.db.ghi_chu = ghi_chu;
        //                if (is_da_chi == "")
        //                {
        //                    model.db.is_chi_du = null;
        //                }
        //                else
        //                {
        //                    model.db.is_chi_du = int.Parse(is_da_chi) == 1 ? true : false;

        //                }
        //                if (is_da_nhap == "")
        //                {
        //                    model.db.is_nhap_du = null;
        //                }
        //                else
        //                {
        //                    model.db.is_nhap_du = int.Parse(is_da_nhap) == 1 ? true : false;

        //                }
        //                //user import
        //                error = CheckErrorImport(model, ct + 1, error);
        //                list_import.Add(model);
        //            }

        //            var lst_ma = list_import.Select(q => q.db.ma).Distinct().ToList();



        //            var db_log = new sys_lich_su_import_db();
        //            db_log.id = ObjectId.GenerateNewId().ToString();
        //            db_log.ten = file.FileName;
        //            db_log.error = error;
        //            db_log.controller = "sys_don_hang_mua";
        //            db_log.ngay_cap_nhat = DateTime.Now;
        //            db_log.nguoi_cap_nhat = getUserId();
        //            await repo.add_log(db_log);



        //            if (string.IsNullOrEmpty(error))
        //            {
        //                foreach (var item in lst_ma)
        //                {
        //                    var dh = list_import.Where(q => q.db.ma == item).FirstOrDefault();

        //                    dh.list_mat_hang = list_import.Where(q => q.db.ma == item).Select(d => new sys_don_hang_mua_mat_hang_model
        //                    {
        //                        id_mat_hang = d.ma_mat_hang,
        //                        id_don_vi_tinh = repo._context.erp_mat_hangs.AsQueryable().Where(q => q.ma == d.ma_mat_hang).Select(q => q.id_don_vi_tinh).SingleOrDefault(),
        //                        so_luong = d.so_luong,
        //                        don_gia = d.don_gia,
        //                        chiet_khau = (d.chiet_khau == null ? repo._context.erp_mat_hangs.AsQueryable().Where(q => q.ma == d.ma_mat_hang).Select(q => q.ty_le_chiet_khau).SingleOrDefault() : d.chiet_khau) ?? 0,
        //                        //chiet_khau = d.chiet_khau,
        //                        ghi_chu_chi_tiet = d.ghi_chu_chi_tiet,
        //                        vat = d.vat == "" ? repo._context.erp_mat_hangs.AsQueryable().Where(q => q.ma == d.ma_mat_hang).Select(q => q.vat).SingleOrDefault() : d.vat,
        //                    }).ToList();

        //                    dh.list_mat_hang.ForEach(q =>
        //                    {
        //                        var thanh_tien_truoc_thue = (q.so_luong ?? 0) * (q.don_gia ?? 0);
        //                        var ty_le_chiet_khau = (q.chiet_khau / 100);
        //                        //decimal vat = int.Parse(q.vat);
        //                        var vat = Constant.list_vat.Where(d => d.id.ToLower().Trim() == q.vat.Trim().ToLower()).Select(d => d.value).SingleOrDefault();
        //                        decimal? vat_mat_hang = vat / 100;
        //                        q.thanh_tien_truoc_thue = thanh_tien_truoc_thue;
        //                        q.thanh_tien_chiet_khau = thanh_tien_truoc_thue - (thanh_tien_truoc_thue * ty_le_chiet_khau);
        //                        q.tien_vat = q.thanh_tien_chiet_khau * vat_mat_hang;
        //                        q.thanh_tien_sau_thue = q.thanh_tien_chiet_khau + q.tien_vat;
        //                        q.db.thanh_tien_truoc_thue = thanh_tien_truoc_thue;
        //                        q.db.thanh_tien_chiet_khau = thanh_tien_truoc_thue - (thanh_tien_truoc_thue * ty_le_chiet_khau);
        //                        q.db.tien_vat = q.thanh_tien_chiet_khau * vat_mat_hang;
        //                        q.db.thanh_tien_sau_thue = q.thanh_tien_chiet_khau + q.tien_vat;
        //                        var mat_hang = repo._context.erp_mat_hangs.AsQueryable().Where(d => d.id == q.id_mat_hang).SingleOrDefault();
        //                        q.db.id_mat_hang = mat_hang.id;
        //                        q.db.id_loai_mat_hang = mat_hang.id_loai_mat_hang;
        //                        q.db.id_don_vi_tinh = mat_hang.id_don_vi_tinh;
        //                        q.db.ghi_chu = q.ghi_chu_chi_tiet;
        //                        q.db.so_luong = q.so_luong;
        //                        q.db.don_gia = q.don_gia;
        //                        q.db.vat = q.vat;
        //                    });

        //                    dh.db.id = dh.db.ma;
        //                    if (dh.db.id_doi_tuong == "DTTD" || dh.db.id_doi_tuong == null)
        //                    {
        //                        dh.db.hinh_thuc_doi_tuong = 1;
        //                        dh.db.ten_doi_tuong = "Đối tượng tự do";
        //                        dh.db.ma_so_thue = "";
        //                        dh.db.dien_thoai = "";
        //                        dh.db.email = "";
        //                        dh.db.dia_chi_doi_tuong = "";
        //                    }
        //                    else
        //                    {
        //                        var doi_tuong = repo._context.erp_khach_hang_nha_cung_caps.AsQueryable().Where(d => d.id.Trim().ToLower() == dh.db.id_doi_tuong.Trim().ToLower()).SingleOrDefault();
        //                        dh.db.hinh_thuc_doi_tuong = 2;
        //                        dh.db.ten_doi_tuong = doi_tuong.ten;
        //                        dh.db.ma_so_thue = doi_tuong.ma_so_thue;
        //                        dh.db.dien_thoai = doi_tuong.dien_thoai;
        //                        dh.db.email = doi_tuong.email;
        //                        dh.db.dia_chi_doi_tuong = doi_tuong.dia_chi;
        //                    }
        //                    dh.db.ngay_du_kien_nhan_hang = dh.db.ngay_dat_hang.Value.AddDays((double)(dh.db.so_ngay_du_kien));
        //                    dh.db.id_kho_nhap = repo._context.Users.AsQueryable().Where(d => d.Id == getUserId()).Select(d => d.id_kho_nhap).SingleOrDefault();
        //                    dh.db.nguoi_cap_nhat = getUserId();
        //                    dh.db.ngay_cap_nhat = DateTime.Now;
        //                    dh.db.status_del = 1;
        //                    await repo.insert_import(dh);
        //                }
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
        //        catch (Exception ex)
        //        {
        //            return Json("-1");
        //        }
        //    }
        //    else
        //    {
        //        return Json("-1");
        //    }

        //}
        
        //public void add_log(sys_lich_su_import_db db)
        //{
        //    repo._context.sys_lich_su_imports.AddAsync(db);
        //    repo._context.SaveChanges();
        //}


        //[HttpPost]
        //public async Task<IActionResult> ImportFromExcelDetails()
        //{
        //    var error = "";
        //    IFormFile file = Request.Form.Files[0];
        //    var name = "DonHangMua_ChiTiet";

        //    if (file.Length > 0)
        //    {
        //        try
        //        {
        //            var list = new List<sys_don_hang_mua_mat_hang_model>();
        //            var model = new sys_don_hang_mua_model();
        //            var list_row = handleImportFileSheetName(file, name);
        //            for (int ct = 0; ct < list_row.Count(); ct++)
        //            {
        //                var fileImport = list_row[ct].list_cell.ToList();

        //                var model_detail = new sys_don_hang_mua_mat_hang_model();
        //                var stt = (fileImport[0].value.ToString() ?? "").Trim();
        //                var ma_don_hang = (fileImport[1].value.ToString() ?? "").Trim();
        //                var ma_mat_hang = (fileImport[2].value.ToString() ?? "").Trim();
        //                var so_luong = (fileImport[3].value.ToString() ?? "").Trim();
        //                var don_gia = (fileImport[4].value.ToString() ?? "").Trim();
        //                var ghi_chu = (fileImport[5].value.ToString() ?? "").Trim();

        //                model_detail.db.id_mat_hang = ma_mat_hang;
        //                model_detail.db.id_don_hang = ma_don_hang;
        //                model_detail.db.so_luong = decimal.Parse(so_luong);
        //                model_detail.db.don_gia = decimal.Parse(don_gia);
        //                model_detail.db.ghi_chu = ghi_chu;
        //                model_detail.ma_mat_hang = ma_mat_hang;
        //                list.Add(model_detail);


        //                //user import
        //                error = CheckErrorImportDetail(model_detail, ct + 1, error);
        //            }
        //            if (!string.IsNullOrEmpty(error))
        //            {

        //            }
        //            else
        //            {

        //                var list_group = list.GroupBy(g => new { ma_phieu_chi = g.db.id_phieu_chi }).Select(q => new { ma_phieu_chi = q.Key.ma_phieu_chi }).ToList();
        //                for (int i = 0; i < list_group.Count(); i++)
        //                {
        //                    var id_phieu_chi = list_group[i].ma_phieu_chi;
        //                    var query = repo._context.erp_phieu_chis.AsQueryable().Where(d => d.status_del == 1);
        //                    model = repo.FindAll(query).Where(d => d.db.id == id_phieu_chi).SingleOrDefault();
        //                    model.list_phieu_chi_chi_tiet = list.Where(d => d.db.id_phieu_chi == id_phieu_chi).ToList();
        //                    await repo.import_detail(model);
        //                }

        //            }
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
        //            return Json("-1");
        //        }
        //    }
        //    else
        //    {
        //        return Json("-1");
        //    }

        //}


        [AllowAnonymous]
        public ActionResult downloadtemp()
        {
            var currentpath = Directory.GetCurrentDirectory();
            string newPath = Path.Combine(currentpath, "wwwroot", "assets", "template");
            if (!Directory.Exists(newPath))
                Directory.CreateDirectory(newPath);

            string Files = newPath + "\\sys_don_hang_mua.xlsx";
            byte[] fileBytes = System.IO.File.ReadAllBytes(Files);
            System.IO.File.WriteAllBytes(Files, fileBytes);
            MemoryStream ms = new MemoryStream(fileBytes);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, "sys_don_hang_mua.xlsx");
        }




    }
}
