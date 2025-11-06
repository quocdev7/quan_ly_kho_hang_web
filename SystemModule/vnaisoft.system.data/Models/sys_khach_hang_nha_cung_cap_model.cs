using vnaisoft.DataBase.Mongodb.Collection.system;

namespace vnaisoft.system.data.Models
{
    public class sys_khach_hang_nha_cung_cap_model
    {
        public sys_khach_hang_nha_cung_cap_model()
        {
            db = new sys_khach_hang_nha_cung_cap_col();
        }
        public sys_khach_hang_nha_cung_cap_col db { get; set; }
        public string nguoi_tao { get; set; }
        public string nguoi_cap_nhat { get; set; }
        public string ten_hinh_thuc { get { return db.hinh_thuc == 1 ? "Cá nhân" : "Tổ chức"; } }
    }

}
