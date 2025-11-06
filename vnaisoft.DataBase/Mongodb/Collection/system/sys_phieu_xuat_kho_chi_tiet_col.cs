using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;


namespace vnaisoft.DataBase.Mongodb.Collection.system
{
    [Table("sys_phieu_xuat_kho_chi_tiet_col")]
    public class sys_phieu_xuat_kho_chi_tiet_col
    {
        [BsonId]
        public string id { get; set; }
        public string id_phieu_xuat_kho { get; set; }
        public string id_mat_hang { get; set; }
        public string id_loai_mat_hang { get; set; }
        public string ten_mat_hang { get; set; }
        //public string ma_vach { get; set; }
        public string id_don_vi_tinh { get; set; }
        [BsonRepresentation(BsonType.Decimal128)] public decimal? so_luong { get; set; }
        [BsonRepresentation(BsonType.Decimal128)] public decimal? don_gia { get; set; }
        [BsonRepresentation(BsonType.Decimal128)] public decimal? gia_tri { get; set; }
        [BsonRepresentation(BsonType.Decimal128)] public decimal? don_gia_von { get; set; }
        [BsonRepresentation(BsonType.Decimal128)] public decimal? gia_tri_gia_von { get; set; }
        public string ghi_chu { get; set; }
        public string nguoi_cap_nhat { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)] public DateTime? ngay_cap_nhat { get; set; }
        //public Boolean? is_dinh_khoan { get; set; }
        //public string tai_khoan_no { get; set; }
        //public string doi_tuong_no { get; set; }
        //public string tai_khoan_co { get; set; }
        //public string doi_tuong_co { get; set; }
        //public string tai_khoan_no_gia_von { get; set; }
        //public string tai_khoan_co_gia_von { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)] public DateTime? ngay_xuat { get; set; }

        public int? status_del { get; set; }

        //public string id_kho { get; set; }
        public string id_loai_xuat { get; set; }
        //public bool? is_vat { get; set; }
        //public decimal? ty_suat_vat { get; set; }

    }

}
