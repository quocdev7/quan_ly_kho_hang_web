//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Options;
//using MongoDB.Bson;
//using MongoDB.Driver;
//using Newtonsoft.Json;
//using Newtonsoft.Json.Linq;
//using NPOI.XSSF.UserModel;
//using QRCoder;
//using System;
//using System.Collections.Generic;
//using System.Drawing;
//using System.Drawing.Imaging;
//using System.IO;
//using System.Linq;
//using System.Threading;
//using System.Threading.Tasks;
//using System.Web;
//using vnaisoft.common.BaseClass;
//using vnaisoft.common.common;
//using vnaisoft.common.Common;
//using vnaisoft.common.Helpers;
//using vnaisoft.common.Services;

//using vnaisoft.DataBase.Mongodb;
//using vnaisoft.DataBase.Mongodb.Collection.system;
//using vnaisoft.fireBase.API;
//using vnaisoft.system.data.DataAccess;
//using vnaisoft.system.data.Models;
//using worldsoft.bao_cao.data.DataAccess;
//using WS.CRM.Data.Helper;


//namespace vnaisoft.system.web.Controller
//{
//    public partial class bao_cao_nhap_khoController : BaseAuthenticationController
//    {
//        public bao_cao_nhap_kho_repo repo;

//        public AppSettings _appsetting;
//        public bao_cao_nhap_khoController(IUserService userService, MongoDBContext context, IOptions<AppSettings> appsetting) : base(userService)
//        {
//            repo = new bao_cao_nhap_kho_repo(context);
//            _appsetting = appsetting.Value;
//        }
//        public async Task<FileStreamResult> exportExcel(string search, string tu_ngay, string den_ngay, string id_kho, string id_loai_mat_hang)
//        {
//            var excel = new ExcelHelper(_appsetting);
//            var workbook = new XSSFWorkbook();
//            var file_name = "BaoCaoNhapKho";

//            search = search ?? "";
//            var tu_ngay_d = Convert.ToDateTime(tu_ngay, System.Globalization.CultureInfo.InvariantCulture);
//            var den_ngay_d = Convert.ToDateTime(den_ngay, System.Globalization.CultureInfo.InvariantCulture);
//            tu_ngay_d = new DateTime(tu_ngay_d.Year, tu_ngay_d.Month, 1, 0, 0, 0);
//            var lastDayOfMonth = DateTime.DaysInMonth(den_ngay_d.Year, den_ngay_d.Month);
//            den_ngay_d = new DateTime(den_ngay_d.Year, den_ngay_d.Month, lastDayOfMonth, 23, 59, 59);
//            workbook = repo.exportExcelRepo(workbook, search, tu_ngay_d, den_ngay_d, id_kho, id_loai_mat_hang, _appsetting, file_name);

//            var directory = Path.Combine(_appsetting.folder_path, "file_upload", "tempExport");
//            if (!Directory.Exists(directory))
//                Directory.CreateDirectory(directory);

//            var date = DateTime.Now;
//            var filePath = directory + "\\" + file_name + DateTime.Now.Ticks + ".xlsx";
//            var fileStream2 = new FileStream(filePath, FileMode.CreateNew);
//            workbook.Write(fileStream2);
//            fileStream2.Close();


//            if (!string.IsNullOrEmpty(filePath))
//            {

//                var memory = new MemoryStream();
//                using (var stream = new FileStream(filePath, FileMode.Open))
//                {
//                    await stream.CopyToAsync(memory);
//                }
//                memory.Position = 0;

//                return File(memory, excel.GetContentType(filePath), Path.GetFileName(filePath));
//            }
//            return null;

//        }
//        [HttpPost]

//        public async Task<IActionResult> DataHandler([FromBody] JObject json)
//        {
//            try
//            {
//                var a = Request;
//                var param = JsonConvert.DeserializeObject<DTParameters>(json.GetValue("param1").ToString());
//                var dictionary = new Dictionary<string, string>();
//                dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json.GetValue("data").ToString());


