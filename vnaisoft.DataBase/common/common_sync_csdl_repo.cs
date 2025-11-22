using MongoDB.Driver;
using System;
using System.Linq;


using vnaisoft.DataBase.Mongodb;
using vnaisoft.DataBase.Mongodb.Collection.system;

namespace vnaisoft.DataBase.commonFunc
{


    public class common_sync_csdl_repo
    {


        public string GenerateRandomCode(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            Random random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                                        .Select(s => s[random.Next(s.Length)])
                                        .ToArray());
        }

        public long? getDateTime(DateTime currentime)
        {
            long unixTimeMilliseconds = new DateTimeOffset(currentime).ToUnixTimeMilliseconds();
            return unixTimeMilliseconds * 1000;
        }
        public DateTime? convertNanoToDate(long nanoseconds)
        {


            long milisecond = (long)(nanoseconds / 1000);
            //long unixTimeMilliseconds = new DateTimeOffset(currentime).ToUnixTimeMilliseconds();

            var datetime =
               DateTimeOffset.FromUnixTimeMilliseconds(milisecond);

            var sourceTime = datetime.LocalDateTime;
            var d = sourceTime.ToLocalTime();
            return d;
        }
        public string NonUnicode(string text)
        {
            string[] arr1 = new string[] { "á", "à", "ả", "ã", "ạ", "â", "ấ", "ầ", "ẩ", "ẫ", "ậ", "ă", "ắ", "ằ", "ẳ", "ẵ", "ặ",
    "đ",
    "é","è","ẻ","ẽ","ẹ","ê","ế","ề","ể","ễ","ệ",
    "í","ì","ỉ","ĩ","ị",
    "ó","ò","ỏ","õ","ọ","ô","ố","ồ","ổ","ỗ","ộ","ơ","ớ","ờ","ở","ỡ","ợ",
    "ú","ù","ủ","ũ","ụ","ư","ứ","ừ","ử","ữ","ự",
    "ý","ỳ","ỷ","ỹ","ỵ",};
            string[] arr2 = new string[] { "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a",
    "d",
    "e","e","e","e","e","e","e","e","e","e","e",
    "i","i","i","i","i",
    "o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o",
    "u","u","u","u","u","u","u","u","u","u","u",
    "y","y","y","y","y",};
            for (int i = 0; i < arr1.Length; i++)
            {
                text = text.Replace(arr1[i], arr2[i]);
                text = text.Replace(arr1[i].ToUpper(), arr2[i].ToUpper());
            }
            return text;
        }
        public string generateCode(string preFixCode, int Num, string max)
        {
            var result = preFixCode;
            int numGenerate = 1;
            for (int i = 0; i < Num; i++)
            {
                numGenerate = numGenerate * 10;
            }
            if (string.IsNullOrEmpty(max))
            {
                result += ((numGenerate + 1) + "").Remove(0, 1);
            }
            else
            {
                var parse = int.Parse(max.Replace(preFixCode, ""));
                result += (numGenerate + (parse + 1)).ToString().Remove(0, 1);
            }
            return result;
        }

    }
}
