using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using vnaisoft.common.BaseClass;
using vnaisoft.common.Models;
using vnaisoft.system.data.Models;

namespace vnaisoft.system.web.Controller
{
    partial class sys_cau_hinh_anh_mac_dinhController
    {
        public static ControllerAppModel declare = new ControllerAppModel()
        {
            controller = "sys_cau_hinh_anh_mac_dinh",
            icon = "heroicons_solid:user",
            icon_image = "/assets/icons/team.png",
            module = "he_thong",
            id = "sys_cau_hinh_anh_mac_dinh",
            url = "/sys_cau_hinh_anh_mac_dinh_index",
            title = "sys_cau_hinh_anh_mac_dinh",
            translate = "NAV.sys_cau_hinh_anh_mac_dinh",
            type = "item",
            //is_show_all_user = true,
            list_controller_action_public = new List<string>(){

                "sys_cau_hinh_anh_mac_dinh;getListUse",

            },
            list_controller_action_publicNonLogin = new List<string>()
            {
                "sys_cau_hinh_anh_mac_dinh;getLogo",
                "sys_cau_hinh_anh_mac_dinh;getQRCode",

            },
            list_role = new List<ControllerRoleModel>()
            {
                new ControllerRoleModel()
               {
                   id="sys_cau_hinh_anh_mac_dinh;create",
                   name="create",
                   list_controller_action = new List<string>()
                   {
                         "sys_cau_hinh_anh_mac_dinh;create",
                   }
               },
               new ControllerRoleModel()
               {
                   id="sys_cau_hinh_anh_mac_dinh;edit",
                   name="edit",
                   list_controller_action = new List<string>()
                   {
                         "sys_cau_hinh_anh_mac_dinh;edit",
                   }
               },
               new ControllerRoleModel()
               {
                   id="sys_cau_hinh_anh_mac_dinh;update_status_del",
                   name="delete",
                   list_controller_action = new List<string>()
                   {
                         "sys_cau_hinh_anh_mac_dinh;delete",
                         "sys_cau_hinh_anh_mac_dinh;update_status_del",
                   }
               },
                  new ControllerRoleModel()
                {
                    id="sys_cau_hinh_anh_mac_dinh;list",
                    name="list",
                    list_controller_action = new List<string>()
                    {
                          "sys_cau_hinh_anh_mac_dinh;DataHandler",
                    }
                }
            }
        };

        private bool checkModelStateCreate(sys_cau_hinh_anh_mac_dinh_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.create, item);
        }

        private bool checkModelStateEdit(sys_cau_hinh_anh_mac_dinh_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.edit, item);
        }
        private bool checkModelStateCreateEdit(ActionEnumForm action, sys_cau_hinh_anh_mac_dinh_model item)
        {

            if (item.db.type == null)
            {
                ModelState.AddModelError("db.type", "required");
            }
            else
            {
                var search = repo.FindAll().Where(d => d.db.type == item.db.type && d.db.id != item.db.id).Count();
                if (search > 0)
                {
                    ModelState.AddModelError("db.type", "existed");
                }
            }

            if (item.db.type != 8)
            {
                if (item.image == null)
                {
                    ModelState.AddModelError("db.image", "required");
                }
            }

            return ModelState.IsValid;
        }
        //private void removeUserModel(sys_cau_hinh_anh_mac_dinh_model obj)
        //{
        //    obj.db.otp = null;
        //    obj.db.PasswordHash = null;
        //    obj.db.PasswordSalt = null;
        //    obj.db.token_notification = null;
        //    obj.db.token_reset_pass = null;
        //    obj.db.token_reset_pass = null;
        //    obj.db.expiration_date_reset_pass = null;
        //}
    }
}
