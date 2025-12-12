using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
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
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace quan_ly_kho.system.web.Controller
{
    public partial class sys_loai_nhap_xuatController : BaseAuthenticationController
    {
        public sys_loai_nhap_xuat_repo repo;

        public AppSettings _appsetting;
        public sys_loai_nhap_xuatController(IUserService userService, MongoDBContext context, IOptions<AppSettings> appsetting) : base(userService)
        {
            repo = new sys_loai_nhap_xuat_repo(context);
            _appsetting = appsetting.Value;
        }
        public async Task<FileStreamResult> exportExcel(string search, int status_del, int type)
        {
            search = search ?? "";
            search = search.Trim().ToLower();
            var query = repo.FindAll()
                .Where(d => d.db.status_del == status_del)
                      .Where(d => d.db.loai == type || type == -1)
                .Where(d => d.db.ten.Trim().ToLower().Contains(search) || d.db.ghi_chu.Trim().ToLower().Contains(search)).ToList()
                 ;
            var dataList = query.OrderByDescending(d => d.db.ma).ToList();
            dataList.ForEach(t =>
            {
                t.ngay_cap_nhat_str = t.db.ngay_cap_nhat.Value.ToString("dd/MM/yyyy");
            });
            string[] header = new string[] {
                "STT (No.)","Loại","Mã","Tên","Ghi chú","Người cập nhật","Ngày cập nhật"
            };

            string[] listKey = new string[]
            {
                "ten_loai","db.ma","db.ten","db.ghi_chu","ten_nguoi_cap_nhat","ngay_cap_nhat_str"
            };
            return await exportFileExcel(_appsetting, header, listKey, dataList, "sys_loai_nhap_xuat");
        }
        [HttpPost]
        public async Task<IActionResult> ImportFromExcel()
        {
            var error = "";
            IFormFile file = Request.Form.Files[0];

            var name = "LoaiNhapXuat";


            if (file.Length > 0)

            {


                try
                {
                    var list_row = handleImportFileSheetName(file, name);
                    for (int ct = 0; ct < list_row.Count(); ct++)
                    {
                        var fileImport = list_row[ct].list_cell.ToList();


                        var model = new sys_loai_nhap_xuat_model();

                        var stt = (fileImport[0].value.ToString() ?? "").Trim();
                        var loai = (fileImport[1].value.ToString() ?? "").Trim();
                        var nguon = (fileImport[2].value.ToString() ?? "").Trim();
                        var ma = (fileImport[3].value.ToString() ?? "").Trim();
                        var ten = (fileImport[4].value.ToString() ?? "").Trim();
                        var ghi_chu = (fileImport[5].value.ToString() ?? "").Trim();

                        model.db.nguon = nguon;
                        model.db.ten = ten;
                        model.db.ma = ma;
                        model.db.ghi_chu = ghi_chu;
                        if ((loai ?? "").ToLower().Trim() == "Nhập".ToLower().Trim())
                        {
                            model.db.loai = 1;
                        }
                        else
                         if ((loai ?? "").ToLower().Trim() == "Xuất".ToLower().Trim())
                        {
                            model.db.loai = 2;
                        }
                        //user import
                        error = CheckErrorImport(model, ct + 1, error);
                        if (!string.IsNullOrEmpty(error))
                        {

                        }
                        else
                        {
                            model.db.id = ObjectId.GenerateNewId().ToString();
                            model.db.nguoi_cap_nhat = getUserId();
                            model.db.ngay_cap_nhat = DateTime.Now;
                            model.db.status_del = 1;
                            await repo.insert(model);
                        }


                    }

                    if (error == "")
                    {
                        return Json("1");
                    }
                    else
                    {
                        var path_err = get_file_err(name, error, _appsetting.folder_path);

                        try
                        {
                            var memory = new MemoryStream();
                            using (var stream = new FileStream(path_err, FileMode.Open))
                            {
                                await stream.CopyToAsync(memory);
                            }
                            memory.Position = 0;

                            return Json(path_err);

                        }
                        catch (Exception ex)
                        {
                            return Json("-1");
                        }
                    }
                }
                catch
                {
                    return Json("-1");
                }
            }
            else
            {
                return Json("-1");
            }
        }


        [AllowAnonymous]
        public ActionResult downloadtemp()
        {
            var currentpath = Directory.GetCurrentDirectory();
            string newPath = Path.Combine(currentpath, "wwwroot", "assets", "template");
            if (!Directory.Exists(newPath))
                Directory.CreateDirectory(newPath);

            string Files = newPath + "\\sys_loai_nhap_xuat.xlsx";
            byte[] fileBytes = System.IO.File.ReadAllBytes(Files);
            System.IO.File.WriteAllBytes(Files, fileBytes);
            MemoryStream ms = new MemoryStream(fileBytes);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, "sys_loai_nhap_xuat.xlsx");
        }

        public IActionResult getListUse()
        {
            var result = repo._context.sys_loai_nhap_xuat_col.AsQueryable()
                .Where(d => d.status_del == 1).
                 Select(d => new
                 {
                     id = d.id,
                     name = d.ten,
                     loai = d.loai,
                     ma = d.ma,
                     nguon = d.nguon
                 }).ToList();
            return Json(result);
        }
        [HttpPost]
        public async Task<IActionResult> create([FromBody] JObject json)
        {

            var model = JsonConvert.DeserializeObject<sys_loai_nhap_xuat_model>(json.GetValue("data").ToString());
            var check = checkModelStateCreate(model);
            if (!check)
            {
                return generateError();
            }
            model.db.id = ObjectId.GenerateNewId().ToString();
            model.db.status_del = 1;
            model.db.nguoi_cap_nhat = getUserId();
            model.db.ngay_cap_nhat = DateTime.Now;
            if (model.ten_loai == "Nhập")
            {
                model.db.loai = 1;
            }
            else if (model.ten_loai == "Xuất")
            {
                model.db.loai = 2;
            }
            await repo.insert(model);
            return Json(model);
        }

        [HttpPost]
        public async Task<IActionResult> edit([FromBody] JObject json)
        {
            var model = JsonConvert.DeserializeObject<sys_loai_nhap_xuat_model>(json.GetValue("data").ToString());
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

                var status_del = int.Parse(dictionary["status_del"]);
                var search = dictionary["search"].Trim().ToLower();
                var loai = int.Parse(dictionary["loai"]);
                var query = repo.FindAll()
                      //.Where(d=>d.db.status_del==1)
                      .Where(d => d.db.loai == loai || loai == -1)
                      .Where(d => d.db.status_del == status_del)
                     .Where(d => d.db.ten.ToLower().Contains(search) || d.db.ma.ToLower().Contains(search) || d.db.ghi_chu.ToLower().Contains(search) || search == "")
                     ;
                var count = query.Count();
                var dataList = await Task.Run(() => query.OrderByDescending(d => d.db.ngay_cap_nhat).Skip(param.Start).Take(param.Length)
        .ToList());
                DTResult<sys_loai_nhap_xuat_model> result = new DTResult<sys_loai_nhap_xuat_model>
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

