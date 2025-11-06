using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using ZXing;
using ZXing.Windows.Compatibility;

namespace vnaisoft.common.Helpers
{
    public  class BarCodeHelper
    {
        public  byte[] geneRateQRCodeLink(string url)
        {
            var writer = new BarcodeWriter<Bitmap> { Format = BarcodeFormat.QR_CODE };
            //var writer = new ZXing.BarcodeWriter();
            //writer.Format = BarcodeFormat.QR_CODE;
            var result = writer.Write(url);
            var barcodeBitmap = new Bitmap(result);
            using (var stream = new MemoryStream())
            {
                barcodeBitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
                return stream.ToArray();
            }
        }
        public  string readBarCode(string pathFile)
        {
            var reader = new BarcodeReader();
            var result = reader.Decode(new Bitmap(pathFile));
            return result.Text;
        }
        public  string readBarCode(byte[] blob)
        {
            var reader = new BarcodeReader();
            var result = reader.Decode(ByteToImage(blob));
            return result.Text;
        }

        public  Bitmap ByteToImage(byte[] blob)
        {
            MemoryStream mStream = new MemoryStream();
            byte[] pData = blob;
            mStream.Write(pData, 0, Convert.ToInt32(pData.Length));
            Bitmap bm = new Bitmap(mStream);
            mStream.Dispose();
            return bm;

        }

    }
}