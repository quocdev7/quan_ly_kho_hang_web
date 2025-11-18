using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;


namespace vnaisoft.DataBase.Mongodb.Collection.system
{
    [Table("sys_phieu_xuat_kho_col")]
    public class sys_phieu_xuat_kho_col
    {
        [BsonId]
        public string id { get; set; }
        public string ma { get; set; }
        public string ten { get; set; }
        public string ten_khong_dau { get; set; }
        public string id_don_hang_ban { get; set; }
        public string id_don_hang_mua { get; set; }
        //1 don hang ban, 2 don hang mua,
        public int? nguon { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)] public DateTime? ngay_xuat { get; set; }
        public string id_loai_xuat { get; set; }
        public string ghi_chu { get; set; }
        public string nguoi_cap_nhat { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)] public DateTime? ngay_cap_nhat { get; set; }
        public int? status_del { get; set; }

        //public bool? is_sinh_tu_dong { get; set; }
        //public int? loai { get; set; }
        //public string so_phieu { get; set; }
    }
}
