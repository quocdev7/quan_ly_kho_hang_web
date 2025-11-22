using System.Collections.Generic;
using vnaisoft.DataBase.Mongodb.Collection.system;

namespace vnaisoft.system.data.Models
{
    public class sys_don_hang_mua_model
    {
        public sys_don_hang_mua_model()
        {
            db = new sys_don_hang_mua_col();
            list_mat_hang = new List<sys_don_hang_mua_mat_hang_model>();
        }
        public sys_don_hang_mua_col db { get; set; }
        public string ten_nguoi_cap_nhat { get; set; }
        public List<sys_don_hang_mua_mat_hang_model> list_mat_hang { get; set; }

    }
    public class sys_don_hang_mua_mat_hang_model
    {
        public sys_don_hang_mua_mat_hang_model()
        {
            db = new sys_don_hang_mua_mat_hang_col();
        }
        public sys_don_hang_mua_mat_hang_col db { get; set; }
        public string ten_don_vi_tinh { get; set; }
        public string ten_mat_hang { get; set; }

    }

}


