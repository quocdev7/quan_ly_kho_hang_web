using System;
using System.Collections.Generic;
using System.Text;

namespace vnaisoft.common.Models
{
    public class ControllerAppModel
    {
        public ControllerAppModel()
        {
            list_role = new List<ControllerRoleModel>();
            list_controller_action_public = new List<string>();
            list_controller_action_publicNonLogin = new List<string>();
        }
        public string id { get; set; }
        public string title { get; set; }
        public string url { get; set; }
        public string icon { get; set; }
        public string icon_image { get; set; }
        public string translate { get; set; }
        public string type { get; set; }
        public string controller { get; set; }
        public string module { get; set; }
        public bool is_badge { get; set; }
        public bool is_approval { get; set; }
        public bool is_show_all_user { get; set; }

        public bool is_show_domain_not_init { get; set; }

        // default:1 , 1 building user, 2 company user, 
        public int? type_user { get; set; }

        public List<ControllerRoleModel> list_role { get; set; }
        public List<string> list_controller_action_public { get; set; }
        public List<string> list_controller_action_publicNonLogin { get; set; }

    }
    public class ControllerRoleModel
    {
        public ControllerRoleModel()
        {
            list_controller_action = new List<string>();
        }
        public string id { get; set; }

        public string name { get; set; }

        public List<string> list_controller_action { get; set; }
    }
}
