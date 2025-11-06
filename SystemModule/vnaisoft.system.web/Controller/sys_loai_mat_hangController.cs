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
    public partial class sys_loai_mat_hangController : BaseAuthenticationController
    {
        private sys_loai_mat_hang_repo repo;
        public MongoClientFactory config;
        private IConfiguration _configuration;
        private IMailService _mailService;
        public AppSettings _appsetting;
        public SendNotificationApi firebaseAPI;
        private IUserService _userService;
        public sys_loai_mat_hangController(IConfiguration configuration, IUserService userService, IMailService mailService, MongoDBContext context, IOptions<AppSettings> appsetting) : base(userService)
        {
            _appsetting = appsetting.Value;
            repo = new sys_loai_mat_hang_repo(context);
            _userService = userService;
            _mailService = mailService;
            firebaseAPI = new SendNotificationApi(context);
            _configuration = configuration;
            config = new MongoClientFactory(configuration);
        }
        public async Task<IActionResult> get_code([FromBody] JObject json)
        {
            var code = repo.getCode();
            return Json(code);
        }
        public async Task<FileStreamResult> exportExcel(string search, int status_del)
        {
            search = search ?? "";
            search = search.Trim().ToLower();
            var queryTable = repo._context.sys_loai_mat_hang_col.AsQueryable().Where(d => d.status_del == status_del)
                .Where(d => d.ten.Trim().ToLower().Contains(search) || d.ma.Trim().ToLower().Contains(search) || d.ghi_chu.Trim().ToLower().Contains(search))
                 ;
            var query = repo.FindAll(queryTable);

            var dataList = query.OrderByDescending(d => d.db.ma).ToList();

            string[] header = new string[] {
                "STT (No.)","Mã Loại","Tên Loại","Ghi chú","Người cập nhật","Ngày cập nhật"
            };

            string[] listKey = new string[]
            {
               "db.ma","db.ten","db.ghi_chu","ten_nguoi_cap_nhat","db.ngay_cap_nhat"
            };
            return await exportFileExcel(_appsetting, header, listKey, dataList, "sys_loai_mat_hang");
        }
        //[HttpPost]
        //public async Task<IActionResult> ImportFromExcel()
        //{
        //    var error = "";
        //    IFormFile file = Request.Form.Files[0];

        //    var name = "LoaiMatHang";


        //    if (file.Length > 0)

        //    {


        //        try
        //        {
        //            var list_row = handleImportFileSheetName(file, name);
        //            for (int ct = 0; ct < list_row.Count(); ct++)
        //            {
        //                var fileImport = list_row[ct].list_cell.ToList();


        //                var model = new sys_loai_mat_hang_model();

        //                var stt = (fileImport[0].value.ToString() ?? "").Trim();
        //                var ma_loai_dinh_khoan_mh = (fileImport[1].value.ToString() ?? "").Trim();
        //                var ma = (fileImport[2].value.ToString() ?? "").Trim();
        //                var ten = (fileImport[3].value.ToString() ?? "").Trim();
        //                var ghi_chu = (fileImport[4].value.ToString() ?? "").Trim();

        //                model.ma_loai_dinh_khoan_mat_hang = ma_loai_dinh_khoan_mh;
        //                model.db.ma = ma;
        //                model.db.ten = ten;
        //                model.db.ghi_chu = ghi_chu;

        //                //user import
        //                error = CheckErrorImport(model, ct + 1, error);
        //                if (!string.IsNullOrEmpty(error))
        //                {

        //                }
        //                else
        //                {
        //                    var id_loai_dk_mh = repo._context.erp_loai_dinh_khoan_mat_hangs.AsQueryable().Where(d => d.ma == ma_loai_dinh_khoan_mh).Select(d => d.id).SingleOrDefault();
        //                    model.db.id_loai_dinh_khoan_mat_hang = id_loai_dk_mh;

        //                    if (string.IsNullOrEmpty(model.db.ma))
        //                    {
        //                        model.db.ma = repo.getCode();
        //                    }
        //                    model.db.id = model.db.ma;
        //                    model.db.status_del = 1;
        //                    model.db.nguoi_cap_nhat = getUserId();
        //                    model.db.ngay_cap_nhat = DateTime.Now;
        //                    await repo.insert(model);
        //                }


        //            }
        //            var db_log = new sys_lich_su_import_db();
        //            db_log.id = ObjectId.GenerateNewId().ToString();
        //            db_log.ten = file.FileName;
        //            db_log.error = error;
        //            db_log.controller = "sys_loai_mat_hang";
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
        //            return Json("-1");
        //        }
        //    }
        //    else
        //    {
        //        return Json("-1");
        //    }


        //}
        public async Task<IActionResult> getElementByMa([FromBody] JObject json)
        {
            var ma = json.GetValue("ma").ToString();
            var query = repo._context.sys_loai_mat_hang_col.AsQueryable().Where(q => q.ma == ma);
            var model = repo.FindAll(query).AsQueryable().SingleOrDefault();
            return Json(model);
        }
        [AllowAnonymous]
        public ActionResult downloadtemp()
        {
            var currentpath = Directory.GetCurrentDirectory();
            string newPath = Path.Combine(currentpath, "wwwroot", "assets", "template");
            if (!Directory.Exists(newPath))
                Directory.CreateDirectory(newPath);

            string Files = newPath + "\\sys_loai_mat_hang.xlsx";
            byte[] fileBytes = System.IO.File.ReadAllBytes(Files);
            System.IO.File.WriteAllBytes(Files, fileBytes);
            MemoryStream ms = new MemoryStream(fileBytes);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, "sys_loai_mat_hang.xlsx");
        }

        public IActionResult getListUse()
        {
            var result = repo._context.sys_loai_mat_hang_col.AsQueryable()
                .Where(d => d.status_del == 1).
                 Select(d => new
                 {
                     id = d.id,
                     name = d.ten,
                 }).ToList();
            return Json(result);
        }
        [HttpPost]
        public async Task<IActionResult> create([FromBody] JObject json)
        {

            var model = JsonConvert.DeserializeObject<sys_loai_mat_hang_model>(json.GetValue("data").ToString());
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

            await repo.insert(model);
            return Json(model);
        }

        [HttpPost]
        public async Task<IActionResult> edit([FromBody] JObject json)
        {
            var model = JsonConvert.DeserializeObject<sys_loai_mat_hang_model>(json.GetValue("data").ToString());
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
        public IActionResult getElementById([FromBody] JObject json)
        {
            var id = json.GetValue("id").ToString();
            var query = repo._context.sys_loai_mat_hang_col.AsQueryable().Where(q => q.id == id);
            var model = repo.FindAll(query).SingleOrDefault();
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

                var status_del = int.Parse(dictionary["status_del"]);
                var search = dictionary["search"].Trim().ToLower();
                var queryTable = repo._context.sys_loai_mat_hang_col.AsQueryable().Where(d => d.status_del == status_del)
                     .Where(d => d.ten.ToLower().Contains(search) || d.ma.ToLower().Contains(search) || d.ghi_chu.ToLower().Contains(search))
                     ;
                var query = repo.FindAll(queryTable);
                //.Where(d=>d.db.status_del==1)

                var count = queryTable.Count();
                var dataList = await Task.Run(() => repo.FindAll(queryTable.Skip(param.Start).Take(param.Length)).ToList());
                DTResult<sys_loai_mat_hang_model> result = new DTResult<sys_loai_mat_hang_model>
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
