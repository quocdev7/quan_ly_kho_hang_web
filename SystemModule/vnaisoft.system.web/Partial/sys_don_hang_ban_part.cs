using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using vnaisoft.common.BaseClass;
using vnaisoft.common.Models;
using vnaisoft.system.data.Models;

namespace vnaisoft.system.web.Controller
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
                "sys_don_hang_ban;getElementByIdLog",
                "sys_don_hang_ban;getListUse",
                "sys_don_hang_ban;create",
                 "sys_don_hang_ban;edit",
                 "sys_don_hang_ban;update_status_del",
                 "sys_don_hang_ban;get_list_don_vi_tinh",
                 "sys_don_hang_ban;exportExcel",
                 "sys_don_hang_ban;get_code",
                   "sys_don_hang_ban;downloadtemp",
                    "sys_don_hang_ban;ImportFromExcel",
                    "sys_don_hang_ban;ImportFromExcelChiTiet",
                    "sys_don_hang_ban;exportExcel",
                    "sys_don_hang_ban;getElementById",
                    "sys_don_hang_ban;getListUseDetails",
                    "sys_don_hang_ban;get_list_don_hang_ban",
                     "sys_don_hang_ban;ImportFromExcelDetail",
                     "sys_don_hang_ban;ImportFromExcelMatHang",
                     "sys_don_hang_ban;get_list_mat_hang",
                     "sys_don_hang_ban;getPrint",
                     "sys_don_hang_ban;cap_nhap",
                     "sys_don_hang_ban;check_kho",
                     "sys_don_hang_ban;DataHandlerDonHangBanHH",
                     "sys_don_hang_ban;DataHandlerDonHangBanChuaThu",
                     "sys_don_hang_ban;DatahandlerHistoryEdit",
                     "sys_don_hang_ban;get_list_nhan_vien",
                     "sys_don_hang_ban;get_cau_hinh_he_thong_gia_ban",
                     "sys_don_hang_ban;exportExcelDetails",

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
            if (item.db.phuong_thuc_thanh_toan == 2)
            {
                if (string.IsNullOrEmpty(item.db.id_tai_khoan_ngan_hang))
                {
                    ModelState.AddModelError("db.id_tai_khoan_ngan_hang", "required");
                }
            }
            //else
            //{
            //    for (int i = 0; i < item.db.list_mat_hang.Count; i++)
            //    {
            //        var mat_hang = item.db.list_mat_hang[i];
            //        var ton_kho = repo._context.erp_ton_kho_mat_hangs.AsQueryable().Where(d => d.id_mat_hang == mat_hang.ma_mat_hang).Sum(q => q.so_luong_ton > 0 ? (q.so_luong_ton ?? 0) : 0);
            //        decimal? sl_ton_kho = (decimal?)((ton_kho / 1000));
            //        var value_cau_hinh = repo._context.erp_cau_hinh_he_thongs.AsQueryable().Where(d => d.code == "cho_phep_ton_kho_am").Select(d => d.value).SingleOrDefault();
            //        if (mat_hang.so_luong == null)
            //        {
            //            ModelState.AddModelError("db.so_luong" + i, "required");
            //        }
            //        else
            //        {
            //            if (mat_hang.so_luong <= 0)
            //            {
            //                ModelState.AddModelError("db.so_luong" + i, "erp.phai_lon_hon_0");
            //            }
            //            if (mat_hang.thuoc_tinh != 6)
            //            {
            //                if (item.db.is_sinh_tu_dong == true)
            //                {
            //                    if (value_cau_hinh == "2")
            //                    {
            //                        if (mat_hang.db.so_luong > sl_ton_kho)
            //                        {
            //                            ModelState.AddModelError("db.so_luong" + i, "so_luong_ban_ra_khong_duoc_vuot_qua_so_luong_ton_kho");
            //                        }
            //                    }
            //                }
            //            }

            //        }
            //    }
            //}
            //if (item.db.hinh_thuc_van_chuyen == 2)
            //{
            //    if (item.db.ngay_dat_hang == null)
            //    {
            //        ModelState.AddModelError("db.ngay_dat_hang", "required");
            //    }
            //    if (String.IsNullOrEmpty(item.db.so_dien_thoai_nguoi_nhan))
            //    {
            //        ModelState.AddModelError("db.so_dien_thoai_nguoi_nhan", "required");
            //    }
            //    else
            //    {

            //        if (item.db.so_dien_thoai_nguoi_nhan.Length > 10)
            //        {
            //            ModelState.AddModelError("db.so_dien_thoai_nguoi_nhan", "system.soDienThoaiKhongHopLe");
            //        }
            //        else
            //        {
            //            var rgSoDienThoai = new Regex(@"(^[\+]?[0-9]{10,13}$) 
            //        |(^[0-9]{3}-[0-9]{4}-[0-9]{4}$)
            //        |(^\+[0-9]{2}\s+[0-9]{2}[0-9]{8}$)
            //        |(^[(]?[\+]?[\s]?[(]?[0-9]{2,3}[)]?[-\s\.]?[0-9]{2,4}[-\s\.]?[0-9]{2,4}[-\s\.]?[0-9]{2,4}[-\s\.]?[0-9]{0,4}[-\s\.]?$)");

            //            //var rgSoDienThoai = new Regex(@"(^[0-9]{10,13}$)|(^\+[0-9]{2}\s+[0-9]{2}[0-9]{8}$)|(^\+[0-9]{2}\s+[0-9]{4}\s+[0-9]{3}\s+[0-9]{3}$)|(^[0-9]{3}-[0-9]{4}-[0-9]{4}$)|(^[0-9]{4}\.[0-9]{3}\.[0-9]{3}$)");
            //            var checkSDT = rgSoDienThoai.IsMatch(item.db.so_dien_thoai_nguoi_nhan);
            //            if (checkSDT == false)
            //            {
            //                ModelState.AddModelError("db.so_dien_thoai_nguoi_nhan", "system.soDienThoaiKhongHopLe");
            //            }
            //            else
            //            {

            //            }
            //        }
            //    }
            //    if (string.IsNullOrEmpty(item.db.dia_chi_giao_hang))
            //    {
            //        ModelState.AddModelError("db.dia_chi_giao_hang", "required");
            //    }
            //}

            var queryTable = repo._context.sys_don_hang_ban_col.AsQueryable().Where(q => q.id == item.db.id.Trim());
            var search = repo.FindAll(queryTable).Where(d => d.db.ma == item.db.ma && d.db.id != item.db.id).Count();
            if (search > 0)
            {
                ModelState.AddModelError("db.ma", "existed");
            }
            return ModelState.IsValid;
        }
        //public string CheckErrorImport(sys_don_hang_ban_model model, int ct, string error)
        //{
        //    //if (model.db.is_thu_du == null)
        //    //{
        //    //    error += "Phải nhập đã thu tiền tại dòng" + (ct + 1) + "<br />";
        //    //}
        //    //if (model.db.is_xuat_du == null)
        //    //{
        //    //    error += "Phải nhập đã xuất kho tại dòng" + (ct + 1) + "<br />";
        //    //}

        //    if (model.db.loai_giao_dich == null)
        //    {
        //        error += "Phải nhập loại giao dịch tại dòng" + (ct + 1) + "<br />";
        //    }

        //    if (String.IsNullOrEmpty(model.db.ma.ToString()))
        //    {
        //        error += "Phải nhập mã đơn hàng bán tại dòng" + (ct + 1) + "<br />";
        //    }
        //    else
        //    {
        //        var check = repo._context.sys_don_hang_bans.AsQueryable().Where(q => q.ma.Trim().ToLower() == model.db.ma.Trim().ToLower()).SingleOrDefault();
        //        if (check != null)
        //        {
        //            error += "Mã đơn hàng bán  tại dòng " + (ct + 1) + "đã tồn tại <br />";
        //        }
        //    }

        //    if (String.IsNullOrEmpty(model.ma_mat_hang.ToString()))
        //    {
        //        error += "Phải nhập mã mặt hàng mua tại dòng" + (ct + 1) + "<br />";
        //    }
        //    else
        //    {
        //        var check = repo._context.erp_mat_hangs.AsQueryable().Where(q => q.ma.Trim().ToLower() == model.ma_mat_hang.Trim().ToLower()).SingleOrDefault();
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
        //        var check = repo._context.erp_khach_hang_nha_cung_caps.AsQueryable().Where(q => q.ma.Trim().ToLower() == model.db.id_doi_tuong.Trim().ToLower()).SingleOrDefault();
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
        //            var check = repo._context.erp_tai_khoan_ngan_hangs.AsQueryable().Where(q => q.so_tai_khoan == model.db.so_tai_khoan).SingleOrDefault();
        //            if (check == null)
        //            {
        //                error += "Số tài khoản ngân hàng chuyển tại dòng " + (ct + 1) + " không tồn tại <br />";
        //            }
        //        }

        //    }
        //    if (model.db.ngay_dat_hang == null)
        //    {
        //        error += "Phải nhập ngày thu tại dòng" + (ct + 1) + "<br />";
        //    }

        //    if (model.db.hinh_thuc_van_chuyen == 2)
        //    {
        //        if (model.db.so_ngay_du_kien == null)
        //        {
        //            error += "Phải nhập số ngày dự kiến tại dòng" + (ct + 1) + "<br />";
        //        }
        //        else
        //        {
        //            if (model.db.so_ngay_du_kien < 0)
        //            {
        //                error += "Số ngày dự kiến không được nhỏ hơn 0" + (ct + 1) + "<br />";
        //            }
        //        }


        //        if (model.db.tien_van_chuyen == null)
        //        {
        //            error += "Phải nhập tiền vận chuyển tại dòng" + (ct + 1) + "<br />";
        //        }
        //        else
        //        {
        //            if (model.db.so_ngay_du_kien < 0)
        //            {
        //                error += "Tiền vận chuyển không được nhỏ hơn 0" + (ct + 1) + "<br />";
        //            }
        //        }

        //        if (String.IsNullOrEmpty(model.db.vat_van_chuyen.ToString()))
        //        {
        //            error += "Phải nhập thuế vận chuyển tại dòng" + (ct + 1) + "<br />";
        //        }
        //        else
        //        {
        //            var check = Constant.list_vat.Where(q => q.id.Trim().ToLower() == model.db.vat_van_chuyen.Trim().ToLower()).SingleOrDefault();
        //            if (check == null)
        //            {
        //                error += "Vat vận chuyển tại dòng " + (ct + 1) + " không tồn tại <br />";
        //            }
        //        }
        //        if (String.IsNullOrEmpty(model.db.id_don_vi_van_chuyen.ToString()))
        //        {
        //            error += "Phải nhập đơn vị vận chuyển tại dòng" + (ct + 1) + "<br />";
        //        }
        //        if (String.IsNullOrEmpty(model.db.ma_don_van_chuyen.ToString()))
        //        {
        //            error += "Phải nhập mã đơn vị vận chuyển tại dòng" + (ct + 1) + "<br />";
        //        }
        //        if (model.db.ngay_den_han_thanh_toan == null)
        //        {
        //            error += "Phải nhập ngày đến hạn thanh toán" + (ct + 1) + "<br />";
        //        }
        //    }


        //    return error;
        //}
        //public string CheckErrorImportDetail(sys_don_hang_ban_mat_hang_model model, int ct, string error)
        //{
        //    if (model.db.id_mat_hang == null)
        //    {
        //        error += "Mặt hàng không tồn tại tại dòng " + (ct + 1) + "<br />";
        //    }
        //    else
        //    {
        //        if (model.db.so_luong == null)
        //        {
        //            error += "Phải nhập số lượng tại dòng" + (ct + 1) + "<br />";

        //        }
        //        else
        //        {
        //            if (model.db.so_luong <= 0)
        //            {

        //                error += "Số lượng  phải lớn hơn 0 tại dòng" + (ct + 1) + "<br />";

        //            }
        //        }
        //    }

        //    return error;
        //}
        //public string CheckErrorImportMatHang(sys_don_hang_ban_mat_hang_model model, int ct, string error)
        //{
        //    if (model.ma_mat_hang == null)
        //    {
        //        error += "Phải nhập Mã mặt hàng tại dòng " + (ct + 1) + "<br />";
        //    }
        //    else
        //    {
        //        var check = repo._context.erp_mat_hangs.AsQueryable().Where(q => q.ma.Trim().ToLower() == model.ma_mat_hang.Trim().ToLower()).SingleOrDefault();
        //        if (check == null)
        //        {
        //            error += "Mã mặt hàng tại dòng " + (ct + 1) + " không tồn tại <br />";
        //        }
        //    }

        //    if (model.db.so_luong == null)
        //    {
        //        error += "Phải nhập Số lượng tại dòng " + (ct + 1) + "<br />";
        //    }
        //    else
        //    {
        //        if (model.db.so_luong <= 0)
        //        {
        //            error += "Số lượng tại dòng " + (ct + 1) + " phải lớn hơn 0 " + "<br />";
        //        }
        //    }
        //    return error;
        //}

    }
}
