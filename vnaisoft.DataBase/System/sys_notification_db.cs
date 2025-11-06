using MongoDB.Bson.Serialization.Attributes;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace vnaisoft.DataBase.System
{
    [Table("sys_notification")]
    public class sys_notification_db
    {
        [BsonId]
        public string id { get; set; }
        public string user_id { get; set; }
        public string id_thong_bao { get; set; }
        public string user_id_send { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)] public DateTime? ngay_xem { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        //0 unread,1 read
        public int? status_read { get; set; }
        public int? check_xem { get; set; }
        public string menu { get; set; }
        public string link { get; set; }
        public string param { get; set; }
        public string logo { get; set; }
        public string contenthtml { get; set; }
        public int? status_del { get; set; }

 

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)] public DateTime? date_send { get; set; }
    }
}
