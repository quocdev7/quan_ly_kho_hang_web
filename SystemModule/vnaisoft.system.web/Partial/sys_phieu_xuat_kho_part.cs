using MongoDB.Driver;
using quan_ly_kho.common.BaseClass;
using quan_ly_kho.common.Models;
using quan_ly_kho.system.data.Models;
using System.Collections.Generic;
using System.Linq;

namespace quan_ly_kho.system.web.Controller
{
    partial class sys_phieu_xuat_khoController
    {
        public static ControllerAppModel declare = new ControllerAppModel()
        {
            controller = "sys_phieu_xuat_kho",
            icon = "badge",
            icon_image = "/assets/images/shungo/blue_feed_icon.png",
            module = "erp",
            id = "sys_phieu_xuat_kho",
            url = "/sys_phieu_xuat_kho_index",
            title = "sys_phieu_xuat_kho",
            translate = "NAV.sys_phieu_xuat_kho",
            type = "item",
            // is_show_all_user = true,
            list_controller_action_public = new List<string>(){
                  "sys_phieu_xuat_kho;create",
                   "sys_phieu_xuat_kho;edit",
                    "sys_phieu_xuat_kho;delete",
                     "sys_phieu_xuat_kho;get_code",
                       "sys_phieu_xuat_kho;getElementById",
                       "sys_phieu_xuat_kho;getListUse",
                         "sys_phieu_xuat_kho;update_status_del",
                         "sys_phieu_xuat_kho;getElementById",
                         "sys_phieu_xuat_kho;get_list_mat_hang_xuat_kho",
                         "sys_phieu_xuat_kho;get_list_mat_hang_cua_phieu_xuat",
                         "sys_phieu_xuat_kho;get_list_phieu_xuat",
            },

            list_controller_action_publicNonLogin = new List<string>(){

                    "sys_phieu_xuat_kho;downloadtemp",
                    "sys_phieu_xuat_kho;downloadtempdetail",
            },
            list_role = new List<ControllerRoleModel>()
            {
                  new ControllerRoleModel()
                {
                    id="sys_phieu_xuat_kho;list",
                    name="list",
                    list_controller_action = new List<string>()
                    {
                          "sys_phieu_xuat_kho;DataHandler",
                    }
                }
            }
        };

        private bool checkModelStateCreate(sys_phieu_xuat_kho_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.create, item);
        }

        private bool checkModelStateEdit(sys_phieu_xuat_kho_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.edit, item);
        }
        private bool checkModelStateCreateEdit(ActionEnumForm action, sys_phieu_xuat_kho_model item)
        {
            if (item.db.id_loai_xuat == null)
            {
                ModelState.AddModelError("db.id_loai_xuat", "required");
            }
            else
            {

            }
            if (item.db.ngay_xuat == null)
            {
                ModelState.AddModelError("db.ngay_xuat", "required");
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
                    var ton_kho = repo._context.sys_ton_kho_mat_hang_col.AsQueryable().Where(d => d.id_mat_hang == itemNew.ma_mat_hang).Sum(q => q.so_luong_ton > 0 ? (q.so_luong_ton ?? 0) : 0);
                    decimal? sl_ton_kho = (decimal?)((ton_kho / 1000));
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
