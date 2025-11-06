using MongoDB.Bson.Serialization.Attributes;
using System;
using System.ComponentModel.DataAnnotations.Schema;


namespace vnaisoft.DataBase.Mongodb.Collection.system
{
    [Table("sys_cau_hinh_ma_he_thong_col")]
    public class sys_cau_hinh_ma_he_thong_col
    {
        [BsonId]
        public string id { get; set; }
        public string controller { get; set; }
        public string tien_to { get; set; }
        public int? so_chu_so_tu_tang { get; set; }
        public bool? is_ngay_gio { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)] public DateTime? ngay_tao { get; set; }
        public string nguoi_tao { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)] public DateTime? ngay_cap_nhat { get; set; }
        public string nguoi_cap_nhat { get; set; }
        public int? status_del { get; set; }




    }
}
