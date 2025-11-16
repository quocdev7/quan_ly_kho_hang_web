using MongoDB.Driver;
using System;
using System.Linq;
using System.Threading.Tasks;


using vnaisoft.DataBase.Mongodb;

namespace vnaisoft.DataBase.commonFunc
{


    public class common_mongo_repo
    {
        public MongoDBContext _context;

        public common_mongo_repo(MongoDBContext context)
        {
            _context = context;
        }
        public async Task get_data(string name)
        {

        }

        public class commonConfigCode
        {
            public string prefix { get; set; }
            public int numIncrease { get; set; }
        }
        public string GenerateRandomCode(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            Random random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                                        .Select(s => s[random.Next(s.Length)])
                                        .ToArray());
        }



        public string get_anh_dai_dien(string hinh_anh_dai_dien, int? type)
        {
            var id = _context.sys_cau_hinh_anh_mac_dinh_col.AsQueryable().Where(q => q.type == type).Select(q => q.image).SingleOrDefault();
            var hinh_anh_dai_dien_default = _context.sys_file_upload_col.AsQueryable().Where(q => q.id == id).Select(q => q.file_path).SingleOrDefault();

            hinh_anh_dai_dien = hinh_anh_dai_dien == null ? hinh_anh_dai_dien_default : hinh_anh_dai_dien;
            return hinh_anh_dai_dien;
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
        public commonConfigCode get_code_config(bool isDanhMuc, string maController, string defaultPrefix)
        {
            if (isDanhMuc)
            {

                var sotutang = 6;
                return new commonConfigCode
                {
                    prefix = defaultPrefix,
                    numIncrease = sotutang
                };
            }
            else
            {
                string nam = DateTime.Now.ToString("yyMMdd");
                var sotutang = 4;
                return new commonConfigCode
                {
                    prefix = defaultPrefix + nam,
                    numIncrease = sotutang
                };
            }

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

        //public async Task<int> insert_file(string idPhieu, string controller)
        //{
        //    var db = new erp_file_upload_db();
        //    db.id = idPhieu + controller;
        //    db.controller = controller;
        //    db.id_phieu = idPhieu;
        //    await _context.erp_file_uploads.InsertOneAsync(db);
        //    return 1;

        //}
    }
}
