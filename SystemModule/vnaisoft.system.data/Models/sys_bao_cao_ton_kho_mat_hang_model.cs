using System;

namespace quan_ly_kho.system.data.Models
{
    public class bao_cao_ton_kho_mat_hang_model
    {

        public string ma_loai_mat_hang { get; set; }
        public string id_loai_mat_hang { get; set; }
        public string ten_loai_mat_hang { get; set; }
        public string ma_mat_hang { get; set; }
        public string ten_mat_hang { get; set; }
        public string id_don_vi_tinh { get; set; }
        public string ten_don_vi_tinh { get; set; }
        public decimal? so_luong_ton { get; set; }
        public DateTime? ngay_cap_nhat { get; set; }
        public string so_luong_ton_str { get; set; }
    }

}

