using System;
using System.Collections.Generic;
using System.Text;

namespace quan_ly_kho.common.Models
{
    public class MailSettings
    {
        public string Mail { get; set; }
        public string DisplayName { get; set; }
        public string Password { get; set; }
        public string gmail_application_password { get; set; }

        public bool gmail { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public int Max_Count_false { get; set; }
    }
}
