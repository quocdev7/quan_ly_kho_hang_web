using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FireBase.API.API
{

    public class TokenNotiUserModel
    {
        public TokenNotiUserModel()
        {
            listDevice = new List<TokenNotiDeviceUser> { };
        }
        public string id { get; set; }
        public string user_id { get; set; }
        public string token_firebase { get; set; }
        public DateTime create_date { get; set; }
        public int count_notification { get; set; }
        public List<TokenNotiDeviceUser> listDevice { get; set; }
        public string user_name { get; set; }
        public DateTime date_upDate { get; set; }

    }
    public class TokenNotiDeviceUser
    {
        public long date_sign_in { get; set; }
        public string device_id { get; set; }
        public string device_name { get; set; }
        public string device_type { get; set; }
        public string device_version { get; set; }
        public int status { get; set; }
        public string token_firebase { get; set; }
    }
    public class NotificationModel
    {
        public string id { get; set; }
        public string title { get; set; }
        public string desc { get; set; }
        DateTime send_time { get; set; }
        int send_time_index { get; set; }
        public string user_id_create { get; set; }
        public string user_id_receive { get; set; }
        // 1 la send , 2 read
        int status { get; set; }
        public string route { get; set; }
    }

}
