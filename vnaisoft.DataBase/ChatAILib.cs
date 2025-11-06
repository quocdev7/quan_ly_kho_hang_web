using Google.Api.Gax.Grpc;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.AIPlatform.V1;
using Google.Protobuf;
using MongoDB.Driver;
using OpenAI.Chat;
using OpenAI.Responses;
using Syncfusion.Pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using TiktokenSharp;
using vnaisoft.DataBase.Mongodb;
using vnaisoft.DataBase.Mongodb.Collection.HocAI;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;
namespace vnaisoft.DataBase
{
    public class ChatAILib
    {
        MongoDBContext _contextMongo;
        String user_id;
        public ChatAILib(MongoDBContext _Context,string _user_id)
        {
            _contextMongo = _Context;
            user_id = _user_id;
        }
        private long getCurrentEpochMicrosecond()
        {
            var currentTime = DateTime.UtcNow;
            long unixTimeMilliseconds = new DateTimeOffset(currentTime).ToUnixTimeMilliseconds();
            return unixTimeMilliseconds * 1000;
        }
        public byte[] CompressImage(string filePath, int maxWidth, int maxHeight)
        {
            if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
            {
                throw new Exception("Lỗi: Đường dẫn tệp không hợp lệ hoặc tệp không tồn tại.");
            }

            using (var image = Image.Load(filePath))
            {
                image.Mutate(x => x.Resize(new ResizeOptions
                {
                    Size = new Size(maxWidth, maxHeight),
                    Mode = ResizeMode.Max
                }));

                using (var outputStream = new MemoryStream())
                {
                    var encoder = new PngEncoder
                    {
                        CompressionLevel = PngCompressionLevel.Level9
                    };

                    image.Save(outputStream, encoder);

                    // Nếu file vẫn > 1MB, giảm kích thước ảnh
                    while (outputStream.Length > 1024 * 1024 && maxWidth > 200)
                    {
                        maxWidth -= 100;
                        maxHeight -= 100;
                        outputStream.SetLength(0); // Xóa dữ liệu cũ

                        image.Mutate(x => x.Resize(new ResizeOptions
                        {
                            Size = new Size(maxWidth, maxHeight),
                            Mode = ResizeMode.Max
                        }));

                        image.Save(outputStream, encoder);
                    }

                    return outputStream.ToArray();
                }
            }
        }

    }
}
