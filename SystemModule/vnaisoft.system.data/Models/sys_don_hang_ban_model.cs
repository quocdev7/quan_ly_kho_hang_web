using quan_ly_kho.DataBase.Mongodb.Collection.system;
using System.Collections.Generic;

namespace quan_ly_kho.system.data.Models
{
    public class sys_don_hang_ban_model
    {
        public sys_don_hang_ban_model()
        {
            db = new sys_don_hang_ban_col();
            list_mat_hang = new List<sys_don_hang_ban_mat_hang_model>();
        }
        public string ten_nguoi_cap_nhat { get; set; }
        public sys_don_hang_ban_col db { get; set; }
        public List<sys_don_hang_ban_mat_hang_model> list_mat_hang { get; set; }
    }
    public class sys_don_hang_ban_mat_hang_model
    {
        public sys_don_hang_ban_mat_hang_model()
        {
            db = new sys_don_hang_ban_mat_hang_col();
        }
        public sys_don_hang_ban_mat_hang_col db { get; set; }
        public string ten_don_vi_tinh { get; set; }
        public string ten_mat_hang { get; set; }

    }
}


