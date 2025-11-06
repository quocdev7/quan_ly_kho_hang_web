using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using vnaisoft.common.Common;
using vnaisoft.DataBase.commonFunc;
using vnaisoft.DataBase.Helper;
using vnaisoft.DataBase.Mongodb;
using vnaisoft.DataBase.Mongodb.Collection.system;
using vnaisoft.system.data.Models;

namespace vnaisoft.system.data.DataAccess
{
    public class sys_don_hang_mua_repo
    {
        public MongoDBContext _context;
        public common_mongo_repo _common_repo;
        //public common_trigger_don_hang_mua_repo _common_trigger_don_hang_mua_repo;
        public sys_don_hang_mua_repo(MongoDBContext context)
        {
            _context = context;
            _common_repo = new common_mongo_repo(context);
            //_common_trigger_don_hang_mua_repo = new common_trigger_don_hang_mua_repo(context);
        }

        public string getCode()
        {
            var max = "";
            var config = _common_repo.get_code_config(false, "sys_don_hang_mua", "DHM");
            var max_query = _context.sys_don_hang_mua_col.AsQueryable()
             .Where(d => d.ma.StartsWith(config.prefix))
             .Where(d => d.ma.Length == config.prefix.Length + config.numIncrease)
             .Select(d => d.ma);
            if (max_query.Count() > 0)
            {
                max = max_query.Max();
            }
            var code = _common_repo.generateCode(config.prefix, config.numIncrease, max);

            return code;
        }

        //public async Task<sys_don_hang_mua_log_model> getElementByIdLog(string id)
        //{
        //    var query = _context.sys_don_hang_mua_logs.AsQueryable().Where(m => m.id == id);
        //    var obj = FindAllLog(query).SingleOrDefault();
        //    var queryTableDetail = _context.sys_don_hang_mua_mat_hang_logs.AsQueryable().Where(q => q.id_don_hang == id.Trim());
        //    obj.list_mat_hang = FindAllDetailLog(queryTableDetail).AsQueryable().Where(q => q.db.id_don_hang == id).ToList();
        //    obj.list_mat_hang.ForEach(t =>
        //    {
        //        if (t.db.id_loai_mat_hang != null)
        //        {
        //            var loai_dinh_khoan_mat_hang = _context.erp_loai_mat_hangs.AsQueryable().Where(d => d.id == t.db.id_loai_mat_hang && d.status_del == 1).SingleOrDefault();
        //            if (loai_dinh_khoan_mat_hang.id_loai_dinh_khoan_mat_hang != null)
        //            {
        //                var ldkmh = _context.erp_loai_dinh_khoan_mat_hangs.AsQueryable().Where(d => d.id == loai_dinh_khoan_mat_hang.id_loai_dinh_khoan_mat_hang && d.status_del == 1).SingleOrDefault();
        //                t.tai_khoan_no = ldkmh.ma_tk_no_tien_mat;
        //                t.tai_khoan_co = ldkmh.ma_tk_co_tien_mat;
        //            }
        //        }
        //        t.doi_tuong_no = obj.db.id_doi_tuong;
        //        t.doi_tuong_co = obj.db.id_doi_tuong;
        //        if (t.he_so_quy_doi != null)
        //        {
        //            t.so_luong_quy_doi = Math.Round((t.he_so_quy_doi ?? 0) * (t.db.so_luong ?? 0));
        //            t.don_gia_quy_doi = Math.Round((t.db.thanh_tien_truoc_thue ?? 0) / (t.so_luong_quy_doi ?? 1));
        //        }
        //    });
        //    obj.id_mat_hangs = string.Join(',', obj.list_mat_hang.Select(q => q.db.id_mat_hang).ToList());
        //    try
        //    {
        //        var rs = StringHelper.ChuyenSo(obj.db.tong_tien_sau_thue + "");
        //        obj.tong_tien_bang_chu = rs;
        //    }
        //    catch
        //    {
        //        obj.tong_tien_bang_chu = "";
        //    }
        //    return obj;
        //}
        //public async Task<int> upset_doituong(sys_don_hang_mua_model model)
        //{

        //    if (model.check_doi_tuong != 1)
        //    {
        //        var filter = Builders<erp_khach_hang_nha_cung_cap_db>.Filter.Eq(x => x.id, model.db.id_doi_tuong);

        //        var update = Builders<erp_khach_hang_nha_cung_cap_db>.Update
        //        .Set(x => x.laNhaCungCap, true);

        //        // Create an update definition to set the "Name" property to a new value
        //        await _context.erp_khach_hang_nha_cung_caps.UpdateOneAsync(filter, update);
        //    }




