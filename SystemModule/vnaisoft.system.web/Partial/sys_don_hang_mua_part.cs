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
            //if (string.IsNullOrEmpty(item.db.id_kho_nhap))
            //{
            //    ModelState.AddModelError("db.id_kho_nhap", "required");
            //}
            //if (item.db.loai_giao_dich == null)
            //{
            //    ModelState.AddModelError("db.loai_giao_dich", "required");
            //}
            //if (item.db.phuong_thuc_thanh_toan == 2)
            //{
            //    if (string.IsNullOrEmpty(item.db.id_tai_khoan_ngan_hang))
            //    {
            //        ModelState.AddModelError("db.id_tai_khoan_ngan_hang", "required");
            //    }
            //}

            //if (item.db.list_mat_hang.Count == 0)
            //{
            //    ModelState.AddModelError("db.list_mat_hang", "sys.phai_chon_mat_hang");
            //}
            //else
            //{
            //    for (int i = 0; i < item.db.list_mat_hang.Count; i++)
            //    {
            //        var mat_hang = item.db.list_mat_hang[i];
            //        if (mat_hang.so_luong == null)
            //        {
            //            ModelState.AddModelError("db.so_luong" + i, "required");
            //        }
            //        else
            //        {
            //            if (mat_hang.so_luong <= 0)
            //            {
            //                ModelState.AddModelError("db.so_luong" + i, "sys.phai_lon_hon_0");
            //            }
            //        }
            //    }
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
            //if (item.db.tien_van_chuyen == null)
            //{
            //    ModelState.AddModelError("db.tien_van_chuyen", "required");
            //}
            //if (item.db.vat_van_chuyen == null)
            //{
            //    ModelState.AddModelError("db.vat_van_chuyen", "required");
            //}
            if (item.db.ngay_dat_hang == null)
            {
                ModelState.AddModelError("db.ngay_dat_hang", "required");
            }
            //if (item.db.so_ngay_du_kien == null)
            //{
            //    ModelState.AddModelError("db.so_ngay_du_kien", "required");
            //}


            //if (item.db.ngay_du_kien_nhan_hang == null)
            //{
            //    ModelState.AddModelError("db.ngay_du_kien_nhan_hang", "required");
            //}
            var queryTable = repo._context.sys_don_hang_mua_col.AsQueryable().Where(q => q.id == item.db.id.Trim());
            var search = repo.FindAll(queryTable).Where(d => d.db.ma == item.db.ma && d.db.id != item.db.id).Count();
            if (search > 0)
            {
                ModelState.AddModelError("db.ma", "existed");
            }

            return ModelState.IsValid;
        }
        //public string CheckErrorImport(sys_don_hang_mua_model model, int ct, string error)
        //{
        //    if (model.db.is_chi_du == null)
        //    {
        //        error += "Phải nhập đã chi tiền tại dòng" + (ct + 1) + "<br />";
        //    }
        //    if (model.db.is_nhap_du == null)
        //    {
        //        error += "Phải nhập đã nhập kho tại dòng" + (ct + 1) + "<br />";
        //    }
        //    if (model.db.loai_giao_dich == null)
        //    {
        //        error += "Phải nhập loại giao dịch tại dòng" + (ct + 1) + "<br />";
        //    }

        //    if (String.IsNullOrEmpty(model.db.ma.ToString()))
        //    {
        //        error += "Phải nhập mã đơn hàng mua tại dòng" + (ct + 1) + "<br />";
        //    }
        //    else
        //    {
        //        var check = repo._context.sys_don_hang_muas.AsQueryable().Where(q => q.ma.Trim().ToLower() == model.db.ma.Trim().ToLower() && q.status_del == 1).SingleOrDefault();
        //        if (check != null)
        //        {
        //            error += "Mã đơn hàng mua  tại dòng " + (ct + 1) + "đã tồn tại <br />";
        //        }
        //    }

        //    if (String.IsNullOrEmpty(model.ma_mat_hang.ToString()))
        //    {
        //        error += "Phải nhập mã mặt hàng mua tại dòng" + (ct + 1) + "<br />";
        //    }
        //    else
        //    {
        //        var check = repo._context.sys_mat_hangs.AsQueryable().Where(q => q.ma.Trim().ToLower() == model.ma_mat_hang.Trim().ToLower()).SingleOrDefault();
        //        if (check == null)
        //        {
        //            error += "Mã mặt hàng tại dòng " + (ct + 1) + "không tồn tại <br />";
        //        }
        //    }

        //    if (model.so_luong == null)
        //    {
        //        error += "Phải nhập số lượng  tại dòng" + (ct + 1) + "<br />";
        //    }
        //    if (model.don_gia == null)
        //    {
        //        error += "Phải nhập đơn giá tại dòng" + (ct + 1) + "<br />";
        //    }
        //    if (String.IsNullOrEmpty(model.db.id_doi_tuong.ToString()))
        //    {
        //        error += "Phải nhập mã đối tượng tại dòng" + (ct + 1) + "<br />";
        //    }
        //    else
        //    {
        //        var check = repo._context.sys_khach_hang_nha_cung_caps.AsQueryable().Where(q => q.ma.Trim().ToLower() == model.db.id_doi_tuong.Trim().ToLower()).SingleOrDefault();
        //        if (check == null)
        //        {
        //            error += "Mã đối tượng tại dòng " + (ct + 1) + " không tồn tại <br />";
        //        }
        //    }

        //    if (model.db.phuong_thuc_thanh_toan == 2)
        //    {
        //        if (String.IsNullOrEmpty(model.db.so_tai_khoan.ToString()))
        //        {
        //            error += "Phải nhập Số tài khoản ngân hàng chuyển tại dòng" + (ct + 1) + "<br />";
        //        }
        //        else
        //        {
        //            var check = repo._context.sys_tai_khoan_ngan_hangs.AsQueryable().Where(q => q.so_tai_khoan == model.db.so_tai_khoan).SingleOrDefault();
        //            if (check == null)
        //            {
        //                error += "Số tài khoản ngân hàng chuyển tại dòng " + (ct + 1) + " không tồn tại <br />";
        //            }
        //        }
        //        //// kiểm tra nếu ma_doi_tuong = 'DTTD' thì nhập
        //        //if (model.db.id_doi_tuong == "DTTD")
        //        //{
        //        //    if (String.IsNullOrEmpty(model.ma_ngan_hang_nhan.ToString()))
        //        //    {
        //        //        error += "Phải nhập mã ngân hàng nhận tại dòng" + (ct + 1) + "<br />";
        //        //    }
        //        //    if (String.IsNullOrEmpty(model.db.so_tai_khoan_doi_tuong.ToString()))
        //        //    {
        //        //        error += "Phải nhập số tài khoản ngân hàng nhận tại dòng" + (ct + 1) + "<br />";
        //        //    }
        //        //}



        //    }
        //    if (model.db.ngay_dat_hang == null)
        //    {
        //        error += "Phải nhập ngày đặt hàng tại dòng" + (ct + 1) + "<br />";
        //    }

        //    if (model.db.so_ngay_du_kien == null)
        //    {
        //        // error += "Phải nhập số ngày dự kiến tại dòng" + (ct + 1) + "<br />";
        //    }
        //    else
        //    {
        //        if (model.db.so_ngay_du_kien < 0)
        //        {
        //            error += "Số ngày dự kiến không được nhỏ hơn 0" + (ct + 1) + "<br />";
        //        }
        //    }


        //    if (model.db.tien_van_chuyen == null)
        //    {
        //        error += "Phải nhập tiền vận chuyển tại dòng" + (ct + 1) + "<br />";
        //    }
        //    else
        //    {
        //        if (model.db.so_ngay_du_kien < 0)
        //        {
        //            error += "Tiền vận chuyển không được nhỏ hơn 0" + (ct + 1) + "<br />";
        //        }
        //    }

        //    if (String.IsNullOrEmpty(model.db.vat_van_chuyen.ToString()))
        //    {
        //        error += "Phải nhập thuế vận chuyển tại dòng" + (ct + 1) + "<br />";
        //    }
        //    else
        //    {
        //        var check = Constant.list_vat.Where(q => q.id.Trim().ToLower() == model.db.vat_van_chuyen.Trim().ToLower()).SingleOrDefault();
        //        if (check == null)
        //        {
        //            error += "Vat vận chuyển tại dòng " + (ct + 1) + " không tồn tại <br />";
        //        }
        //    }






        //    return error;
        //}

        //public string CheckErrorImportDetail(sys_don_hang_mua_mat_hang_model model, int ct, string error)
        //{
        //    if (String.IsNullOrEmpty(model.db.id_don_hang.ToString()))
        //    {
        //        error += "Phải nhập mã đơn hàng tại dòng" + (ct + 1) + "<br />";
        //    }
        //    else
        //    {
        //        var check = repo._context.sys_don_hang_muas.AsQueryable().Where(q => q.id.Trim().ToLower() == model.db.id_don_hang.Trim().ToLower()).SingleOrDefault();
        //        if (check == null)
        //        {
        //            error += "Mã đơn hàng tại dòng " + (ct + 1) + " không tồn tại <br />";
        //        }
        //    }

        //    if (String.IsNullOrEmpty(model.db.id_mat_hang.ToString()))
        //    {
        //        error += "Phải nhập mã mặt hàng tại dòng" + (ct + 1) + "<br />";
        //    }
        //    else
        //    {
        //        var check = repo._context.sys_mat_hangs.AsQueryable().Where(q => q.id.Trim().ToLower() == model.db.id_mat_hang.Trim().ToLower()).SingleOrDefault();
        //        if (check == null)
        //        {
        //            error += "Mã mặt hàng tại dòng " + (ct + 1) + " không tồn tại <br />";
        //        }
        //    }

        //    if (model.db.don_gia == null)
        //    {
        //        error += "Phải nhập đơn giá tại dòng" + (ct + 1) + "<br />";
        //    }

        //    if (model.db.so_luong == null)
        //    {
        //        error += "Phải nhập số lượng tại dòng" + (ct + 1) + "<br />";
        //    }
        //    if (model.db.so_luong <= 0)
        //    {
        //        error += "Số lượng phải lớn hơn 0 tại dòng" + (ct + 1) + "<br />";
        //    }

        //    return error;
        //}


        //public string CheckErrorImportMatHang(sys_don_hang_mua_mat_hang_model model, int ct, string error)
        //{
        //    if (model.ma_mat_hang == null)
        //    {
        //        error += "Phải nhập Mã mặt hàng tại dòng " + (ct + 1) + "<br />";
        //    }
        //    else
        //    {
        //        var check = repo._context.sys_mat_hangs.AsQueryable().Where(q => q.ma.Trim().ToLower() == model.ma_mat_hang.Trim().ToLower()).SingleOrDefault();
        //        if (check == null)
        //        {
        //            error += "Mã mặt hàng tại dòng " + (ct + 1) + " không tồn tại <br />";
        //        }
        //    }

        //    if (model.db.so_luong == null)
        //    {
        //        error += "Phải nhập Số lượng tại dòng " + (ct + 1) + "<br />";
        //    }
        //    return error;
        //}

    }
}
