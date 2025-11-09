using MongoDB.Bson.Serialization.Attributes;
using System;
using System.ComponentModel.DataAnnotations.Schema;


namespace vnaisoft.DataBase.Mongodb.Collection.system
{
    [Table("sys_ton_kho_mat_hang_col")]
    public class sys_ton_kho_mat_hang_col
    {
        [BsonId]
        public string id { get; set; }
        public string id_mat_hang { get; set; }
        public long? so_luong_ton { get; set; }
        public long? gia_tri { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)] public DateTime? ngay_cap_nhat { get; set; }
        public string id_loai_mat_hang { get; set; }
        public string ma_mat_hang { get; set; }
        public string ten_mat_hang { get; set; }
        public string id_don_vi_tinh { get; set; }
        public string ten_don_vi_tinh { get; set; }
    }
}
