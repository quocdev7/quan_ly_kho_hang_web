using vnaisoft.DataBase.Mongodb.Collection.system;
using vnaisoft.DataBase.System;

namespace vnaisoft.system.data.Models
{
    public class sys_template_mail_model
    {
        public sys_template_mail_model()
        {
            db = new sys_template_mail_col();
        }
        public string nguoi_cap_nhat { get; set; }
        public sys_template_mail_col db { get; set; }
    }
}
