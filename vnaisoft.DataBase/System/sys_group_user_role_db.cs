using MongoDB.Bson.Serialization.Attributes;
using System;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace quan_ly_kho.DataBase.System
{
    [Table("sys_group_user_role_db")]
    public class sys_group_user_role_db
    {
        [BsonId]    
        public string id { get; set; }
        public string id_group_user{ get; set; }
        public string id_controller_role { get; set; }
        public string controller_name { get; set; }
        public string role_name { get; set; }
        public string id_module { get; set; }
    }
}
