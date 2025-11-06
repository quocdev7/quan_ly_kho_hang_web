using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

public class S3Uploader
{

    // --- Cấu hình thông tin AWS của bạn ---
    private const string awsAccessKeyId = "2d6f7967a22c7b43abb8cb5e01fc2f27";
    private const string awsSecretAccessKey = "4319813f532d8bfa6fad28d45e1b6a16776ffe452d2cc2326dcb4060ef5ceb40";
    private const string bucketName = "school"; // Tên bucket của bạn
    // Thay thế bằng Region của bucket bạn, ví dụ: USWest2, EUCentral1
    private static readonly RegionEndpoint bucketRegion = RegionEndpoint.APEast1;
    private static IAmazonS3 s3Client;
    private const string accountId = "44d21aeec82759f044d3a20e118207d9";
    /// <summary>
    /// Tải tệp lên Amazon S3 và trả về URL của tệp.
    /// </summary>
    /// <param name="filePath">Đường dẫn đến tệp cục bộ cần tải lên.</param>
    /// <param name="s3Key">Tên (khóa) của đối tượng trên S3. Có thể bao gồm cả tiền tố (thư mục).</param>
    /// <returns>URL công khai của tệp đã tải lên.</returns>
    public async Task<string> UploadFileAsync(byte[] fileBytes, string s3Key)
    {
       
        try
        {

            var config = new AmazonS3Config
            {
                // Điểm cuối của Cloudflare R2
                ServiceURL = $"https://{accountId}.r2.cloudflarestorage.com",
                // Chữ ký phiên bản 4 là bắt buộc cho R2
                //  SignatureVersion = "4",
                // R2 không sử dụng region của AWS, nhưng SDK yêu cầu một giá trị. 
                // 'us-east-1' là giá trị mặc định an toàn.
                AuthenticationRegion = "auto", // Hoặc có thể để trống
            };

            s3Client = new AmazonS3Client(awsAccessKeyId, awsSecretAccessKey, config);



            using (var stream = new MemoryStream(fileBytes))
            {
                var request = new PutObjectRequest
                {
                    BucketName = bucketName,
                    Key = s3Key, // Key là tên của đối tượng trên S3
                    InputStream = stream, // Sử dụng InputStream thay vì FilePath
                    DisablePayloadSigning = true,
                    DisableDefaultChecksumValidation = true
                };

                var response = await s3Client.PutObjectAsync(request);

                // Kiểm tra xem việc tải lên có thành công không (tùy chọn nhưng được khuyến nghị)
                if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                {
                    string fileUrl = $"https://school.vietnamai.com.vn/{s3Key}";
                    return fileUrl;
                }
                else
                {
                    Console.WriteLine($"Lỗi khi tải tệp lên: HTTP {response.HttpStatusCode}");
                    return null;
                }
            }
            //var request = new PutObjectRequest
            //{
            //    FilePath = filePath,
            //    BucketName = bucketName,
            //    DisablePayloadSigning = true,
            //    DisableDefaultChecksumValidation = true
            //};
            //var request = new PutObjectRequest
            //{
            //    BucketName = bucketName,
            //    Key = s3Key, // Key là tên của đối tượng trên S3
            //    InputStream = stream, // Sử dụng InputStream thay vì FilePath
            //    DisablePayloadSigning = true,
            //    DisableDefaultChecksumValidation = true
            //};
            //var response = await s3Client.PutObjectAsync(request);

            //string fileUrl = $"https://school.vietnamai.com.vn/{s3Key}";
            //return fileUrl;
        }
        catch (AmazonS3Exception e)
        {
            Console.WriteLine("Lỗi khi tải tệp lên S3: " + e.Message);
            return null;
        }
      
    }
    public async Task<bool> CreateFolderAsync(string folderName)
    {
        try
        {
            var config = new AmazonS3Config
            {
                // Điểm cuối của Cloudflare R2
                ServiceURL = $"https://{accountId}.r2.cloudflarestorage.com",
                // Chữ ký phiên bản 4 là bắt buộc cho R2
                //  SignatureVersion = "4",
                // R2 không sử dụng region của AWS, nhưng SDK yêu cầu một giá trị. 
                // 'us-east-1' là giá trị mặc định an toàn.
                AuthenticationRegion = "auto", // Hoặc có thể để trống
            };

            s3Client = new AmazonS3Client(awsAccessKeyId, awsSecretAccessKey, config);
            // Đảm bảo tên thư mục kết thúc bằng dấu gạch chéo "/"
            if (!folderName.EndsWith("/"))
            {
                folderName += "/";
            }

            var request = new PutObjectRequest
            {
                BucketName = bucketName,
                Key = folderName, // Tên khóa chính là tên thư mục
                InputStream = new MemoryStream() // Nội dung trống
            };

            var response = await s3Client.PutObjectAsync(request);

            // Kiểm tra xem yêu cầu có thành công không
            return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
        }
        catch (AmazonS3Exception e)
        {
            Console.WriteLine($"Lỗi khi tạo thư mục S3: {e.Message}");
            return false;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Đã xảy ra lỗi không mong muốn: {e.Message}");
            return false;
        }
    }

    // --- PHƯƠNG THỨC MỚI ĐỂ ĐỌC FILE TỪ S3 ---
    public async Task<string> DownloadFileAsStringAsync(string keyName)
    {
        try
        {
            var request = new GetObjectRequest
            {
                BucketName = bucketName,
                Key = keyName
            };

            var config = new AmazonS3Config
            {
                // Điểm cuối của Cloudflare R2
                ServiceURL = $"https://{accountId}.r2.cloudflarestorage.com",
                // Chữ ký phiên bản 4 là bắt buộc cho R2
                //  SignatureVersion = "4",
                // R2 không sử dụng region của AWS, nhưng SDK yêu cầu một giá trị. 
                // 'us-east-1' là giá trị mặc định an toàn.
                AuthenticationRegion = "auto", // Hoặc có thể để trống
            };

            s3Client = new AmazonS3Client(awsAccessKeyId, awsSecretAccessKey, config);

            // Sử dụng 'using' để đảm bảo response được giải phóng đúng cách
            using (var response = await s3Client.GetObjectAsync(request))
            using (var responseStream = response.ResponseStream)
            // Sử dụng StreamReader để đọc stream, và phải chỉ định đúng Encoding
            // Vì lúc ghi bạn dùng UTF8, lúc đọc cũng phải dùng UTF8
            using (var reader = new StreamReader(responseStream, Encoding.UTF8))
            {
                // Đọc toàn bộ nội dung của file thành một chuỗi
                return await reader.ReadToEndAsync();
            }
        }
        catch (AmazonS3Exception e)
        {
            // Xử lý trường hợp file không tồn tại
            if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null; // hoặc throw new FileNotFoundException(...)
            }
            // Ném lại lỗi nếu là lỗi khác
            throw;
        }
    }
}