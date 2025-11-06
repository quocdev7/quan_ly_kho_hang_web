using vnaisoft.DataBase.Mongodb.Collection.system;

namespace vnaisoft.system.data.Models
{
    public class sys_cau_hinh_anh_mac_dinh_model
    {
        public sys_cau_hinh_anh_mac_dinh_model()
        {
            db = new sys_cau_hinh_anh_mac_dinh_col();
            file = new sys_file_upload_col();
            file_avartar = new sys_file_upload_col();

        }
        public sys_cau_hinh_anh_mac_dinh_col db { get; set; }
        public string nguoi_cap_nhat { get; set; }
        public string nguoi_tao { get; set; }

        public string image { get; set; }
        public string avatar { get; set; }
        public sys_file_upload_col file { get; set; }
        public sys_file_upload_col file_avartar { get; set; }
    }
}