//                var search = dictionary["search"].Trim().ToLower();
//                var id_kho = dictionary["id_kho"];
//                var id_loai_mat_hang = dictionary["id_loai_mat_hang"];
//                var tu_ngay = dictionary["tu_ngay"].ToString();
//                var tu_ngay_d = Convert.ToDateTime(tu_ngay, System.Globalization.CultureInfo.InvariantCulture);
//                var den_ngay = dictionary["den_ngay"].ToString();
//                var den_ngay_d = Convert.ToDateTime(den_ngay, System.Globalization.CultureInfo.InvariantCulture);
//                // var id_mat_hang = dictionary["id_mat_hang"];

//                //var lst_pn = repo._context.erp_phieu_nhap_khos.AsQueryable().Where(q=>q.status_del==1).Select(q => q.id).ToList();

//                var querytable = repo._context.erp_phieu_nhap_kho_chi_tiets.AsQueryable()
//                  //.Where(q => lst_pn.Contains(q.id_phieu_nhap_kho))
//                  .Where(q => q.is_vat != true)
//                   .Where(q => q.status_del == 1)
//                   .Where(q => id_kho == "-1" || q.id_kho == id_kho)
//                   .Where(q => id_loai_mat_hang == "-1" || q.id_loai_mat_hang == id_loai_mat_hang)
//                   .Where(q => q.id_tai_san == null)
//                   .Where(d => tu_ngay_d <= d.ngay_nhap && d.ngay_nhap <= den_ngay_d)
//                   .Where(d => d.id_mat_hang.ToLower().Contains(search) || d.ten_mat_hang.ToLower().Contains(search))

//                    ;


//                var count = querytable.Count();
//                var dataList = await Task.Run(() => repo.FindAll(querytable.Skip(param.Start).Take(param.Length))
//        .ToList());
//                dataList.ForEach(q =>
//                {
//                    //var mat_hang = repo._context.erp_mat_hangs.AsQueryable().Where(d => d.id == q.id_mat_hang).FirstOrDefault();
//                    //  q.ten_mat_hang = mat_hang.ten;
//                    q.id_loai_mat_hang = repo._context.erp_mat_hangs.AsQueryable().Where(d => d.id == q.ma_mat_hang).Select(d => d.id_loai_mat_hang).FirstOrDefault();
//                    q.ma_loai_mat_hang = repo._context.erp_loai_mat_hangs.AsQueryable().Where(d => d.id == q.id_loai_mat_hang).Select(d => d.ma).SingleOrDefault();
//                    q.ten_loai_mat_hang = repo._context.erp_loai_mat_hangs.AsQueryable().Where(d => d.id == q.id_loai_mat_hang).Select(d => d.ten).SingleOrDefault();
//                    var doi_tuong = repo._context.erp_phieu_nhap_khos.AsQueryable().Where(d => d.id == q.id_phieu_nhap_kho).SingleOrDefault();

//                    q.ma_doi_tuong = doi_tuong.id_doi_tuong;
//                    q.ten_doi_tuong = doi_tuong.ten_doi_tuong;
//                    q.ma_so_thue = doi_tuong.ma_so_thue;
//                    q.dien_thoai = doi_tuong.dien_thoai;
//                    q.email = doi_tuong.email;
//                    q.dia_chi_doi_tuong = doi_tuong.dia_chi_doi_tuong;
//                });
//                DTResult<bao_cao_nhap_kho_model> result = new DTResult<bao_cao_nhap_kho_model>
//                {
//                    start = param.Start,
//                    draw = param.Draw,
//                    data = dataList,
//                    recordsFiltered = count,
//                    recordsTotal = count
//                };
//                return Json(result);
//            }

//            catch (Exception ex)
//            {
//                return Json(new { error = ex.Message });
//            }

//        }

//    }
//}
