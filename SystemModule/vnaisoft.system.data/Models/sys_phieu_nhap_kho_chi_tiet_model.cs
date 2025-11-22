using System;
using vnaisoft.DataBase.Mongodb.Collection.system;

namespace vnaisoft.system.data.Models
{
    public class sys_phieu_nhap_kho_chi_tiet_model
    {
        public sys_phieu_nhap_kho_chi_tiet_model()
        {
            db = new sys_phieu_nhap_kho_chi_tiet_col();
        }
        public sys_phieu_nhap_kho_chi_tiet_col db { get; set; }
        public string id_deatils_nhap_kho { get; set; }
        public string ten_loai_nhap { get; set; }
        public string trang_thai_str { get; set; }
        public string ngay_cap_nhap_str { get; set; }
        public string ten_nguoi_cap_nhat { get; set; }
        public string ma_don_hang { get; set; }
        public string ten_phieu_nhap { get; set; }
        public string ghi_chu { get; set; }
        public string id_mat_hang { get; set; }
        public string id_don_vi_tinh { get; set; }
        public decimal? don_gia { get; set; }
        public DateTime? ngay_nhap { get; set; }
        public string ten_don_vi_tinh { get; set; }
        public string ten_mat_hang { get; set; }
        public string ma_mat_hang { get; set; }
        public decimal? thanh_tien { get; set; }
        public decimal? gia_mua { get; set; }
        public decimal? don_gia_mua { get; set; }
        public decimal? don_gia_ban { get; set; }
        public string id_don_hang_mua { get; set; }
        public string id_loai_mat_hang { get; set; }
        public string vat { get; set; }
    }
    
}


