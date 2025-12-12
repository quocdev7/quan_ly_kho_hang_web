using System;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using System.Text;

namespace quan_ly_kho.DataBase.Function
{
	public class Fn_get_sys_approval
    {
		public string id { get; set; }
		 [BsonDateTimeOptions(Kind = DateTimeKind.Local)] public DateTime? deadline { get; set; }
		public int? status_action { get; set; }
		public int? status_finish { get; set; }
		public string to_user { get; set; }
		public string from_user { get; set; }


		public string to_user_name { get; set; }
		public string from_user_name { get; set; }
		public int? step_num { get; set; }
		public string step_name { get; set; }
		public string last_note { get; set; }
		 [BsonDateTimeOptions(Kind = DateTimeKind.Local)] public DateTime? last_date_action { get; set; }
	}

}
