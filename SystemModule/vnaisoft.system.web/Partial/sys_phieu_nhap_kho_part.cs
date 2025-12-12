using MongoDB.Driver;
using quan_ly_kho.common.BaseClass;
using quan_ly_kho.common.Models;
using quan_ly_kho.system.data.Models;
using System.Collections.Generic;
using System.Linq;

namespace quan_ly_kho.system.web.Controller
{
    partial class sys_phieu_nhap_khoController
    {
        public static ControllerAppModel declare = new ControllerAppModel()
        {
            controller = "sys_phieu_nhap_kho",
            icon = "badge",
            icon_image = "/assets/images/shungo/blue_feed_icon.png",
            module = "erp",
            id = "sys_phieu_nhap_kho",
            url = "/sys_phieu_nhap_kho_index",
            title = "sys_phieu_nhap_kho",
            translate = "NAV.sys_phieu_nhap_kho",
            type = "item",
            //is_show_all_user = true,
            list_controller_action_public = new List<string>(){
                  "sys_phieu_nhap_kho;create",
                   "sys_phieu_nhap_kho;edit",
                    "sys_phieu_nhap_kho;delete",
                     "sys_phieu_nhap_kho;get_code",
                       "sys_phieu_nhap_kho;getElementById",
                         "sys_phieu_nhap_kho;get_list_mat_hang_ban",
                         "sys_phieu_nhap_kho;getListUse",
                         "sys_phieu_nhap_kho;update_status_del",
                         "sys_phieu_nhap_kho;getElementById",
                         "sys_phieu_nhap_kho;get_list_mat_hang_cua_phieu_nhap",
            },

            list_controller_action_publicNonLogin = new List<string>(){

                    "sys_phieu_nhap_kho;downloadtemp",
                    "sys_phieu_nhap_kho;downloadtempdetail",
            },


            list_role = new List<ControllerRoleModel>()
            {
                  new ControllerRoleModel()
                {
                    id="sys_phieu_nhap_kho;list",
                    name="list",
                    list_controller_action = new List<string>()
                    {
                          "sys_phieu_nhap_kho;DataHandler",
                    }
                }
            }
        };

        private bool checkModelStateCreate(sys_phieu_nhap_kho_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.create, item);
        }

        private bool checkModelStateEdit(sys_phieu_nhap_kho_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.edit, item);
        }
        private bool checkModelStateCreateEdit(ActionEnumForm action, sys_phieu_nhap_kho_model item)
        {
            if (item.db.id_loai_nhap == null)
            {
                ModelState.AddModelError("db.id_loai_nhap", "required");
            }
            if (item.db.ngay_nhap == null)
            {
                ModelState.AddModelError("db.ngay_nhap", "required");
            }
            if (item.list_mat_hang.Count() == 0)
            {
                ModelState.AddModelError("list_mat_hang", "msgphaichonmathang");
            }
            else
            {
                for (int i = 0; i < item.list_mat_hang.Count; i++)
                {
                    var itemNew = item.list_mat_hang[i];

                    if (itemNew.db.so_luong <= 0)
                    {
                        ModelState.AddModelError("db.so_luong" + i, "soluonglonhon0");
                    }
                    if (itemNew.db.so_luong == null)
                    {
                        ModelState.AddModelError("db.so_luong" + i, "required");
                    }
                }

            }
            return ModelState.IsValid;
        }
    }
}
