using quan_ly_kho.DataBase.Mongodb;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;
using System;
using System.IO;
namespace quan_ly_kho.DataBase
{
    public class ChatAILib
    {
        MongoDBContext _contextMongo;
        string user_id;
        public ChatAILib(MongoDBContext _Context, string _user_id)
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
