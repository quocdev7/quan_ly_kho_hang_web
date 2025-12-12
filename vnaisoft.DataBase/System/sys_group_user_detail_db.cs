using MongoDB.Bson.Serialization.Attributes;
using System;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace quan_ly_kho.DataBase.System
{
    [Table("sys_group_user_detail_db")]
    public class sys_group_user_detail_db
    {
        [BsonId]

        public string id { get; set; }
        public string id_group_user { get; set; }
        public string user_id { get; set; }

        public int? status_del { get; set; }
        public string nguoi_cap_nhat { get; set; }
         [BsonDateTimeOptions(Kind = DateTimeKind.Local)] public DateTime? ngay_cap_nhat { get; set; }
        public bool? is_system { get; set; }
    }
}
