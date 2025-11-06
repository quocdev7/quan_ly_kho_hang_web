using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using vnaisoft.common.Common;
using vnaisoft.common.Helpers;
using vnaisoft.DataBase.Helper;
using vnaisoft.DataBase.Mongodb;
using vnaisoft.DataBase.Mongodb.Collection.system;

namespace vnaisoft.common.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class FileManagerController : Controller
    {
        public MongoDBContext _context;
        public  string bucketName = "vnaisoft";        // 🔹 Replace with your bucket name
        //public  string keyName = "uploads/yourfile.txt";       // 🔹 S3 object key (path in bucket)
        //public  string filePath = @"C:\path\to\yourfile.txt";  // 🔹 Local file path
        public  RegionEndpoint bucketRegion = RegionEndpoint.APSoutheast1; // 🔹 Your region (example: ap-southeast-1)

        public  IAmazonS3 s3Client;

        public AppSettings _appsetting;
        public FileManagerController(IOptions<AppSettings> appsetting, MongoDBContext context)
        {
            _appsetting = appsetting.Value;
            _context = context;

        }
      


    


        [HttpGet("downloadFile")]
        public async Task<IActionResult> downloadFile(string id, string file_name)
        {
            var filedb = _context.sys_file_upload_col.AsQueryable().Where(t => t.id == id).FirstOrDefault();

            var host = Request.Host.Value;
            if (host.Contains("localhost"))
            {
                host = "localhost";
            }


            if (filedb != null)
            {
                //file_path = filedb.file_upload.file_path;
                file_name = filedb.file_name;
            }
            var path_file = HttpUtility.UrlDecode(filedb.file_path.Replace("/FileManager/Download/?filename=", ""));
            path_file = path_file.Substring(1, path_file.Length - 1);
            var path = Path.Combine(_appsetting.folder_path, "file_upload", host);
            var pathsave = Path.Combine(path, path_file);

            if (filedb.file_path.Contains("https://school.vietnamai"))
            {
                return Redirect(filedb.file_path);
            }
            else
            {  //var path = _appsetting.folder_path + filedb.file_path;
                return new PhysicalFileResult(pathsave,
                        File_content_type.GetContentType(pathsave))
                { FileDownloadName = file_name };

            }
              
        }
        // [HttpGet("downloadFileMul")]
        // public async Task<IActionResult> downloadFileMul(string ids)
        // {
        //     var client = new HttpClient();
        //     if (string.IsNullOrWhiteSpace(ids))
        //     {
        //         return BadRequest("IDs are required.");
        //     }

        //     var idList = ids.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();
        //     var host = Request.Host.Value;
        //     if (host.Contains("localhost"))
        //     {
        //         host = "localhost";
        //     }

        //     var files = _context.sys_file_upload_col.AsQueryable()
        //         .Where(t => idList.Contains(t.id))
        //         .ToList();

        //     if (!files.Any())
        //         return NotFound("Không tìm thấy file nào.");

        //     // Sử dụng MemoryStream để tạo zip trong bộ nhớ, tránh ghi vào đĩa tạm
        //     var memoryStream = new MemoryStream();

        //     using (var zip = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
        //     {
        //         foreach (var file in files)
        //         {
        //             try
        //             {
        //                 // Lấy tên tệp, đảm bảo có tên hợp lệ
        //                 var entryFileName = string.IsNullOrWhiteSpace(file.file_name)
        //                     ? $"file_{file.id}"
        //                     : file.file_name;

        //                 // CASE 1: Xử lý file từ URL
        //                 if (file.file_path.Contains("https://school.vietnamai"))
        //                 {
        //                     // Tải file từ URL về dạng byte array
        //                     var fileBytes = await client.GetByteArrayAsync(file.file_path);

        //                     if (fileBytes != null && fileBytes.Length > 0)
        //                     {
        //                         // Tạo một entry trong zip và ghi dữ liệu byte vào đó
        //                         var zipEntry = zip.CreateEntry(entryFileName);
        //                         using (var entryStream = zipEntry.Open())
        //                         {
        //                             await entryStream.WriteAsync(fileBytes, 0, fileBytes.Length);
        //                         }
        //                     }
        //                 }
        //                 // CASE 2: Xử lý file cục bộ
        //                 else
        //                 {
        //                     var path_file = HttpUtility.UrlDecode(file.file_path.Replace("/FileManager/Download/?filename=", ""));
        //                     path_file = path_file.TrimStart('/');
        //                     var path = Path.Combine(_appsetting.folder_path, "file_upload", host);
        //                     var filePath = Path.Combine(path, path_file);

        //                     if (System.IO.File.Exists(filePath))
        //                     {
        //                         zip.CreateEntryFromFile(filePath, entryFileName);
        //                     }
        //                 }
        //             }
        //             catch (Exception ex)
        //             {
        //                 // Ghi lại lỗi nếu một file không tải được và tiếp tục
        //                 // Điều này ngăn chặn toàn bộ quá trình zip bị lỗi chỉ vì một file
        //                 Console.WriteLine($"Error processing file ID {file.id}: {ex.Message}");
        //             }
        //         }
        //     }

        //     // Đặt lại vị trí của stream về đầu trước khi trả về
        //     memoryStream.Position = 0;

        //     var zipFileName = $"download_{DateTime.Now:yyyyMMddHHmmss}.zip";
        //     // Trả về FileStreamResult trực tiếp từ MemoryStream
        //     return File(memoryStream, "application/zip", zipFileName);
        // }


        // [HttpGet("downloadFileMul")]
        // public async Task<IActionResult> downloadFileMulold(string ids)
        // {
        //     var idList = ids.Split(',').Where(id => !string.IsNullOrWhiteSpace(id)).ToList();
        //     var host = Request.Host.Value;
        //     if (host.Contains("localhost"))
        //     {
        //         host = "localhost";
        //     }

        //     var files = _context.sys_file_upload_col.AsQueryable()
        //         .Where(t => idList.Contains(t.id))
        //         .ToList();

        //     if (!files.Any())
        //         return NotFound("Không tìm thấy file nào");

        //     var zipFileName = $"download_{DateTime.Now:yyyyMMddHHmmss}.zip";
        //     var tempZipPath = Path.Combine(Path.GetTempPath(), zipFileName);

        //     using (var zip = ZipFile.Open(tempZipPath, ZipArchiveMode.Create))
        //     {
        //         foreach (var file in files)
        //         {
        //             var path_file = HttpUtility.UrlDecode(file.file_path.Replace("/FileManager/Download/?filename=", ""));
        //             //path_file = path_file.TrimStart('/');
        //             path_file = path_file.Substring(1, path_file.Length - 1);
        //             var path = Path.Combine(_appsetting.folder_path, "file_upload", host);
        //             var filePath = Path.Combine(path, path_file);

        //             if (System.IO.File.Exists(filePath))
        //             {
        //                 zip.CreateEntryFromFile(filePath, file.file_name + ".pdf" ?? Path.GetFileName(filePath));
        //             }
        //         }
        //     }

        //     var zipBytes = await System.IO.File.ReadAllBytesAsync(tempZipPath);
        //     System.IO.File.Delete(tempZipPath); // dọn dẹp file tạm

        //     return File(zipBytes, "application/zip", zipFileName);
        // }

        [HttpGet("downloadFileMul")]
        public async Task<IActionResult> downloadFileMul(string ids)
        {
            var client = new HttpClient();
            if (string.IsNullOrWhiteSpace(ids))
            {
                return BadRequest("IDs are required.");
            }

            var idList = ids.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();
            var host = Request.Host.Value;
            if (host.Contains("localhost"))
            {
                host = "localhost";
            }

            var files = _context.sys_file_upload_col.AsQueryable()
                .Where(t => idList.Contains(t.id))
                .ToList();

            if (!files.Any())
                return NotFound("Không tìm thấy file nào.");

            var memoryStream = new MemoryStream();
            try
            {
                // leaveOpen: true để MemoryStream không bị đóng sau khi ZipArchive được giải phóng
                using (var zip = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    foreach (var file in files)
                    {
                        try
                        {
                            // Đảm bảo tên file hợp lệ, thêm đuôi .pdf nếu cần
                            var entryFileName = string.IsNullOrWhiteSpace(file.file_name)
                                ? $"file_{file.id}.pdf"
                                : Path.HasExtension(file.file_name) ? file.file_name : $"{file.file_name}.pdf";

                            byte[] fileBytes = null;

                            // CASE 1: Xử lý file từ URL
                            if (file.file_path.Contains("https://school.vietnamai"))
                            {
                                fileBytes = await client.GetByteArrayAsync(file.file_path);
                            }
                            // CASE 2: Xử lý file cục bộ
                            else
                            {
                                var path_file = HttpUtility.UrlDecode(file.file_path.Replace("/FileManager/Download/?filename=", ""));
                                path_file = path_file.TrimStart('/', '\\');
                                var path = Path.Combine(_appsetting.folder_path, "file_upload", host);
                                var filePath = Path.Combine(path, path_file);

                                if (System.IO.File.Exists(filePath))
                                {
                                    fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
                                }
                            }

                            // Nếu có dữ liệu file, thêm vào file ZIP
                            if (fileBytes != null && fileBytes.Length > 0)
                            {
                                var zipEntry = zip.CreateEntry(entryFileName, CompressionLevel.Optimal);
                                using (var entryStream = zipEntry.Open())
                                {
                                    await entryStream.WriteAsync(fileBytes, 0, fileBytes.Length);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            // Ghi log lỗi đầy đủ để dễ dàng debug
                            Console.WriteLine($"Error processing file ID {file.id} for zipping: {ex.ToString()}");
                        }
                    }
                }

                // Nếu stream rỗng (không file nào được thêm), bạn có thể trả về lỗi thay vì file zip rỗng
                if (memoryStream.Length == 0)
                {
                    return NotFound("Không thể xử lý bất kỳ tệp nào để nén. File có thể bị lỗi hoặc không tồn tại.");
                }

                memoryStream.Position = 0;
                var zipFileName = $"HocBa_{DateTime.Now:yyyyMMddHHmmss}.zip";
                return File(memoryStream, "application/zip", zipFileName);
            }
            catch
            {
                memoryStream.Dispose();
                throw;
            }
        }

        // [HttpGet("viewonlineMul")]
        // public async Task<IActionResult> viewonlineMul(string ids)
        // {
        //     var client = new HttpClient();
        //     if (string.IsNullOrWhiteSpace(ids))
        //     {
        //         return BadRequest("IDs are required.");
        //     }

        //     var idList = ids.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();
        //     var files = _context.sys_file_upload_col.AsQueryable()
        //         .Where(f => idList.Contains(f.id))
        //         .ToList();

        //     if (files == null || !files.Any())
        //     {
        //         return NotFound("Không tìm thấy file nào.");
        //     }

        //     var host = Request.Host.Value;
        //     if (host.Contains("localhost"))
        //     {
        //         host = "localhost";
        //     }

        //     // Sử dụng MemoryStream để chứa file PDF đã gộp
        //     using (var mergedStream = new MemoryStream())
        //     {
        //         using (PdfDocument mergedPdf = new PdfDocument())
        //         {
        //             foreach (var file in files)
        //             {
        //                 try
        //                 {
        //                     Stream pdfStream = null;

        //                     // CASE 1: Xử lý file từ URL
        //                     if (file.file_path.Contains("https://school.vietnamai"))
        //                     {
        //                         // Tải file từ URL về dạng byte array
        //                         var fileBytes = await client.GetByteArrayAsync(file.file_path);
        //                         if (fileBytes != null && fileBytes.Length > 0)
        //                         {
        //                             // Tạo một MemoryStream từ byte array đã tải
        //                             pdfStream = new MemoryStream(fileBytes);
        //                         }
        //                     }
        //                     // CASE 2: Xử lý file cục bộ
        //                     else
        //                     {
        //                         var path_file = HttpUtility.UrlDecode(file.file_path.Replace("/FileManager/Download/?filename=", ""));
        //                         path_file = path_file.TrimStart('/'); // An toàn hơn Substring
        //                         var path = Path.Combine(_appsetting.folder_path, "file_upload", host);
        //                         var fullPath = Path.Combine(path, path_file);

        //                         if (System.IO.File.Exists(fullPath))
        //                         {
        //                             // Mở file cục bộ vào một FileStream
        //                             pdfStream = System.IO.File.OpenRead(fullPath);
        //                         }
        //                     }

        //                     // Nếu đã có stream (từ URL hoặc file cục bộ), tiến hành gộp
        //                     if (pdfStream != null)
        //                     {
        //                         using (pdfStream) // Đảm bảo stream được giải phóng
        //                         {
        //                             PdfDocument inputPdf = PdfReader.Open(pdfStream, PdfDocumentOpenMode.Import);
        //                             for (int i = 0; i < inputPdf.PageCount; i++)
        //                             {
        //                                 mergedPdf.AddPage(inputPdf.Pages[i]);
        //                             }
        //                         }
        //                     }
        //                 }
        //                 catch (Exception ex)
        //                 {
        //                     // Ghi lại lỗi nếu một file không xử lý được và tiếp tục
        //                     Console.WriteLine($"Error processing file ID {file.id} ({file.file_path}): {ex.Message}");
        //                 }
        //             } // Kết thúc vòng lặp foreach

        //             // Chỉ lưu nếu có trang được thêm vào
        //             if (mergedPdf.PageCount > 0)
        //             {
        //                 mergedPdf.Save(mergedStream, false); // Tham số false để không đóng stream
        //             }
        //         } // Kết thúc using mergedPdf

        //         // Trả về file PDF đã gộp
        //         return File(mergedStream.ToArray(), "application/pdf");
        //     } // Kết thúc using mergedStream
        // }

        // [HttpGet("viewonlineMul")]
        // public async Task<IActionResult> viewonlineMulold(string ids)
        // {
        //     var request = Request;
        //     //var filedb = _context.sys_file_upload_col.AsQueryable().Where(t => t.id == id).FirstOrDefault();
        //     var idList = ids.Split(',').Where(id => !string.IsNullOrWhiteSpace(id)).ToList();
        //     var files = _context.sys_file_upload_col.AsQueryable()
        //         .Where(f => idList.Contains(f.id))
        //         .ToList();
        //     var host = request.Host.Value;
        //     if (host.Contains("localhost"))
        //     {
        //         host = "localhost";
        //     }

        //     if (files == null || files.Count == 0)
        //     {
        //         return NotFound("Không tìm thấy file.");
        //     }

        //     using (var mergedStream = new MemoryStream())
        //     {
        //         using (PdfDocument mergedPdf = new PdfDocument())
        //         {
        //             foreach (var file in files)
        //             {
        //                 var path_file = HttpUtility.UrlDecode(file.file_path.Replace("/FileManager/Download/?filename=", ""));
        //                 path_file = path_file.Substring(1, path_file.Length - 1);
        //                 var path = Path.Combine(_appsetting.folder_path, "file_upload", host);
        //                 var fullPath = Path.Combine(path, path_file);

        //                 if (!System.IO.File.Exists(fullPath))
        //                     continue;

        //                 using (var fileStream = System.IO.File.OpenRead(fullPath))
        //                 {
        //                     PdfDocument inputPdf = PdfReader.Open(fileStream, PdfDocumentOpenMode.Import);
        //                     for (int i = 0; i < inputPdf.PageCount; i++)
        //                     {
        //                         mergedPdf.AddPage(inputPdf.Pages[i]);
        //                     }
        //                 }
        //             }

        //             mergedPdf.Save(mergedStream);
        //         }

        //         mergedStream.Position = 0;
        //         return new FileContentResult(mergedStream.ToArray(), "application/pdf");
        //     }

        // }

        [HttpGet("viewonlineMul")]
        public async Task<IActionResult> viewonlineMul(string ids)
        {
            var client = new HttpClient();
            if (string.IsNullOrWhiteSpace(ids))
            {
                return BadRequest("IDs are required.");
            }

            var idList = ids.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();
            var files = _context.sys_file_upload_col.AsQueryable()
                .Where(f => idList.Contains(f.id))
                .ToList();

            if (!files.Any())
            {
                return NotFound("Không tìm thấy thông tin file nào.");
            }

            var host = Request.Host.Value;
            if (host.Contains("localhost"))
            {
                host = "localhost";
            }

            using (var mergedPdf = new PdfDocument())
            {
                bool atLeastOneFileMerged = false;
                foreach (var file in files)
                {
                    try
                    {
                        byte[] fileBytes = null;

                        // CASE 1: Xử lý file từ URL
                        if (file.file_path.Contains("https://school.vietnamai"))
                        {
                            fileBytes = await client.GetByteArrayAsync(file.file_path);
                        }
                        // CASE 2: Xử lý file cục bộ
                        else
                        {
                            var path_file = HttpUtility.UrlDecode(file.file_path.Replace("/FileManager/Download/?filename=", ""));
                            // Xử lý an toàn hơn cho cả dấu gạch chéo xuôi và ngược
                            path_file = path_file.TrimStart('/', '\\'); 
                            var path = Path.Combine(_appsetting.folder_path, "file_upload", host);
                            var fullPath = Path.Combine(path, path_file);

                            if (System.IO.File.Exists(fullPath))
                            {
                                fileBytes = await System.IO.File.ReadAllBytesAsync(fullPath);
                            }
                        }

                        // Nếu đã có dữ liệu file, tiến hành gộp
                        if (fileBytes != null && fileBytes.Length > 0)
                        {
                            using (var pdfStream = new MemoryStream(fileBytes))
                            {
                                PdfDocument inputPdf = PdfReader.Open(pdfStream, PdfDocumentOpenMode.Import);
                                for (int i = 0; i < inputPdf.PageCount; i++)
                                {
                                    mergedPdf.AddPage(inputPdf.Pages[i]);
                                }
                                atLeastOneFileMerged = true;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Ghi log lỗi đầy đủ để dễ dàng debug (quan trọng!)
                        Console.WriteLine($"Error processing file ID {file.id} for merging: {ex.ToString()}");
                    }
                }

                // QUAN TRỌNG: Nếu không có file nào được gộp, trả về lỗi
                if (!atLeastOneFileMerged)
                {
                    return NotFound("Không thể xử lý bất kỳ tệp PDF nào được yêu cầu. Vui lòng kiểm tra lại file gốc.");
                }

                // Lưu file đã gộp vào memory stream để trả về
                using (var resultStream = new MemoryStream())
                {
                    mergedPdf.Save(resultStream, false);
                    return File(resultStream.ToArray(), "application/pdf");
                }
            }
        }

        [HttpGet("viewonline")]
        public async Task<IActionResult> viewonline(string id, string id_file, string file_name, string file_type)
        {
            var request = Request;
            var filedb = _context.sys_file_upload_col.AsQueryable().Where(t => t.id == id).FirstOrDefault();
            var host = request.Host.Value;
            if (host.Contains("localhost"))
            {
                host = "localhost";
            }
            if (!string.IsNullOrEmpty(file_type))
            {
                filedb.file_path = Path.ChangeExtension(filedb.file_path, ".pdf");
            }
            if (filedb != null)
            {
                file_name = filedb.file_path;
            }

            var path_file = HttpUtility.UrlDecode(filedb.file_path.Replace("/FileManager/Download/?filename=", ""));
            path_file = path_file.Substring(1, path_file.Length - 1);
            var path = Path.Combine(_appsetting.folder_path, "file_upload", host);
            var pathsave = Path.Combine(path, path_file);



            if (filedb.file_path.Contains("sachdientu.vnaisoft.com"))
            {
                path_file = HttpUtility.UrlDecode(filedb.file_path.Replace("https://sachdientu.vnaisoft.com/FileManager/Download/?filename=", ""));

                path_file = path_file.Substring(1, path_file.Length - 1);
                path = Path.Combine(_appsetting.folder_path_sach_dien_tu, "file_upload");
                pathsave = Path.Combine(path, path_file);
            }

            if (filedb.file_path.Contains("https://school.vietnamai.com.vn"))
            {

                try
                {
                    return Redirect(filedb.file_path);
                    //// Trích xuất object key từ URL đầy đủ
                    //// Ví dụ URL: https://school.vietnamai.com.vn/localhost/sys_banner/file.png
                    //// Object Key sẽ là: localhost/sys_banner/file.png
                    //var uri = new Uri(filedb.file_path);
                    //var objectKey = uri.AbsolutePath.TrimStart('/');

                    //// 3. Gọi phương thức để chuyển đổi
                    //byte[] imageBytes = await ConvertS3ObjectToBytesAsync(objectKey);


                    //return new FileContentResult(imageBytes, File_content_type.GetContentType(pathsave));
                }
                catch (Exception ex)
                {
                    // Ghi lại lỗi
                    return StatusCode(500, "An error occurred while processing the file.");
                }



            }
            else
            {

                try
                {
                    var bytes = System.IO.File.ReadAllBytes(pathsave);
                    return new FileContentResult(bytes, File_content_type.GetContentType(pathsave));
                }
                catch (Exception ex)
                {
                    // Ghi lại lỗi
                    return StatusCode(500, "An error occurred while processing the file.");
                }

            }
            



        }   
        
        [HttpGet("view_file_common")]
        public async Task<IActionResult> view_file_common(string id, string id_file, string file_name, string file_type)
        {
            var request = Request;
            var filedb = _context.sys_file_upload_col.AsQueryable().Where(t => t.id == id).FirstOrDefault();
            var host = request.Host.Value;
            if (host.Contains("localhost"))
            {
                host = "localhost";
            }
            if (!string.IsNullOrEmpty(file_type))
            {
                filedb.file_path = Path.ChangeExtension(filedb.file_path, ".pdf");
            }
            if (filedb != null)
            {
                file_name = filedb.file_path;
            }

            var path_file = HttpUtility.UrlDecode(filedb.file_path.Replace("/FileManager/Download/?filename=", ""));
            path_file = path_file.Substring(1, path_file.Length - 1);
            var path = Path.Combine(_appsetting.folder_path, "file_upload", host);
            var pathsave = Path.Combine(path, path_file);



            if (filedb.file_path.Contains("sachdientu.vnaisoft.com"))
            {
                path_file = HttpUtility.UrlDecode(filedb.file_path.Replace("https://sachdientu.vnaisoft.com/FileManager/Download/?filename=", ""));

                path_file = path_file.Substring(1, path_file.Length - 1);
                path = Path.Combine(_appsetting.folder_path_sach_dien_tu, "file_upload");
                pathsave = Path.Combine(path, path_file);
            }

            var bytes = System.IO.File.ReadAllBytes(pathsave);
            return new FileContentResult(bytes, File_content_type.GetContentType(pathsave));



        }

        [HttpGet("downloadFileXml")]
        public async Task<IActionResult> downloadFileXml(string id, string file_name)
        {
            var filedb = _context.sys_file_upload_col.AsQueryable().Where(t => t.id == id).FirstOrDefault();

            var host = Request.Host.Value;
            if (host.Contains("localhost"))
            {
                host = "localhost";
            }


            if (filedb != null)
            {
                //file_path = filedb.file_upload.file_path;
                file_name = filedb.file_name;
            }
            var path_file = HttpUtility.UrlDecode(filedb.file_path.Replace("/FileManager/Download/?filename=", ""));
            path_file = path_file.Substring(1, path_file.Length - 1);
            var path = Path.Combine(_appsetting.folder_path, "file_upload", host);
            var pathsave = Path.Combine(path, path_file);

            //var path = _appsetting.folder_path + filedb.file_path;
            return new PhysicalFileResult(pathsave,
                        File_content_type.GetContentType(pathsave))
            { FileDownloadName = file_name + ".xml" };
        }

        [HttpGet("download")]
        public async Task<ActionResult> Download(string filename, string name)
        {
            if (string.IsNullOrWhiteSpace(filename))
            {
                return BadRequest("Tên tệp không được rỗng.");
            }

            // 1. Chuẩn hóa tên miền host để đảm bảo tính nhất quán
            var host = Request.Host.Value;
            if (host.Contains("localhost", StringComparison.OrdinalIgnoreCase))
            {
                host = "localhost";
            }

            // 2. Xác định thư mục gốc an toàn nơi các tệp được phép download.
            // Đây là thư mục CƠ SỞ mà người dùng không được phép thoát ra khỏi.
            // Ví dụ: "C:\YourApplicationRoot\file_upload\"
            string baseStoragePath = Path.Combine(_appsetting.folder_path, "file_upload");

            // Thêm thư mục con dựa trên host hoặc quy tắc đặc biệt
            string targetSubdirectory;
            if (filename.Contains("sys_huong_dan_su_dung_detail", StringComparison.OrdinalIgnoreCase) ||
                filename.Contains("sys_huong_dan_su_dung", StringComparison.OrdinalIgnoreCase))
            {
                targetSubdirectory = Path.Combine(baseStoragePath, "init.vnaisoft.com.vn");
            }
            else
            {
                targetSubdirectory = Path.Combine(baseStoragePath, host);
            }

            // Đảm bảo thư mục con tồn tại và hợp lệ
            // Có thể thêm kiểm tra Directory.Exists(targetSubdirectory) ở đây nếu muốn
            // và xử lý lỗi nếu thư mục không tồn tại

            // 3. Chuẩn hóa đường dẫn tệp do người dùng cung cấp và kết hợp với thư mục gốc an toàn
            // Sử dụng Path.Combine để xây dựng đường dẫn một cách an toàn
            // Sử dụng Path.GetFullPath để chuẩn hóa đường dẫn và loại bỏ các thành phần ".."
            string fullPath = "";
            try
            {

                string safeFilename = filename.Replace("..", string.Empty); // Loại bỏ ".." an toàn hơn là chỉ dựa vào Path.GetFullPath
                                                                            // Vì Path.GetFullPath có thể xử lý ".." nhưng việc loại bỏ trước
                                                                            // cũng là một lớp bảo vệ tốt.
                                                                            //string safeFilename = SafeFileName(filename);
                                                                            // Quan trọng: Sử dụng Path.Combine và Path.GetFullPath
                fullPath = Path.Combine(targetSubdirectory) + safeFilename;
            }
            catch (PathTooLongException)
            {
                return BadRequest("Đường dẫn tệp quá dài.");
            }
            catch (ArgumentException)
            {
                return BadRequest("Tên tệp không hợp lệ.");
            }

            // 4. Kiểm tra xem đường dẫn được chuẩn hóa có còn nằm trong thư mục gốc an toàn hay không.
            // Đây là bước quan trọng nhất để ngăn chặn Path Traversal.
            if (!fullPath.StartsWith(targetSubdirectory, StringComparison.OrdinalIgnoreCase))
            {
                // Người dùng đang cố gắng truy cập tệp ngoài thư mục cho phép.
                // Trả về lỗi Unauthorized hoặc Not Found để không tiết lộ thông tin.
                return Unauthorized("Truy cập tệp không hợp lệ.");
                // Hoặc: return NotFound();
            }

            // 5. Kiểm tra xem tệp có thực sự tồn tại và là tệp (không phải thư mục)
            if (!System.IO.File.Exists(fullPath))
            {
                return NotFound("Tệp không tồn tại.");
            }

            // 6. Đọc tệp và trả về
            var memory = new MemoryStream();
            try
            {
                using (var stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read))
                {
                    await stream.CopyToAsync(memory);
                }
            }
            catch (FileNotFoundException)
            {
                return NotFound("Tệp không tồn tại.");
            }
            catch (UnauthorizedAccessException)
            {
                // Xử lý nếu ứng dụng không có quyền đọc tệp (mặc dù đã kiểm tra phía trên)
                return Unauthorized("Không có quyền truy cập tệp.");
            }
            catch (IOException ex)
            {
                // Xử lý các lỗi I/O khác (ví dụ: tệp đang được sử dụng)
                return StatusCode(StatusCodes.Status500InternalServerError, $"Lỗi đọc tệp: {ex.Message}");
            }

            memory.Position = 0;

            // Xác định tên tệp để tải về
            string downloadFileName = string.IsNullOrWhiteSpace(name) ? Path.GetFileName(fullPath) : name;


            return File(memory, GetContentType(fullPath), downloadFileName);
        }
        string SafeFileName(string filename)
        {
            // Loại bỏ ký tự nguy hiểm
            string cleaned = filename.Replace("..", "")
                                      .Replace("/", "")

                                      .Replace(":", "")

                                      .Replace("\0", "")
                                      .Replace("~", "")
                                      .Replace("|", "")
                                      .Replace(">", "")
                                      .Replace("<", "")
                                      .Replace("&", "");
            return Path.GetFileName(cleaned); // chỉ giữ tên file, không có thư mục
        }
        [HttpGet("download3")]
        public async Task<ActionResult> Download3(string filename, string name)
        {
            if (filename == null)
                return Content("filename not present");
            var current = _appsetting.folder_path;

            var host = Request.Host.Value;
            if (host.Contains("localhost"))
            {
                host = "localhost";
            }
            var path = "";

            if (filename.Contains("sys_huong_dan_su_dung_detail") || filename.Contains("sys_huong_dan_su_dung"))
            {
                host = "init.vnaisoft.com.vn";
                path = Path.Combine(current, "file_upload", host) + filename;
            }
            else
            {
                path = Path.Combine(current, "file_upload", host) + filename;
            }
            string safeFilename = Path.GetFileName(filename);



            string fullPath = Path.Combine(current, "file_upload", host, safeFilename);
            string rootPath = Path.GetFullPath(Path.Combine(current, "file_upload", host));
            if (!fullPath.StartsWith(rootPath))
            {
                throw new UnauthorizedAccessException("Path Traversal detected");
            }

            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            if (!string.IsNullOrWhiteSpace(name))
            {
                return File(memory, GetContentType(path), name);
            }
            return File(memory, GetContentType(path), Path.GetFileName(path));
        }
        [HttpPost("uploadimage")]
        public async Task<ActionResult> uploadimage()
        {
            var request = Request;
            foreach (var formFile in Request.Form.Files)
            {
                var tick = ObjectId.GenerateNewId().ToString();
                var filename = formFile.FileName.Trim('"') + "";
                var currentpath = _appsetting.folder_path; // Directory.GetCurrentDirectory();
                var host = request.Host.Value;
                if (host.Contains("localhost"))
                {
                    host = "localhost";
                }
                var path = Path.Combine(currentpath, "file_upload", host, "image_upload");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                var pathsave = Path.Combine(path, tick + "." + filename.Split(".").Last());
                using (System.IO.Stream stream = new FileStream(pathsave, FileMode.Create))
                {
                    await formFile.CopyToAsync(stream);
                }
                var file_path = "/FileManager/Download/?filename=" + HttpUtility.UrlEncode(pathsave.Replace(Path.Combine(currentpath, "file_upload", host), ""));
                //+ "&name=" + HttpUtility.UrlEncode(filename));
                return Json(new { location = file_path });
            }
            return generateError();
        }
     
        [HttpPost("uploadimagenew")]
        public async Task<ActionResult> uploadimagenew()
        {


            //file = JsonConvert.DeserializeObject<sys_file_model>(Request.Form["thumbnail"]);
            var request = Request;
            var host = request.Host.Value;
            if (host.Contains("localhost"))
            {
                host = "localhost";
            }
            var user_id = User.Identity.Name ?? "nonlogin";
            var id = Request.Form["id"].ToString();
            var type = Request.Form["type"].ToString();
            var controller = Request.Form["controller"].ToString();
            //var controller = Request.Form.Keys.ToString();
            var i = 0;
            var item = Request.Form.Files[0];


            if (item != null)
            {
                var filename = item.FileName.Trim('"') + "";
                filename = StringFunctions.NonUnicode(filename);
                var currentpath = _appsetting.folder_path;//Directory.GetCurrentDirectory();
                //file_upload => san_pham => thumnail | image_upload
                var path = Path.Combine(currentpath, "file_upload", host, controller, type);
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                var pathsave = Path.Combine(path, id + "." + filename.Split(".").Last());
                //neu file ton tai => replace
                //tồn tại 2 đuôi khác nhau



                string[] files = Directory.GetFiles(path, id + "*.*", SearchOption.AllDirectories);



                if (files.Length > 0)
                {
                    foreach (string file in files)
                        try
                        {
                            System.IO.File.Delete(file);
                        }
                        catch { }
                    ;

                    using (System.IO.Stream stream = new FileStream(pathsave, FileMode.Create))
                    {
                        await item.CopyToAsync(stream);
                    }
                }
                else
                {
                    using (System.IO.Stream stream = new FileStream(pathsave, FileMode.Create))
                    {
                        await item.CopyToAsync(stream);
                    }

                }

                var file_path = "/FileManager/Download/?filename=" + HttpUtility.UrlEncode(pathsave.Replace(Path.Combine(currentpath, "file_upload", host), ""));
                //+ "&name=" + HttpUtility.UrlEncode(filename));
                return Json(new { location = file_path });

            }


            return generateError();
        }
      
        //ko có parse file
        [HttpPost("upload_file_common_old")]
        public async Task<ActionResult> upload_file_common_old()
        {
            var request = Request;
            var user_id = User.Identity.Name ?? "nonlogin";
            var controller = Request.Form["controller"].ToString();
            var id_phieu = Request.Form["id_phieu"].ToString();
            var id = ObjectId.GenerateNewId().ToString();
            var folder = "";
            try
            {
                folder = Request.Form["folder"].ToString();

            }
            catch { }
            var item = Request.Form.Files[0];
            if (item != null)
            {
                var filename = item.FileName.Trim('"') + "";
                var currentpath = _appsetting.folder_path;//Directory.GetCurrentDirectory();
                                                          //file_upload => san_pham => thumnail | image_upload
                var host = request.Host.Value;
                if (host.Contains("localhost"))
                {
                    host = "localhost";
                }
                var path = Path.Combine(currentpath, "file_upload", host, controller, user_id);
                if (!String.IsNullOrEmpty(folder))
                {
                    path = Path.Combine(currentpath, "file_upload", host, controller, folder, user_id);
                }
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                var pathsave = Path.Combine(path, id + "." + filename.Split(".").Last());


                using (System.IO.Stream stream = new FileStream(pathsave, FileMode.Create))
                {
                    await item.CopyToAsync(stream);
                }
                var file_path = "/FileManager/Download/?filename=" + HttpUtility.UrlEncode(pathsave.Replace(Path.Combine(currentpath, "file_upload", host), ""));
                var model = new sys_file_upload_col();
                model.id = id;
                model.id_phieu = id_phieu;
                model.ma_cong_viec = controller;
                model.file_name = item.FileName.ToString();
                model.file_path = file_path;
                model.file_size = item.Length;
                model.extension_file = item.ContentType.ToString();
                model.ngay_tao = DateTime.Now;
                model.nguoi_tao = user_id;
                model.ngay_cap_nhat = model.ngay_tao;
                model.nguoi_cap_nhat = user_id;
                model.status_del = 1;

                try
                {
                    using (var image = Image.FromFile(pathsave))
                    {
                        model.heightpx = image.Height;
                        model.widthpx = image.Width;
                    }
                }
                catch (Exception ex)
                {

                    Console.WriteLine($"Error getting image height: {ex.Message}");

                }
                _context.sys_file_upload_col.InsertOne(model);
                return Json(model);
            }
            return generateError();
        }

       
        [HttpPost("upload_file_common")]
        public async Task<ActionResult> upload_file_common()
        {
            var request = Request;
            var user_id = User.Identity.Name ?? "nonlogin";
            var controller = Request.Form["controller"].ToString();
         
            var id_phieu = Request.Form["id_phieu"].ToString();
            var id = ObjectId.GenerateNewId().ToString();
            var folder = "";
            try
            {
                folder = Request.Form["folder"].ToString();

            }
            catch { }
            var item = Request.Form.Files[0];
            if (item != null)
            {
                var filename = item.FileName.Trim('"') + "";
                var currentpath = _appsetting.folder_path;//Directory.GetCurrentDirectory();
                var uploader = new S3Uploader();

                //file_upload => san_pham => thumnail | image_upload
                var host = request.Host.Value;
                if (host.Contains("localhost"))
                {
                    host = "localhost";
                }

                // Tạo một thư mục cấp cao nhất
                //bool createdRootFolder = await uploader.CreateFolderAsync(host);

                string s3KeyName = "";
                string date = DateTime.Now.ToString("yyyy") + DateTime.Now.ToString("MM") + DateTime.Now.ToString("dd");
                if (!String.IsNullOrEmpty(folder))
                {

                    bool createdNestedFolder = await uploader.CreateFolderAsync($"{host}/{user_id}/{date}/{controller}/{folder}");
                    s3KeyName = $"{host}/{user_id}/{date}/{controller}/{folder}/{id + "." + filename.Split(".").Last()}";
                }
                else
                {
                    bool createdNestedFolder = await uploader.CreateFolderAsync($"{host}/{user_id}/{date}/{controller}");
                    s3KeyName = $"{host}/{user_id}/{date}/{controller}/{id + "." + filename.Split(".").Last()}";

                }








                byte[] fileBytes;
                using (var memoryStream = new MemoryStream())
                {
                    await item.CopyToAsync(memoryStream);
                    fileBytes = memoryStream.ToArray();
                }
                string uploadedFileUrl = await uploader.UploadFileAsync(fileBytes, s3KeyName);


                var model = new sys_file_upload_col();
                model.id = id;
                model.id_phieu = id_phieu;
                model.ma_cong_viec = controller;
                model.file_name = filename;
                model.file_path = uploadedFileUrl;
                model.file_size = item.Length;
                model.extension_file = item.ContentType; // Get the actual content type from S3
                model.ngay_tao = DateTime.Now;
                model.nguoi_tao = user_id;
                model.ngay_cap_nhat = model.ngay_tao;
                model.nguoi_cap_nhat = user_id;
                model.status_del = 1;

                try
                {
                    using (var ms = new MemoryStream(fileBytes))
                    {
                        // Sử dụng Image.FromStream để đọc dữ liệu từ stream trong bộ nhớ
                        using (var image = Image.FromStream(ms))
                        {
                            model.heightpx = image.Height;
                            model.widthpx = image.Width;
                        }
                    }
                    //using (var image = Image.FromFile(pathsave))
                    //{
                    //    model.heightpx = image.Height;
                    //    model.widthpx = image.Width;
                    //}
                }
                catch (Exception ex)
                {

                    Console.WriteLine($"Error getting image height: {ex.Message}");

                }
                _context.sys_file_upload_col.InsertOne(model);
                return Json(model);
            }





            return generateError();
        }


        [HttpPost("upload_list_file_common_old")]
        public async Task<ActionResult> upload_list_file_common_old()
        {
            var request = Request;
            var user_id = User.Identity.Name ?? "nonlogin";
            var controller = Request.Form["controller"].ToString();
            var folder = "";
            try
            {
                folder = Request.Form["folder"].ToString();

            }
            catch { }
            var id_phieu = "";
            try
            {
                id_phieu = Request.Form["id_phieu"].ToString();

            }
            catch { }
            var list_file = Request.Form.Files;
            var list_item_file = new List<sys_file_upload_col>();
            for (int index = 0; index < list_file.Count(); index++)
            {
                var item = list_file[index];
                var id = ObjectId.GenerateNewId().ToString();
                if (item != null)
                {
                    var filename = item.FileName.Trim('"') + "";
                    var currentpath = _appsetting.folder_path;//Directory.GetCurrentDirectory();
                                                              //file_upload => san_pham => thumnail | image_upload
                    var host = request.Host.Value;
                    if (host.Contains("localhost"))
                    {
                        host = "localhost";
                    }
                    var path = Path.Combine(currentpath, "file_upload", host, controller, user_id);
                    if (!!String.IsNullOrEmpty(folder))
                    {
                        path = Path.Combine(currentpath, "file_upload", host, controller, user_id, folder);
                    }
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);

                    var pathsave = Path.Combine(path, id + "." + filename.Split(".").Last());


                    using (System.IO.Stream stream = new FileStream(pathsave, FileMode.Create))
                    {
                        await item.CopyToAsync(stream);
                    }


                    var file_path = "/FileManager/Download/?filename=" + HttpUtility.UrlEncode(pathsave.Replace(Path.Combine(currentpath, "file_upload", host), ""));


                    var model = new sys_file_upload_col();



                    model.id = id;
                    model.id_phieu = id_phieu;
                    model.ma_cong_viec = controller;
                    model.file_name = item.FileName.ToString();
                    model.file_path = file_path;
                    model.file_size = item.Length;
                    model.extension_file = item.ContentType.ToString();
                    model.ngay_tao = DateTime.Now;
                    model.nguoi_tao = user_id;
                    model.ngay_cap_nhat = model.ngay_tao;
                    model.nguoi_cap_nhat = user_id;
                    model.status_del = 1;
                    model.nguoi_cap_nhat = _context.sys_user_col.AsQueryable().Where(q => q.id == model.nguoi_cap_nhat).Select(q => q.ho_va_ten).FirstOrDefault();
                    list_item_file.Add(model);

                }
            }
            return Json(list_item_file);
        }
        [HttpPost("upload_list_file_common")]
        public async Task<ActionResult> upload_list_file_common()
        {
            var request = Request;
            var user_id = User.Identity.Name ?? "nonlogin";
            var controller = Request.Form["controller"].ToString();
            var folder = "";
            try
            {
                folder = Request.Form["folder"].ToString();

            }
            catch { }
            var id_phieu = "";
            try
            {
                id_phieu = Request.Form["id_phieu"].ToString();

            }
            catch { }
            var list_file = Request.Form.Files;
            var list_item_file = new List<sys_file_upload_col>();
            for (int index = 0; index < list_file.Count(); index++)
            {
                var item = list_file[index];
                var id = ObjectId.GenerateNewId().ToString();
                if (item != null)
                {

                    var filename = item.FileName.Trim('"') + "";
                    var currentpath = _appsetting.folder_path;//Directory.GetCurrentDirectory();
                    var uploader = new S3Uploader();

                    //file_upload => san_pham => thumnail | image_upload
                    var host = request.Host.Value;
                    if (host.Contains("localhost"))
                    {
                        host = "localhost";
                    }

                    // Tạo một thư mục cấp cao nhất
                    //bool createdRootFolder = await uploader.CreateFolderAsync(host);

                    string s3KeyName = "";

                    if(controller == "sys_tin_tuc_website")
                    {
                        folder = user_id;
                    }
                    if (!String.IsNullOrEmpty(folder))
                    {

                        bool createdNestedFolder = await uploader.CreateFolderAsync($"{host}/{controller}/{folder}");
                        s3KeyName = $"{host}/{controller}/{folder}/{id + "." + filename.Split(".").Last()}";
                    }
                    else
                    {
                        bool createdNestedFolder = await uploader.CreateFolderAsync($"{host}/{controller}");
                        s3KeyName = $"{host}/{controller}/{id + "." + filename.Split(".").Last()}";

                    }
                    byte[] fileBytes;
                    using (var memoryStream = new MemoryStream())
                    {
                        await item.CopyToAsync(memoryStream);
                        fileBytes = memoryStream.ToArray();
                    }
                    string uploadedFileUrl = await uploader.UploadFileAsync(fileBytes, s3KeyName);
                    var model = new sys_file_upload_col();
                    model.id = id;
                    model.id_phieu = id_phieu;
                    model.ma_cong_viec = controller;
                    model.file_name = item.FileName.ToString();
                    model.file_path = uploadedFileUrl;
                    model.file_size = item.Length;
                    model.extension_file = item.ContentType.ToString();
                    model.ngay_tao = DateTime.Now;
                    model.nguoi_tao = user_id;
                    model.ngay_cap_nhat = model.ngay_tao;
                    model.nguoi_cap_nhat = user_id;
                    model.status_del = 1;
                    model.nguoi_cap_nhat = _context.sys_user_col.AsQueryable().Where(q => q.id == model.nguoi_cap_nhat).Select(q => q.ho_va_ten).FirstOrDefault();
                    list_item_file.Add(model);

                }
            }
            _context.sys_file_upload_col.InsertMany(list_item_file);
            return Json(list_item_file);
        }

        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".323", "text/h323"},
        {".3g2", "video/3gpp2"},
        {".3gp", "video/3gpp"},
        {".3gp2", "video/3gpp2"},
        {".3gpp", "video/3gpp"},
        {".7z", "application/x-7z-compressed"},
        {".aa", "audio/audible"},
        {".aac", "audio/aac"},
        {".aaf", "application/octet-stream"},
        {".aax", "audio/vnd.audible.aax"},
        {".ac3", "audio/ac3"},
        {".aca", "application/octet-stream"},
        {".accda", "application/msaccess.addin"},
        {".accdb", "application/msaccess"},
        {".accdc", "application/msaccess.cab"},
        {".accde", "application/msaccess"},
        {".accdr", "application/msaccess.runtime"},
        {".accdt", "application/msaccess"},
        {".accdw", "application/msaccess.webapplication"},
        {".accft", "application/msaccess.ftemplate"},
        {".acx", "application/internet-property-stream"},
        {".AddIn", "text/xml"},
        {".ade", "application/msaccess"},
        {".adobebridge", "application/x-bridge-url"},
        {".adp", "application/msaccess"},
        {".adt", "audio/vnd.dlna.adts"},
        {".adts", "audio/aac"},
        {".afm", "application/octet-stream"},
        {".ai", "application/postscript"},
        {".aif", "audio/x-aiff"},
        {".aifc", "audio/aiff"},
        {".aiff", "audio/aiff"},
        {".air", "application/vnd.adobe.air-application-installer-package+zip"},
        {".amc", "application/x-mpeg"},
        {".application", "application/x-ms-application"},
        {".art", "image/x-jg"},
        {".asa", "application/xml"},
        {".asax", "application/xml"},
        {".ascx", "application/xml"},
        {".asd", "application/octet-stream"},
        {".asf", "video/x-ms-asf"},
        {".ashx", "application/xml"},
        {".asi", "application/octet-stream"},
        {".asm", "text/plain"},
        {".asmx", "application/xml"},
        {".aspx", "application/xml"},
        {".asr", "video/x-ms-asf"},
        {".asx", "video/x-ms-asf"},
        {".atom", "application/atom+xml"},
        {".au", "audio/basic"},
        {".avi", "video/x-msvideo"},
        {".axs", "application/olescript"},
        {".bas", "text/plain"},
        {".bcpio", "application/x-bcpio"},
        {".bin", "application/octet-stream"},
        {".bmp", "image/bmp"},
        {".c", "text/plain"},
        {".cab", "application/octet-stream"},
        {".caf", "audio/x-caf"},
        {".calx", "application/vnd.ms-office.calx"},
        {".cat", "application/vnd.ms-pki.seccat"},
        {".cc", "text/plain"},
        {".cd", "text/plain"},
        {".cdda", "audio/aiff"},
        {".cdf", "application/x-cdf"},
        {".cer", "application/x-x509-ca-cert"},
        {".chm", "application/octet-stream"},
        {".class", "application/x-java-applet"},
        {".clp", "application/x-msclip"},
        {".cmx", "image/x-cmx"},
        {".cnf", "text/plain"},
        {".cod", "image/cis-cod"},
        {".config", "application/xml"},
        {".contact", "text/x-ms-contact"},
        {".coverage", "application/xml"},
        {".cpio", "application/x-cpio"},
        {".cpp", "text/plain"},
        {".crd", "application/x-mscardfile"},
        {".crl", "application/pkix-crl"},
        {".crt", "application/x-x509-ca-cert"},
        {".cs", "text/plain"},
        {".csdproj", "text/plain"},
        {".csh", "application/x-csh"},
        {".csproj", "text/plain"},
        {".css", "text/css"},
        {".csv", "text/csv"},
        {".cur", "application/octet-stream"},
        {".cxx", "text/plain"},
        {".dat", "application/octet-stream"},
        {".datasource", "application/xml"},
        {".dbproj", "text/plain"},
        {".dcr", "application/x-director"},
        {".def", "text/plain"},
        {".deploy", "application/octet-stream"},
        {".der", "application/x-x509-ca-cert"},
        {".dgml", "application/xml"},
        {".dib", "image/bmp"},
        {".dif", "video/x-dv"},
        {".dir", "application/x-director"},
        {".disco", "text/xml"},
        {".dll", "application/x-msdownload"},
        {".dll.config", "text/xml"},
        {".dlm", "text/dlm"},
        {".doc", "application/msword"},
        {".docm", "application/vnd.ms-word.document.macroEnabled.12"},
        {".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document"},
        {".dot", "application/msword"},
        {".dotm", "application/vnd.ms-word.template.macroEnabled.12"},
        {".dotx", "application/vnd.openxmlformats-officedocument.wordprocessingml.template"},
        {".dsp", "application/octet-stream"},
        {".dsw", "text/plain"},
        {".dtd", "text/xml"},
        {".dtsConfig", "text/xml"},
        {".dv", "video/x-dv"},
        {".dvi", "application/x-dvi"},
        {".dwf", "drawing/x-dwf"},
        {".dwp", "application/octet-stream"},
        {".dxr", "application/x-director"},
        {".eml", "message/rfc822"},
        {".emz", "application/octet-stream"},
        {".eot", "application/octet-stream"},
        {".eps", "application/postscript"},
        {".etl", "application/etl"},
        {".etx", "text/x-setext"},
        {".evy", "application/envoy"},
        {".exe", "application/octet-stream"},
        {".exe.config", "text/xml"},
        {".fdf", "application/vnd.fdf"},
        {".fif", "application/fractals"},
        {".filters", "Application/xml"},
        {".fla", "application/octet-stream"},
        {".flr", "x-world/x-vrml"},
        {".flv", "video/x-flv"},
        {".fsscript", "application/fsharp-script"},
        {".fsx", "application/fsharp-script"},
        {".generictest", "application/xml"},
        {".webp", "image/webp"},
        {".gif", "image/gif"},
        {".group", "text/x-ms-group"},
        {".gsm", "audio/x-gsm"},
        {".gtar", "application/x-gtar"},
        {".gz", "application/x-gzip"},
        {".h", "text/plain"},
        {".hdf", "application/x-hdf"},
        {".hdml", "text/x-hdml"},
        {".hhc", "application/x-oleobject"},
        {".hhk", "application/octet-stream"},
        {".hhp", "application/octet-stream"},
        {".hlp", "application/winhlp"},
        {".hpp", "text/plain"},
        {".hqx", "application/mac-binhex40"},
        {".hta", "application/hta"},
        {".htc", "text/x-component"},
        {".htm", "text/html"},
        {".html", "text/html"},
        {".htt", "text/webviewhtml"},
        {".hxa", "application/xml"},
        {".hxc", "application/xml"},
        {".hxd", "application/octet-stream"},
        {".hxe", "application/xml"},
        {".hxf", "application/xml"},
        {".hxh", "application/octet-stream"},
        {".hxi", "application/octet-stream"},
        {".hxk", "application/xml"},
        {".hxq", "application/octet-stream"},
        {".hxr", "application/octet-stream"},
        {".hxs", "application/octet-stream"},
        {".hxt", "text/html"},
        {".hxv", "application/xml"},
        {".hxw", "application/octet-stream"},
        {".hxx", "text/plain"},
        {".i", "text/plain"},
        {".ico", "image/x-icon"},
        {".ics", "application/octet-stream"},
        {".idl", "text/plain"},
        {".ief", "image/ief"},
        {".iii", "application/x-iphone"},
        {".inc", "text/plain"},
        {".inf", "application/octet-stream"},
        {".inl", "text/plain"},
        {".ins", "application/x-internet-signup"},
        {".ipa", "application/x-itunes-ipa"},
        {".ipg", "application/x-itunes-ipg"},
        {".ipproj", "text/plain"},
        {".ipsw", "application/x-itunes-ipsw"},
        {".iqy", "text/x-ms-iqy"},
        {".isp", "application/x-internet-signup"},
        {".ite", "application/x-itunes-ite"},
        {".itlp", "application/x-itunes-itlp"},
        {".itms", "application/x-itunes-itms"},
        {".itpc", "application/x-itunes-itpc"},
        {".IVF", "video/x-ivf"},
        {".jar", "application/java-archive"},
        {".java", "application/octet-stream"},
        {".jck", "application/liquidmotion"},
        {".jcz", "application/liquidmotion"},
        {".jfif", "image/pjpeg"},
        {".jnlp", "application/x-java-jnlp-file"},
        {".jpb", "application/octet-stream"},
        {".jpe", "image/jpeg"},
        {".jpeg", "image/jpeg"},
        {".jpg", "image/jpeg"},
        {".js", "application/x-javascript"},
        {".json", "application/json"},
        {".jsx", "text/jscript"},
        {".jsxbin", "text/plain"},
        {".latex", "application/x-latex"},
        {".library-ms", "application/windows-library+xml"},
        {".lit", "application/x-ms-reader"},
        {".loadtest", "application/xml"},
        {".lpk", "application/octet-stream"},
        {".lsf", "video/x-la-asf"},
        {".lst", "text/plain"},
        {".lsx", "video/x-la-asf"},
        {".lzh", "application/octet-stream"},
        {".m13", "application/x-msmediaview"},
        {".m14", "application/x-msmediaview"},
        {".m1v", "video/mpeg"},
        {".m2t", "video/vnd.dlna.mpeg-tts"},
        {".m2ts", "video/vnd.dlna.mpeg-tts"},
        {".m2v", "video/mpeg"},
        {".m3u", "audio/x-mpegurl"},
        {".m3u8", "audio/x-mpegurl"},
        {".m4a", "audio/m4a"},
        {".m4b", "audio/m4b"},
        {".m4p", "audio/m4p"},
        {".m4r", "audio/x-m4r"},
        {".m4v", "video/x-m4v"},
        {".mac", "image/x-macpaint"},
        {".mak", "text/plain"},
        {".man", "application/x-troff-man"},
        {".manifest", "application/x-ms-manifest"},
        {".map", "text/plain"},
        {".master", "application/xml"},
        {".mda", "application/msaccess"},
        {".mdb", "application/x-msaccess"},
        {".mde", "application/msaccess"},
        {".mdp", "application/octet-stream"},
        {".me", "application/x-troff-me"},
        {".mfp", "application/x-shockwave-flash"},
        {".mht", "message/rfc822"},
        {".mhtml", "message/rfc822"},
        {".mid", "audio/mid"},
        {".midi", "audio/mid"},
        {".mix", "application/octet-stream"},
        {".mk", "text/plain"},
        {".mmf", "application/x-smaf"},
        {".mno", "text/xml"},
        {".mny", "application/x-msmoney"},
        {".mod", "video/mpeg"},
        {".mov", "video/quicktime"},
        {".movie", "video/x-sgi-movie"},
        {".mp2", "video/mpeg"},
        {".mp2v", "video/mpeg"},
        {".mp3", "audio/mpeg"},
        {".mp4", "video/mp4"},
        {".mp4v", "video/mp4"},
        {".mpa", "video/mpeg"},
        {".mpe", "video/mpeg"},
        {".mpeg", "video/mpeg"},
        {".mpf", "application/vnd.ms-mediapackage"},
        {".mpg", "video/mpeg"},
        {".mpp", "application/vnd.ms-project"},
        {".mpv2", "video/mpeg"},
        {".mqv", "video/quicktime"},
        {".ms", "application/x-troff-ms"},
        {".msi", "application/octet-stream"},
        {".mso", "application/octet-stream"},
        {".mts", "video/vnd.dlna.mpeg-tts"},
        {".mtx", "application/xml"},
        {".mvb", "application/x-msmediaview"},
        {".mvc", "application/x-miva-compiled"},
        {".mxp", "application/x-mmxp"},
        {".nc", "application/x-netcdf"},
        {".nsc", "video/x-ms-asf"},
        {".nws", "message/rfc822"},
        {".ocx", "application/octet-stream"},
        {".oda", "application/oda"},
        {".odc", "text/x-ms-odc"},
        {".odh", "text/plain"},
        {".odl", "text/plain"},
        {".odp", "application/vnd.oasis.opendocument.presentation"},
        {".ods", "application/oleobject"},
        {".odt", "application/vnd.oasis.opendocument.text"},
        {".one", "application/onenote"},
        {".onea", "application/onenote"},
        {".onepkg", "application/onenote"},
        {".onetmp", "application/onenote"},
        {".onetoc", "application/onenote"},
        {".onetoc2", "application/onenote"},
        {".orderedtest", "application/xml"},
        {".osdx", "application/opensearchdescription+xml"},
        {".p10", "application/pkcs10"},
        {".p12", "application/x-pkcs12"},
        {".p7b", "application/x-pkcs7-certificates"},
        {".p7c", "application/pkcs7-mime"},
        {".p7m", "application/pkcs7-mime"},
        {".p7r", "application/x-pkcs7-certreqresp"},
        {".p7s", "application/pkcs7-signature"},
        {".pbm", "image/x-portable-bitmap"},
        {".pcast", "application/x-podcast"},
        {".pct", "image/pict"},
        {".pcx", "application/octet-stream"},
        {".pcz", "application/octet-stream"},
        {".pdf", "application/pdf"},
        {".pfb", "application/octet-stream"},
        {".pfm", "application/octet-stream"},
        {".pfx", "application/x-pkcs12"},
        {".pgm", "image/x-portable-graymap"},
        {".pic", "image/pict"},
        {".pict", "image/pict"},
        {".pkgdef", "text/plain"},
        {".pkgundef", "text/plain"},
        {".pko", "application/vnd.ms-pki.pko"},
        {".pls", "audio/scpls"},
        {".pma", "application/x-perfmon"},
        {".pmc", "application/x-perfmon"},
        {".pml", "application/x-perfmon"},
        {".pmr", "application/x-perfmon"},
        {".pmw", "application/x-perfmon"},
        {".png", "image/png"},
        {".pnm", "image/x-portable-anymap"},
        {".pnt", "image/x-macpaint"},
        {".pntg", "image/x-macpaint"},
        {".pnz", "image/png"},
        {".pot", "application/vnd.ms-powerpoint"},
        {".potm", "application/vnd.ms-powerpoint.template.macroEnabled.12"},
        {".potx", "application/vnd.openxmlformats-officedocument.presentationml.template"},
        {".ppa", "application/vnd.ms-powerpoint"},
        {".ppam", "application/vnd.ms-powerpoint.addin.macroEnabled.12"},
        {".ppm", "image/x-portable-pixmap"},
        {".pps", "application/vnd.ms-powerpoint"},
        {".ppsm", "application/vnd.ms-powerpoint.slideshow.macroEnabled.12"},
        {".ppsx", "application/vnd.openxmlformats-officedocument.presentationml.slideshow"},
        {".ppt", "application/vnd.ms-powerpoint"},
        {".pptm", "application/vnd.ms-powerpoint.presentation.macroEnabled.12"},
        {".pptx", "application/vnd.openxmlformats-officedocument.presentationml.presentation"},
        {".prf", "application/pics-rules"},
        {".prm", "application/octet-stream"},
        {".prx", "application/octet-stream"},
        {".ps", "application/postscript"},
        {".psc1", "application/PowerShell"},
        {".psd", "application/octet-stream"},
        {".psess", "application/xml"},
        {".psm", "application/octet-stream"},
        {".psp", "application/octet-stream"},
        {".pub", "application/x-mspublisher"},
        {".pwz", "application/vnd.ms-powerpoint"},
        {".qht", "text/x-html-insertion"},
        {".qhtm", "text/x-html-insertion"},
        {".qt", "video/quicktime"},
        {".qti", "image/x-quicktime"},
        {".qtif", "image/x-quicktime"},
        {".qtl", "application/x-quicktimeplayer"},
        {".qxd", "application/octet-stream"},
        {".ra", "audio/x-pn-realaudio"},
        {".ram", "audio/x-pn-realaudio"},
        {".rar", "application/octet-stream"},
        {".ras", "image/x-cmu-raster"},
        {".rat", "application/rat-file"},
        {".rc", "text/plain"},
        {".rc2", "text/plain"},
        {".rct", "text/plain"},
        {".rdlc", "application/xml"},
        {".resx", "application/xml"},
        {".rf", "image/vnd.rn-realflash"},
        {".rgb", "image/x-rgb"},
        {".rgs", "text/plain"},
        {".rm", "application/vnd.rn-realmedia"},
        {".rmi", "audio/mid"},
        {".rmp", "application/vnd.rn-rn_music_package"},
        {".roff", "application/x-troff"},
        {".rpm", "audio/x-pn-realaudio-plugin"},
        {".rqy", "text/x-ms-rqy"},
        {".rtf", "application/rtf"},
        {".rtx", "text/richtext"},
        {".ruleset", "application/xml"},
        {".s", "text/plain"},
        {".safariextz", "application/x-safari-safariextz"},
        {".scd", "application/x-msschedule"},
        {".sct", "text/scriptlet"},
        {".sd2", "audio/x-sd2"},
        {".sdp", "application/sdp"},
        {".sea", "application/octet-stream"},
        {".searchConnector-ms", "application/windows-search-connector+xml"},
        {".setpay", "application/set-payment-initiation"},
        {".setreg", "application/set-registration-initiation"},
        {".settings", "application/xml"},
        {".sgimb", "application/x-sgimb"},
        {".sgml", "text/sgml"},
        {".sh", "application/x-sh"},
        {".shar", "application/x-shar"},
        {".shtml", "text/html"},
        {".sit", "application/x-stuffit"},
        {".sitemap", "application/xml"},
        {".skin", "application/xml"},
        {".sldm", "application/vnd.ms-powerpoint.slide.macroEnabled.12"},
        {".sldx", "application/vnd.openxmlformats-officedocument.presentationml.slide"},
        {".slk", "application/vnd.ms-excel"},
        {".sln", "text/plain"},
        {".slupkg-ms", "application/x-ms-license"},
        {".smd", "audio/x-smd"},
        {".smi", "application/octet-stream"},
        {".smx", "audio/x-smd"},
        {".smz", "audio/x-smd"},
        {".snd", "audio/basic"},
        {".snippet", "application/xml"},
        {".snp", "application/octet-stream"},
        {".sol", "text/plain"},
        {".sor", "text/plain"},
        {".spc", "application/x-pkcs7-certificates"},
        {".spl", "application/futuresplash"},
        {".src", "application/x-wais-source"},
        {".srf", "text/plain"},
        {".SSISDeploymentManifest", "text/xml"},
        {".ssm", "application/streamingmedia"},
        {".sst", "application/vnd.ms-pki.certstore"},
        {".stl", "application/vnd.ms-pki.stl"},
        {".sv4cpio", "application/x-sv4cpio"},
        {".sv4crc", "application/x-sv4crc"},
        {".svc", "application/xml"},
        {".swf", "application/x-shockwave-flash"},
        {".t", "application/x-troff"},
        {".tar", "application/x-tar"},
        {".tcl", "application/x-tcl"},
        {".testrunconfig", "application/xml"},
        {".testsettings", "application/xml"},
        {".tex", "application/x-tex"},
        {".texi", "application/x-texinfo"},
        {".texinfo", "application/x-texinfo"},
        {".tgz", "application/x-compressed"},
        {".thmx", "application/vnd.ms-officetheme"},
        {".thn", "application/octet-stream"},
        {".tif", "image/tiff"},
        {".tiff", "image/tiff"},
        {".tlh", "text/plain"},
        {".tli", "text/plain"},
        {".toc", "application/octet-stream"},
        {".tr", "application/x-troff"},
        {".trm", "application/x-msterminal"},
        {".trx", "application/xml"},
        {".ts", "video/vnd.dlna.mpeg-tts"},
        {".tsv", "text/tab-separated-values"},
        {".ttf", "application/octet-stream"},
        {".tts", "video/vnd.dlna.mpeg-tts"},
        {".txt", "text/plain"},
        {".u32", "application/octet-stream"},
        {".uls", "text/iuls"},
        {".user", "text/plain"},
        {".ustar", "application/x-ustar"},
        {".vb", "text/plain"},
        {".vbdproj", "text/plain"},
        {".vbk", "video/mpeg"},
        {".vbproj", "text/plain"},
        {".vbs", "text/vbscript"},
        {".vcf", "text/x-vcard"},
        {".vcproj", "Application/xml"},
        {".vcs", "text/plain"},
        {".vcxproj", "Application/xml"},
        {".vddproj", "text/plain"},
        {".vdp", "text/plain"},
        {".vdproj", "text/plain"},
        {".vdx", "application/vnd.ms-visio.viewer"},
        {".vml", "text/xml"},
        {".vscontent", "application/xml"},
        {".vsct", "text/xml"},
        {".vsd", "application/vnd.visio"},
        {".vsi", "application/ms-vsi"},
        {".vsix", "application/vsix"},
        {".vsixlangpack", "text/xml"},
        {".vsixmanifest", "text/xml"},
        {".vsmdi", "application/xml"},
        {".vspscc", "text/plain"},
        {".vss", "application/vnd.visio"},
        {".vsscc", "text/plain"},
        {".vssettings", "text/xml"},
        {".vssscc", "text/plain"},
        {".vst", "application/vnd.visio"},
        {".vstemplate", "text/xml"},
        {".vsto", "application/x-ms-vsto"},
        {".vsw", "application/vnd.visio"},
        {".vsx", "application/vnd.visio"},
        {".vtx", "application/vnd.visio"},
        {".wav", "audio/wav"},
        {".wave", "audio/wav"},
        {".wax", "audio/x-ms-wax"},
        {".wbk", "application/msword"},
        {".wbmp", "image/vnd.wap.wbmp"},
        {".wcm", "application/vnd.ms-works"},
        {".wdb", "application/vnd.ms-works"},
        {".wdp", "image/vnd.ms-photo"},
        {".webarchive", "application/x-safari-webarchive"},
        {".webtest", "application/xml"},
        {".wiq", "application/xml"},
        {".wiz", "application/msword"},
        {".wks", "application/vnd.ms-works"},
        {".WLMP", "application/wlmoviemaker"},
        {".wlpginstall", "application/x-wlpg-detect"},
        {".wlpginstall3", "application/x-wlpg3-detect"},
        {".wm", "video/x-ms-wm"},
        {".wma", "audio/x-ms-wma"},
        {".wmd", "application/x-ms-wmd"},
        {".wmf", "application/x-msmetafile"},
        {".wml", "text/vnd.wap.wml"},
        {".wmlc", "application/vnd.wap.wmlc"},
        {".wmls", "text/vnd.wap.wmlscript"},
        {".wmlsc", "application/vnd.wap.wmlscriptc"},
        {".wmp", "video/x-ms-wmp"},
        {".wmv", "video/x-ms-wmv"},
        {".wmx", "video/x-ms-wmx"},
        {".wmz", "application/x-ms-wmz"},
        {".wpl", "application/vnd.ms-wpl"},
        {".wps", "application/vnd.ms-works"},
        {".wri", "application/x-mswrite"},
        {".wrl", "x-world/x-vrml"},
        {".wrz", "x-world/x-vrml"},
        {".wsc", "text/scriptlet"},
        {".wsdl", "text/xml"},
        {".wvx", "video/x-ms-wvx"},
        {".x", "application/directx"},
        {".xaf", "x-world/x-vrml"},
        {".xaml", "application/xaml+xml"},
        {".xap", "application/x-silverlight-app"},
        {".xbap", "application/x-ms-xbap"},
        {".xbm", "image/x-xbitmap"},
        {".xdr", "text/plain"},
        {".xht", "application/xhtml+xml"},
        {".xhtml", "application/xhtml+xml"},
        {".xla", "application/vnd.ms-excel"},
        {".xlam", "application/vnd.ms-excel.addin.macroEnabled.12"},
        {".xlc", "application/vnd.ms-excel"},
        {".xld", "application/vnd.ms-excel"},
        {".xlk", "application/vnd.ms-excel"},
        {".xll", "application/vnd.ms-excel"},
        {".xlm", "application/vnd.ms-excel"},
        {".xls", "application/vnd.ms-excel"},
        {".xlsb", "application/vnd.ms-excel.sheet.binary.macroEnabled.12"},
        {".xlsm", "application/vnd.ms-excel.sheet.macroEnabled.12"},
        {".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
        {".xlt", "application/vnd.ms-excel"},
        {".xltm", "application/vnd.ms-excel.template.macroEnabled.12"},
        {".xltx", "application/vnd.openxmlformats-officedocument.spreadsheetml.template"},
        {".xlw", "application/vnd.ms-excel"},
        {".xml", "text/xml"},
        {".xmta", "application/xml"},
        {".xof", "x-world/x-vrml"},
        {".XOML", "text/plain"},
        {".xpm", "image/x-xpixmap"},
        {".xps", "application/vnd.ms-xpsdocument"},
        {".xrm-ms", "text/xml"},
        {".xsc", "application/xml"},
        {".xsd", "text/xml"},
        {".xsf", "text/xml"},
        {".xsl", "text/xml"},
        {".xslt", "text/xml"},
        {".xsn", "application/octet-stream"},
        {".xss", "application/xml"},
        {".xtp", "application/octet-stream"},
        {".xwd", "image/x-xwindowdump"},
        {".z", "application/x-compress"},
        {".zip", "application/x-zip-compressed"},

            };
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
                       Key = kvp.Key,
                       Value = kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                   }
               ).ToList();
            return Json(errorList);
        }

    }
}
