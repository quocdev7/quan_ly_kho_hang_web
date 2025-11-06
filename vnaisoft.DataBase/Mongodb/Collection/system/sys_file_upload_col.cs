using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;


namespace vnaisoft.DataBase.Mongodb.Collection.system
{
    [Table("sys_file_upload_col")]
    public class sys_file_upload_col
    {
        [BsonId]
        public string id { get; set; }
        // id của phiếu
        public string id_phieu { get; set; }
        //Luu lại cái tên controller mà uploadFileNay
        public string ma_cong_viec { get; set; }
        public string extension_file { get; set; }
        public string file_name { get; set; }
        public string file_path { get; set; }
        public string file_path_download { get; set; }
        public long? file_size { get; set; }
        public long? create_time_micro_epoch { get; set; }
        public string thumbnail_file { get; set; }
        //1 use, 2 not_use, 3 delete from server
        public decimal? length_video { get; set; }
        public int? status_del { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)] public DateTime? ngay_tao { get; set; }
        public string nguoi_tao { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)] public DateTime? ngay_cap_nhat { get; set; }
        public string nguoi_cap_nhat { get; set; }


        public int? page_processing { get; set; }
        public int? total_page { get; set; }
        public int? total_word_errors { get; set; }
        public int? total_word_errors_fix { get; set; }
        public List<sys_file_upload_page> list_pages { get; set; }
        public List<sys_file_upload_slide> list_slide { get; set; }
        public int? page_error { get; set; }
        public int? page_process { get; set; }


        public string noi_dung_text { get; set; }
        public string tom_tat_tai_file { get; set; }
        public float widthpx { get; set; }
        public float heightpx { get; set; }
        public decimal? duration { get; set; }



    }
    public class sys_file_upload_slide
    {
        public int? slide_number { get; set; }
        public string thumbnail { get; set; }
        public string path_thumbnail { get; set; }
        public bool? only_image { get; set; }
    }
    public class sys_file_upload_page
    {
        public int? page_num { get; set; }
        public string content_text { get; set; }
        public string content_text_origin { get; set; }
        public List<sys_tai_lieu_line_poly_gon> lines { get; set; }
        public List<sys_tai_lieu_line_word_error> line_word_errors { get; set; }
        public string path_image { get; set; }
        public double? widthpixel { get; set; }
        public double? heightpixel { get; set; }
        public string thumbnail_path_image { get; set; }
    }
    public class sys_tai_lieu_line_poly_gon
    {
        public string content_text { get; set; }
        public string content_text_origin { get; set; }
        public int? status_del { get; set; }
        public List<sys_tai_lieu_image_point> boundingPolygon { get; set; }

    }
    public class sys_tai_lieu_line_word_error
    {
        public int? line_num { get; set; }
        public string word_error { get; set; }
        public bool? is_fixed { get; set; }
        public List<string> recommend_word { get; set; }
    }
    public class sys_tai_lieu_image_point
    {
        public float? X { get; set; }
        public float? Y { get; set; }

    }
}
