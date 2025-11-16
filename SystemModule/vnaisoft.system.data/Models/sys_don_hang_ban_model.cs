using System.Collections.Generic;
using vnaisoft.DataBase.Mongodb.Collection.system;

namespace vnaisoft.system.data.Models
{
    public class sys_don_hang_ban_model
    {
        public sys_don_hang_ban_model()
        {
            db = new sys_don_hang_ban_col();
            list_mat_hang = new List<sys_don_hang_ban_mat_hang_col>();
        }
        public string ten_nguoi_cap_nhat { get; set; }
        public sys_don_hang_ban_col db { get; set; }
        public List<sys_don_hang_ban_mat_hang_col> list_mat_hang { get; set; }
    }
}


