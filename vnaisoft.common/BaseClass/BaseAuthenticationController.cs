using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Newtonsoft.Json.Linq;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using vnaisoft.common.Common;
using vnaisoft.common.Helpers;
using vnaisoft.common.Models;
using vnaisoft.common.Services;
using vnaisoft.DataBase.Mongodb;
using WS.CRM.Data.Helper;

namespace vnaisoft.common.BaseClass
{
    public enum ActionEnumForm
    {
        create = 1,
        edit = 2,
        detail = 3,
        revise = 4
    }



    [ServiceFilter(typeof(vnaisoftAuthorize))]
    [ApiController]
    [Route("[controller].ctr/[action]")]
    public abstract class BaseAuthenticationController : Controller
    {
        public IUserService _userService;
        public BaseAuthenticationController(IUserService userService)
        {
            _userService = userService;

        }

        public string get_file_err(string name, string error, string currentpath)
        {
            var tick = Guid.NewGuid().ToString();
            var path = Path.Combine(currentpath, "file_upload", name, "Error");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            var pathsave = Path.Combine(path, tick + ".txt");
            using (System.IO.Stream stream = new FileStream(pathsave, FileMode.Create))
            {
                stream.CopyTo(stream);
            }
            System.IO.File.WriteAllText(pathsave, error.Replace("<br />", Environment.NewLine));
            return pathsave;
        }
        public string get_file_err_name(string name, string error, string currentpath, string controller)
        {
            var tick = Guid.NewGuid().ToString();
            var path = Path.Combine(currentpath, "file_upload", controller, "Error");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            var pathsave = Path.Combine(path, tick + ".txt");
            using (System.IO.Stream stream = new FileStream(pathsave, FileMode.Create))
            {
                stream.CopyTo(stream);
            }
            System.IO.File.WriteAllText(pathsave, error.Replace("<br />", Environment.NewLine));
            return pathsave;
        }
        public string getUserId()
        {
            //school login tại trường
            var existingNameIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier && c.Value == "school");
            if (existingNameIdClaim != null)
                return User.Identity.Name;
            else
            {
                // Đăng nhập bên hoccung ai lấy id dựa token

                //
                var loai_dang_nhap = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                  ?? User?.FindFirst("nameid")?.Value
                  ?? User?.FindFirst("sub")?.Value;
                // dang nhap co khong co email(tai khpan so )
                if (loai_dang_nhap!=null && loai_dang_nhap == "5")
                {
                    return User.Identity.Name;
                }

                var emailClaim = User?.FindFirst(ClaimTypes.Email)?.Value
                     ?? User?.FindFirst("email")?.Value
                     ?? User?.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value;

                var email = emailClaim;

                // dang nhap bang email google,facebook

                String user_id_school;
                var cacheServices = HttpContext.RequestServices.GetService<IMemoryCache>();
                var cache_key = "UserId" + User.Identity.Name;
                var cache_value = cacheServices.Get(cache_key);
                if (cacheServices.TryGetValue(cache_key, out user_id_school) && cache_value != null)
                {
                    return user_id_school;
                }
                else
                {

                    if (string.IsNullOrEmpty(User.Identity.Name)) return null;
                    if (string.IsNullOrEmpty(email)) return null;
                    var context = HttpContext.RequestServices.GetService<MongoDBContext>();
                    // lay ra usẻ_id của db trương hpcj dựa vào email hoccungai truyền vào
                    var userIdSchool = context.sys_user_col.AsQueryable().Where(x => x.email.ToLower() == email.ToLower()).Select(x => x.id).FirstOrDefault();

                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromDays(1));
                    cacheServices.Set(cache_key, userIdSchool, cacheEntryOptions);

                    return userIdSchool;

                }

            }

        }

        public vnaisoft.DataBase.System.User getUser()
        {
            var username = User.Identity.Name;
            var user = _userService.GetById(username);
            return user;

        }
        public JsonResult generateError()
        {
            Response.StatusCode = 400;
            var errorList = ModelState
               .Where(x => x.Value != null)
               .ToList().
               Where(d => d.Value.Errors.Count > 0)
               .Select(kvp =>
                   new
                   {
                       key = kvp.Key,
                       value = kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                   }
               ).ToList();
            return Json(errorList);
        }
        public JsonResult generateSuscess()
        {
            Response.StatusCode = 200;
            return Json(new { result = "" });
        }



        [HttpPost]
        public virtual async Task<IActionResult> sync_cancel_approval([FromBody] JObject json)
        {
            return generateSuscess();
        }
        [HttpPost]
        public virtual async Task<IActionResult> sync_open_approval([FromBody] JObject json)
        {
            return generateSuscess();
        }
        private object GetValueByKey(object item, string key)
        {
            var prop = item.GetType().GetProperty(key);
            return prop != null ? prop.GetValue(item, null) : null;
        }
        public class row_excel_model
        {
            public int? row_index { get; set; }
            public List<col_excel_model> lst_col { get; set; }
        }
        public class col_excel_model
        {
            public string name { get; set; }
            public int? row_index { get; set; }
            public int? col_index { get; set; }
            public string style { get; set; }
        }
        public class SheetData
        {
            public string SheetName { get; set; }
            public string[] Headers { get; set; }
            public List<object> DataList { get; set; }
            public string[] ListKey { get; set; }
        }
        [NonAction]
        public async Task<FileStreamResult> exportExcelFileHeaderFooter(AppSettings _appsetting, string[] header, string[] listKey, object dataList, string name)
        {
            var excel = new ExcelHelper(_appsetting);
            string file = "";
            try
            {

                file = excel.exportExcelHeaderFooter(dataList, null, new List<string[]> { header }, listKey, new List<NPOI.SS.Util.CellRangeAddress>(), name, true);
            }
            catch { }

            if (!string.IsNullOrEmpty(file))
            {

                var memory = new MemoryStream();
                using (var stream = new FileStream(file, FileMode.Open))
                {
                    await stream.CopyToAsync(memory);
                }
                memory.Position = 0;

                return File(memory, excel.GetContentType(file), Path.GetFileName(file));
            }
            return null;
        }

        [NonAction]
        public async Task<FileStreamResult> exportFileExcel(AppSettings _appsetting, string[] header, string[] listKey, object dataList, string name)
        {
            var excel = new ExcelHelper(_appsetting);
            string file = "";
            try
            {
                file = excel.exportExcel(dataList, null, new List<string[]> { header }, listKey, new List<NPOI.SS.Util.CellRangeAddress>(), name, true);
            }
            catch { }

            if (!string.IsNullOrEmpty(file))
            {

                var memory = new MemoryStream();
                using (var stream = new FileStream(file, FileMode.Open))
                {
                    await stream.CopyToAsync(memory);
                }
                memory.Position = 0;

                return File(memory, excel.GetContentType(file), Path.GetFileName(file));
            }
            return null;
        }
        [NonAction]
        public string exportFileLog(AppSettings _appsetting, string[] header, string[] listKey, object dataList, string name)
        {
            var excel = new ExcelHelper(_appsetting);
            string file = "";
            try
            {

                file = excel.exportExcelLog(dataList, null, new List<string[]> { header }, listKey, new List<NPOI.SS.Util.CellRangeAddress>(), name, true);
            }
            catch { }


            return file;
        }

        [NonAction]
        public async Task<FileStreamResult> exportFileExcelMulti(XSSFWorkbook workbook, AppSettings _appsetting, string[] header, string[] listKey, object dataList, string name)
        {
            var excel = new ExcelHelper(_appsetting);
            string file = "";
            try
            {

                var rs = excel.exportExcelMulti(workbook, name, dataList, null, new List<string[]> { header }, listKey, new List<NPOI.SS.Util.CellRangeAddress>(), true, null);
            }
            catch { }

            if (!string.IsNullOrEmpty(file))
            {

                var memory = new MemoryStream();
                using (var stream = new FileStream(file, FileMode.Open))
                {
                    await stream.CopyToAsync(memory);
                }
                memory.Position = 0;

                return File(memory, excel.GetContentType(file), Path.GetFileName(file));
            }
            return null;
        }
        [NonAction]
        public async Task<FileStreamResult> exportFileExcelLog(AppSettings _appsetting, string file)
        {

            var excel = new ExcelHelper(_appsetting);
            if (!string.IsNullOrEmpty(file))
            {
                var memory = new MemoryStream();
                using (var stream = new FileStream(file, FileMode.Open))
                {
                    await stream.CopyToAsync(memory);
                }
                memory.Position = 0;

                return File(memory, excel.GetContentType(file), Path.GetFileName(file));
            }
            return null;
        }




        [NonAction]
        public async Task<FileStreamResult> exportFileExcelTotal(AppSettings _appsetting, string[] header, string[] listKey, object dataList, string name, int? row_total)
        {
            var excel = new ExcelHelper(_appsetting);
            string file = "";
            try
            {


                file = excel.exportExcel(dataList, null, new List<string[]> { header }, listKey, new List<NPOI.SS.Util.CellRangeAddress>(), name, true, row_total);

            }
            catch { }

            if (!string.IsNullOrEmpty(file))
            {

                var memory = new MemoryStream();
                using (var stream = new FileStream(file, FileMode.Open))
                {
                    await stream.CopyToAsync(memory);
                }
                memory.Position = 0;

                return File(memory, excel.GetContentType(file), Path.GetFileName(file));


            }
            return null;
        }



        public List<row> handleImportFileSheetName(IFormFile file, string name)
        {
            string folderName = name;

            var currentpath = Directory.GetCurrentDirectory();

            string newPath = Path.Combine(currentpath, "file_upload", folderName);
            var tick = Guid.NewGuid();

            if (!Directory.Exists(newPath))
            {
                Directory.CreateDirectory(newPath);
            }
            string sFileExtension = Path.GetExtension(file.FileName).ToLower();

            ISheet sheet;
            string fullPath = Path.Combine(newPath, tick + "." + file.FileName.Split(".").Last());
            var list_cell = new List<cell>();
            var list_row = new List<row>();
            using (var stream = new FileStream(fullPath, FileMode.Create))

            {

                file.CopyTo(stream);

                stream.Position = 0;

                if (sFileExtension == ".xls")

                {

                    HSSFWorkbook hssfwb = new HSSFWorkbook(stream); //This will read the Excel 97-2000 formats  

                    sheet = hssfwb.GetSheet(name); //get first sheet from workbook  

                }

                else

                {

                    XSSFWorkbook hssfwb = new XSSFWorkbook(stream); //This will read 2007 Excel format  

                    sheet = hssfwb.GetSheet(name); //get first sheet from workbook   

                }

                IRow headerRow = sheet.GetRow(0); //Get Header Row

                int cellCount = headerRow.LastCellNum;



                for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++) //Read Excel File

                {

                    IRow row = sheet.GetRow(i);

                    if (row == null) continue;

                    if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;

                    for (int j = row.FirstCellNum; j < cellCount; j++)

                    {
                        var cell = new cell();
                        var value = "";
                        if (row.GetCell(j) == null)
                        {
                            value = "";
                        }
                        else
                        {
                            value = row.GetCell(j).ToString();

                        }

                        cell.value = value;
                        list_cell.Add(cell);

                    }

                    var data_row = new row();


                    data_row.key = i.ToString();
                    data_row.list_cell = list_cell;
                    list_cell = new List<cell>();
                    list_row.Add(data_row);
                }
            }
            return list_row;

        }

        public List<row> handleImportFile(IFormFile file)
        {
            string folderName = "import_excel";

            var currentpath = Directory.GetCurrentDirectory();

            string newPath = Path.Combine(currentpath, "file_upload", folderName);
            var tick = Guid.NewGuid();

            if (!Directory.Exists(newPath))
            {
                Directory.CreateDirectory(newPath);
            }
            string sFileExtension = Path.GetExtension(file.FileName).ToLower();

            ISheet sheet;
            string fullPath = Path.Combine(newPath, tick + "." + file.FileName.Split(".").Last());
            var list_cell = new List<cell>();
            var list_row = new List<row>();
            using (var stream = new FileStream(fullPath, FileMode.Create))

            {

                file.CopyTo(stream);

                stream.Position = 0;

                if (sFileExtension == ".xls")

                {

                    HSSFWorkbook hssfwb = new HSSFWorkbook(stream); //This will read the Excel 97-2000 formats  

                    sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook  

                }

                else

                {

                    XSSFWorkbook hssfwb = new XSSFWorkbook(stream); //This will read 2007 Excel format  

                    sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook   

                }

                IRow headerRow = sheet.GetRow(0); //Get Header Row

                int cellCount = headerRow.LastCellNum;



                for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++) //Read Excel File

                {

                    IRow row = sheet.GetRow(i);

                    if (row == null) continue;

                    if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;

                    for (int j = row.FirstCellNum; j < cellCount; j++)

                    {
                        var cell = new cell();
                        var value = "";
                        if (row.GetCell(j) == null)
                        {
                            value = "";
                        }
                        else
                        {
                            value = row.GetCell(j).ToString();

                        }

                        cell.value = value;
                        list_cell.Add(cell);

                    }

                    var data_row = new row();


                    data_row.key = i.ToString();
                    data_row.list_cell = list_cell;
                    list_cell = new List<cell>();
                    list_row.Add(data_row);
                }
            }
            return list_row;

        }








        public List<row> handleImportFileManySheet(IFormFile file)
        {
            string folderName = "import_excel";

            var currentpath = Directory.GetCurrentDirectory();

            string newPath = Path.Combine(currentpath, "file_upload", folderName);
            var tick = Guid.NewGuid();

            if (!Directory.Exists(newPath))
            {
                Directory.CreateDirectory(newPath);
            }
            string sFileExtension = Path.GetExtension(file.FileName).ToLower();

            ISheet sheet;
            string fullPath = Path.Combine(newPath, tick + "." + file.FileName.Split(".").Last());
            var list_cell = new List<cell>();
            var list_row = new List<row>();

            using (var stream = new FileStream(fullPath, FileMode.Create))

            {

                file.CopyTo(stream);

                stream.Position = 0;

                if (sFileExtension == ".xls")

                {

                    HSSFWorkbook hssfwb = new HSSFWorkbook(stream); //This will read the Excel 97-2000 formats  

                    sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook  

                }

                else

                {

                    XSSFWorkbook hssfwb = new XSSFWorkbook(stream); //This will read 2007 Excel format  

                    sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook   

                }

                IRow headerRow = sheet.GetRow(0); //Get Header Row

                int cellCount = headerRow.LastCellNum;



                for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++) //Read Excel File

                {

                    IRow row = sheet.GetRow(i);

                    if (row == null) continue;

                    if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;

                    for (int j = row.FirstCellNum; j < cellCount; j++)

                    {
                        var cell = new cell();
                        var value = "";
                        if (row.GetCell(j) == null)
                        {
                            value = "";
                        }
                        else
                        {
                            value = row.GetCell(j).ToString();

                        }

                        cell.value = value;
                        list_cell.Add(cell);

                    }

                    var data_row = new row();


                    data_row.key = i.ToString();
                    data_row.list_cell = list_cell;
                    list_cell = new List<cell>();
                    list_row.Add(data_row);
                }
            }
            return list_row;

        }

    }
}
