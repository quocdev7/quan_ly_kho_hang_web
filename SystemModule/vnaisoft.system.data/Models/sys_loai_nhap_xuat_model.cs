using vnaisoft.DataBase.Mongodb.Collection.system;

namespace vnaisoft.system.data.Models
{
    public class sys_loai_nhap_xuat_model
    {
        public sys_loai_nhap_xuat_model()
        {
            db = new sys_loai_nhap_xuat_col();
        }
        public string ten_nguoi_cap_nhat { get; set; }
        public string ngay_cap_nhat_str { get; set; }
        public string ten_loai { get { return db.loai == 1 ? "Nhập" : "Xuất"; } }
        public sys_loai_nhap_xuat_col db { get; set; }
    }

}