        //    return 1;
        //}
        //public async Task<sys_don_hang_mua_model> getElementById(string id)
        //{
        //    var query = _context.sys_don_hang_mua_col.AsQueryable().Where(m => m.id == id);
        //    var obj = FindAll(query).SingleOrDefault();
        //    var queryTableDetail = _context.sys_don_hang_mua_mat_hangs.AsQueryable().Where(q => q.id_don_hang == id.Trim());
        //    obj.list_mat_hang = FindAllDetail(queryTableDetail).AsQueryable().Where(q => q.db.id_don_hang == id).ToList();
        //    var tong_thanh_tien_chiet_khau = obj.list_mat_hang.Sum(d => d.db.thanh_tien_chiet_khau);
        //    var list = new List<decimal?>();
        //    for (int i = 0; i < obj.list_mat_hang.Count; i++)
        //    {
        //        var t = obj.list_mat_hang[i];
        //        if (t.db.id_loai_mat_hang != null)
        //        {
        //            var loai_dinh_khoan_mat_hang = _context.erp_loai_mat_hangs.AsQueryable().Where(d => d.id == t.db.id_loai_mat_hang && d.status_del == 1).SingleOrDefault();
        //            if (loai_dinh_khoan_mat_hang.id_loai_dinh_khoan_mat_hang != null)
        //            {
        //                var ldkmh = _context.erp_loai_dinh_khoan_mat_hangs.AsQueryable().Where(d => d.id == loai_dinh_khoan_mat_hang.id_loai_dinh_khoan_mat_hang && d.status_del == 1).SingleOrDefault();
        //                t.tai_khoan_no = ldkmh.ma_tk_no_tien_mat;
        //                t.tai_khoan_co = ldkmh.ma_tk_co_tien_mat;
        //            }
        //        }
        //        t.doi_tuong_no = obj.db.id_doi_tuong;
        //        t.doi_tuong_co = obj.db.id_doi_tuong;
        //        if (i != obj.list_mat_hang.Count - 1)
        //        {
        //            var ty_le_phan_bo = (t.db.thanh_tien_chiet_khau / tong_thanh_tien_chiet_khau);
        //            var chi_phi_van_chuyen_sau_phan_bo = ty_le_phan_bo * (obj.db.tien_van_chuyen ?? 0);
        //            var thanh_tien = t.db.thanh_tien_chiet_khau + Math.Round((decimal)(chi_phi_van_chuyen_sau_phan_bo));
        //            t.thanh_tien = Math.Round((decimal)(thanh_tien));
        //            var don_gia = thanh_tien / t.db.so_luong;
        //            t.don_gia = don_gia;
        //            list.Add(Math.Round((decimal)(chi_phi_van_chuyen_sau_phan_bo)));
        //        }
        //        else
        //        {
        //            var tong_tien_van_chuyen = list.Sum(d => d);
        //            var chi_phi_van_chuyen_sau_phan_bo = (obj.db.tien_van_chuyen ?? 0) - (tong_tien_van_chuyen ?? 0);
        //            var thanh_tien = t.db.thanh_tien_chiet_khau + chi_phi_van_chuyen_sau_phan_bo;
        //            t.thanh_tien = Math.Round((decimal)(thanh_tien));
        //            var don_gia = thanh_tien / t.db.so_luong;
        //            t.don_gia = don_gia;
        //        }
        //        var vat = Constant.list_vat.Where(q => q.id.Trim().ToLower() == t.db.vat).Select(q => q.value).SingleOrDefault();
        //        t.thanh_tien_sau_thue = t.thanh_tien * vat;
        //        var ty_suat_vat = t.thanh_tien * vat / 100;
        //        t.ty_suat_vat = Math.Round((decimal)(ty_suat_vat));
        //        t.vat = t.db.vat;
        //        if (t.he_so_quy_doi != null)
        //        {

        //            t.so_luong_quy_doi = Math.Round((t.he_so_quy_doi ?? 0) * (t.db.so_luong ?? 0));
        //            t.don_gia_quy_doi = Math.Round((t.db.thanh_tien_truoc_thue ?? 0) / (t.so_luong_quy_doi ?? 1));
        //        }

        //        //var tien_vat = t.thanh_tien * t.ty_suat_vat / 100;
        //        //t.tien_vat = Math.Round((decimal)(tien_vat));
        //    }
        //    ;
        //    obj.id_mat_hangs = string.Join(',', obj.list_mat_hang.Select(q => q.db.id_mat_hang).ToList());
        //    try
        //    {
        //        var rs = StringHelper.ChuyenSo(obj.db.tong_tien_sau_thue + "");
        //        obj.tong_tien_bang_chu = rs;
        //    }
        //    catch
        //    {
        //        obj.tong_tien_bang_chu = "";
        //    }
        //    return obj;
        //}
        public string generate_ten(sys_don_hang_mua_model model)
        {
            var ten = "";
            var pttt = "";
            //var loai = model.db.loai_giao_dich == 1 ? "Hàng hóa" : "Dịch vụ";
            var ngay = model.db.ngay_dat_hang.Value.ToString("dd/MM/yyyy");

            if (model.db.phuong_thuc_thanh_toan == 1)
            {
                pttt = "Tiền mặt";
            }
            else if (model.db.phuong_thuc_thanh_toan == 2)
            {

                //var ngan_hang = Constant.listbank.Where(q => q.id.Trim().ToLower() == model.db.id_tai_khoan_ngan_hang.Trim().ToLower()).SingleOrDefault();

                pttt = model.db.id_tai_khoan_ngan_hang + "-" + model.db.so_tai_khoan;
            }
            var tong_tien = model.db.tong_thanh_tien;
            //var ten_kh = model.db.id_khach_hang_nha_cung_cap + "(" + model.db.id_doi_tuong + ")";

            ten = "Đơn hàng mua " + " " + model.ten_doi_tuong + " " + ngay + ", " + pttt + ", " + String.Format("{0:#,##0}", tong_tien) + "đ";
            return ten;
        }
        //public void tinhTongTien(sys_don_hang_mua_model model)
        //{
        //    model.db.tong_tien_truoc_thue = model.list_mat_hang.Sum(d => d.db.thanh_tien_truoc_thue ?? 0);
        //    model.db.tong_tien_chiet_khau = model.list_mat_hang.Sum(data => (data.db.thanh_tien_truoc_thue ?? 0) - (data.db.thanh_tien_chiet_khau ?? 0));
        //    model.db.tong_tien_thue = model.list_mat_hang.Sum(data => (data.db.tien_vat ?? 0)) + (model.db.tien_vat_van_chuyen ?? 0);
        //    model.db.tong_tien_truoc_thue = model.db.tong_tien_truoc_thue + (model.db.tien_van_chuyen ?? 0);
        //    model.db.tong_tien_sau_chiet_khau = model.db.tong_tien_truoc_thue - model.db.tong_tien_chiet_khau;
        //    model.db.tong_tien_sau_thue = model.db.tong_tien_sau_chiet_khau + model.db.tong_tien_thue;

