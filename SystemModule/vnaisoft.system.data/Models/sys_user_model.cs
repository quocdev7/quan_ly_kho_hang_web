using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using vnaisoft.DataBase.Mongodb.Collection.system;
using vnaisoft.DataBase.System;

namespace vnaisoft.system.data.Models
{
    public class sys_user_model
    {
        public sys_user_model()
        {
            db = new User();
            file = new sys_file_upload_col();
        }
        public User db { get; set; }
        public string id { get; set; }
        public int? actionEnum { get; set; }
        public string hinh_anh_dai_dien { get; set; }
        public string ho_va_ten { get; set; }
        public string id_phong_ban { get; set; }
        public string Username { get; set; }
        public string phone { get; set; }
        public string email { get; set; }

        public string nguoi_duyet { get; set; }
        public string otp { get; set; }
        public string nguoi_cap_nhat { get; set; }
        public string password { get; set; }
        public string repassword { get; set; }
        public string oldPassword { get; set; }
        public string nguoi_tao { get; set; }
        public string capcha { get; set; }
        public int? showCaptcha { get; set; }
        public sys_file_upload_col file { get; set; }



    }
}


