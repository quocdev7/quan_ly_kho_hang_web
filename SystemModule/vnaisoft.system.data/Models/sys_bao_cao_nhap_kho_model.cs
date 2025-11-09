using System;

namespace vnaisoft.system.data.Models
{
    public class bao_cao_nhap_kho_model
    {
        public string ma_loai_nhap { get; set; }
        public string ten_loai_nhap { get; set; }
        public string ma_loai_mat_hang { get; set; }
        public string id_loai_mat_hang { get; set; }
        public string ten_loai_mat_hang { get; set; }
        public string ma_mat_hang { get; set; }
        public string ten_mat_hang { get; set; }
        public string ten_don_vi_tinh { get; set; }
        public decimal? so_luong { get; set; }
        public decimal? don_gia { get; set; }
        public decimal? gia_tri { get; set; }
        public string id_don_vi_tinh { get; set; }
        public DateTime? ngay_cap_nhat { get; set; }
        public string id_phieu_nhap_kho { get; set; }
        public string ma_phieu_nhap_kho { get; set; }
        public DateTime? ngay_nhap_kho { get; set; }
        public decimal? tong_so_luong { get; set; }
        public int? status_del { get; set; }
    }

}



