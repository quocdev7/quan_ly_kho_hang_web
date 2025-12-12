using MongoDB.Driver;
using quan_ly_kho.common.BaseClass;
using quan_ly_kho.common.Models;
using quan_ly_kho.system.data.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace quan_ly_kho.system.web.Controller
{
    partial class sys_userController
    {
        public static ControllerAppModel declare = new ControllerAppModel()
        {
            controller = "sys_user",
            icon = "heroicons_solid:user",
            icon_image = "/assets/icons/team.png",
            module = "he_thong",
            id = "sys_user",
            url = "/sys_user_index",
            title = "sys_user",
            translate = "NAV.sys_user",
            type = "item",
            //is_show_all_user = true,
            list_controller_action_public = new List<string>(){

                "sys_user;delete",
                "sys_user;getListUse",
                "sys_user;forgot_pass",
                 "sys_user;changePassword",

                "sys_user;ImportFromExcel",
                "sys_user;Download",
                "sys_user;changePasswordByAdmin",
                  "sys_user;getListUseNew",



            "sys_user;create",
            "sys_user;edit",
            "sys_user;revert",
            "sys_user;delete",
            "sys_user;DataHandler",
                 "sys_user;get_list_thanh_vien",
                 "sys_user;getUserLogin",
                  "sys_user;getUserLoginSS0",
                 "sys_user;update_status_del",
            "sys_user;insert_log_thao_tac",

             "sys_user;create_loi_moi",
              "sys_user;edit_loi_moi",
              "sys_user;getListUseNew",


              "sys_user;get_list_thanh_vien_hchinh_nsu",
              "sys_user;update_view_file",
              "sys_user;go_bo",
               "sys_user;get_list_message",
                 "sys_user;get_host",


            },
            list_controller_action_publicNonLogin = new List<string>(){
                  "sys_user;save_token_hoc_cung_ai",


                   "sys_user;get_user_in_group",
                   "sys_user;authenticate_sso",
                      "sys_user;send_mail_otp_new",
                        "sys_user;confirm_otp_new",
                 "sys_user;GoogleLogin",
                 "sys_user;downloadtemp",
                "sys_user;ImportFromExcel",
                "sys_user;authenticate",
                "sys_user;sync_full_name",
                "sys_user;forgot_pass",
                "sys_user;checkResetPass",
                "sys_user;changePasswordNonLogin",
                "sys_user;register",

                "sys_user;updateProfile",
                "sys_user;getUserOtp",
                "sys_user;getAnotherUserInfo",
                "sys_user;xac_thuc",
                "sys_user;send_otp",
                "sys_user;authenticate",

                "sys_user;get_profile_user",
                "sys_user;getUserInfo",
                "sys_user;updateProfile",

                 "sys_user;check_error_loi_moi",
                  "sys_user;get_thong_tin_loi_moi_nonlogin",
                   "sys_user;access_loi_moi",
                   "sys_user;moi_lai",
                    "sys_user;check_error_loi_moi",
                    "sys_user;getInfoProfile",
                    "sys_user;GetCaptchaImage",
                     "sys_user;signInWithFacebook",
                "sys_user;confirm_otp",
                "sys_user;send_mail_otp",
                "sys_user;send_mail_otp",
                "sys_user;authenticate_facebook",
                "sys_user;authenticate_google",
                "sys_user;authenticate_apple",
            },
            list_role = new List<ControllerRoleModel>()
            {
                new ControllerRoleModel()
                {
                  id="sys_user;list",
                  name="list",
                  list_controller_action = new List<string>()
                  {

                  }
                }
            }
        };
        private bool checkModelStateUser(string full_name, string phone, string password)
        {
            if (string.IsNullOrEmpty(full_name))
            {
                ModelState.AddModelError("db.full_name", "required");
            }
            else
            {
                if (string.IsNullOrEmpty(password))
                {
                    ModelState.AddModelError("db.password", "system.vuilongnhapmatkhau");
                }
                else
                {
                    if (password.Length < 5)
                    {
                        ModelState.AddModelError("db.password", "system.matkhauphaitu6kitutrolen");
                    }
                }

            }
            if (string.IsNullOrEmpty(phone))
            {
                ModelState.AddModelError("db.phone", "required");
            }
            else
            {

                if (phone.Length > 10)
                {
                    ModelState.AddModelError("db.phone", "system.soDienThoaiKhongHopLe");
                }
                else
                {
                    var rgSoDienThoai = new Regex(@"(^[\+]?[0-9]{10,13}$) 
            |(^[0-9]{3}-[0-9]{4}-[0-9]{4}$)
            |(^\+[0-9]{2}\s+[0-9]{2}[0-9]{8}$)
            |(^[(]?[\+]?[\s]?[(]?[0-9]{2,3}[)]?[-\s\.]?[0-9]{2,4}[-\s\.]?[0-9]{2,4}[-\s\.]?[0-9]{2,4}[-\s\.]?[0-9]{0,4}[-\s\.]?$)");

                    //var rgSoDienThoai = new Regex(@"(^[0-9]{10,13}$)|(^\+[0-9]{2}\s+[0-9]{2}[0-9]{8}$)|(^\+[0-9]{2}\s+[0-9]{4}\s+[0-9]{3}\s+[0-9]{3}$)|(^[0-9]{3}-[0-9]{4}-[0-9]{4}$)|(^[0-9]{4}\.[0-9]{3}\.[0-9]{3}$)");
                    var checkSDT = rgSoDienThoai.IsMatch(phone);
                    if (checkSDT == false)
                    {
                        ModelState.AddModelError("db.phone", "system.soDienThoaiKhongHopLe");
                    }
                    //else
                    //{
                    //    var dienthoai = phone;  //CMAESCrypto.EncryptText(item.db.dienthoai);

                    //    var checkdienthoai = repo.FindAll().Where(d => d.db.phone == dienthoai && d.db.Id != item.db.Id).Count();
                    //    if (checkdienthoai > 0)
                    //    {
                    //        ModelState.AddModelError("db.phone", "existed");
                    //    }
                    //}
                }
            }
            return ModelState.IsValid;
        }


        private bool checkModelStateCreate(sys_user_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.create, item);
        }

        private bool checkModelStateEdit(sys_user_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.edit, item);
        }
        private bool checkModelStateCreateEdit(ActionEnumForm action, sys_user_model item)
        {
            if (string.IsNullOrEmpty(item.email))
            {
                ModelState.AddModelError("db.email", "required");
            }
            if (item.actionEnum == 1)
            {
                if (string.IsNullOrEmpty(item.password))
                {
                    ModelState.AddModelError("db.password", "required");
                }
            }

            if (!string.IsNullOrEmpty(item.phone))
            {
                if (item.phone.Length > 10)
                {
                    ModelState.AddModelError("db.phone", "system.soDienThoaiKhongHopLe");
                }
                else
                {
                    var rgSoDienThoai = new Regex(@"(^[\+]?[0-9]{10,13}$) 
                                        |(^[0-9]{3}-[0-9]{4}-[0-9]{4}$)
                                        |(^\+[0-9]{2}\s+[0-9]{2}[0-9]{8}$)
                                        |(^[(]?[\+]?[\s]?[(]?[0-9]{2,3}[)]?[-\s\.]?[0-9]{2,4}[-\s\.]?[0-9]{2,4}[-\s\.]?[0-9]{2,4}[-\s\.]?[0-9]{0,4}[-\s\.]?$)");

                    //var rgSoDienThoai = new Regex(@"(^[0-9]{10,13}$)|(^\+[0-9]{2}\s+[0-9]{2}[0-9]{8}$)|(^\+[0-9]{2}\s+[0-9]{4}\s+[0-9]{3}\s+[0-9]{3}$)|(^[0-9]{3}-[0-9]{4}-[0-9]{4}$)|(^[0-9]{4}\.[0-9]{3}\.[0-9]{3}$)");
                    var checkSDT = rgSoDienThoai.IsMatch(item.phone);
                    if (checkSDT == false)
                    {
                        ModelState.AddModelError("db.phone", "system.soDienThoaiKhongHopLe");
                    }
                    else
                    {

                    }
                }
            }
            if (!string.IsNullOrEmpty(item.email) && item.email != "administrator")
            {


                var rgEmail = new Regex(@"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
                                   + "@"
                                   + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))\z");
                var checkEmail = rgEmail.IsMatch(item.email);
                if (checkEmail == false)
                {
                    ModelState.AddModelError("db.email", "system.emailKhongHopLe");
                }
                else
                {
                    var email = item.email;// CMAESCrypto.EncryptText(item.db.email);
                    var checkemail = repo.FindAll().Where(d => d.db.status_del != 2)
                        .Where(d => d.email == email && d.id != item.id).Count();
                    if (checkemail > 0)
                    {
                        ModelState.AddModelError("db.email", "existed");
                    }
                }


            }

            if (string.IsNullOrEmpty(item.ho_va_ten))
            {
                ModelState.AddModelError("db.ho_va_ten", "required");
            }




            return ModelState.IsValid;
        }
        private void removeUserModel(sys_user_model obj)
        {
            obj.db.otp = null;
            obj.db.PasswordHash = null;
            obj.db.PasswordSalt = null;
        }
    }
}
