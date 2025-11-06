using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
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
    public partial class sys_cau_hinh_anh_mac_dinhController : BaseAuthenticationController
    {
        private sys_cau_hinh_anh_mac_dinh_repo repo;
        public MongoClientFactory config;
        private IConfiguration _configuration;
        private IMailService _mailService;
        public AppSettings _appsetting;
        public SendNotificationApi firebaseAPI;
        private IUserService _userService;
        public sys_cau_hinh_anh_mac_dinhController(IConfiguration configuration, IUserService userService, IMailService mailService, MongoDBContext context, IOptions<AppSettings> appsetting) : base(userService)
        {
            _appsetting = appsetting.Value;
            repo = new sys_cau_hinh_anh_mac_dinh_repo(context);
            _userService = userService;
            _mailService = mailService;
            firebaseAPI = new SendNotificationApi(context);
            _configuration = configuration;
            config = new MongoClientFactory(configuration);
        }
        public void generate_qr_code(sys_cau_hinh_anh_mac_dinh_model model)

        {
            try
            {

                //D:\\mr_agri2022\\worldsoft.zapp 
                var currentpath = _appsetting.folder_path;//Directory.GetCurrentDirectory();
                var path_logo = Path.Combine(currentpath, "wwwroot", "assets", "images", "logos");
                //var path_logo = Path.Combine(currentpath, "file_upload", "qr");
                if (!Directory.Exists(path_logo))
                    Directory.CreateDirectory(path_logo);
                var file_logo = Path.Combine(path_logo, "qrcode.png");

                QRCodeGenerator qr_generator = new QRCodeGenerator();

                //string link = "https://localhost:44324/mragri_qr/" + model.db.id_htx + "/" + model.db.id_mua_vu + "/" + model.db.id;

                //var id = ObjectId.GenerateNewId().ToString();
                string link = "https://schooltest.vnaisoft.com/";
                //_appsetting.domain;
                QRCodeData qr_code_info = qr_generator.CreateQrCode(link, QRCodeGenerator.ECCLevel.Q);

                var image = System.Drawing.Image.FromFile(file_logo);

                var qr_code = new QRCode(qr_code_info);
                //Bitmap qr_bit_map_mua_vu = qr_code_mua_vu.GetGraphic(10);
                //"C:\\myimage.png"
                Bitmap qr_bit_map = qr_code.GetGraphic(20, Color.Black, Color.White, (Bitmap)Bitmap.FromFile(file_logo));

                byte[] bit_map_array = BitmapToByteArray(qr_bit_map);
                string qr_uri = string.Format("data:image/png;base64,{0}", Convert.ToBase64String(bit_map_array));
                var qr_mua_vu = save_img_qr(bit_map_array, model.db.id);
                //sync_logo_qr(model);

            }
            catch (Exception e)
            {

            }

        }
        public byte[] BitmapToByteArray(Bitmap bitmap)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                bitmap.Save(ms, ImageFormat.Png);
                return ms.ToArray();
            }
        }

        public string save_img_qr(byte[] bit_map, string id)
        {
            using (MemoryStream ms = new MemoryStream(bit_map))
            {
                var id_file_upload = DateTime.Now.Ticks.ToString();
                var img = Image.FromStream(ms);
                var currentpath = _appsetting.folder_path;
                //var currentpath = Directory.GetCurrentDirectory();
                var host = "https://localhost:44399/";
                if (host.Contains("localhost"))
                {
                    host = "localhost";
                }
                var path = Path.Combine(currentpath, "file_upload", host, "qr");
                Thread.Sleep(1);
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                System.IO.DirectoryInfo di = new DirectoryInfo(path);
                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }
                var pathsave = Path.Combine(path, id_file_upload + ".png");
                img.Save(pathsave, ImageFormat.Png);


                var file_path = "/FileManager/Download/?filename=" + HttpUtility.UrlEncode(pathsave.Replace(Path.Combine(currentpath, "file_upload", host), ""));
                ////var path = Path.Combine(currentpath, "file_upload", "qr", id);
                ////Thread.Sleep(1);
                ////if (!Directory.Exists(path))
                ////    Directory.CreateDirectory(path);
                ////System.IO.DirectoryInfo di = new DirectoryInfo(path);
                ////foreach (FileInfo file in di.GetFiles())
                ////{
                ////    file.Delete();
                ////}
                //var pathsave = Path.Combine(path, "qr.png");
                //img.Save(pathsave, ImageFormat.Png);
                //var file_path = "/FileManager/Download/?filename=" + HttpUtility.UrlEncode(pathsave.Replace(Path.Combine(currentpath, "file_upload"), ""));

                string decodedFilePath = HttpUtility.UrlDecode(file_path);
                string fullFilePath = Path.Combine(currentpath, "file_upload", Path.GetFileName(decodedFilePath));
                long fileSize = 0;

                if (System.IO.File.Exists(fullFilePath))
                {
                    FileInfo fileInfo = new FileInfo(fullFilePath);
                    fileSize = fileInfo.Length;
                }
                var model = new sys_file_upload_col();

                model.id = id_file_upload;
                model.id_phieu = "";
                model.ma_cong_viec = "";
                model.file_name = Path.GetFileName(decodedFilePath);
                model.file_path = file_path;
                model.file_size = fileSize;
                model.extension_file = Path.GetExtension(decodedFilePath);
                model.ngay_tao = DateTime.Now;
                model.nguoi_tao = getUserId();
                model.ngay_cap_nhat = DateTime.Now;
                model.nguoi_cap_nhat = getUserId();
                model.status_del = 1;


                repo._context.sys_file_upload_col.InsertOne(model);

                var update = Builders<sys_cau_hinh_anh_mac_dinh_col>.Update
                      .Set(x => x.image, model.id)
                      ;
                var filter = Builders<sys_cau_hinh_anh_mac_dinh_col>.Filter.Eq(q => q.id, id);
                repo._context.sys_cau_hinh_anh_mac_dinh_col.UpdateOneAsync(filter, update);
                return file_path;
            }
        }
        public IActionResult getQRCode()
        {
            var result = repo._context.sys_cau_hinh_anh_mac_dinh_col
                .AsQueryable().Where(d => d.status_del == 1 && d.type == 8)
                 .Select(d => new sys_cau_hinh_anh_mac_dinh_model
                 {
                     db = d,
                 }).ToList();
            result.ForEach(q =>
            {
                q.image = repo._context.sys_file_upload_col.AsQueryable().Where(d => d.id == q.db.image).Select(d => d.file_path).SingleOrDefault();
            });
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> create([FromBody] JObject json)
        {

            var model = JsonConvert.DeserializeObject<sys_cau_hinh_anh_mac_dinh_model>(json.GetValue("data").ToString());
            var check = checkModelStateCreate(model);
            if (!check)
            {
                return generateError();
            }
            //id sinh view
            //model.db.id = ObjectId.GenerateNewId().ToString();
            model.db.nguoi_cap_nhat = getUserId();
            model.db.nguoi_tao = getUserId();
            model.db.ngay_cap_nhat = DateTime.Now;
            model.db.ngay_tao = DateTime.Now;
            model.db.status_del = 1;

            model.db.image = model.file.id;
            model.db.avatar = model.file_avartar.id;
            await repo.insert(model);
            if (model.db.type == 8)
            {
                generate_qr_code(model);
            }
            return Json(model);
        }



        [HttpPost]
        public async Task<IActionResult> edit([FromBody] JObject json)
        {
            var model = JsonConvert.DeserializeObject<sys_cau_hinh_anh_mac_dinh_model>(json.GetValue("data").ToString());
            var check = checkModelStateEdit(model);
            if (!check)
            {
                return generateError();
            }
            model.db.nguoi_cap_nhat = getUserId();
            model.db.ngay_cap_nhat = DateTime.Now;
            model.db.image = model.file.id;
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

        public IActionResult getLogo()
        {
            var result = repo._context.sys_cau_hinh_anh_mac_dinh_col
                .AsQueryable().Where(d => d.status_del == 1)
                .Where(q => q.type == 2)
                 .Select(d => new
                 {
                     id = d.id,
                     image = d.image,
                     type = d.type,
                     avatar = d.avatar
                 }).SingleOrDefault();
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

                var type = int.Parse(dictionary["type"].Trim().ToLower());
                var status_del = int.Parse(dictionary["status_del"]);
                var query = repo.FindAll()
                                    .Where(d => d.db.type == type || type == -1)
                                    .Where(d => d.db.status_del == status_del)
                                    .ToList()
                                    ;

                var count = query.Count();
                var dataList = await Task.Run(() => query.OrderByDescending(d => d.db.ngay_cap_nhat).Skip(param.Start).Take(param.Length)
      .ToList());
                dataList.ForEach(q =>
                {
                    q.nguoi_cap_nhat = repo._context.sys_user_col.AsQueryable().Where(d => d.id == q.db.nguoi_cap_nhat).Select(d => d.ho_va_ten).SingleOrDefault();
                    q.image = repo._context.sys_file_upload_col.AsQueryable().Where(d => d.id == q.db.image).Select(d => d.file_path).SingleOrDefault();
                    q.avatar = repo._context.sys_file_upload_col.AsQueryable().Where(d => d.id == q.db.avatar).Select(d => d.file_path).SingleOrDefault();
                });
                DTResult<sys_cau_hinh_anh_mac_dinh_model> result = new DTResult<sys_cau_hinh_anh_mac_dinh_model>
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
