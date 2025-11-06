using MongoDB.Driver;
using NPOI.SS.Formula.Functions;
using Opc.Ua;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using vnaisoft.common.BaseClass;
using vnaisoft.common.Models;
using vnaisoft.system.data.Models;

namespace vnaisoft.system.web.Controller
{
    partial class sys_khach_hang_nha_cung_capController
    {
        public static ControllerAppModel declare = new ControllerAppModel()
        {
            controller = "  ",
            icon = "badge",
            icon_image = "/assets/images/shungo/blue_feed_icon.png",
            module = "erp",
            id = "sys_khach_hang_nha_cung_cap",
            url = "/sys_khach_hang_nha_cung_cap_index",
            title = "sys_khach_hang_nha_cung_cap",
            translate = "NAV.sys_khach_hang_nha_cung_cap",
            type = "item",
            //is_show_all_user = true,
            list_controller_action_public = new List<string>(){
                "sys_khach_hang_nha_cung_cap;getListUse",
                "sys_khach_hang_nha_cung_cap;get_lst_nha_cung_cap_thuoc_du_an",
                "sys_khach_hang_nha_cung_cap;get_list_nha_cung_cap",
                "sys_khach_hang_nha_cung_cap;getElementByMa",
               "sys_khach_hang_nha_cung_cap;create",
                   "sys_khach_hang_nha_cung_cap;edit",
                    "sys_khach_hang_nha_cung_cap;delete",
                     "sys_khach_hang_nha_cung_cap;get_list_doi_tuong",
                     "sys_khach_hang_nha_cung_cap;get_code",
                     "sys_khach_hang_nha_cung_cap;get_list_ma_so_thue",
                     "sys_khach_hang_nha_cung_cap;sync_cong_ty",
                     "sys_khach_hang_nha_cung_cap;ImportFromExcel",
                       "sys_khach_hang_nha_cung_cap;exportExcel",
                 "sys_khach_hang_nha_cung_cap;update_status_del",

               "sys_khach_hang_nha_cung_cap;get_doi_tuong",
               "sys_khach_hang_nha_cung_cap;get_khach_hang_le",
               "sys_khach_hang_nha_cung_cap;getListUseNew",
               "sys_khach_hang_nha_cung_cap;get_doi_tuong_tu_do",
               "sys_khach_hang_nha_cung_cap;get_lst_khach_hang_multiple",
               "sys_khach_hang_nha_cung_cap;get_list_doi_tuong",



            },
            list_controller_action_publicNonLogin = new List<string>()
            {
                 "sys_khach_hang_nha_cung_cap;downloadtemp",

            },
            list_role = new List<ControllerRoleModel>()
            {
            //     new ControllerRoleModel()
            //    {
            //        id="sys_khach_hang_nha_cung_cap;create",
            //        name="create",
            //        list_controller_action = new List<string>()
            //        {
            //              "sys_khach_hang_nha_cung_cap;create",
            //        }
            //    },
            //    new ControllerRoleModel()
            //    {
            //        id="sys_khach_hang_nha_cung_cap;edit",
            //        name="edit",
            //        list_controller_action = new List<string>()
            //        {
            //              "sys_khach_hang_nha_cung_cap;edit",
            //        }
            //    },
            //    new ControllerRoleModel()
            //    {
            //        id="sys_khach_hang_nha_cung_cap;delete",
            //        name="delete",
            //        list_controller_action = new List<string>()
            //        {
            //              "sys_khach_hang_nha_cung_cap;delete",
            //        }
            //    },
            new ControllerRoleModel()
            {
                id = "sys_khach_hang_nha_cung_cap;list",
                name = "list",
                list_controller_action = new List<string>()
                    {
                          "sys_khach_hang_nha_cung_cap;DataHandler",
                             "sys_khach_hang_nha_cung_cap;DataHandlerCaNhanToChuc",
                    }
            }
            }
        };

        private bool checkModelStateCreate(sys_khach_hang_nha_cung_cap_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.create, item);
        }

