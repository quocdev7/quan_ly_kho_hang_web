using quan_ly_kho.common.BaseClass;
using quan_ly_kho.common.Models;
using quan_ly_kho.system.data.Models;
using System.Collections.Generic;
using System.Linq;

namespace quan_ly_kho.system.web.Controller
{
    partial class sys_don_vi_tinhController
    {
        public static ControllerAppModel declare = new ControllerAppModel()
        {
            controller = "sys_don_vi_tinh",
            icon = "badge",
            icon_image = "/assets/images/shungo/blue_feed_icon.png",
            module = "system",
            id = "sys_don_vi_tinh",
            url = "/sys_don_vi_tinh_index",
            title = "sys_don_vi_tinh",
            translate = "NAV.sys_don_vi_tinh",
            type = "item",
            //is_show_all_user = true,
            list_controller_action_public = new List<string>(){
                "sys_don_vi_tinh;getListUse",
                "sys_don_vi_tinh;create",
                 "sys_don_vi_tinh;edit",
                 "sys_don_vi_tinh;update_status_del",
                 "sys_don_vi_tinh;get_list_don_vi_tinh",
                 "sys_don_vi_tinh;exportExcel",
                 "sys_don_vi_tinh;get_code",
                   "sys_don_vi_tinh;downloadtemp",
                    "sys_don_vi_tinh;ImportFromExcel",
                    "sys_don_vi_tinh;exportExcel",
            },
            list_controller_action_publicNonLogin = new List<string>(){
                    "sys_don_vi_tinh;downloadtemp",
                    "sys_don_vi_tinh;downloadtempFileError",
            },

            list_role = new List<ControllerRoleModel>()
            {
                 new ControllerRoleModel()
                {
                    id="sys_don_vi_tinh;create",
                    name="create",
                    list_controller_action = new List<string>()
                    {
                          "sys_don_vi_tinh;create",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_don_vi_tinh;edit",
                    name="edit",
                    list_controller_action = new List<string>()
                    {
                          "sys_don_vi_tinh;edit",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_don_vi_tinh;delete",
                    name="delete",
                    list_controller_action = new List<string>()
                    {
                          "sys_don_vi_tinh;delete",
                    }
                },
                  new ControllerRoleModel()
                {
                    id="sys_don_vi_tinh;list",
                    name="list",
                    list_controller_action = new List<string>()
                    {
                          "sys_don_vi_tinh;DataHandler",
                    }
                }
            }
        };

        private bool checkModelStateCreate(sys_don_vi_tinh_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.create, item);
        }

        private bool checkModelStateEdit(sys_don_vi_tinh_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.edit, item);
        }
        private bool checkModelStateCreateEdit(ActionEnumForm action, sys_don_vi_tinh_model item)
        {
            if (string.IsNullOrEmpty(item.db.ten))
            {
                ModelState.AddModelError("db.ten", "required");
            }
            if (string.IsNullOrEmpty(item.db.ma))
            {
                ModelState.AddModelError("db.ma", "required");
            }
            var search = repo.FindAll().Where(d => d.db.ten == item.db.ten && d.db.id != item.db.id).Count();
            if (search > 0)
            {
                ModelState.AddModelError("db.ten", "existed");
            }

            return ModelState.IsValid;
        }
        //public string CheckErrorImport(sys_don_vi_tinh_model model, int ct, string error)
        //{
        //    if (!string.IsNullOrEmpty(model.db.ma))
        //    {
        //        var check_ten = repo._context.sys_don_vi_tinhs.AsQueryable().Where(q => q.ma == model.db.ma).Count();
        //        if (check_ten > 0)
        //        {
        //            error += "Mã tại dòng " + (ct + 1) + " đã tồn tại <br />";
        //        }
        //    }

        //    if (String.IsNullOrEmpty(model.db.ten.ToString()))
        //    {
        //        error += "Phải nhập tên tại dòng" + (ct + 1) + "<br />";
        //    }
        //    else
        //    {
        //        var check_ten = repo._context.sys_don_vi_tinhs.AsQueryable().Where(q => q.ten == model.db.ten).Count();
        //        if (check_ten > 0)
        //        {
        //            error += "Tên đơn vị tại dòng " + (ct + 1) + " đã tồn tại <br />";
        //        }
        //    }
        //    return error;
        //}

    }
}
