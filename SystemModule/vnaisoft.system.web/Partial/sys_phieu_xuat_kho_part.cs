using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using vnaisoft.common.BaseClass;
using vnaisoft.common.Models;
using vnaisoft.system.data.Models;

namespace vnaisoft.system.web.Controller
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
            //if (action == ActionEnumForm.edit)
            //{
            //    if (string.IsNullOrEmpty(item.ly_do_chinh_sua))
            //    {
            //        ModelState.AddModelError("db.ly_do_chinh_sua", "required");
            //    }
            //}
            //if (item.db.id_kho == null)
            //{
            //    ModelState.AddModelError("db.id_kho", "required");
            //}
            //if (item.db.id_don_hang_mua == null && item.db.id_loai_xuat == "XTH")
            //{
            //    ModelState.AddModelError("db.id_don_hang", "required");
            //}
            //if (item.db.id_don_hang_ban == null && item.db.id_loai_xuat == "XBH")
            //{
            //    ModelState.AddModelError("db.id_don_hang", "required");
            //}
            //if (item.check_doi_tuong == 1)
            //{
            //}
            //else
            //{
            //    if (item.db.id_doi_tuong == null || item.db.id_doi_tuong == "")
            //    {
            //        ModelState.AddModelError("db.id_doi_tuong", "required");
            //    }

            //}
            if (item.db.id_loai_xuat == null)
            {
                ModelState.AddModelError("db.id_loai_xuat", "required");
            }
            else
            {
                //if (item.ma_loai_xuat == "XBH")
                //{
                //    if (string.IsNullOrEmpty(item.db.id_don_hang_ban))
                //    {
                //        ModelState.AddModelError("db.id_don_hang_ban", "required");
                //    }


                //}
                //else if (item.ma_loai_xuat == "XTH")
                //{
                //    if (string.IsNullOrEmpty(item.db.id_don_hang_mua))
                //    {
                //        ModelState.AddModelError("db.id_don_hang_mua", "required");
                //    }

                //}

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
                    //var id_kho = repo._context.sys_khos.AsQueryable().Where(d => d.ma == item.db.id_kho).Select(d => d.id).SingleOrDefault();
                    var ton_kho = repo._context.sys_ton_kho_mat_hang_col.AsQueryable().Where(d => d.id_mat_hang == itemNew.ma_mat_hang).Sum(q => q.so_luong_ton > 0 ? (q.so_luong_ton ?? 0) : 0);
                    decimal? sl_ton_kho = (decimal?)((ton_kho / 1000));
                    //var value_cau_hinh = repo._context.sys_cau_hinh_he_thongs.AsQueryable().Where(d => d.code == "cho_phep_ton_kho_am").Select(d => d.value).SingleOrDefault();
                    if (itemNew.db.so_luong <= 0)
                    {
                        ModelState.AddModelError("db.so_luong" + i, "soluonglonhon0");
                    }
                    if (itemNew.db.so_luong == null)
                    {
                        ModelState.AddModelError("db.so_luong" + i, "required");
                    }
                    //if (value_cau_hinh == "2")
                    //{
                    //    if (itemNew.db.so_luong > sl_ton_kho)
                    //    {
                    //        ModelState.AddModelError("db.so_luong" + i, "so_luong_xuat_kho_khong_duoc_vuot_qua_so_luong_ton_kho");
                    //    }
                    //}

                    //if (string.IsNullOrEmpty(item.db.id_don_hang_ban) && string.IsNullOrEmpty(item.db.id_don_hang_mua))
                    //{
                    //    if (value_cau_hinh == "2")
                    //    {
                    //        if (itemNew.db.so_luong > sl_ton_kho)
                    //        {
                    //            ModelState.AddModelError("db.so_luong" + i, "so_luong_xuat_kho_khong_duoc_vuot_qua_so_luong_ton_kho");
                    //        }
                    //    }
                    //}    
                    //if (!string.IsNullOrEmpty(item.db.id_don_hang_ban))
                    //{
                    //    var so_luong_mat_hang_dhb = repo._context.sys_don_hang_ban_mat_hangs.AsQueryable()
                    //    .Where(q => q.status_del == 1 && q.id_don_hang == item.db.id_don_hang_ban && q.id_mat_hang == itemNew.id_mat_hang)
                    //    .Sum(q => q.so_luong) ?? 0;
                    //    var px = repo._context.sys_phieu_xuat_khos.AsQueryable().Where(q => q.id_don_hang_ban == item.db.id_don_hang_ban).Select(q => q.id).ToList();
                    //    var so_luong_mat_hang_px = repo._context.sys_phieu_xuat_kho_chi_tiets.AsQueryable()
                    //       .Where(q => px.Contains(q.id_phieu_xuat_kho) && q.status_del == 1 && q.id_mat_hang == itemNew.id_mat_hang).Sum(q => q.so_luong) ?? 0;

                    //    var pn = repo._context.sys_phieu_nhap_khos.AsQueryable().Where(q => q.id_don_hang_ban == item.db.id_don_hang_ban).Select(q => q.id).ToList();
                    //    var so_luong_mat_hang_pn = repo._context.sys_phieu_nhap_kho_chi_tiets.AsQueryable()
                    //       .Where(q => pn.Contains(q.id_phieu_nhap_kho) && q.status_del == 1 && q.id_mat_hang == itemNew.id_mat_hang).Sum(q => q.so_luong) ?? 0;

                    //    var so_luong = so_luong_mat_hang_dhb - (so_luong_mat_hang_px - so_luong_mat_hang_pn);

                    //    if (itemNew.db.so_luong > so_luong && action == ActionEnumForm.create)
                    //    {
                    //        ModelState.AddModelError("db.so_luong" + i, "so_luong_xuat_ban_khong_vuot_qua_so_luong_don_hang");
                    //    }
                    //    if (itemNew.db.so_luong > so_luong_mat_hang_dhb && action == ActionEnumForm.edit)
                    //    {
                    //        ModelState.AddModelError("db.so_luong" + i, "so_luong_xuat_ban_khong_vuot_qua_so_luong_don_hang");
                    //    }

                    //}

                    //if (!string.IsNullOrEmpty(item.db.id_don_hang_mua))
                    //{
                    //    var so_luong_mat_hang_dhm = repo._context.sys_don_hang_mua_mat_hangs.AsQueryable()
                    //   .Where(q => q.status_del == 1 && q.id_don_hang == item.db.id_don_hang_mua && q.id_mat_hang == itemNew.id_mat_hang)
                    //   .Sum(q => q.so_luong) ?? 0;
                    //    var px = repo._context.sys_phieu_xuat_khos.AsQueryable().Where(q => q.id_don_hang_mua == item.db.id_don_hang_mua).Select(q => q.id).ToList();
                    //    var so_luong_mat_hang_px = repo._context.sys_phieu_xuat_kho_chi_tiets.AsQueryable()
                    //       .Where(q => px.Contains(q.id_phieu_xuat_kho) && q.status_del == 1 && q.id_mat_hang == itemNew.id_mat_hang).Sum(q => q.so_luong) ?? 0;

                    //    var pn = repo._context.sys_phieu_nhap_khos.AsQueryable().Where(q => q.id_don_hang_mua == item.db.id_don_hang_mua).Select(q => q.id).ToList();
                    //    var so_luong_mat_hang_pn = repo._context.sys_phieu_nhap_kho_chi_tiets.AsQueryable()
                    //       .Where(q => pn.Contains(q.id_phieu_nhap_kho) && q.status_del == 1 && q.id_mat_hang == itemNew.id_mat_hang).Sum(q => q.so_luong) ?? 0;


                    //    var so_luong = so_luong_mat_hang_pn - so_luong_mat_hang_px;


                    //    //if (itemNew.db.so_luong > so_luong)
                    //    //{
                    //    //    ModelState.AddModelError("db.so_luong" + i, "so_luong_hoan_tra_khong_duoc_lon_hon_so_luong_da_nhap_ve");
                    //    //}

                    //    if (itemNew.db.so_luong > so_luong && action == ActionEnumForm.create)
                    //    {
                    //        ModelState.AddModelError("db.so_luong" + i, "so_luong_hoan_tra_khong_duoc_lon_hon_so_luong_da_nhap_ve");
                    //    }
                    //    if (itemNew.db.so_luong > so_luong_mat_hang_dhm && action == ActionEnumForm.edit)
                    //    {
                    //        ModelState.AddModelError("db.so_luong" + i, "so_luong_hoan_tra_khong_duoc_lon_hon_so_luong_da_nhap_ve");
                    //    }
                    //}






                }

            }
            return ModelState.IsValid;
        }

    }
}
