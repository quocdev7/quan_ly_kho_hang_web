using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;


namespace vnaisoft.DataBase.Mongodb.Collection.system
{
    [Table("sys_don_hang_mua_mat_hang_col")]
    public class sys_don_hang_mua_mat_hang_col
    {
        [BsonId]
        public string id { get; set; }
        public string id_don_hang { get; set; }
        public string id_mat_hang { get; set; }
        public string id_loai_mat_hang { get; set; }
        public string ten_mat_hang { get; set; }
        public string id_don_vi_tinh { get; set; }
        [BsonRepresentation(BsonType.Decimal128)] public decimal? so_luong { get; set; }
        [BsonRepresentation(BsonType.Decimal128)] public decimal? don_gia { get; set; }
        public string vat { get; set; }
        [BsonRepresentation(BsonType.Decimal128)] public decimal? thanh_tien { get; set; }

        public string ghi_chu { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)] public DateTime? ngay_tao { get; set; }
        public string nguoi_tao { get; set; }
        public string nguoi_cap_nhat { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)] public DateTime? ngay_cap_nhat { get; set; }
        public int? status_del { get; set; }
    }
}
