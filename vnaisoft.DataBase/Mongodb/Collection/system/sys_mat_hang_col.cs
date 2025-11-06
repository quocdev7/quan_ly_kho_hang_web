using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;


namespace vnaisoft.DataBase.Mongodb.Collection.system
{
    [Table("sys_mat_hang_col")]
    public class sys_mat_hang_col
    {
        [BsonId]
        public string id { get; set; }
        public string ma { get; set; }
        public string ten { get; set; }
        public string ten_khong_dau { get; set; }
        public string tien_te { get; set; }
        public string id_loai_mat_hang { get; set; }
        public string id_don_vi_tinh { get; set; }
        //[BsonRepresentation(BsonType.Decimal128)]
        public decimal? gia_ban_le { get; set; }
        //[BsonRepresentation(BsonType.Decimal128)]
        public decimal? gia_ban_si { get; set; }
        //[BsonRepresentation(BsonType.Decimal128)]
        public string vat { get; set; }
        public string ghi_chu { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)] public DateTime? ngay_tao { get; set; }
        public string nguoi_tao { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)] public DateTime? ngay_cap_nhat { get; set; }
        public string nguoi_cap_nhat { get; set; }
        public int? status_del { get; set; }
    }
}
