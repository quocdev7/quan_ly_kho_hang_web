using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using vnaisoft.common.BaseClass;
using vnaisoft.common.Models;
using vnaisoft.system.data.Models;

namespace vnaisoft.system.web.Controller
{
    partial class sys_don_hang_muaController
    {
        public static ControllerAppModel declare = new ControllerAppModel()
        {
            controller = "sys_don_hang_mua",
            icon = "badge",
            icon_image = "/assets/images/shungo/blue_feed_icon.png",
            module = "system",
            id = "sys_don_hang_mua",
            url = "/sys_don_hang_mua_index",
            title = "sys_don_hang_mua",
            translate = "NAV.sys_don_hang_mua",
            type = "item",
            //is_show_all_user = true,
            list_controller_action_public = new List<string>(){
                "sys_don_hang_mua;getListUse",
                "sys_don_hang_mua;create",
                "sys_don_hang_mua;edit",
                "sys_don_hang_mua;update_status_del",
                "sys_don_hang_mua;get_list_don_vi_tinh",
                "sys_don_hang_mua;get_code",
                "sys_don_hang_mua;getElementById",
                "sys_don_hang_mua;getListUseDetails",
                "sys_don_hang_mua;get_list_don_hang_mua",
                "sys_don_hang_mua;getPrint",
                "sys_don_hang_mua;DataHandlerDonHangMua",
            },
            list_controller_action_publicNonLogin = new List<string>(){
                   "sys_don_hang_mua;downloadtempdetail",
                   "sys_don_hang_mua;downloadtemp",

            },

            list_role = new List<ControllerRoleModel>()
            {
                // new ControllerRoleModel()
                //{
                //    id="sys_don_hang_mua;create",
                //    name="create",
                //    list_controller_action = new List<string>()
                //    {
                //          "sys_don_hang_mua;create",
                //    }
                //},
                //new ControllerRoleModel()
                //{
                //    id="sys_don_hang_mua;edit",
                //    name="edit",
                //    list_controller_action = new List<string>()
                //    {
                //          "sys_don_hang_mua;edit",
                //    }
                //},
                //new ControllerRoleModel()
                //{
                //    id="sys_don_hang_mua;delete",
                //    name="delete",
                //    list_controller_action = new List<string>()
                //    {
                //          "sys_don_hang_mua;delete",
                //    }
                //},
                  new ControllerRoleModel()
                {
                    id="sys_don_hang_mua;list",
                    name="list",
                    list_controller_action = new List<string>()
                    {
                          "sys_don_hang_mua;DataHandler",
                    }
                }
            }
        };

        private bool checkModelStateCreate(sys_don_hang_mua_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.create, item);
        }

        private bool checkModelStateEdit(sys_don_hang_mua_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.edit, item);
        }
        private bool checkModelStateCreateEdit(ActionEnumForm action, sys_don_hang_mua_model item)
        {
            if (string.IsNullOrEmpty(item.db.ma))
            {
                ModelState.AddModelError("db.ma", "required");
            }
            if (string.IsNullOrEmpty(item.db.ten))
            {
                ModelState.AddModelError("db.ten", "required");
            }
            if (item.db.ngay_dat_hang == null)
            {
                ModelState.AddModelError("db.ngay_dat_hang", "required");
            }
            if (item.list_mat_hang.Count == 0)
            {
                ModelState.AddModelError("db.list_mat_hang", "sys.phai_chon_mat_hang");
            }
            else
            {
                for (int i = 0; i < item.list_mat_hang.Count; i++)
                {
                    var mat_hang = item.list_mat_hang[i];
                    if (mat_hang.db.so_luong == null)
                    {
                        ModelState.AddModelError("db.so_luong" + i, "required");
                    }
                    else
                    {
                        if (mat_hang.db.so_luong <= 0)
                        {
                            ModelState.AddModelError("db.so_luong" + i, "sys.phai_lon_hon_0");
                        }
                    }
                }
            }



            var queryTable = repo._context.sys_don_hang_mua_col.AsQueryable().Where(q => q.id == item.db.id.Trim());
            var search = repo.FindAll(queryTable).Where(d => d.db.ma == item.db.ma && d.db.id != item.db.id).Count();
            if (search > 0)
            {
                ModelState.AddModelError("db.ma", "existed");
            }

            return ModelState.IsValid;
        }

    }
}
