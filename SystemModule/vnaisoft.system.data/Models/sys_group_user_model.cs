using MimeKit.Cryptography;
using System;
using System.Collections.Generic;
using System.Text;
using vnaisoft.common.Models;
using vnaisoft.DataBase.System;

namespace vnaisoft.system.data.Models
{
    public class sys_group_user_model
    {
        public sys_group_user_model()
        {
            db = new sys_group_user_db();
            list_item = new List<sys_group_user_detail_model>();
            list_module = new List<sys_group_user_module_model>();
        }
        public string ten_nguoi_cap_nhat { get; set; }
        public string createby_name { get; set; }
        public int? count_user { get; set; }
        public string updateby_name { get; set; }
        public sys_group_user_db db { get; set; }
        public List<sys_group_user_detail_model> list_item { get; set; }
        public List<sys_group_user_role_model> list_role { get; set; }
        public List<sys_group_user_module_model> list_module { get; set; }

    }
    public class sys_group_user_detail_model
    {
        public string user_name { get; set; }
        public string user_id { get; set; }
        public string department_name { get; set; }
        public string position_name { get; set; }
        public bool? isCheck { get; set; }
        public int? type_user { get; set; }
        public bool? is_system { get; set; }
        
    }
    public class sys_group_user_role_model
    {
        public sys_group_user_role_model()
        {
            db = new sys_group_user_role_db();
        }
        public string user_name { get; set; }
        public sys_group_user_role_db db { get; set; }

    }

    public class sys_group_user_module_model
    {
        public string id_module { get; set; }
        public string ten { get; set; }
        public int? loai { get; set; }
        public bool? is_check { get; set; }
        public bool? is_expand { get; set; }
        public List<sys_group_user_controller_model> lst_controller { get; set; }

    }

    public class sys_group_user_controller_model
    {
        public string id_controller { get; set; }
        public string name_controller { get; set; }
        public string type_user { get; set; }
        public bool? is_show_all_user { get; set; }
        public bool? is_check { get; set; }

        public List<sys_group_user_role_new_model> list_role { get; set; }


    }
    public class sys_group_user_role_new_model
    {
        public string id_role { get; set; }
        public string name_role { get; set; }
        public bool? is_check { get; set; }
        public List<string> list_controller_action { get; set; }


    }

}
