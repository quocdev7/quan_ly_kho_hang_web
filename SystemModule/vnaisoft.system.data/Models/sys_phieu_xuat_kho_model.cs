using quan_ly_kho.common.Helpers;
using quan_ly_kho.DataBase.Mongodb.Collection.system;
using System.Collections.Generic;
using System.Linq;

namespace quan_ly_kho.system.data.Models
{
    public class sys_phieu_xuat_kho_model
    {
        public sys_phieu_xuat_kho_model()
        {
            db = new sys_phieu_xuat_kho_col();
            list_mat_hang = new List<sys_phieu_xuat_kho_chi_tiet_model>();
        }
        public sys_phieu_xuat_kho_col db { get; set; }
        public string ten_nguoi_cap_nhat { get; set; }
        public string ngay_cap_nhap_str { get; set; }
        public string ngay_xuat_str { get; set; }
        public string ten_loai_xuat { get; set; }
        public string ma_don_hang { get; set; }
        public string ma_loai_xuat { get; set; }
        public decimal? tong_so_luong { get; set; }
        public string ten_mat_hang { get; set; }
        public decimal? tong_thanh_tien { get; set; }
        public string ma_mat_hang { get; set; }
        public long? so_luong { get; set; }
        public decimal? don_gia { get; set; }
        public string ten_nguon
        {
            get
            {
                return Constant.list_nguon.Where(q => q.id == db.nguon.ToString()).Select(q => q.name).SingleOrDefault();
            }
        }
        public List<sys_phieu_xuat_kho_chi_tiet_model> list_mat_hang { get; set; }
    }
}

