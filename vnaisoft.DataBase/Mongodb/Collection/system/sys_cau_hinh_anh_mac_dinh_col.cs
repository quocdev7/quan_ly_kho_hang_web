using MongoDB.Bson.Serialization.Attributes;
using System;
using System.ComponentModel.DataAnnotations.Schema;


namespace vnaisoft.DataBase.Mongodb.Collection.system
{
    [Table("sys_cau_hinh_anh_mac_dinh_col")]
    public class sys_cau_hinh_anh_mac_dinh_col
    {
        [BsonId]
        public string id { get; set; }
        public string image { get; set; }
        public string avatar { get; set; }
        /// <summary>
        /// 1: Hình ảnh đại diện
        /// 2: Logo
        /// 3: Học sinh
        /// 4: Giáo viên
        /// 5: Phụ huynh
        /// 6: Môn học
        /// 7: Khác
        /// </summary>
        public int? type { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)] public DateTime? ngay_tao { get; set; }
        public string nguoi_tao { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)] public DateTime? ngay_cap_nhat { get; set; }
        public string nguoi_cap_nhat { get; set; }
        public int? status_del { get; set; }




    }
}
