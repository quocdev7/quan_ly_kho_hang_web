using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace quan_ly_kho.DataBase.System
{
    [Table("Users")]
    public class User
    {

        [BsonId]
        public string id { get; set; }
        public string ho_va_ten { get; set; }
        public string hinh_anh_dai_dien { get; set; }
        public int? loai { get; set; }

        //2 thirdparty,3 email, 4 sdt
        public int? loai_dang_nhap { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)] public DateTime? ngay_login { get; set; }

        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string so_dien_thoai { get; set; }
        public string email { get; set; }
        public string dia_chi { get; set; }
        public int? status_del { get; set; }

        public string otp { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)] public DateTime? ngay_tao { get; set; }
        public string nguoi_tao { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)] public DateTime? ngay_cap_nhat { get; set; }
        public string nguoi_cap_nhat { get; set; }
    }
}