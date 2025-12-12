using MongoDB.Driver;
using quan_ly_kho.common.BaseClass;
using quan_ly_kho.common.Models;
using quan_ly_kho.system.data.Models;
using System.Collections.Generic;
using System.Linq;

namespace quan_ly_kho.system.web.Controller
{
    partial class sys_don_hang_banController
    {
        public static ControllerAppModel declare = new ControllerAppModel()
        {
            controller = "sys_don_hang_ban",
            icon = "badge",
            icon_image = "/assets/images/shungo/blue_feed_icon.png",
            module = "system",
            id = "sys_don_hang_ban",
            url = "/sys_don_hang_ban_index",
            title = "sys_don_hang_ban",
            translate = "NAV.sys_don_hang_ban",
            type = "item",
            //is_show_all_user = true,
            list_controller_action_public = new List<string>(){
                "sys_don_hang_ban;getListUse",
                "sys_don_hang_ban;create",
                 "sys_don_hang_ban;edit",
                 "sys_don_hang_ban;update_status_del",
                 "sys_don_hang_ban;get_list_don_vi_tinh",
                 "sys_don_hang_ban;get_code",
                   "sys_don_hang_ban;downloadtemp",
                    "sys_don_hang_ban;getElementById",
                     "sys_don_hang_ban;get_list_mat_hang",
                     "sys_don_hang_ban;cap_nhap",
                     "sys_don_hang_ban;DataHandlerDonHangBan",
                     "sys_don_hang_ban;get_list_nhan_vien",

            },
            list_controller_action_publicNonLogin = new List<string>(){
                    "sys_don_hang_ban;downloadtemp",
                    "sys_don_hang_ban;downloadtempdetail",
            },

            list_role = new List<ControllerRoleModel>()
            {
                // new ControllerRoleModel()
                //{
                //    id="sys_don_hang_ban;create",
                //    name="create",
                //    list_controller_action = new List<string>()
                //    {
                //          "sys_don_hang_ban;create",
                //    }
                //},
                //new ControllerRoleModel()
                //{
                //    id="sys_don_hang_ban;edit",
                //    name="edit",
                //    list_controller_action = new List<string>()
                //    {
                //          "sys_don_hang_ban;edit",
                //    }
                //},
                //new ControllerRoleModel()
                //{
                //    id="sys_don_hang_ban;delete",
                //    name="delete",
                //    list_controller_action = new List<string>()
                //    {
                //          "sys_don_hang_ban;delete",
                //    }
                //},
                  new ControllerRoleModel()
                {
                    id="sys_don_hang_ban;list",
                    name="list",
                    list_controller_action = new List<string>()
                    {
                          "sys_don_hang_ban;DataHandler",
                    }
                }
            }
        };

        private bool checkModelStateCreate(sys_don_hang_ban_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.create, item);
        }

        private bool checkModelStateEdit(sys_don_hang_ban_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.edit, item);
        }
        private bool checkModelStateCreateEdit(ActionEnumForm action, sys_don_hang_ban_model item)
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
                ModelState.AddModelError("db.list_mat_hang", "Phải chọn mặt hàng");
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
                            ModelState.AddModelError("db.so_luong" + i, "Phải lớn hơn 0");
                        }
                    }
                }
            }
            var queryTable = repo._context.sys_don_hang_ban_col.AsQueryable().Where(q => q.id == item.db.id.Trim());
            var search = repo.FindAll(queryTable).Where(d => d.db.ma == item.db.ma && d.db.id != item.db.id).Count();
            if (search > 0)
            {
                ModelState.AddModelError("db.ma", "existed");
            }
            return ModelState.IsValid;
        }
    }
}
