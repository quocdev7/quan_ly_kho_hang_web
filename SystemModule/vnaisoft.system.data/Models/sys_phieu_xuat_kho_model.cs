using System.Collections.Generic;
using System.Linq;
using vnaisoft.common.Helpers;
using vnaisoft.DataBase.Mongodb.Collection.system;

namespace vnaisoft.system.data.Models
{
    public class sys_phieu_xuat_kho_model
    {
        public sys_phieu_xuat_kho_model()
        {
            db = new sys_phieu_xuat_kho_col();
            list_mat_hang = new List<sys_phieu_xuat_kho_chi_tiet_model>();
        }
        public sys_phieu_xuat_kho_col db { get; set; }
        public string ly_do_chinh_sua { get; set; }
        public string ten_nguoi_cap_nhat { get; set; }
        public string ngay_cap_nhap_str { get; set; }
        public string ngay_xuat_str { get; set; }
        public string ten_loai_xuat { get; set; }
        public string ten_kho { get; set; }
        public string ma_chuyen_kho { get; set; }
        public string ma_don_hang { get; set; }
        public string ma_loai_xuat { get; set; }
        public string ten_hinh_thuc { get; set; }
        public int? loai_giao_dich { get; set; }
        public decimal? tong_so_luong { get; set; }
        public string ten_mat_hang { get; set; }
        public decimal? tong_thanh_tien { get; set; }
        public decimal? tong_thanh_tien_thue { get; set; }
        public decimal? tong_thanh_tien_gia_von { get; set; }
        public string ma_kho { get; set; }
        public string ghi_chu_detail { get; set; }
        public string ma_mat_hang { get; set; }
        public long? so_luong { get; set; }
        public decimal? don_gia { get; set; }
        public string tai_khoan_no { get; set; }
        public string doi_tuong_no { get; set; }
        public string tai_khoan_co { get; set; }
        public string doi_tuong_co { get; set; }
        public string ten_nguon
        {
            get
            {
                return Constant.list_nguon.Where(q => q.id == db.nguon.ToString()).Select(q => q.name).SingleOrDefault();
            }
        }
        //public string tong_tien_bang_chu { get; set; }

        public List<sys_phieu_xuat_kho_chi_tiet_model> list_mat_hang { get; set; }
        public int? check_doi_tuong { get; set; }
        public bool? is_sinh_tu_dong { get; set; }

    }
}

