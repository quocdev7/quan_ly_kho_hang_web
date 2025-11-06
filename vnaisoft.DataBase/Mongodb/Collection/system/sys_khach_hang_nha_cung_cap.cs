using MongoDB.Bson.Serialization.Attributes;
using System;
using System.ComponentModel.DataAnnotations.Schema;


namespace vnaisoft.DataBase.Mongodb.Collection.system
{
    [Table("sys_khach_hang_nha_cung_cap_col")]
    public class sys_khach_hang_nha_cung_cap_col
    {
        [BsonId]
        public string id { get; set; }
        public string ma { get; set; }
        public bool? laNhaCungCap { get; set; }
        public bool? laKhachHang { get; set; }

        //1 = cá nhân 2 = tổ chức 3 = phòng ban 4 nhân viên 5 = khách hàng 6 nhà cung cấp
        public int? hinh_thuc { get; set; }
        public string ten { get; set; }
        public string ten_khong_dau { get; set; }
        public string ma_so_thue { get; set; }
        public string dien_thoai { get; set; }
        public string email { get; set; }
        public string dia_chi { get; set; }
        public string ghi_chu { get; set; }
        public string so_tai_khoan { get; set; }
        public string id_ngan_hang { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)] public DateTime? ngay_tao { get; set; }
        public string nguoi_tao { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)] public DateTime? ngay_cap_nhat { get; set; }
        public string nguoi_cap_nhat { get; set; }
        public int? status_del { get; set; }
    }
}