        //}
        //public void tinhTongTienImport(sys_don_hang_mua_model model)
        //{
        //    model.db.tong_tien_truoc_thue = model.list_mat_hang.Sum(d => d.thanh_tien_truoc_thue ?? 0);
        //    model.db.tong_tien_chiet_khau = model.list_mat_hang.Sum(data => (data.thanh_tien_truoc_thue ?? 0) - (data.thanh_tien_chiet_khau ?? 0));
        //    model.db.tong_tien_thue = model.list_mat_hang.Sum(data => (data.tien_vat ?? 0)) + (model.db.tien_vat_van_chuyen ?? 0);
        //    model.db.tong_tien_truoc_thue = model.db.tong_tien_truoc_thue + (model.db.tien_van_chuyen ?? 0);
        //    model.db.tong_tien_sau_chiet_khau = model.db.tong_tien_truoc_thue - model.db.tong_tien_chiet_khau;
        //    model.db.tong_tien_sau_thue = model.db.tong_tien_sau_chiet_khau + model.db.tong_tien_thue;

        //}
        //public async Task<int> add_log(sys_lich_su_import_db db)
        //{
        //    await _context.sys_lich_su_imports.InsertOneAsync(db);
        //    return 1;
        //}

        public async Task<int> insert(sys_don_hang_mua_model model)
        {
            //tinhTongTien(model);
            model.db.ten = generate_ten(model);
            model.db.ten_khong_dau = Regex.Replace(StringFunctions.NonUnicode(HttpUtility.HtmlDecode(model.db.ten ?? "")).ToLower().Normalize(), "<.*?>|&.*?;", String.Empty);
            //model.db.id_file_upload = model.db.id + "sys_don_hang_mua";
            await _context.sys_don_hang_mua_col.InsertOneAsync(model.db);
            //var doi_tuong = _context.sys_khach_hang_nha_cung_cap_col.AsQueryable().Where(q => q.id == model.db.id_khach_hang_nha_cung_cap).SingleOrDefault();
            //if (doi_tuong != null)
            //{
            //    if (doi_tuong.id_ngan_hang == null)
            //    {
            //        await update_doituong(model);
            //    }
            //}
            //if (model.db.list_mat_hang.Count() > 0)
            //{
            //    await upset_detail(model);
            //}
            //await _common_repo.insert_file(model.db.id, "sys_don_hang_mua");
            //if (model.db.is_chi_du == true)
            //{
            //    await _common_trigger_don_hang_mua_repo.InsertTriggerPhieuChi(model.db, model.list_mat_hang.Select(d => d.db).ToList());
            //    var id_dh = model.db.id;
            //    var update = Builders<sys_don_hang_mua_db>.Update
            //        .Set(x => x.so_tien_da_chi, model.db.tong_tien_sau_thue)
            //        .Set(x => x.is_chi_du, true)
            //        ;
            //    var filter = Builders<sys_don_hang_mua_db>.Filter.Eq(x => x.id, id_dh);
            //    await _context.sys_don_hang_mua_col.UpdateOneAsync(filter, update);
            //}
            //else
            //{
            //    var id_dh = model.db.id;
            //    var update = Builders<sys_don_hang_mua_db>.Update
            //        .Set(x => x.is_chi_du, false)
            //        ;
            //    var filter = Builders<sys_don_hang_mua_db>.Filter.Eq(x => x.id, id_dh);
            //    await _context.sys_don_hang_mua_col.UpdateOneAsync(filter, update);
            //}

            //if (model.db.is_nhap_du == true)
            //{
            //    await _common_trigger_don_hang_mua_repo.InsertTriggerPhieuNhap(model.db, model.list_mat_hang.Select(d => d.db).ToList());
            //    var id_dh = model.db.id;
            //    var update = Builders<sys_don_hang_mua_db>.Update
            //        .Set(x => x.is_nhap_du, true)
            //        ;
            //    var filter = Builders<sys_don_hang_mua_db>.Filter.Eq(x => x.id, id_dh);
            //    await _context.sys_don_hang_mua_col.UpdateOneAsync(filter, update);
            //}
            //else
            //{
            //    var id_dh = model.db.id;
            //    var update = Builders<sys_don_hang_mua_db>.Update
            //        .Set(x => x.is_nhap_du, false)
            //        ;
            //    var filter = Builders<sys_don_hang_mua_db>.Filter.Eq(x => x.id, id_dh);
            //    await _context.sys_don_hang_mua_col.UpdateOneAsync(filter, update);
            //}


            //if (model.db.is_sinh_tu_dong == true)
            //{
            //    await _common_trigger_don_hang_mua_repo.InsertTrigger(model.db, model.list_mat_hang.Select(d => d.db).ToList());
            //    var id_dh = model.db.id;
            //    var update = Builders<sys_don_hang_mua_db>.Update
            //        .Set(x => x.so_tien_da_chi, model.db.tong_tien_sau_thue)
            //        .Set(x => x.is_nhap_du, true)
            //        .Set(x => x.is_chi_du, true)
            //        ;
            //    var filter = Builders<sys_don_hang_mua_db>.Filter.Eq(x => x.id, id_dh);
            //    await _context.sys_don_hang_mua_col.UpdateOneAsync(filter, update);
            //}
            //else
            //{
            //    var id_dh = model.db.id;
            //    var update = Builders<sys_don_hang_mua_db>.Update
            //        .Set(x => x.is_nhap_du, false)
            //        .Set(x => x.is_chi_du, false)
            //        ;
            //    var filter = Builders<sys_don_hang_mua_db>.Filter.Eq(x => x.id, id_dh);
            //    await _context.sys_don_hang_mua_col.UpdateOneAsync(filter, update);
            //}
            //await insert_log(model);
            //await upset_doituong(model);
            return 1;
        }
        //public async Task<int> insert_import(sys_don_hang_mua_model model)
        //{
        //    tinhTongTienImport(model);
        //    model.db.ten = generate_ten(model);
        //    model.db.ten_khong_dau = Regex.Replace(StringFunctions.NonUnicode(HttpUtility.HtmlDecode(model.db.ten ?? "")).ToLower().Normalize(), "<.*?>|&.*?;", String.Empty);
        //    model.db.id_file_upload = model.db.id + "sys_don_hang_mua";
        //    await _context.sys_don_hang_mua_col.InsertOneAsync(model.db);
        //    var doi_tuong = _context.erp_khach_hang_nha_cung_caps.AsQueryable().Where(q => q.id == model.db.id_doi_tuong).SingleOrDefault();
        //    if (doi_tuong != null)
        //    {
        //        if (doi_tuong.id_ngan_hang == null)
        //        {
        //            await update_doituong(model);
        //        }
        //    }
        //    if (model.list_mat_hang.Count() > 0)
        //    {
        //        await upset_detail_import(model);
        //    }

