using MongoDB.Bson.Serialization.Attributes;
using System;
using System.ComponentModel.DataAnnotations.Schema;


namespace vnaisoft.DataBase.Mongodb.Collection.system
{
    [Table("sys_loai_nhap_xuat_col")]
    public class sys_loai_nhap_xuat_col
    {
        [BsonId]
        public string id { get; set; }
        //loai == 1 nhap
        //loai == 2 xuat
        public int? loai { get; set; }
        public string ma { get; set; }
        public string ten { get; set; }
        public string ghi_chu { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)] public DateTime? ngay_tao { get; set; }
        public string nguoi_tao { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)] public DateTime? ngay_cap_nhat { get; set; }
        public string nguoi_cap_nhat { get; set; }
        public int? status_del { get; set; }
        // 1 don hang ban, 2 don hang mua,
        public string nguon { get; set; }
    }
}
