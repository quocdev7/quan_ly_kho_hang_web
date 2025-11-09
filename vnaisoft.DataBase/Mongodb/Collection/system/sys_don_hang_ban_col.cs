using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.ComponentModel.DataAnnotations.Schema;


namespace vnaisoft.DataBase.Mongodb.Collection.system
{
    [Table("sys_don_hang_ban_col")]
    public class sys_don_hang_ban_col
    {
        [BsonId]
        public string id { get; set; }
        public string ma { get; set; }
        public string ten { get; set; }
        public string ten_khong_dau { get; set; }
        public string id_khach_hang_nha_cung_cap { get; set; }
        public int? phuong_thuc_thanh_toan { get; set; }
        public string id_tai_khoan_ngan_hang { get; set; }
        public string ma_ngan_hang { get; set; }
        public string so_tai_khoan { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)] public DateTime? ngay_dat_hang { get; set; }
        [BsonRepresentation(BsonType.Decimal128)] public decimal? tong_thanh_tien { get; set; }
        public string ghi_chu { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)] public DateTime? ngay_tao { get; set; }
        public string nguoi_tao { get; set; }
        public string nguoi_cap_nhat { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)] public DateTime? ngay_cap_nhat { get; set; }
        public int? status_del { get; set; }
        //public string id { get; set; }

        //public string ma { get; set; }

        //public string ten { get; set; }
        //public string ten_khong_dau { get; set; }
        //public string ma_bao_gia { get; set; }
        //// 1 bán sĩ ,  2 bán lẻ
        //public int? kieu_ban { get; set; }

        //// 1 hàng hóa,  2 dịch vụ
        //public int? loai_giao_dich { get; set; }
        //[BsonRepresentation(BsonType.Decimal128)] public decimal? thanh_tien_truoc_thue { get; set; }
        //[BsonRepresentation(BsonType.Decimal128)] public decimal? tien_thue { get; set; }
        //public string ghi_chu { get; set; }

        //// 1 tiền mặt 2 chuyển khoản 3 ví điện tử
        //public int? phuong_thuc_thanh_toan { get; set; }
        //// thuộc phương thức thanh toán 1
        //[BsonRepresentation(BsonType.Decimal128)] public decimal? tien_khach_hang_tra { get; set; }
        //[BsonRepresentation(BsonType.Decimal128)] public decimal? tien_tra_lai_khach { get; set; }

        //// thuộc phương thức thanh toán 3 ( 1 momo, 2 zalopay, 3 payoo, 5 shopee pay, 6 moca )
        //public int? vi_dien_tu { get; set; }
        //// thuộc phương thức thanh toán 2
        //public string id_tai_khoan_ngan_hang { get; set; }
        //public string ma_ngan_hang { get; set; }
        //public string so_tai_khoan { get; set; }

        ////1 tai chỗ, 2 giao hàng
        //public int? hinh_thuc_van_chuyen { get; set; }

        //public string id_kho_xuat_ban_le { get; set; }
        //[BsonRepresentation(BsonType.Decimal128)] public decimal? tien_van_chuyen { get; set; }
        //public string vat_van_chuyen { get; set; }
        //[BsonRepresentation(BsonType.Decimal128)] public decimal? tien_vat_van_chuyen { get; set; }
        //[BsonRepresentation(BsonType.Decimal128)] public decimal? tien_khac { get; set; }
        //public string vat_khac { get; set; }
        //[BsonRepresentation(BsonType.Decimal128)] public decimal? tien_vat_khac { get; set; }
        //[BsonDateTimeOptions(Kind = DateTimeKind.Local)] public DateTime? ngay_dat_hang { get; set; }
        //public int? so_ngay_du_kien { get; set; }
        //[BsonDateTimeOptions(Kind = DateTimeKind.Local)] public DateTime? ngay_du_kien_giao_hang { get; set; }
        //[BsonDateTimeOptions(Kind = DateTimeKind.Local)] public DateTime? ngay_giao_hang { get; set; }
        //public string dia_chi_giao_hang { get; set; }
        //public string so_dien_thoai_nguoi_nhan { get; set; }

        //// 1 cá nhân, 2 tổ chức  
        //public int? hinh_thuc_doi_tuong { get; set; }
        //public string id_doi_tuong { get; set; }
        //public string ten_doi_tuong { get; set; }
        //public string ma_so_thue { get; set; }

        //public string dien_thoai { get; set; }

        //public string email { get; set; }

        //public string dia_chi_doi_tuong { get; set; }
        //[BsonRepresentation(BsonType.Decimal128)] public decimal? thanh_tien_sau_thue { get; set; }
        //public string id_file_upload { get; set; }

        //// 1 Báo giá, 2 Hợp đồng,3 Hoàn tất
        //public string tinh_trang_don_hang { get; set; }

        //[BsonRepresentation(BsonType.Decimal128)] public decimal? tong_tien_truoc_thue { get; set; }

        //[BsonRepresentation(BsonType.Decimal128)] public decimal? tong_tien_chiet_khau { get; set; }
        //[BsonRepresentation(BsonType.Decimal128)] public decimal? tong_tien_sau_chiet_khau { get; set; }

        //[BsonRepresentation(BsonType.Decimal128)] public decimal? tong_tien_thue { get; set; }
        //[BsonRepresentation(BsonType.Decimal128)] public decimal? tong_tien_sau_thue { get; set; }


        //public string id_hoa_don { get; set; }
        //public bool? is_sinh_tu_dong { get; set; }
        //public bool? is_xuat_du { get; set; }
        //public bool? is_thu_du { get; set; }
        //public bool? is_hang_tang { get; set; }

        //public string id_don_vi_van_chuyen { get; set; }
        //public string ma_don_van_chuyen { get; set; }
        //[BsonDateTimeOptions(Kind = DateTimeKind.Local)] public DateTime? ngay_den_han_thanh_toan { get; set; }
        //[BsonRepresentation(BsonType.Decimal128)] public decimal? so_tien_da_thu { get; set; }
        //[BsonRepresentation(BsonType.Decimal128)] public decimal? so_tien_da_chi { get; set; }

        //[BsonDateTimeOptions(Kind = DateTimeKind.Local)] public DateTime? ngay_tao { get; set; }
        //public string nguoi_tao { get; set; }
        //public string nguoi_cap_nhat { get; set; }
        //[BsonDateTimeOptions(Kind = DateTimeKind.Local)] public DateTime? ngay_cap_nhat { get; set; }
        //public int? status_del { get; set; }
    }
}