        //    await _common_repo.insert_file(model.db.id, "sys_don_hang_mua");
        //    if (model.db.is_chi_du == true)
        //    {
        //        await _common_trigger_don_hang_mua_repo.InsertTriggerPhieuChi(model.db, model.list_mat_hang.Select(d => d.db).ToList());
        //        var id_dh = model.db.id;
        //        var update = Builders<sys_don_hang_mua_db>.Update
        //            .Set(x => x.so_tien_da_chi, model.db.tong_tien_sau_thue)
        //            .Set(x => x.is_chi_du, true)
        //            ;
        //        var filter = Builders<sys_don_hang_mua_db>.Filter.Eq(x => x.id, id_dh);
        //        await _context.sys_don_hang_mua_col.UpdateOneAsync(filter, update);
        //    }
        //    else
        //    {
        //        var id_dh = model.db.id;
        //        var update = Builders<sys_don_hang_mua_db>.Update
        //            .Set(x => x.is_chi_du, false)
        //            ;
        //        var filter = Builders<sys_don_hang_mua_db>.Filter.Eq(x => x.id, id_dh);
        //        await _context.sys_don_hang_mua_col.UpdateOneAsync(filter, update);
        //    }

        //    if (model.db.is_nhap_du == true)
        //    {
        //        await _common_trigger_don_hang_mua_repo.InsertTriggerPhieuNhap(model.db, model.list_mat_hang.Select(d => d.db).ToList());
        //        var id_dh = model.db.id;
        //        var update = Builders<sys_don_hang_mua_db>.Update
        //            .Set(x => x.is_nhap_du, true)
        //            ;
        //        var filter = Builders<sys_don_hang_mua_db>.Filter.Eq(x => x.id, id_dh);
        //        await _context.sys_don_hang_mua_col.UpdateOneAsync(filter, update);
        //    }
        //    else
        //    {
        //        var id_dh = model.db.id;
        //        var update = Builders<sys_don_hang_mua_db>.Update
        //            .Set(x => x.is_nhap_du, false)
        //            ;
        //        var filter = Builders<sys_don_hang_mua_db>.Filter.Eq(x => x.id, id_dh);
        //        await _context.sys_don_hang_mua_col.UpdateOneAsync(filter, update);
        //    }
        //    await insert_log(model);
        //    return 1;
        //}
        //public async Task<int> insert_log(sys_don_hang_mua_model model)
        //{
        //    sys_don_hang_mua_log_db item = new sys_don_hang_mua_log_db();
        //    item.id = Guid.NewGuid().ToString();
        //    item.id_don_hang = model.db.id;
        //    item.ma = model.db.ma;
        //    item.ten = model.db.ten;
        //    item.ten_khong_dau = model.db.ten_khong_dau;
        //    item.loai_giao_dich = model.db.loai_giao_dich;
        //    item.thanh_tien_truoc_thue = model.db.thanh_tien_truoc_thue;
        //    item.tien_thue = model.db.tien_thue;
        //    item.ghi_chu = model.db.ghi_chu;
        //    item.ly_do_chinh_sua = model.db.ly_do_chinh_sua;
        //    item.id_kho_nhap = model.db.id_kho_nhap;
        //    item.tien_van_chuyen = model.db.tien_van_chuyen;
        //    item.vat_van_chuyen = model.db.vat_van_chuyen;
        //    item.tien_vat_van_chuyen = model.db.tien_vat_van_chuyen;
        //    item.tien_khac = model.db.tien_khac;
        //    item.vat_khac = model.db.vat_khac;
        //    item.tien_vat_khac = model.db.tien_vat_khac;
        //    item.thanh_tien_sau_thue = model.db.thanh_tien_sau_thue;
        //    item.ngay_dat_hang = model.db.ngay_dat_hang;
        //    item.ngay_du_kien_nhan_hang = model.db.ngay_du_kien_nhan_hang;
        //    item.hinh_thuc_doi_tuong = model.db.hinh_thuc_doi_tuong;
        //    item.id_doi_tuong = model.db.id_doi_tuong;
        //    item.ten_doi_tuong = model.db.ten_doi_tuong;
        //    item.ma_so_thue = model.db.ma_so_thue;
        //    item.dien_thoai = model.db.dien_thoai;
        //    item.email = model.db.email;
        //    item.dia_chi_doi_tuong = model.db.dia_chi_doi_tuong;
        //    item.so_tai_khoan_doi_tuong = model.db.so_tai_khoan_doi_tuong;
        //    item.id_ngan_hang_doi_tuong = model.db.id_ngan_hang_doi_tuong;
        //    item.nguoi_cap_nhat = model.db.nguoi_cap_nhat;
        //    item.ngay_cap_nhat = DateTime.Now;
        //    item.status_del = model.db.status_del;
        //    item.so_ngay_du_kien = model.db.so_ngay_du_kien;
        //    item.id_file_upload = model.db.id_file_upload;
        //    item.phuong_thuc_thanh_toan = model.db.phuong_thuc_thanh_toan;
        //    item.vi_dien_tu = model.db.vi_dien_tu;
        //    item.id_tai_khoan_ngan_hang = model.db.id_tai_khoan_ngan_hang;
        //    item.ma_ngan_hang = model.db.ma_ngan_hang;
        //    item.so_tai_khoan = model.db.so_tai_khoan;
        //    item.tong_tien_truoc_thue = model.db.tong_tien_truoc_thue;
        //    item.tong_tien_chiet_khau = model.db.tong_tien_chiet_khau;
        //    item.tong_tien_sau_chiet_khau = model.db.tong_tien_sau_chiet_khau;
        //    item.tong_tien_thue = model.db.tong_tien_thue;
        //    item.tong_tien_sau_thue = model.db.tong_tien_sau_thue;
        //    item.id_hoa_don = model.db.id_hoa_don;
        //    item.is_sinh_tu_dong = model.db.is_sinh_tu_dong;
        //    item.list_id_phieu_nhap = model.db.list_id_phieu_nhap;
        //    await _context.sys_don_hang_mua_logs.InsertOneAsync(item);
        //    if (model.db.is_chi_du == true)
        //    {
        //        var id_dh = item.id;
        //        var update = Builders<sys_don_hang_mua_log_db>.Update
        //            .Set(x => x.so_tien_da_chi, model.db.tong_tien_sau_thue)
        //            .Set(x => x.is_chi_du, true)
        //            ;
        //        var filter = Builders<sys_don_hang_mua_log_db>.Filter.Eq(x => x.id, id_dh);
        //        await _context.sys_don_hang_mua_logs.UpdateOneAsync(filter, update);
        //    }
        //    else
        //    {
        //        var id_dh = item.id;
        //        var update = Builders<sys_don_hang_mua_log_db>.Update
        //            .Set(x => x.is_chi_du, false)
        //            ;
        //        var filter = Builders<sys_don_hang_mua_log_db>.Filter.Eq(x => x.id, id_dh);
        //        await _context.sys_don_hang_mua_logs.UpdateOneAsync(filter, update);
        //    }

