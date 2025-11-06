using System.Collections.Generic;
using System.Linq;
using vnaisoft.common.Helpers;
using vnaisoft.DataBase.Mongodb.Collection.system;

namespace vnaisoft.system.data.Models
{
    public class sys_phieu_nhap_kho_model
    {
        public sys_phieu_nhap_kho_model()
        {
            db = new sys_phieu_nhap_kho_col();
            list_mat_hang = new List<sys_phieu_nhap_kho_chi_tiet_model>();
        }
        public sys_phieu_nhap_kho_col db { get; set; }
        public string ten_nguoi_cap_nhat { get; set; }
        public string ngay_cap_nhap_str { get; set; }
        public string ngay_nhap_str { get; set; }
        public string ngay_nhap { get; set; }
        public string ten_loai_nhap { get; set; }
        public string ma_mat_hang { get; set; }
        public string ten_mat_hang { get; set; }
        public string ma_don_hang { get; set; }
        public string ma_loai_nhap { get; set; }
        public string ten_hinh_thuc { get; set; }
        public string ten_nguon
        {
            get
            {
                return Constant.list_nguon.Where(q => q.id == db.nguon.ToString()).Select(q => q.name).SingleOrDefault();
            }
        }

        public List<sys_phieu_nhap_kho_chi_tiet_model> list_mat_hang { get; set; }
        public int? check_doi_tuong { get; set; }
    }
    
}


