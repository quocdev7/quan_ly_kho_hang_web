using MongoDB.Bson.Serialization.Attributes;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace vnaisoft.DataBase.Mongodb.Collection.HocAI
{
    [Table("sys_send_otp_col")]
    public class sys_send_otp_col
    {
        [BsonId]
        public string id { get; set; }
        public string otp { get; set; }
        public string phone { get; set; }
        //1 active, 2 non-active
        public int status_del { get; set; }
        public DateTime? send_date { get; set; }
        public string err { get; set; }

        public DateTime? active_date { get; set; }
        public string device_id { get; set; }
        public string device_name { get; set; }

    }


}
