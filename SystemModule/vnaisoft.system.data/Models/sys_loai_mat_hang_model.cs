using vnaisoft.DataBase.Mongodb.Collection.system;

namespace vnaisoft.system.data.Models
{
    public class sys_loai_mat_hang_model
    {
        public sys_loai_mat_hang_model()
        {
            db = new sys_loai_mat_hang_col();
        }
        public string ten_nguoi_cap_nhat { get; set; }
        public sys_loai_mat_hang_col db { get; set; }
    }
}


