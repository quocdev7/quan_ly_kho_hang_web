using vnaisoft.DataBase.Mongodb.Collection.system;

namespace vnaisoft.system.data.Models
{
    public class sys_don_hang_ban_model
    {
        public sys_don_hang_ban_model()
        {
            db = new sys_don_hang_ban_col();
        }
        public string ten_nguoi_cap_nhat { get; set; }
        public string ten_doi_tuong { get; set; }
        public sys_don_hang_ban_col db { get; set; }
    }
}


