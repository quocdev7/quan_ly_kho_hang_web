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
//using WS.CRM.Data.Helper;


//namespace vnaisoft.system.web.Controller
//{
//    public partial class bao_cao_ton_kho_mat_hangController : BaseAuthenticationController
//    {
//        public bao_cao_ton_kho_mat_hang_repo repo;

//        public AppSettings _appsetting;
//        public bao_cao_ton_kho_mat_hangController(IUserService userService, MongoDBContext context, IOptions<AppSettings> appsetting) : base(userService)
//        {
//            repo = new bao_cao_ton_kho_mat_hang_repo(context);
//            _appsetting = appsetting.Value;
//        }
//        public async Task<FileStreamResult> exportExcel(string search, string id_kho)
//        {
//            var excel = new ExcelHelper(_appsetting);
//            var workbook = new XSSFWorkbook();
//            var file_name = "XemTonKhoHienTai";

//            search = search ?? "";
//            workbook = repo.exportExcelRepo(workbook, search, id_kho, _appsetting, file_name);

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
//                var queryTable = repo._common_repo._context.erp_ton_kho_mat_hangs.AsQueryable()
//                  .Where(q => id_kho == "-1" || q.id_kho == id_kho)
//                  .Where(q => id_loai_mat_hang == "-1" || q.id_loai_mat_hang == id_loai_mat_hang)
//                    .Where(q => q.id_mat_hang.ToLower().Contains(search) || q.ten_mat_hang.ToLower().Contains(search) ||

//              search == ""
//              );



//                var count = queryTable.Count();
//                var dataList = await Task.Run(() => repo.FindAll(queryTable.OrderBy(d => d.id_kho).Skip(param.Start).Take(param.Length))
//        .ToList());
//                dataList.ForEach(q =>
//                {
//                    q.ten_don_vi_tinh = repo._context.erp_don_vi_tinhs.AsQueryable().Where(d => d.id == q.id_don_vi_tinh).Select(d => d.ten).SingleOrDefault();
//                    q.ma_loai_mat_hang = repo._context.erp_loai_mat_hangs.AsQueryable().Where(d => d.id == q.id_loai_mat_hang).Select(d => d.ma).SingleOrDefault();
//                    q.ten_loai_mat_hang = repo._context.erp_loai_mat_hangs.AsQueryable().Where(d => d.id == q.id_loai_mat_hang).Select(d => d.ten).SingleOrDefault();

//                });
//                DTResult<bao_cao_ton_kho_mat_hang_model> result = new DTResult<bao_cao_ton_kho_mat_hang_model>
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
