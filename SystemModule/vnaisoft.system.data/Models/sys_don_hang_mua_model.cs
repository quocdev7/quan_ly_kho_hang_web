using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using System.Linq;
using vnaisoft.common.Helpers;
using vnaisoft.DataBase.Mongodb.Collection.system;

namespace vnaisoft.system.data.Models
{
    public class sys_don_hang_mua_model
    {
        public sys_don_hang_mua_model()
        {
            db = new sys_don_hang_mua_col();
            //list_mat_hang = new List<erp_don_hang_mua_mat_hang_model>();
        }
        public sys_don_hang_mua_col db { get; set; }
        public string ten_nguoi_cap_nhat { get; set; }
        //public string ten_ngan_hang_doi_tuong
        //{
        //    get
        //    {
        //        return Constant.listbank.Where(q => q.id == db.id_ngan_hang_doi_tuong).Select(q => q.name).SingleOrDefault();
        //    }
        //}
        //public string ngay_cap_nhap_str { get; set; }
        public string ten_doi_tuong { get; set; }
        //public string ten_kho { get; set; }
        //public bool? is_check { get; set; }
        //public string hinh_thuc_doi_tuong_str { get; set; }
        //public string ten_ngan_hang { get; set; }
        ////1 hàng hóa , 2 dịch vụ
        //public string loai_giao_dich_str { get; set; }

        //public string id_mat_hangs { get; set; }
        //public string tong_tien_bang_chu { get; set; }
        //public decimal? tong_tien_sau_thue { get; set; }
        //public decimal? chi_phi_van_chuyen { get; set; }
        //public List<erp_don_hang_mua_mat_hang_model> list_mat_hang { get; set; }
        //public int? check_doi_tuong { get; set; }
        //public string ma_doi_tuong { get; set; }
        //public string ma_ngan_hang_nhan { get; set; }
        //public string ma_mat_hang { get; set; }
        //public decimal? so_luong { get; set; }
        //public decimal? don_gia { get; set; }
        //public string ghi_chu_chi_tiet { get; set; }
        //public decimal? chiet_khau { get; set; }
        //public string vat { get; set; }
    }
    //public class erp_don_hang_mua_mat_hang_model
    //{
    //    public erp_don_hang_mua_mat_hang_model()
    //    {
    //        //db = new erp_don_hang_mua_mat_hang_db();
    //    }
    //    public int? thuoc_tinh { get; set; }
    //    public string ten_thuoc_tinh
    //    {
    //        get
    //        {
    //            return Constant.list_thuoc_tinh.Where(q => q.id == thuoc_tinh).Select(q => q.name).SingleOrDefault();
    //        }
    //    }
    //    public string ten_nguoi_cap_nhat { get; set; }
    //    public string ngay_cap_nhap_str { get; set; }
    //    public string trang_thai_str { get; set; }
    //    public string tai_khoan_no { get; set; }
    //    public string tai_khoan_co { get; set; }
    //    public string doi_tuong_no { get; set; }
    //    public string doi_tuong_co { get; set; }
    //    public string id_mat_hang { get; set; }
    //    public string ma_mat_hang { get; set; }
    //    public string ten_mat_hang { get; set; }
    //    public string ten_don_vi_tinh { get; set; }
    //    public string id_don_vi_tinh { get; set; }
    //    public decimal? so_luong { get; set; }
    //    public decimal? don_gia { get; set; }
    //    public decimal? thanh_tien { get; set; }
    //    public decimal? chiet_khau { get; set; }
    //    public string ghi_chu_chi_tiet { get; set; }
    //    public decimal? thanh_tien_truoc_thue { get; set; }
    //    public decimal? thanh_tien_sau_thue { get; set; }
    //    public decimal? tien_vat { get; set; }
    //    public decimal? ty_suat_vat { get; set; }
    //    public decimal? thanh_tien_chiet_khau { get; set; }
    //    public string vat { get; set; }
    //    [BsonRepresentation(BsonType.Decimal128)] public decimal? he_so_quy_doi { get; set; }
    //    [BsonRepresentation(BsonType.Decimal128)] public decimal? so_luong_quy_doi { get; set; }
    //    [BsonRepresentation(BsonType.Decimal128)] public decimal? don_gia_quy_doi { get; set; }
    //    //public erp_don_hang_mua_mat_hang_db db { get; set; }
    //}
}


