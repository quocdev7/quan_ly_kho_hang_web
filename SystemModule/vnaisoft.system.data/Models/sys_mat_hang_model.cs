using vnaisoft.DataBase.Mongodb.Collection.system;

namespace vnaisoft.system.data.Models
{
    public class sys_mat_hang_model
    {
        public sys_mat_hang_model()
        {
            db = new sys_mat_hang_col();
        }
        public sys_mat_hang_col db { get; set; }
        public string ten_loai_mat_hang { get; set; }
        public string ma_loai_mat_hang { get; set; }
        public string ten_don_vi_tinh { get; set; }
        public string nguoi_tao { get; set; }
        public string nguoi_cap_nhat { get; set; }
        public decimal? ton_kho { get; set; }
    }

}
