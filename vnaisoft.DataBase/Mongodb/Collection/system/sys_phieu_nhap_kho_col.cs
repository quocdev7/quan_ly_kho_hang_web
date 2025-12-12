using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;


namespace quan_ly_kho.DataBase.Mongodb.Collection.system
{
    [Table("sys_phieu_nhap_kho_col")]
    public class sys_phieu_nhap_kho_col
    {
        [BsonId]
        public string id { get; set; }
        public string ma { get; set; }
        public string ten { get; set; }
        public string ten_khong_dau { get; set; }
        //1 don hang ban, 2 don hang mua,
        public int? nguon { get; set; }
        public string id_don_hang_ban { get; set; }
        public string id_don_hang_mua { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)] public DateTime? ngay_nhap { get; set; }
        /// <summary>
        /// NTRH : Nhập trả hàng
        /// NM : nhập mua
        /// </summary>
        public string id_loai_nhap { get; set; }
        public string ghi_chu { get; set; }
        public string nguoi_cap_nhat { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)] public DateTime? ngay_cap_nhat { get; set; }
        public int? status_del { get; set; }
    }
    
}
