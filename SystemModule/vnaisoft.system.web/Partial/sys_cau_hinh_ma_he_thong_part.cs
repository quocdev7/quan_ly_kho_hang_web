using MongoDB.Driver;
using Opc.Ua;
using System;
using System.Collections.Generic;
using System.Linq;
using vnaisoft.common.BaseClass;
using vnaisoft.common.Models;
using vnaisoft.system.data.Models;

namespace vnaisoft.system.web.Controller
{
    partial class sys_cau_hinh_ma_he_thongController
    {
        public static ControllerAppModel declare = new ControllerAppModel()
        {
            controller = "sys_cau_hinh_ma_he_thong",
            icon = "badge",
            icon_image = "/assets/images/shungo/blue_feed_icon.png",
            module = "system",
            id = "sys_cau_hinh_ma_he_thong",
            url = "/sys_cau_hinh_ma_he_thong_index",
            title = "sys_cau_hinh_ma_he_thong",
            translate = "NAV.sys_cau_hinh_ma_he_thong",
            type = "item",
            //is_show_all_user = true,
            list_controller_action_public = new List<string>(){
                "sys_cau_hinh_ma_he_thong;getListUse",
                "sys_cau_hinh_ma_he_thong;create",
                "sys_cau_hinh_ma_he_thong;edit",
                "sys_cau_hinh_ma_he_thong;delete",
                "sys_cau_hinh_ma_he_thong;get_data",
                "sys_cau_hinh_ma_he_thong;getInitCode",

            },

            list_role = new List<ControllerRoleModel>()
            {
                // new ControllerRoleModel()
                //{
                //    id="sys_cau_hinh_ma_he_thong;create",
                //    name="create",
                //    list_controller_action = new List<string>()
                //    {
                //          "sys_cau_hinh_ma_he_thong;create",
                //    }
                //},
                //new ControllerRoleModel()
                //{
                //    id="sys_cau_hinh_ma_he_thong;edit",
                //    name="edit",
                //    list_controller_action = new List<string>()
                //    {
                //          "sys_cau_hinh_ma_he_thong;edit",
                //    }
                //},
                //new ControllerRoleModel()
                //{
                //    id="sys_cau_hinh_ma_he_thong;delete",
                //    name="delete",
                //    list_controller_action = new List<string>()
                //    {
                //          "sys_cau_hinh_ma_he_thong;delete",
                //    }
                //},
                  new ControllerRoleModel()
                {
                    id="sys_cau_hinh_ma_he_thong;list",
                    name="list",
                    list_controller_action = new List<string>()
                    {
                          "sys_cau_hinh_ma_he_thong;DataHandler",
                    }
                }
            }
        };

        private bool checkModelStateCreate(sys_cau_hinh_ma_he_thong_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.create, item);
        }

        private bool checkModelStateEdit(sys_cau_hinh_ma_he_thong_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.edit, item);
        }
        private bool checkModelStateCreateEdit(ActionEnumForm action, sys_cau_hinh_ma_he_thong_model item)
        {
            //if (string.IsNullOrEmpty(item.db.don_gia))
            //{
            //    ModelState.AddModelError("db.ma", "required");
            //}
            if (String.IsNullOrEmpty(item.db.controller) || item.db.controller == null)
            {
                ModelState.AddModelError("db.controller", "required");
            }


            return ModelState.IsValid;
        }

    }
}
