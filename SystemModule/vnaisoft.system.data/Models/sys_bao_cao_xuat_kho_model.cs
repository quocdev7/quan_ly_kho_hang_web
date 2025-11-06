using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using vnaisoft.common.Helpers;
using vnaisoft.DataBase.Mongodb.Collection.system;

namespace vnaisoft.system.data.Models
{
    public class bao_cao_xuat_kho_model
    {

        public string ma_loai_xuat { get; set; }
        public string ten_loai_xuat { get; set; }
        public string ma_loai_mat_hang { get; set; }
        public string id_kho { get; set; }
        public string id_loai_mat_hang { get; set; }
        public string ten_loai_mat_hang { get; set; }
        public string ma_mat_hang { get; set; }
        public string ten_mat_hang { get; set; }
        public string ten_don_vi_tinh { get; set; }
        public string ma_kho { get; set; }
        public string ten_kho { get; set; }
        public decimal? so_luong { get; set; }
        public decimal? don_gia { get; set; }
        public decimal? gia_tri { get; set; }
        public string ten_doi_tuong { get; set; }
        public string id_don_vi_tinh { get; set; }
        public string ma_so_thue { get; set; }
        public string dien_thoai { get; set; }
        public string email { get; set; }
        public string dia_chi_doi_tuong { get; set; }
        public DateTime? ngay_cap_nhat { get; set; }
        public string id_phieu_xuat_kho { get; set; }
        public string ma_phieu_xuat_kho { get; set; }
        public DateTime? ngay_xuat_kho { get; set; }
        public decimal? tong_so_luong { get; set; }
        public string ma_doi_tuong { get; set; }
        public string noi_dung { get; set; }
        public string tai_khoan_no { get; set; }
        public string tai_khoan_co { get; set; }
        public string tai_khoan_kho { get; set; }
        public int? status_del { get; set; }
    }

}


