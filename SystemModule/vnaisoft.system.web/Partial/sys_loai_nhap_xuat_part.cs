using MongoDB.Driver;
using quan_ly_kho.common.BaseClass;
using quan_ly_kho.common.Models;
using quan_ly_kho.system.data.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace quan_ly_kho.system.web.Controller
{
    partial class sys_loai_nhap_xuatController
    {
        public static ControllerAppModel declare = new ControllerAppModel()
        {
            controller = "sys_loai_nhap_xuat",
            icon = "badge",
            icon_image = "/assets/images/shungo/blue_feed_icon.png",
            module = "erp",
            id = "sys_loai_nhap_xuat",
            url = "/sys_loai_nhap_xuat_index",
            title = "sys_loai_nhap_xuat",
            translate = "NAV.sys_loai_nhap_xuat",
            type = "item",
            //is_show_all_user = true,
            list_controller_action_public = new List<string>(){
                "sys_loai_nhap_xuat;getListUse",
               "sys_loai_nhap_xuat;create",
                   "sys_loai_nhap_xuat;edit",
                    "sys_loai_nhap_xuat;delete",
                              "sys_loai_nhap_xuat;exportExcel",
                              "sys_loai_nhap_xuat;get_code",
                    "sys_loai_nhap_xuat;ImportFromExcel",
                            "sys_loai_nhap_xuat;update_status_del",
            },
            list_controller_action_publicNonLogin = new List<string>()
            {
                "sys_loai_nhap_xuat;downloadtemp",
            },
            list_role = new List<ControllerRoleModel>()
            {
            //     new ControllerRoleModel()
            //    {
            //        id="sys_loai_nhap_xuat;create",
            //        name="create",
            //        list_controller_action = new List<string>()
            //        {
            //              "sys_loai_nhap_xuat;create",
            //        }
            //    },
            //    new ControllerRoleModel()
            //    {
            //        id="sys_loai_nhap_xuat;edit",
            //        name="edit",
            //        list_controller_action = new List<string>()
            //        {
            //              "sys_loai_nhap_xuat;edit",
            //        }
            //    },
            //    new ControllerRoleModel()
            //    {
            //        id="sys_loai_nhap_xuat;delete",
            //        name="delete",
            //        list_controller_action = new List<string>()
            //        {
            //              "sys_loai_nhap_xuat;delete",
            //        }
            //    },
            new ControllerRoleModel()
            {
                id = "sys_loai_nhap_xuat;list",
                name = "list",
                list_controller_action = new List<string>()
                    {
                          "sys_loai_nhap_xuat;DataHandler",
                    }
            }
            }
        };

        private bool checkModelStateCreate(sys_loai_nhap_xuat_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.create, item);
        }

        private bool checkModelStateEdit(sys_loai_nhap_xuat_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.edit, item);
        }
        private bool checkModelStateCreateEdit(ActionEnumForm action, sys_loai_nhap_xuat_model item)
        {
            if (!String.IsNullOrEmpty(item.db.ma))
            {
                var check_ma = repo._context.sys_loai_nhap_xuat_col.AsQueryable().Where(q => q.ma == item.db.ma && q.id != item.db.id).SingleOrDefault();
                if (check_ma != null)
                {
                    ModelState.AddModelError("db.ma", "existed");
                }
            }
            if (string.IsNullOrEmpty(item.db.ma))
            {
                ModelState.AddModelError("db.ma", "required");
            }
            if (string.IsNullOrEmpty(item.db.ten))
            {
                ModelState.AddModelError("db.ten", "required");
            }
            if (item.db.loai == null)
            {
                ModelState.AddModelError("db.loai", "required");
            }
            var search = repo.FindAll().Where(d => d.db.loai == item.db.loai && d.db.ma == item.db.ma).Where(d => d.db.ten == item.db.ten && d.db.id != item.db.id).Count();
            if (search > 0)
            {
                ModelState.AddModelError("db.ten", "existed");
            }

            return ModelState.IsValid;
        }
        public string CheckErrorImport(sys_loai_nhap_xuat_model model, int ct, string error)
        {
            if (String.IsNullOrEmpty(model.db.ma.ToString()))
            {
                error += "Phải nhập mã  tại dòng" + (ct + 1) + "<br />";
            }
            if (String.IsNullOrEmpty(model.db.loai.ToString()))
            {
                error += "Phải nhập loại tại dòng" + (ct + 1) + "<br />";
            }
            if (String.IsNullOrEmpty(model.db.ten.ToString()))
            {
                error += "Phải nhập tên loại tại dòng" + (ct + 1) + "<br />";
            }
            else
            {
                var check_ten = repo._context.sys_loai_nhap_xuat_col.AsQueryable().Where(t => t.loai == model.db.loai && t.ma == model.db.ma).Where(q => q.ten == model.db.ten).SingleOrDefault();
                if (check_ten != null)
                {
                    error += "Tên loại tại dòng " + (ct + 1) + " đã tồn tại <br />";
                }
            }
            return error;
        }


    }
}
