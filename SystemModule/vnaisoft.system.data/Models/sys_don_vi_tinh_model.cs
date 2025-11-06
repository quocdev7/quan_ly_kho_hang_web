using vnaisoft.DataBase.Mongodb.Collection.system;

namespace vnaisoft.system.data.Models
{
    public class sys_don_vi_tinh_model
    {
        public sys_don_vi_tinh_model()
        {
            db = new sys_don_vi_tinh_col();
        }
        public sys_don_vi_tinh_col db { get; set; }
        public string nguoi_tao { get; set; }
        public string nguoi_cap_nhat { get; set; }
    }

}