        //    if (model.db.is_nhap_du == true)
        //    {
        //        var id_dh = item.id;
        //        var update = Builders<sys_don_hang_mua_log_db>.Update
        //            .Set(x => x.is_nhap_du, true)
        //            ;
        //        var filter = Builders<sys_don_hang_mua_log_db>.Filter.Eq(x => x.id, id_dh);
        //        await _context.sys_don_hang_mua_logs.UpdateOneAsync(filter, update);
        //    }
        //    else
        //    {
        //        var id_dh = item.id;
        //        var update = Builders<sys_don_hang_mua_log_db>.Update
        //            .Set(x => x.is_nhap_du, false)
        //            ;
        //        var filter = Builders<sys_don_hang_mua_log_db>.Filter.Eq(x => x.id, id_dh);
        //        await _context.sys_don_hang_mua_logs.UpdateOneAsync(filter, update);
        //    }

        //    await upset_detail_log(model, item.id);
        //    return 1;
        //}
        //public async Task<int> upset_detail_log(sys_don_hang_mua_model model, string id_log)
        //{
        //    var filter = Builders<sys_don_hang_mua_mat_hang_log_db>.Filter.Eq(x => x.id_don_hang, model.db.id);
        //    await _context.sys_don_hang_mua_mat_hang_logs.DeleteManyAsync(filter);
        //    for (int i = 0; i < model.list_mat_hang.Count(); i++)
        //    {
        //        var data = model.list_mat_hang[i];
        //        var mat_hang = _context.erp_mat_hangs.AsQueryable().Where(d => d.id == data.db.id_mat_hang).SingleOrDefault();
        //        var db = new sys_don_hang_mua_mat_hang_log_db();
        //        var sotutang = 100 + i;
        //        db.id = Guid.NewGuid().ToString();
        //        db.ten_mat_hang = mat_hang.ten;
        //        db.id_loai_mat_hang = mat_hang.id_loai_mat_hang;
        //        db.ma_vach = mat_hang.ma_vach;
        //        db.status_del = 1;
        //        db.id_don_hang = id_log;
        //        db.ngay_mua_hang = model.db.ngay_dat_hang;
        //        db.id_mat_hang = data.db.id_mat_hang;
        //        db.id_don_vi_tinh = data.db.id_don_vi_tinh;
        //        db.so_luong = data.db.so_luong;
        //        db.don_gia = data.db.don_gia;
        //        db.thanh_tien_truoc_thue = data.db.thanh_tien_truoc_thue;
        //        db.thanh_tien_sau_thue = data.db.thanh_tien_sau_thue;
        //        db.vat = data.db.vat;
        //        db.tien_vat = data.db.tien_vat;
        //        db.ghi_chu = data.db.ghi_chu;
        //        db.nguoi_cap_nhat = model.db.nguoi_cap_nhat;
        //        db.ngay_cap_nhat = DateTime.Now;
        //        db.chiet_khau = data.db.chiet_khau;
        //        db.thanh_tien_chiet_khau = data.db.thanh_tien_chiet_khau;
        //        db.status_del = 1;
        //        await _context.sys_don_hang_mua_mat_hang_logs.InsertOneAsync(db);
        //    }

