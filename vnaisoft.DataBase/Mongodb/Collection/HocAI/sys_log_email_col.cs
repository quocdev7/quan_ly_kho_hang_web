using MongoDB.Bson.Serialization.Attributes;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace vnaisoft.DataBase.Mongodb.Collection.HocAI
{
    [Table("sys_log_email_col")]
    public class sys_log_email_col
    {
        [BsonId]

        public string id { get; set; }

        public string user_id { get; set; }
        public int ket_qua { get; set; }
        public string email { get; set; }
        public DateTime? send_date { get; set; }
        public DateTime? active_date { get; set; }
        public string id_template { get; set; }
        public string tieu_de { get; set; }
        public string noi_dung { get; set; }
        public string otp { get; set; }
        public int status_del { get; set; }
    }


}
