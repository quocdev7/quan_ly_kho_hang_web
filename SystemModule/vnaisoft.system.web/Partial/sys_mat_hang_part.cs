using MongoDB.Driver;
using NPOI.SS.Formula.Functions;
using Opc.Ua;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using vnaisoft.common.BaseClass;
using vnaisoft.common.Helpers;
using vnaisoft.common.Models;
using vnaisoft.system.data.Models;

namespace vnaisoft.system.web.Controller
{
    partial class sys_mat_hangController
    {
        public static ControllerAppModel declare = new ControllerAppModel()
        {
            controller = "sys_mat_hang",
            icon = "badge",
            icon_image = "/assets/images/shungo/blue_feed_icon.png",
            module = "erp",
            id = "sys_mat_hang",
            url = "/sys_mat_hang_index",
            title = "sys_mat_hang",
            translate = "NAV.sys_mat_hang",
            type = "item",
            //is_show_all_user = true,
            list_controller_action_public = new List<string>(){
                  "sys_mat_hang;create",
                  "sys_mat_hang;edit",
                  "sys_mat_hang;delete",
                  //"sys_mat_hang;DataHandler",
                  "sys_mat_hang;get_code",
                  "sys_mat_hang;ImportFromExcel",
                  "sys_mat_hang;exportExcel",
                  "sys_mat_hang;get_list_mat_hang_ban",
                  "sys_mat_hang;get_list_mat_hang",
                  "sys_mat_hang;add_mat_hang",
                  "sys_mat_hang;add_mat_hang_da_chon",
                  "sys_mat_hang;DataHandlerPopup",
                  "sys_mat_hang;update_status_del",
                  "sys_mat_hang;getListUse",
                  "sys_mat_hang;get_list_mat_hang_theo_loai",
                  "sys_mat_hang;getElementByMa",
                  "sys_mat_hang;getElementById",
                     "sys_mat_hang;getPrint",
                     "sys_mat_hang;print",
                     "sys_mat_hang;print_all",

            },

            list_controller_action_publicNonLogin = new List<string>(){
                    "sys_mat_hang;downloadtemp",
                    "sys_mat_hang;print",
                    "sys_mat_hang;print_all",
            },
            list_role = new List<ControllerRoleModel>()
            {
                 new ControllerRoleModel()
                {
                    id="sys_mat_hang;create",
                    name="create",
                    list_controller_action = new List<string>()
                    {
                          "sys_mat_hang;create",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_mat_hang;edit",
                    name="edit",
                    list_controller_action = new List<string>()
                    {
                          "sys_mat_hang;edit",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_mat_hang;delete",
                    name="delete",
                    list_controller_action = new List<string>()
                    {
                          "sys_mat_hang;delete",
                    }
                },
                  new ControllerRoleModel()
                {
                    id="sys_mat_hang;list",
                    name="list",
                    list_controller_action = new List<string>()
                    {
                          "sys_mat_hang;DataHandler",
                           "sys_mat_hang;DataHandlerTonKho",

                    }
                }
            }
        };

        private bool checkModelStateCreate(sys_mat_hang_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.create, item);
        }

        private bool checkModelStateEdit(sys_mat_hang_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.edit, item);
        }
        private bool checkModelStateCreateEdit(ActionEnumForm action, sys_mat_hang_model item)
        {
            if (string.IsNullOrEmpty(item.db.ten))
            {
                ModelState.AddModelError("db.ten", "required");
            }
            if (item.db.id_loai_mat_hang == null)
            {
                ModelState.AddModelError("db.loai_mat_hang", "required");
            }
            if (item.db.id_don_vi_tinh == null)
            {
                ModelState.AddModelError("db.don_vi_tinh", "required");
            }
            if (string.IsNullOrEmpty(item.db.ma))
            {
                ModelState.AddModelError("db.ma", "required");
            }
            else
            {
                var search = repo._context.sys_mat_hang_col.AsQueryable().Where(d => d.ma == item.db.ma && d.id != item.db.id).Count();
                if (search > 0)
                {
                    ModelState.AddModelError("db.ma", "existed");
                }
            }
            if (item.db.gia_ban_si > item.db.gia_ban_le && item.db.gia_ban_le > 0)
            {
                ModelState.AddModelError("db.gia_ban_si", "system.gia_si_phai_nho_hon_gia_ban_le");
            }
            return ModelState.IsValid;
        }
    }
}
