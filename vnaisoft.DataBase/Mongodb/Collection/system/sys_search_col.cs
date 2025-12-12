using MongoDB.Bson.Serialization.Attributes;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace quan_ly_kho.DataBase.Mongodb.Collection.system
{
    [Table("sys_search_col")]
    public class sys_search_col
    {
        [BsonId]
        public string id { get; set; }
        public string search_text { get; set; }
        public int? type { get; set; }
        public int? status_del { get; set; }
        public string id_ref { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)] public DateTime? ngay_dang { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)] public DateTime? ngay_tao { get; set; }
        public string nguoi_tao { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)] public DateTime? ngay_cap_nhat { get; set; }
        public string nguoi_cap_nhat { get; set; }
    }
    

}