        private bool checkModelStateEdit(sys_khach_hang_nha_cung_cap_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.edit, item);
        }
        private bool checkModelStateCreateEdit(ActionEnumForm action, sys_khach_hang_nha_cung_cap_model item)
        {
            if (String.IsNullOrEmpty(item.db.ten))
            {
                ModelState.AddModelError("db.ten", "required");
            }
            if (!String.IsNullOrEmpty(item.db.email))
            {
                var rgEmail = new Regex(@"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
                   + "@"
                   + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))\z");
                var checkEmail = rgEmail.IsMatch(item.db.email);
                if (checkEmail == false)
                {
                    ModelState.AddModelError("db.email", "system.emailKhongHopLe");
                }
                else
                {

                }
            }


            if (item.db.hinh_thuc == null)
            {
                ModelState.AddModelError("db.hinh_thuc", "required");
            }
            else
            {
                if (item.db.hinh_thuc == 2)
                {
                    if (String.IsNullOrEmpty(item.db.ma_so_thue))
                    {
                        ModelState.AddModelError("db.ma_so_thue", "required");
                    }
                    else
                    {
                        var check_mst = repo.FindAll().Where(d => d.db.ma_so_thue == item.db.ma_so_thue && d.db.id != item.db.id).Count();
                        if (check_mst > 0)
                        {
                            ModelState.AddModelError("db.ma_so_thue", "existed");
                        }
                    }
                    if (String.IsNullOrEmpty(item.db.dien_thoai))
                    {
                        ModelState.AddModelError("db.dien_thoai", "required");

                    }
                    else
                    {
                        if (item.db.dien_thoai.Length > 13 || item.db.dien_thoai.Length < 10)
                        {
                            ModelState.AddModelError("db.dien_thoai", "system.soDienThoaiKhongHopLe");
                        }
                        else
                        {
                            var rgSoDienThoai = new Regex(@"(^[\+]?[0-9]{10,13}$) 
            |(^[0-9]{3}-[0-9]{4}-[0-9]{4}$)
            |(^\+[0-9]{2}\s+[0-9]{2}[0-9]{8}$)
            |(^[(]?[\+]?[\s]?[(]?[0-9]{2,3}[)]?[-\s\.]?[0-9]{2,4}[-\s\.]?[0-9]{2,4}[-\s\.]?[0-9]{2,4}[-\s\.]?[0-9]{0,4}[-\s\.]?$)");

                            //var rgSoDienThoai = new Regex(@"(^[0-9]{10,13}$)|(^\+[0-9]{2}\s+[0-9]{2}[0-9]{8}$)|(^\+[0-9]{2}\s+[0-9]{4}\s+[0-9]{3}\s+[0-9]{3}$)|(^[0-9]{3}-[0-9]{4}-[0-9]{4}$)|(^[0-9]{4}\.[0-9]{3}\.[0-9]{3}$)");
                            var checkSDT = rgSoDienThoai.IsMatch(item.db.dien_thoai);
                            if (checkSDT == false)
                            {
                                ModelState.AddModelError("db.dien_thoai", "system.soDienThoaiKhongHopLe");
                            }
                            else
                            {

                            }
                        }
                    }
                }
                else
                {
                    if (String.IsNullOrEmpty(item.db.ma_so_thue))
                    {
                        ModelState.AddModelError("db.ma_so_thue", "required");
                    }
                    else
                    {
                        var check_mst = repo.FindAll().Where(d => d.db.ma_so_thue == item.db.ma_so_thue && d.db.id != item.db.id).Count();
                        if (check_mst > 0)
                        {
                            ModelState.AddModelError("db.ma_so_thue", "existed");
                        }
                    }
                    if (String.IsNullOrEmpty(item.db.dien_thoai))
                    {
                        ModelState.AddModelError("db.dien_thoai", "required");

                    }
                    else
                    {
                        if (item.db.dien_thoai.Length < 10 || item.db.dien_thoai.Length > 13)
                        {
                            ModelState.AddModelError("db.dien_thoai", "system.soDienThoaiKhongHopLe");
                        }
                        else
                        {
                            var rgSoDienThoai = new Regex(@"(^[\+]?[0-9]{10,13}$) 
            |(^[0-9]{3}-[0-9]{4}-[0-9]{4}$)
            |(^\+[0-9]{2}\s+[0-9]{2}[0-9]{8}$)
            |(^[(]?[\+]?[\s]?[(]?[0-9]{2,3}[)]?[-\s\.]?[0-9]{2,4}[-\s\.]?[0-9]{2,4}[-\s\.]?[0-9]{2,4}[-\s\.]?[0-9]{0,4}[-\s\.]?$)");

                            //var rgSoDienThoai = new Regex(@"(^[0-9]{10,13}$)|(^\+[0-9]{2}\s+[0-9]{2}[0-9]{8}$)|(^\+[0-9]{2}\s+[0-9]{4}\s+[0-9]{3}\s+[0-9]{3}$)|(^[0-9]{3}-[0-9]{4}-[0-9]{4}$)|(^[0-9]{4}\.[0-9]{3}\.[0-9]{3}$)");
                            var checkSDT = rgSoDienThoai.IsMatch(item.db.dien_thoai);
                            if (checkSDT == false)
                            {
                                ModelState.AddModelError("db.dien_thoai", "system.soDienThoaiKhongHopLe");
                            }
                            else
                            {

                            }
                        }
                        var check = repo._context.sys_khach_hang_nha_cung_cap_col.AsQueryable().Where(q => q.dien_thoai == item.db.dien_thoai && q.id != item.db.id).SingleOrDefault();
                        if (check != null)
                        {
                            ModelState.AddModelError("db.dien_thoai", "existed");
                        }
                    }

                }

            }



            return ModelState.IsValid;
        }
        //public string CheckErrorImport(sys_khach_hang_nha_cung_cap_model model, int ct, string error)
        //{
        //    if (model.db.hinh_thuc == 2)
        //    {
        //        if (String.IsNullOrEmpty(model.db.ma_so_thue.ToString()))
        //        {
        //            error += "Phải nhập mã số thuế tại dòng" + (ct + 1) + "<br />";
        //        }
        //        else
        //        {
        //            var check = repo._context.sys_khach_hang_nha_cung_cap_col.AsQueryable().Where(q => q.ma_so_thue == model.db.ma_so_thue).SingleOrDefault();
        //            if (check != null)
        //            {
        //                error += "Mã số thuế tại dòng " + (ct + 1) + " đã tồn tại <br />";
        //            }
        //        }
        //    }
        //    if (String.IsNullOrEmpty(model.db.dien_thoai.ToString()))
        //    {
        //        error += "Phải nhập số điện thoại tại dòng" + (ct + 1) + "<br />";
        //    }
        //    else
        //    {
        //        if (model.db.dien_thoai.Length > 10)
        //        {
        //            ModelState.AddModelError("db.dien_thoai", "system.soDienThoaiKhongHopLe");
        //        }
        //        else
        //        {
        //            var rgSoDienThoai = new Regex(@"(^[\+]?[0-9]{10,13}$) 
        //    |(^[0-9]{3}-[0-9]{4}-[0-9]{4}$)
        //    |(^\+[0-9]{2}\s+[0-9]{2}[0-9]{8}$)
        //    |(^[(]?[\+]?[\s]?[(]?[0-9]{2,3}[)]?[-\s\.]?[0-9]{2,4}[-\s\.]?[0-9]{2,4}[-\s\.]?[0-9]{2,4}[-\s\.]?[0-9]{0,4}[-\s\.]?$)");

        //            //var rgSoDienThoai = new Regex(@"(^[0-9]{10,13}$)|(^\+[0-9]{2}\s+[0-9]{2}[0-9]{8}$)|(^\+[0-9]{2}\s+[0-9]{4}\s+[0-9]{3}\s+[0-9]{3}$)|(^[0-9]{3}-[0-9]{4}-[0-9]{4}$)|(^[0-9]{4}\.[0-9]{3}\.[0-9]{3}$)");
        //            var checkSDT = rgSoDienThoai.IsMatch(model.db.dien_thoai);
        //            if (checkSDT == false)
        //            {
        //                ModelState.AddModelError("db.dien_thoai", "system.soDienThoaiKhongHopLe");
        //            }
        //            else
        //            {

        //            }
        //        }
        //        var check = repo._context.sys_khach_hang_nha_cung_cap_col.AsQueryable().Where(q => q.dien_thoai == model.db.dien_thoai).SingleOrDefault();
        //        if (check != null)
        //        {
        //            error += "Số điện thoại tại dòng " + (ct + 1) + " đã tồn tại <br />";
        //        }
        //    }

        //    if (String.IsNullOrEmpty(model.db.email.ToString()))
        //    {
        //        error += "Phải nhập email tại dòng" + (ct + 1) + "<br />";
        //    }
        //    else
        //    {
        //        var rgEmail = new Regex(@"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
        //                           + "@"
        //                           + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))\z");
        //        var checkEmail = rgEmail.IsMatch(model.db.email);
        //        if (checkEmail == false)
        //        {
        //            ModelState.AddModelError("db.email", "system.emailKhongHopLe");
        //        }
        //        else
        //        {

        //        }
        //        var check = repo._context.sys_khach_hang_nha_cung_cap_col.AsQueryable().Where(q => q.email == model.db.email).SingleOrDefault();
        //        if (check != null)
        //        {
        //            error += "Email tại dòng " + (ct + 1) + " đã tồn tại <br />";
        //        }

        //    }

        //    if (String.IsNullOrEmpty(model.db.ten.ToString()))
        //    {
        //        error += "Phải nhập tên đối tượng tại dòng" + (ct + 1) + "<br />";
        //    }
        //    else
        //    {
        //        var check_ten = repo._context.sys_khach_hang_nha_cung_cap_col.AsQueryable().Where(q => q.ten == model.db.ten).SingleOrDefault();
        //        if (check_ten != null)
        //        {
        //            error += "Tên đối tượng tại dòng " + (ct + 1) + " đã tồn tại <br />";
        //        }
        //    }
        //    return error;
        //}

    }
}