        //    return 1;
        //}
        //public async Task<int> upset_detail_import(sys_don_hang_mua_model model)
        //{
        //    for (int i = 0; i < model.list_mat_hang.Count(); i++)
        //    {
        //        var data = model.list_mat_hang[i];

        //        var mat_hang = _context.erp_mat_hangs.AsQueryable().Where(d => d.id == data.id_mat_hang).SingleOrDefault();
        //        var db = new sys_don_hang_mua_mat_hang_db();
        //        var vat = Constant.list_vat.Where(q => q.id.ToLower().Trim() == data.vat.Trim().ToLower()).Select(q => q.value).SingleOrDefault();
        //        var sotutang = 100 + i;
        //        db.id = model.db.ma + sotutang.ToString();
        //        db.ten_mat_hang = mat_hang.ten;
        //        db.id_loai_mat_hang = mat_hang.id_loai_mat_hang;
        //        db.ma_vach = mat_hang.ma_vach;
        //        db.status_del = 1;
        //        db.id_don_hang = model.db.id;
        //        db.ngay_mua_hang = model.db.ngay_dat_hang;
        //        db.id_mat_hang = data.id_mat_hang;
        //        db.id_don_vi_tinh = data.id_don_vi_tinh;
        //        db.so_luong = data.so_luong;
        //        db.don_gia = data.don_gia;
        //        db.thanh_tien_truoc_thue = data.thanh_tien_truoc_thue;
        //        db.thanh_tien_sau_thue = data.thanh_tien_sau_thue;
        //        db.vat = data.vat;
        //        db.tien_vat = data.tien_vat;
        //        db.ghi_chu = data.ghi_chu_chi_tiet;
        //        db.nguoi_cap_nhat = model.db.nguoi_cap_nhat;
        //        db.ngay_cap_nhat = DateTime.Now;
        //        db.chiet_khau = data.chiet_khau;
        //        db.thanh_tien_chiet_khau = data.thanh_tien_chiet_khau;
        //        db.status_del = 1;
        //        await _context.sys_don_hang_mua_mat_hangs.InsertOneAsync(db);
        //    }

        //    return 1;
        //}
        //public async Task<int> upset_detail(sys_don_hang_mua_model model)
        //{
        //    var filter = Builders<sys_don_hang_mua_mat_hang_db>.Filter.Eq(x => x.id_don_hang, model.db.id);

        //    await _context.sys_don_hang_mua_mat_hangs.DeleteManyAsync(filter);


        //    for (int i = 0; i < model.list_mat_hang.Count(); i++)
        //    {
        //        var data = model.list_mat_hang[i];

        //        var mat_hang = _context.erp_mat_hangs.AsQueryable().Where(d => d.id == data.db.id_mat_hang).SingleOrDefault();
        //        var db = new sys_don_hang_mua_mat_hang_db();
        //        var sotutang = 100 + i;
        //        db.id = model.db.ma + sotutang.ToString();
        //        db.ten_mat_hang = mat_hang.ten;
        //        db.id_loai_mat_hang = mat_hang.id_loai_mat_hang;
        //        db.ma_vach = mat_hang.ma_vach;
        //        db.status_del = 1;
        //        db.id_don_hang = model.db.id;
        //        db.ngay_mua_hang = model.db.ngay_dat_hang;
        //        db.id_mat_hang = data.db.id_mat_hang;
        //        db.id_don_vi_tinh = data.db.id_don_vi_tinh;
        //        db.so_luong = data.db.so_luong;
        //        db.don_gia = data.db.don_gia;
        //        db.thanh_tien_truoc_thue = data.db.thanh_tien_truoc_thue;
        //        db.thanh_tien_sau_thue = data.db.thanh_tien_sau_thue;
        //        db.vat = data.db.vat;
        //        db.tien_vat = data.db.tien_vat;
        //        db.ghi_chu = data.db.ghi_chu;
        //        db.nguoi_cap_nhat = model.db.nguoi_cap_nhat;
        //        db.ngay_cap_nhat = DateTime.Now;
        //        db.chiet_khau = data.db.chiet_khau;
        //        db.thanh_tien_chiet_khau = data.db.thanh_tien_chiet_khau;
        //        db.status_del = 1;
        //        await _context.sys_don_hang_mua_mat_hangs.InsertOneAsync(db);
        //    }

