using MongoDB.Bson.Serialization.Attributes;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace quan_ly_kho.DataBase.System
{
    [Table("sys_group_user_db")]
    public class sys_group_user_db
    {
        [BsonId]
        public string id { get; set; }
        public string name { get; set; }
        public string note { get; set; }
        public int? type_user { get; set; }
        public int? status_del { get; set; }

        public string nguoi_cap_nhat { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)] public DateTime? ngay_cap_nhat { get; set; }
        public bool? is_system { get; set; }
        /// <summary>
        /// is_phan_quyen == true: không cho phép phân quyền tài khoản ở sys_phan_quyen_tai_khoan_index; ngược lại
        /// </summary>
        public bool? is_phan_quyen { get; set; }

    }
}
