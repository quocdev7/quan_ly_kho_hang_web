using MongoDB.Driver;
using NPOI.SS.Formula.Functions;
using Opc.Ua;
using System;
using System.Collections.Generic;
using System.Linq;
using vnaisoft.common.BaseClass;
using vnaisoft.common.Models;
using vnaisoft.system.data.Models;

namespace vnaisoft.system.web.Controller
{
    partial class sys_loai_mat_hangController
    {
        public static ControllerAppModel declare = new ControllerAppModel()
        {
            controller = "sys_loai_mat_hang",
            icon = "badge",
            icon_image = "/assets/images/shungo/blue_feed_icon.png",
            module = "erp",
            id = "sys_loai_mat_hang",
            url = "/sys_loai_mat_hang_index",
            title = "sys_loai_mat_hang",
            translate = "NAV.sys_loai_mat_hang",
            type = "item",
            //is_show_all_user = true,
            list_controller_action_public = new List<string>(){
                "sys_loai_mat_hang;getListUse",
               "sys_loai_mat_hang;create",
                   "sys_loai_mat_hang;edit",
                    "sys_loai_mat_hang;delete",
                              "sys_loai_mat_hang;exportExcel",
                              "sys_loai_mat_hang;get_code",
                            "sys_loai_mat_hang;update_status_del",
                            "sys_loai_mat_hang;ImportFromExcel",
                            "sys_loai_mat_hang;getElementByMa",
                            "sys_loai_mat_hang;ImportFromExcelChietKhau",
            },
            list_controller_action_publicNonLogin = new List<string>()
            {
                "sys_loai_mat_hang;downloadtemp",
                "sys_loai_mat_hang;downloadtempdetail",
            },
            list_role = new List<ControllerRoleModel>()
            {
            //     new ControllerRoleModel()
            //    {
            //        id="sys_loai_mat_hang;create",
            //        name="create",
            //        list_controller_action = new List<string>()
            //        {
            //              "sys_loai_mat_hang;create",
            //        }
            //    },
            //    new ControllerRoleModel()
            //    {
            //        id="sys_loai_mat_hang;edit",
            //        name="edit",
            //        list_controller_action = new List<string>()
            //        {
            //              "sys_loai_mat_hang;edit",
            //        }
            //    },
            //    new ControllerRoleModel()
            //    {
            //        id="sys_loai_mat_hang;delete",
            //        name="delete",
            //        list_controller_action = new List<string>()
            //        {
            //              "sys_loai_mat_hang;delete",
            //        }
            //    },
            new ControllerRoleModel()
            {
                id = "sys_loai_mat_hang;list",
                name = "list",
                list_controller_action = new List<string>()
                    {
                          "sys_loai_mat_hang;DataHandler",
                    }
            }
            }
        };

        private bool checkModelStateCreate(sys_loai_mat_hang_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.create, item);
        }

        private bool checkModelStateEdit(sys_loai_mat_hang_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.edit, item);
        }
        private bool checkModelStateCreateEdit(ActionEnumForm action, sys_loai_mat_hang_model item)
        {
            if (string.IsNullOrEmpty(item.db.ten))
            {
                ModelState.AddModelError("db.ten", "required");
            }
            if (string.IsNullOrEmpty(item.db.ma))
            {
                ModelState.AddModelError("db.ma", "required");
            }
            var search = repo._context.sys_loai_mat_hang_col.AsQueryable().Where(d => d.ma == item.db.ma && d.ten == item.db.ten && d.id != item.db.id).Count();
            if (search > 0)
            {
                ModelState.AddModelError("db.ma", "existed");
            }

            return ModelState.IsValid;
        }

        //public string CheckErrorImport(sys_loai_mat_hang_model model, int ct, string error)
        //{
        //    if (!string.IsNullOrEmpty(model.db.ma))
        //    {
        //        var check_ten = repo._context.sys_loai_mat_hang_col.AsQueryable().Where(q => q.ma == model.db.ma).Count();
        //        if (check_ten > 0)
        //        {
        //            error += "Mã tại dòng " + (ct + 1) + " đã tồn tại <br />";
        //        }
        //    }
        //    if (String.IsNullOrEmpty(model.db.ten.ToString()))
        //    {
        //        error += "Phải nhập tên loại mặt hàng tại dòng" + (ct + 1) + "<br />";
        //    }
        //    else
        //    {
        //        var check_ten = repo._context.sys_loai_mat_hang_col.AsQueryable().Where(q => q.ten == model.db.ten).Count();
        //        if (check_ten > 0)
        //        {
        //            error += "Tên loại mặt hàng tại dòng " + (ct + 1) + " đã tồn tại <br />";
        //        }
        //    }
        //    if (!string.IsNullOrEmpty(model.ma_loai_dinh_khoan_mat_hang))
        //    {
        //        var check = repo._context.erp_loai_dinh_khoan_mat_hangs.AsQueryable().Where(q => q.ma == model.ma_loai_dinh_khoan_mat_hang).SingleOrDefault();
        //        if (check == null)
        //        {
        //            error += "Mã loại định khoản mặt hàng tại dòng " + (ct + 1) + " không tồn tại <br />";
        //        }
        //    }
        //    return error;
        //}
        
    }
}