        //    return 1;
        //}
        //public async Task<int> update_doituong(sys_don_hang_mua_model model)
        //{
        //    var update = Builders<erp_khach_hang_nha_cung_cap_db>.Update
        //        .Set(x => x.id_ngan_hang, model.db.id_ngan_hang_doi_tuong)
        //        .Set(x => x.so_tai_khoan, model.db.so_tai_khoan_doi_tuong);
        //    var filter = Builders<erp_khach_hang_nha_cung_cap_db>.Filter.Eq(x => x.id, model.db.id_doi_tuong);
        //    await _context.erp_khach_hang_nha_cung_caps.UpdateOneAsync(filter, update);
        //    return 1;
        //}
        
        public async Task<int> update(sys_don_hang_mua_model model)
        {
            //tinhTongTien(model);
            model.db.ten = generate_ten(model);
            model.db.ten_khong_dau = Regex.Replace(StringFunctions.NonUnicode(HttpUtility.HtmlDecode(model.db.ten ?? "")).ToLower().Normalize(), "<.*?>|&.*?;", String.Empty);
            var update = Builders<sys_don_hang_mua_col>.Update
                .Set(x => x.ma, model.db.ma)
                .Set(x => x.ten, model.db.ten)
                .Set(x => x.ten_khong_dau, model.db.ten_khong_dau)
                .Set(x => x.id_khach_hang_nha_cung_cap, model.db.id_khach_hang_nha_cung_cap)
                .Set(x => x.phuong_thuc_thanh_toan, model.db.phuong_thuc_thanh_toan)
                .Set(x => x.id_tai_khoan_ngan_hang, model.db.id_tai_khoan_ngan_hang)
                .Set(x => x.ma_ngan_hang, model.db.ma_ngan_hang)
                .Set(x => x.so_tai_khoan, model.db.so_tai_khoan)
                .Set(x => x.ngay_dat_hang, model.db.ngay_dat_hang)
                .Set(x => x.list_mat_hang, model.db.list_mat_hang)
                .Set(x => x.tong_thanh_tien, model.db.tong_thanh_tien)
                .Set(x => x.ghi_chu, model.db.ghi_chu)
                .Set(x => x.ngay_cap_nhat, model.db.ngay_cap_nhat)
                .Set(x => x.nguoi_cap_nhat, model.db.nguoi_cap_nhat)
                ;

            // Create a filter to match the document to update
            var filter = Builders<sys_don_hang_mua_col>.Filter.Eq(x => x.id, model.db.id);
            // Create an update definition to set the "Name" property to a new value
            await _context.sys_don_hang_mua_col.UpdateOneAsync(filter, update);

            //if (model.list_mat_hang.Count() > 0)
            //{
            //    await upset_detail(model);
            //}
            //await _common_trigger_don_hang_mua_repo.removeTriggerAuto(model.db.id, model.db.nguoi_cap_nhat);
            //if (model.db.is_chi_du == true)
            //{
            //    await _common_trigger_don_hang_mua_repo.InsertTriggerPhieuChi(model.db, model.list_mat_hang.Select(d => d.db).ToList());
            //    var id_dh = model.db.id;
            //    var update1 = Builders<sys_don_hang_mua_db>.Update
            //        .Set(x => x.so_tien_da_chi, model.db.tong_tien_sau_thue)
            //        .Set(x => x.is_chi_du, true)
            //        ;
            //    var filter1 = Builders<sys_don_hang_mua_db>.Filter.Eq(x => x.id, id_dh);
            //    await _context.sys_don_hang_mua_col.UpdateOneAsync(filter1, update1);
            //}
            //else
            //{
            //    var id_dh = model.db.id;
            //    var update1 = Builders<sys_don_hang_mua_db>.Update
            //        .Set(x => x.is_chi_du, false)
            //        ;
            //    var filter1 = Builders<sys_don_hang_mua_db>.Filter.Eq(x => x.id, id_dh);
            //    await _context.sys_don_hang_mua_col.UpdateOneAsync(filter1, update1);
            //}

            //if (model.db.is_nhap_du == true)
            //{
            //    await _common_trigger_don_hang_mua_repo.InsertTriggerPhieuNhap(model.db, model.list_mat_hang.Select(d => d.db).ToList());
            //    var id_dh = model.db.id;
            //    var update1 = Builders<sys_don_hang_mua_db>.Update
            //        .Set(x => x.is_nhap_du, true)
            //        ;
            //    var filter1 = Builders<sys_don_hang_mua_db>.Filter.Eq(x => x.id, id_dh);
            //    await _context.sys_don_hang_mua_col.UpdateOneAsync(filter1, update1);
            //}
            //else
            //{
            //    var id_dh = model.db.id;
            //    var update1 = Builders<sys_don_hang_mua_db>.Update
            //        .Set(x => x.is_nhap_du, false)
            //        ;
            //    var filter1 = Builders<sys_don_hang_mua_db>.Filter.Eq(x => x.id, id_dh);
            //    await _context.sys_don_hang_mua_col.UpdateOneAsync(filter1, update1);
            //}

            //await insert_log(model);
            //await upset_doituong(model);
            return 1;
        }
        //public IQueryable<sys_don_hang_mua_mat_hang_model> FindAllDetail(IQueryable<sys_don_hang_mua_mat_hang_db> query)
        //{

