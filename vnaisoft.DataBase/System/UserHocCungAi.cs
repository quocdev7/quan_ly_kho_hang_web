using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace vnaisoft.DataBase.System
{
    [Table("Users")]
    public class UserHocCungAi
    {

        [BsonId]
        public string id { get; set; }
        public string ho_va_ten { get; set; }
        public string hinh_anh_dai_dien { get; set; }
        public string hinh_anh_bia { get; set; }

        /// <summary>
        /// 1 staff admin, 
        /// 2 teacher, 
        /// 3 parent, 
        /// 4 học sinh
        /// </summary>
        public int? loai { get; set; }

        //2 thirdparty,3 email, 4 sdt
        public int? loai_dang_nhap { get; set; }

        public string googleaccount { get; set; }
        public string facebookaccount { get; set; }
        public string appleId { get; set; }
        public string appleToken { get; set; }

        public string googleaccounttoken { get; set; }
        public string facebookaccounttoken { get; set; }
        public List<string> login_from_app { get; set; }

        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string so_dien_thoai { get; set; }
        public string email { get; set; }
        public string dia_chi { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)] public DateTime? ngay_login { get; set; }
        // 1 da xac thuc otp
        // 3 chua xac thuc
        public int? status_del { get; set; }
        public int? so_chuoi { get; set; }
        public int? so_diem { get; set; }

        public int? so_lan_cham_bai_ai { get; set; }
        public int? thu_hang { get; set; }
        public string token_reset_pass { get; set; }


        [BsonDateTimeOptions(Kind = DateTimeKind.Local)] public DateTime? expiration_date_reset_pass { get; set; }
        public string token_notification { get; set; }
        public string otp { get; set; }
        public string ly_do { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)] public DateTime? ngay_tao { get; set; }
        public string nguoi_tao { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)] public DateTime? ngay_cap_nhat { get; set; }
        public string nguoi_cap_nhat { get; set; }
        public string id_khoi_lop { get; set; }

        //1 thi lop 10, 2 thi tot nghiep 12
        public int? id_ky_thi { get; set; }
        // -1 toan quoc
        public string id_tinh_thanh { get; set; }
        public bool? is_agree_term { get; set; }
        public bool? is_view_all_book { get; set; }
        public bool? is_view_all_chuong_trinh { get; set; }

        public string token_so { get; set; }
        public string ma_truong { get; set; }
    }
}