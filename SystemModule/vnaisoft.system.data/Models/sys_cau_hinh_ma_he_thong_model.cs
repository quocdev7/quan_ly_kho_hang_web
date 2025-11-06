using vnaisoft.DataBase.Mongodb.Collection.system;

namespace vnaisoft.system.data.Models
{
    public class sys_cau_hinh_ma_he_thong_model
    {
        public sys_cau_hinh_ma_he_thong_model()
        {
            db = new sys_cau_hinh_ma_he_thong_col();

        }
        public string ten_nguoi_cap_nhat { get; set; }

        public string so_tu_tang { get; set; }
        public sys_cau_hinh_ma_he_thong_col db { get; set; }

    }
}