        //    var result = (from d in query.OrderByDescending(d => d.id_don_hang)
        //                  join mh in _context.erp_mat_hangs.AsQueryable()
        //               on d.id_mat_hang equals mh.id into mhG

        //                  join dvt in _context.erp_don_vi_tinhs.AsQueryable()
        //                  on d.id_don_vi_tinh equals dvt.id into dvtG

        //                  from mh in mhG.DefaultIfEmpty()

        //                  from dvt in dvtG.DefaultIfEmpty()

        //                  select new sys_don_hang_mua_mat_hang_model
        //                  {
        //                      db = d,
        //                      ten_don_vi_tinh = dvt.ten,
        //                      ten_mat_hang = mh.ten,
        //                      ma_mat_hang = mh.ma,
        //                      id_mat_hang = mh.id,
        //                      thuoc_tinh = mh.thuoc_tinh,
        //                      he_so_quy_doi = mh.he_so_quy_doi,
        //                  });
        //    return result;

        //}
        //public IQueryable<sys_don_hang_mua_mat_hang_log_model> FindAllDetailLog(IQueryable<sys_don_hang_mua_mat_hang_log_db> query)
        //{
        //    var result = (from d in query.OrderByDescending(d => d.id_don_hang)
        //                  join mh in _context.erp_mat_hangs.AsQueryable()
        //               on d.id_mat_hang equals mh.id into mhG

        //                  join dvt in _context.erp_don_vi_tinhs.AsQueryable()
        //                  on d.id_don_vi_tinh equals dvt.id into dvtG

        //                  from mh in mhG.DefaultIfEmpty()

        //                  from dvt in dvtG.DefaultIfEmpty()

        //                  select new sys_don_hang_mua_mat_hang_log_model
        //                  {
        //                      db = d,
        //                      ten_don_vi_tinh = dvt.ten,
        //                      ten_mat_hang = mh.ten,
        //                      ma_mat_hang = mh.ma,
        //                      id_mat_hang = mh.id,
        //                      thuoc_tinh = mh.thuoc_tinh,
        //                      he_so_quy_doi = mh.he_so_quy_doi,
        //                  });
        //    return result;

        //}
        //public IQueryable<sys_don_hang_mua_log_model> FindAllLog(IQueryable<sys_don_hang_mua_log_db> query)
        //{
        //    var result = (from d in query.OrderByDescending(d => d.ngay_dat_hang)

        //                  join u in _context.Users.AsQueryable()
        //                on d.nguoi_cap_nhat equals u.Id into uG

        //                  join kho in _context.erp_khos.AsQueryable()
        //                  on d.id_kho_nhap equals kho.id into khoG

        //                  from u in uG.DefaultIfEmpty()

        //                  from kho in khoG.DefaultIfEmpty()

        //                  select new sys_don_hang_mua_log_model
        //                  {
        //                      db = d,
        //                      ten_nguoi_cap_nhat = u.full_name,
        //                      ten_kho = kho.ten,
        //                      ten_ngan_hang = d.ma_ngan_hang + " - " + d.so_tai_khoan,
        //                      loai_giao_dich_str = d.loai_giao_dich == 1 ? "Hàng hóa" : "Dịch vụ",
        //                      hinh_thuc_doi_tuong_str = d.hinh_thuc_doi_tuong == 1 ? "Cá nhân" :
        //                                                d.hinh_thuc_doi_tuong == 2 ? "Tổ chức" :
        //                                                d.hinh_thuc_doi_tuong == 3 ? "Phòng ban" : "Nhân viên",

        //                  });
        //    return result;
        //}
        
        public IQueryable<sys_don_hang_mua_model> FindAll(IQueryable<sys_don_hang_mua_col> query)
        {
            var result = (from d in query.OrderByDescending(d => d.ngay_dat_hang)

                          join u in _context.sys_user_col.AsQueryable()
                        on d.nguoi_cap_nhat equals u.id into uG

                          from u in uG.DefaultIfEmpty()

                          select new sys_don_hang_mua_model
                          {
                              db = d,
                              ten_nguoi_cap_nhat = u.ho_va_ten,
                          });
            return result;
        }
        public async Task<int> update_status_del(string id, string userid, int status_del)
        {
            var update = Builders<sys_don_hang_mua_col>.Update
               .Set(x => x.status_del, status_del)
                .Set(x => x.nguoi_cap_nhat, userid)
                 .Set(x => x.ngay_cap_nhat, DateTime.Now);

            // Create a filter to match the document to update
            var filter = Builders<sys_don_hang_mua_col>.Filter.Eq(x => x.id, id);
            _context.sys_don_hang_mua_col.UpdateOne(filter, update);

            return 1;
        }
    }
}
