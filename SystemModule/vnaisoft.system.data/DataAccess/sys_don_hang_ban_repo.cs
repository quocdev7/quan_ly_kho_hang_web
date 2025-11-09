using MongoDB.Driver;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using vnaisoft.DataBase.commonFunc;
using vnaisoft.DataBase.Helper;
using vnaisoft.DataBase.Mongodb;
using vnaisoft.DataBase.Mongodb.Collection.system;
using vnaisoft.system.data.Models;

namespace vnaisoft.system.data.DataAccess
{
    public class sys_don_hang_ban_repo
    {
        public MongoDBContext _context;
        public common_mongo_repo _common_repo;
        //public common_trigger_don_hang_ban_repo _common_trigger_don_hang_ban_repo;

        public sys_don_hang_ban_repo(MongoDBContext context)
        {
            _context = context;
            _common_repo = new common_mongo_repo(context);
            //_common_trigger_don_hang_ban_repo = new common_trigger_don_hang_ban_repo(context);
        }

        public string getCode()
        {
            var max = "";
            var config = _common_repo.get_code_config(false, "sys_don_hang_ban", "DHB");
            var max_query = _context.sys_don_hang_ban_col.AsQueryable()
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
        //public async Task<sys_don_hang_ban_log_model> getElementByIdLog(string id)
        //{
        //    var query = _context.sys_don_hang_ban_logs.AsQueryable().Where(m => m.id == id);
        //    var obj = FindAllLog(query).SingleOrDefault();

        //    var queryTableDetail = _context.sys_don_hang_ban_mat_hang_logs.AsQueryable().Where(q => q.id_don_hang == id.Trim());
        //    obj.list_mat_hang = FindAllLogDetail(queryTableDetail).ToList();
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
        //    });
        //    obj.id_mat_hangs = string.Join(',', obj.list_mat_hang.Select(q => q.db.id_mat_hang).ToList());
        //    obj.don_gia_gom_thue = obj.list_mat_hang.Select(q => q.db.don_gia_gom_thue).Distinct().First();
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
        //public async Task<sys_don_hang_ban_model> getElementById(string id)
        //{
        //    var query = _context.sys_don_hang_bans.AsQueryable().Where(m => m.id == id);
        //    var obj = FindAll(query).SingleOrDefault();

        //    var queryTableDetail = _context.sys_don_hang_ban_mat_hangs.AsQueryable().Where(q => q.id_don_hang == id.Trim());
        //    obj.list_mat_hang = FindAllDetail(queryTableDetail).ToList();
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

        //        t.thanh_tien = Math.Round((decimal)(t.db.thanh_tien_chiet_khau ?? 0));
        //        var don_gia = (t.thanh_tien / (t.db.so_luong ?? 1)) ?? 0;
        //        t.don_gia = don_gia;

        //        var vat = Constant.list_vat.Where(q => q.id.Trim().ToLower() == t.db.vat).Select(q => q.value).SingleOrDefault();
        //        var ty_suat_vat = t.thanh_tien * vat / 100;
        //        t.ty_suat_vat = Math.Round((decimal)(ty_suat_vat));
        //        t.vat = t.db.vat;
        //    }
        //    ;
        //    obj.id_mat_hangs = string.Join(',', obj.list_mat_hang.Select(q => q.db.id_mat_hang).ToList());
        //    obj.don_gia_gom_thue = obj.list_mat_hang.Select(q => q.db.don_gia_gom_thue).Distinct().First();
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

        public string generate_ten(sys_don_hang_ban_model model)
        {
            var ten = "";
            var pttt = "";
            var ngay = model.db.ngay_dat_hang.Value.ToString("dd/MM/yyyy");

            if (model.db.phuong_thuc_thanh_toan == 1)
            {
                pttt = "Tiền mặt";
            }
            else if (model.db.phuong_thuc_thanh_toan == 2)
            {

                //var ngan_hang = _context.erp_tai_khoan_ngan_hangs.AsQueryable().Where(q => q.id == model.db.id_tai_khoan_ngan_hang).SingleOrDefault();
                pttt = model.db.ma_ngan_hang + "-" + model.db.so_tai_khoan;
            }
            var tong_tien = model.db.tong_thanh_tien;
            //var ten_kh = model.db.ten_doi_tuong + "(" + model.db.id_doi_tuong + ")";

            ten = "Đơn hàng bán " + " " + model.ten_doi_tuong + " " + ngay + ", " + pttt + ", " + String.Format("{0:#,##0}", tong_tien) + "đ";
            return ten;
        }

        //public string get_code_bao_gia()
        //{
        //    var prefix = "DHBG";
        //    var max = "";
        //    var config = _common_repo.get_code_config(false, "sys_don_hang_ban", prefix);

        //    var max_query = _context.erp_don_hang_bao_gias.AsQueryable()
        //     .Where(d => d.ma.StartsWith(config.prefix))
        //     .Where(d => d.ma.Length == config.prefix.Length + config.numIncrease)
        //     .Select(d => d.ma);
        //    if (max_query.Count() > 0)
        //    {
        //        max = max_query.Max();
        //    }
        //    var code = _common_repo.generateCode(config.prefix, config.numIncrease, max);
        //    return code;
        //}
        public async Task<int> insert(sys_don_hang_ban_model model)
        {
            //model.db.ma_bao_gia = model.db.tinh_trang_don_hang == "1" ? get_code_bao_gia() : null;
            //model.db.ngay_giao_hang = model.db.ngay_du_kien_giao_hang != null ? model.db.ngay_giao_hang : model.db.ngay_cap_nhat;
            //model.db.hinh_thuc_van_chuyen = model.db.loai_giao_dich == 2 ? 1 : null;
            //if (model.check_doi_tuong == 1)
            //{
            //    model.db.id_doi_tuong = "DTTD";
            //    model.db.hinh_thuc_doi_tuong = 1;
            //    model.db.ma_so_thue = "";
            //    model.db.dien_thoai = "";
            //    model.db.email = "";
            //    model.db.dia_chi_doi_tuong = "";
            //    model.db.ten_doi_tuong = "Đối tượng tự do";
            //}
            //tinhTongTien(model);
            model.db.ten = generate_ten(model);
            model.db.ten_khong_dau = Regex.Replace(StringFunctions.NonUnicode(HttpUtility.HtmlDecode(model.db.ten ?? "")).ToLower().Normalize(), "<.*?>|&.*?;", String.Empty);
            //model.db.id_file_upload = model.db.id + "sys_don_hang_ban";
            await _context.sys_don_hang_ban_col.InsertOneAsync(model.db);
            //await _common_repo.insert_file(model.db.id, "sys_don_hang_ban");
            //if (model.db.tinh_trang_don_hang == "1")
            //{
            //    await insert_bao_gia(model);
            //}
            //if (model.db.is_thu_du == true)
            //{
            //    await _common_trigger_don_hang_ban_repo.InsertTriggerPhieuThu(model.db, model.list_mat_hang.Select(d => d.db).ToList());
            //    var id_dh = model.db.id;
            //    var update = Builders<sys_don_hang_ban_db>.Update
            //        .Set(x => x.so_tien_da_thu, model.db.tong_tien_sau_thue)
            //        .Set(x => x.is_thu_du, true)
            //        ;
            //    var filter = Builders<sys_don_hang_ban_db>.Filter.Eq(x => x.id, id_dh);
            //    await _context.sys_don_hang_bans.UpdateOneAsync(filter, update);
            //}
            //if (model.db.is_xuat_du == true || model.db.is_hang_tang == true)
            //{
            //    await _common_trigger_don_hang_ban_repo.InsertTriggerPhieuXuat(model.db, model.list_mat_hang.Select(d => d.db).ToList());
            //}
            //await upset_detail(model);
            //await upset_doituong(model);
            return 1;
        }



        //public async Task<int> insert_import(sys_don_hang_ban_model model)
        //{
        //    model.db.ma_bao_gia = model.db.tinh_trang_don_hang == "1" ? get_code_bao_gia() : null;
        //    model.db.ngay_giao_hang = model.db.ngay_du_kien_giao_hang != null ? model.db.ngay_giao_hang : model.db.ngay_cap_nhat;
        //    model.db.hinh_thuc_van_chuyen = model.db.loai_giao_dich == 2 ? 1 : null;

        //    tinhTongTienImport(model);
        //    model.db.ten = generate_ten(model);
        //    model.db.ten_khong_dau = Regex.Replace(StringFunctions.NonUnicode(HttpUtility.HtmlDecode(model.db.ten ?? "")).ToLower().Normalize(), "<.*?>|&.*?;", String.Empty);
        //    model.db.id_file_upload = model.db.id + "sys_don_hang_ban";
        //    try
        //    {
        //        await _context.sys_don_hang_bans.InsertOneAsync(model.db);
        //    }
        //    catch (Exception e) { }

        //    await _common_repo.insert_file(model.db.id, "sys_don_hang_ban");
        //    if (model.db.tinh_trang_don_hang == "1")
        //    {
        //        await insert_bao_gia(model);
        //    }
        //    if (model.db.is_thu_du == true)
        //    {
        //        await _common_trigger_don_hang_ban_repo.InsertTriggerPhieuThu(model.db, model.list_mat_hang.Select(d => d.db).ToList());
        //        var id_dh = model.db.id;
        //        var update = Builders<sys_don_hang_ban_db>.Update
        //            .Set(x => x.so_tien_da_thu, model.db.tong_tien_sau_thue)
        //            .Set(x => x.is_thu_du, true)
        //            ;
        //        var filter = Builders<sys_don_hang_ban_db>.Filter.Eq(x => x.id, id_dh);
        //        await _context.sys_don_hang_bans.UpdateOneAsync(filter, update);
        //    }

        //    if (model.db.is_xuat_du == true || model.db.is_hang_tang == true)
        //    {
        //        await _common_trigger_don_hang_ban_repo.InsertTriggerPhieuXuat(model.db, model.list_mat_hang.Select(d => d.db).ToList());
        //    }
        //    await upset_detail_import(model);
        //    return 1;

        //}

        //public async Task<int> insert_bao_gia(sys_don_hang_ban_model model_don_hang)
        //{
        //    var model = new erp_don_hang_bao_gia_model();


        //    model.db.id = ObjectId.GenerateNewId().ToString();
        //    model.db.ma = model_don_hang.db.ma_bao_gia;
        //    model.db.id_don_hang_ban = model_don_hang.db.ma;
        //    model.db.kieu_ban = model_don_hang.db.kieu_ban;
        //    model.db.loai_giao_dich = model_don_hang.db.loai_giao_dich;
        //    model.db.thanh_tien_truoc_thue = model_don_hang.db.thanh_tien_truoc_thue;
        //    model.db.tien_thue = model_don_hang.db.tien_thue;
        //    model.db.ghi_chu = model_don_hang.db.ghi_chu;
        //    model.db.phuong_thuc_thanh_toan = model_don_hang.db.phuong_thuc_thanh_toan;
        //    model.db.vi_dien_tu = model_don_hang.db.vi_dien_tu;
        //    model.db.id_tai_khoan_ngan_hang = model_don_hang.db.id_tai_khoan_ngan_hang;
        //    model.db.ma_ngan_hang = model_don_hang.db.ma_ngan_hang;
        //    model.db.so_tai_khoan = model_don_hang.db.so_tai_khoan;
        //    model.db.hinh_thuc_van_chuyen = model_don_hang.db.hinh_thuc_van_chuyen;
        //    model.db.id_kho_xuat_ban_le = model_don_hang.db.id_kho_xuat_ban_le;
        //    model.db.tien_van_chuyen = model_don_hang.db.tien_van_chuyen;
        //    model.db.vat_van_chuyen = model_don_hang.db.vat_van_chuyen;
        //    model.db.tien_vat_van_chuyen = model_don_hang.db.tien_vat_van_chuyen;
        //    model.db.tien_khac = model_don_hang.db.tien_khac;
        //    model.db.vat_khac = model_don_hang.db.vat_khac;
        //    model.db.tien_vat_khac = model_don_hang.db.tien_vat_khac;
        //    model.db.ngay_dat_hang = model_don_hang.db.ngay_dat_hang;
        //    model.db.so_ngay_du_kien = model_don_hang.db.so_ngay_du_kien;
        //    model.db.ngay_du_kien_giao_hang = model_don_hang.db.ngay_du_kien_giao_hang;
        //    model.db.ngay_giao_hang = model_don_hang.db.ngay_giao_hang;
        //    model.db.dia_chi_giao_hang = model_don_hang.db.dia_chi_giao_hang;
        //    model.db.so_dien_thoai_nguoi_nhan = model_don_hang.db.so_dien_thoai_nguoi_nhan;
        //    model.db.hinh_thuc_doi_tuong = model_don_hang.db.hinh_thuc_doi_tuong;
        //    model.db.id_doi_tuong = model_don_hang.db.id_doi_tuong;
        //    model.db.ten_doi_tuong = model_don_hang.db.ten_doi_tuong;
        //    model.db.ma_so_thue = model_don_hang.db.ma_so_thue;
        //    model.db.dien_thoai = model_don_hang.db.dien_thoai;
        //    model.db.email = model_don_hang.db.email;
        //    model.db.dia_chi_doi_tuong = model_don_hang.db.dia_chi_doi_tuong;
        //    model.db.nguoi_cap_nhat = model_don_hang.db.nguoi_cap_nhat;
        //    model.db.ngay_cap_nhat = model_don_hang.db.ngay_cap_nhat;
        //    model.db.status_del = model_don_hang.db.status_del;
        //    model.db.thanh_tien_sau_thue = model_don_hang.db.thanh_tien_sau_thue;
        //    model.db.id_file_upload = model_don_hang.db.id_file_upload;
        //    model.db.tinh_trang_don_hang = model_don_hang.db.tinh_trang_don_hang;
        //    model.db.tong_tien_truoc_thue = model_don_hang.db.tong_tien_truoc_thue;
        //    model.db.tong_tien_chiet_khau = model_don_hang.db.tong_tien_chiet_khau;
        //    model.db.tong_tien_sau_chiet_khau = model_don_hang.db.tong_tien_sau_chiet_khau;
        //    model.db.tong_tien_thue = model_don_hang.db.tong_tien_thue;
        //    model.db.tong_tien_sau_thue = model_don_hang.db.tong_tien_sau_thue;
        //    model.db.id_hoa_don = model_don_hang.db.id_hoa_don;



        //    await _context.erp_don_hang_bao_gias.InsertOneAsync(model.db);

        //    var filter = Builders<erp_don_hang_bao_gia_mat_hang_db>.Filter.Eq(x => x.id_don_hang, model.db.id);

        //    await _context.erp_don_hang_bao_gia_mat_hangs.DeleteManyAsync(filter);


        //    decimal? tong_tien_truoc_thue = 0;
        //    decimal? tong_tien_chiet_khau = 0;
        //    decimal? tong_tien_sau_chiet_khau = 0;
        //    decimal? tong_tien_thue = 0;
        //    decimal? tong_tien_sau_thue = 0;
        //    for (int i = 0; i < model_don_hang.list_mat_hang.Count(); i++)
        //    {
        //        var data = model_don_hang.list_mat_hang[i];


        //        var db = new erp_don_hang_bao_gia_mat_hang_db();
        //        var sotutang = 100 + i;
        //        db.id = model.db.ma + sotutang.ToString();
        //        db.id_don_hang = model.db.id;
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
        //        await _context.erp_don_hang_bao_gia_mat_hangs.InsertOneAsync(db);

        //        tong_tien_truoc_thue += data.db.thanh_tien_truoc_thue ?? 0;
        //        tong_tien_chiet_khau += (data.db.thanh_tien_truoc_thue ?? 0) - (data.db.thanh_tien_chiet_khau ?? 0);
        //        tong_tien_thue += data.db.tien_vat ?? 0;
        //    }
        //    tong_tien_thue = tong_tien_thue + (model.db.tien_vat_van_chuyen ?? 0);

        //    tong_tien_truoc_thue = tong_tien_truoc_thue + (model.db.tien_van_chuyen ?? 0);
        //    tong_tien_sau_chiet_khau = tong_tien_truoc_thue - tong_tien_chiet_khau;

        //    tong_tien_sau_thue = tong_tien_sau_chiet_khau + tong_tien_thue;


        //    var update_dh = Builders<erp_don_hang_bao_gia_db>.Update
        //    .Set(x => x.tong_tien_truoc_thue, tong_tien_truoc_thue)
        //    .Set(x => x.tong_tien_chiet_khau, tong_tien_chiet_khau)
        //    .Set(x => x.tong_tien_thue, tong_tien_thue)
        //    .Set(x => x.tong_tien_sau_chiet_khau, tong_tien_sau_chiet_khau)
        //    .Set(x => x.tong_tien_sau_thue, tong_tien_sau_thue);

        //    var filter_dh = Builders<erp_don_hang_bao_gia_db>.Filter.Eq(x => x.id, model.db.id);

        //    // Create an update definition to set the "Name" property to a new value

        //    await _context.erp_don_hang_bao_gias.UpdateOneAsync(filter_dh, update_dh);
        //    return 1;
        //}
        //public void tinhTongTien(sys_don_hang_ban_model model)
        //{
        //    model.db.tong_tien_truoc_thue = model.list_mat_hang.Sum(d => d.db.thanh_tien_truoc_thue ?? 0);
        //    model.db.tong_tien_chiet_khau = model.list_mat_hang.Sum(data => (data.db.thanh_tien_truoc_thue ?? 0) - (data.db.thanh_tien_chiet_khau ?? 0)); ;
        //    model.db.tong_tien_thue = model.list_mat_hang.Sum(data => (data.db.tien_vat ?? 0)) + (model.db.tien_vat_van_chuyen ?? 0);
        //    model.db.tong_tien_truoc_thue = model.db.tong_tien_truoc_thue + (model.db.tien_van_chuyen ?? 0);
        //    model.db.tong_tien_sau_chiet_khau = model.db.tong_tien_truoc_thue - model.db.tong_tien_chiet_khau;
        //    model.db.tong_tien_sau_thue = model.db.tong_tien_sau_chiet_khau + model.db.tong_tien_thue;

        //}
        //public void tinhTongTienImport(sys_don_hang_ban_model model)
        //{
        //    model.db.tong_tien_truoc_thue = model.list_mat_hang.Sum(d => d.thanh_tien_truoc_thue ?? 0);
        //    model.db.tong_tien_chiet_khau = model.list_mat_hang.Sum(data => (data.thanh_tien_truoc_thue ?? 0) - (data.thanh_tien_chiet_khau ?? 0)); ;
        //    model.db.tong_tien_thue = model.list_mat_hang.Sum(data => (data.tien_vat ?? 0)) + (model.db.tien_vat_van_chuyen ?? 0);
        //    model.db.tong_tien_truoc_thue = model.db.tong_tien_truoc_thue + (model.db.tien_van_chuyen ?? 0);
        //    model.db.tong_tien_sau_chiet_khau = model.db.tong_tien_truoc_thue - model.db.tong_tien_chiet_khau;
        //    model.db.tong_tien_sau_thue = model.db.tong_tien_sau_chiet_khau + model.db.tong_tien_thue;

        //}
        //public async Task<int> upset_doituong(sys_don_hang_ban_model model)
        //{

        //    if (model.check_doi_tuong != 1)
        //    {
        //        var filter = Builders<erp_khach_hang_nha_cung_cap_db>.Filter.Eq(x => x.id, model.db.id_doi_tuong);

        //        var update = Builders<erp_khach_hang_nha_cung_cap_db>.Update
        //        .Set(x => x.laKhachHang, true);

        //        // Create an update definition to set the "Name" property to a new value
        //        await _context.erp_khach_hang_nha_cung_caps.UpdateOneAsync(filter, update);
        //    }




        //    return 1;
        //}

        //public async Task<int> upset_detail(sys_don_hang_ban_model model)
        //{


        //    var filter = Builders<sys_don_hang_ban_mat_hang_db>.Filter.Eq(x => x.id_don_hang, model.db.id);

        //    await _context.sys_don_hang_ban_mat_hangs.DeleteManyAsync(filter);
        //    for (int i = 0; i < model.list_mat_hang.Count(); i++)
        //    {
        //        var mat_hang = _context.erp_mat_hangs.AsQueryable().Where(d => d.id == model.list_mat_hang[i].db.id_mat_hang).SingleOrDefault();
        //        var sotutang = 100 + i;
        //        model.list_mat_hang[i].db.id = model.db.ma + sotutang.ToString();
        //        model.list_mat_hang[i].db.ngay_dat_hang = model.db.ngay_dat_hang;
        //        model.list_mat_hang[i].db.id_don_hang = model.db.id;
        //        model.list_mat_hang[i].db.nguoi_cap_nhat = model.db.nguoi_cap_nhat;
        //        model.list_mat_hang[i].db.ngay_cap_nhat = DateTime.Now;
        //        model.list_mat_hang[i].db.status_del = 1;
        //        model.list_mat_hang[i].db.tinh_trang_don_hang = model.db.tinh_trang_don_hang;
        //        model.list_mat_hang[i].db.ten_mat_hang = mat_hang.ten;
        //        model.list_mat_hang[i].db.id_loai_mat_hang = mat_hang.id_loai_mat_hang;
        //        model.list_mat_hang[i].db.ma_vach = mat_hang.ma_vach;

        //        await _context.sys_don_hang_ban_mat_hangs.InsertOneAsync(model.list_mat_hang[i].db);
        //    }

        //    return 1;
        //}

        //public async Task<int> upset_detail_import(sys_don_hang_ban_model model)
        //{
        //    for (int i = 0; i < model.list_mat_hang.Count(); i++)
        //    {
        //        var mat_hang = model.list_mat_hang[i];
        //        var mat_hang_find = _context.erp_mat_hangs.AsQueryable().Where(q => q.ma == mat_hang.id_mat_hang).SingleOrDefault();
        //        var sotutang = 100 + i;
        //        model.list_mat_hang[i].db.id = model.db.ma + sotutang.ToString();
        //        model.list_mat_hang[i].db.ngay_dat_hang = model.db.ngay_dat_hang;
        //        model.list_mat_hang[i].db.id_don_hang = model.db.id;
        //        model.list_mat_hang[i].db.nguoi_cap_nhat = model.db.nguoi_cap_nhat;
        //        model.list_mat_hang[i].db.ngay_cap_nhat = DateTime.Now;
        //        model.list_mat_hang[i].db.status_del = 1;
        //        model.list_mat_hang[i].db.tinh_trang_don_hang = model.db.tinh_trang_don_hang;
        //        model.list_mat_hang[i].db.ten_mat_hang = mat_hang.ten_mat_hang;
        //        model.list_mat_hang[i].db.id_mat_hang = mat_hang.id_mat_hang;
        //        model.list_mat_hang[i].db.id_don_vi_tinh = mat_hang.id_don_vi_tinh;
        //        model.list_mat_hang[i].db.so_luong = mat_hang.so_luong;
        //        model.list_mat_hang[i].db.don_gia = mat_hang.don_gia;
        //        model.list_mat_hang[i].db.chiet_khau = mat_hang.chiet_khau;
        //        model.list_mat_hang[i].db.thanh_tien_chiet_khau = mat_hang.thanh_tien_chiet_khau;
        //        model.list_mat_hang[i].db.vat = mat_hang.vat;
        //        model.list_mat_hang[i].db.tien_vat = mat_hang.tien_vat;
        //        model.list_mat_hang[i].db.thanh_tien_sau_thue = mat_hang.thanh_tien_sau_thue;
        //        model.list_mat_hang[i].db.thanh_tien_truoc_thue = mat_hang.thanh_tien_truoc_thue;
        //        model.list_mat_hang[i].db.id_loai_mat_hang = mat_hang_find.id_loai_mat_hang;
        //        model.list_mat_hang[i].db.ma_vach = mat_hang_find.ma_vach;
        //        model.list_mat_hang[i].db.ghi_chu = mat_hang.ghi_chu_chi_tiet;
        //        await _context.sys_don_hang_ban_mat_hangs.InsertOneAsync(model.list_mat_hang[i].db);
        //    }
        //    return 1;
        //}

        public async Task<int> update(sys_don_hang_ban_model model)
        {
            //tinhTongTien(model);
            model.db.ten = generate_ten(model);
            model.db.ten_khong_dau = Regex.Replace(StringFunctions.NonUnicode(HttpUtility.HtmlDecode(model.db.ten ?? "")).ToLower().Normalize(), "<.*?>|&.*?;", String.Empty);
            var update = Builders<sys_don_hang_ban_col>.Update
                .Set(x => x.ma, model.db.ma)
                .Set(x => x.ten, model.db.ten)
                .Set(x => x.ten_khong_dau, model.db.ten_khong_dau)
                .Set(x => x.id_khach_hang_nha_cung_cap, model.db.id_khach_hang_nha_cung_cap)
                .Set(x => x.phuong_thuc_thanh_toan, model.db.phuong_thuc_thanh_toan)
                .Set(x => x.id_tai_khoan_ngan_hang, model.db.id_tai_khoan_ngan_hang)
                .Set(x => x.ma_ngan_hang, model.db.ma_ngan_hang)
                .Set(x => x.so_tai_khoan, model.db.so_tai_khoan)
                .Set(x => x.ngay_dat_hang, model.db.ngay_dat_hang)
                .Set(x => x.tong_thanh_tien, model.db.tong_thanh_tien)
                .Set(x => x.ghi_chu, model.db.ghi_chu)
                .Set(x => x.ngay_cap_nhat, model.db.ngay_cap_nhat)
                .Set(x => x.nguoi_cap_nhat, model.db.nguoi_cap_nhat)
            ;


            // Create a filter to match the document to update
            var filter = Builders<sys_don_hang_ban_col>.Filter.Eq(x => x.id, model.db.id);

            // Create an update definition to set the "Name" property to a new value

            await _context.sys_don_hang_ban_col.UpdateOneAsync(filter, update);
            //await _common_repo.upset_doi_tuong(new erp_khach_hang_nha_cung_cap_db()
            //{
            //    dia_chi = model.db.dia_chi_doi_tuong,
            //    email = model.db.email,
            //    dien_thoai = model.db.dien_thoai,
            //    hinh_thuc = model.db.hinh_thuc_doi_tuong,
            //    id = model.db.id_doi_tuong,
            //    ma = model.db.id_doi_tuong,
            //    ma_so_thue = model.db.ma_so_thue,
            //    ngay_cap_nhat = DateTime.Now,
            //    nguoi_cap_nhat = model.db.nguoi_cap_nhat,
            //    ten = model.db.ten_doi_tuong,

            //});

            //await upset_detail(model);
            //await _common_trigger_don_hang_ban_repo.removeTriggerAuto(model.db.id, model.db.nguoi_cap_nhat);
            //if (model.db.is_thu_du == true)
            //{
            //    await _common_trigger_don_hang_ban_repo.InsertTriggerPhieuThu(model.db, model.list_mat_hang.Select(d => d.db).ToList());
            //    var id_dh = model.db.id;
            //    var update1 = Builders<sys_don_hang_ban_db>.Update
            //        .Set(x => x.so_tien_da_thu, model.db.tong_tien_sau_thue)
            //        .Set(x => x.is_thu_du, true)
            //        ;
            //    var filter1 = Builders<sys_don_hang_ban_db>.Filter.Eq(x => x.id, id_dh);
            //    await _context.sys_don_hang_bans.UpdateOneAsync(filter1, update1);
            //}
            //if (model.db.is_xuat_du == true || model.db.is_hang_tang == true)
            //{
            //    await _common_trigger_don_hang_ban_repo.InsertTriggerPhieuXuat(model.db, model.list_mat_hang.Select(d => d.db).ToList());
            //}
            ////lưu lại lịch sử chỉnh sửa
            //var model_lich_su = new sys_don_hang_ban_log_model();
            //model_lich_su.db.id = ObjectId.GenerateNewId().ToString();
            //model_lich_su.db.is_sinh_tu_dong = model.db.is_sinh_tu_dong;
            //model_lich_su.db.ma = model.db.ma;
            //model_lich_su.db.ten = model.db.ten;
            //model_lich_su.db.ten_khong_dau = model.db.ten_khong_dau;
            //model_lich_su.db.kieu_ban = model.db.kieu_ban;
            //model_lich_su.db.loai_giao_dich = model.db.loai_giao_dich;
            //model_lich_su.db.thanh_tien_truoc_thue = model.db.thanh_tien_truoc_thue;
            //model_lich_su.db.ghi_chu = model.db.ghi_chu;
            //model_lich_su.db.tien_thue = model.db.tien_thue;
            //model_lich_su.db.phuong_thuc_thanh_toan = model.db.phuong_thuc_thanh_toan;
            //model_lich_su.db.hinh_thuc_van_chuyen = model.db.hinh_thuc_van_chuyen;
            //model_lich_su.db.vi_dien_tu = model.db.vi_dien_tu;
            //model_lich_su.db.id_tai_khoan_ngan_hang = model.db.id_tai_khoan_ngan_hang;
            //model_lich_su.db.tien_van_chuyen = model.db.tien_van_chuyen;
            //model_lich_su.db.vat_van_chuyen = model.db.vat_van_chuyen;
            //model_lich_su.db.tien_vat_van_chuyen = model.db.tien_vat_van_chuyen;
            //model_lich_su.db.tien_khac = model.db.tien_khac;
            //model_lich_su.db.vat_khac = model.db.vat_khac;
            //model_lich_su.db.tien_vat_khac = model.db.tien_vat_khac;
            //model_lich_su.db.ngay_dat_hang = model.db.ngay_dat_hang;
            //model_lich_su.db.so_ngay_du_kien = model.db.so_ngay_du_kien;
            //model_lich_su.db.ngay_du_kien_giao_hang = model.db.ngay_du_kien_giao_hang;
            //model_lich_su.db.ngay_giao_hang = model.db.ngay_giao_hang;
            //model_lich_su.db.dia_chi_giao_hang = model.db.dia_chi_giao_hang;
            //model_lich_su.db.so_dien_thoai_nguoi_nhan = model.db.so_dien_thoai_nguoi_nhan;
            //model_lich_su.db.hinh_thuc_doi_tuong = model.db.hinh_thuc_doi_tuong;
            //model_lich_su.db.id_doi_tuong = model.db.id_doi_tuong;
            //model_lich_su.db.ten_doi_tuong = model.db.ten_doi_tuong;
            //model_lich_su.db.ma_so_thue = model.db.ma_so_thue;
            //model_lich_su.db.dien_thoai = model.db.dien_thoai;
            //model_lich_su.db.email = model.db.email;
            //model_lich_su.db.dia_chi_doi_tuong = model.db.dia_chi_doi_tuong;
            //model_lich_su.db.status_del = model.db.status_del;
            //model_lich_su.db.thanh_tien_sau_thue = model.db.thanh_tien_sau_thue;
            //model_lich_su.db.nguoi_cap_nhat = model.db.nguoi_cap_nhat;
            //model_lich_su.db.ngay_cap_nhat = model.db.ngay_cap_nhat;
            //model_lich_su.db.tong_tien_truoc_thue = model.db.tong_tien_truoc_thue;
            //model_lich_su.db.tong_tien_chiet_khau = model.db.tong_tien_chiet_khau;
            //model_lich_su.db.tong_tien_thue = model.db.tong_tien_thue;
            //model_lich_su.db.tong_tien_sau_chiet_khau = model.db.tong_tien_sau_chiet_khau;
            //model_lich_su.db.tong_tien_sau_thue = model.db.tong_tien_sau_thue;
            //model_lich_su.db.tinh_trang_don_hang = model.db.tinh_trang_don_hang;
            //model_lich_su.db.so_tien_da_thu = model.db.so_tien_da_thu;
            //model_lich_su.db.is_xuat_du = model.db.is_xuat_du;
            //model_lich_su.db.is_thu_du = model.db.is_thu_du;
            //model_lich_su.db.is_hang_tang = model.db.is_hang_tang;
            //model_lich_su.db.ly_do_chinh_sua = model.ly_do_chinh_sua;
            //await _context.sys_don_hang_ban_logs.InsertOneAsync(model_lich_su.db);
            //try
            //{
            //    for (int i = 0; i < model.list_mat_hang.Count(); i++)
            //    {
            //        var model_lich_su_chi_tiet = new sys_don_hang_ban_mat_hang_log_db();
            //        var mat_hang = _context.erp_mat_hangs.AsQueryable().Where(d => d.id == model.list_mat_hang[i].db.id_mat_hang).SingleOrDefault();
            //        var sotutang = 100 + i;
            //        model_lich_su_chi_tiet.id = model_lich_su.db.id + sotutang.ToString();
            //        model_lich_su_chi_tiet.ngay_dat_hang = model_lich_su.db.ngay_dat_hang;
            //        model_lich_su_chi_tiet.id_don_hang = model_lich_su.db.id;
            //        model_lich_su_chi_tiet.nguoi_cap_nhat = model_lich_su.db.nguoi_cap_nhat;
            //        model_lich_su_chi_tiet.ngay_cap_nhat = DateTime.Now;
            //        model_lich_su_chi_tiet.status_del = 1;
            //        model_lich_su_chi_tiet.tinh_trang_don_hang = model_lich_su.db.tinh_trang_don_hang;
            //        model_lich_su_chi_tiet.id_mat_hang = mat_hang.id;
            //        model_lich_su_chi_tiet.id_don_vi_tinh = mat_hang.id_don_vi_tinh;
            //        model_lich_su_chi_tiet.so_luong = model.list_mat_hang[i].db.so_luong;
            //        model_lich_su_chi_tiet.don_gia = model.list_mat_hang[i].db.don_gia;
            //        model_lich_su_chi_tiet.don_gia_gom_thue = model.list_mat_hang[i].db.don_gia_gom_thue;
            //        model_lich_su_chi_tiet.thanh_tien_sau_thue = model.list_mat_hang[i].db.thanh_tien_sau_thue;
            //        model_lich_su_chi_tiet.thanh_tien_truoc_thue = model.list_mat_hang[i].db.thanh_tien_truoc_thue;
            //        model_lich_su_chi_tiet.chiet_khau = model.list_mat_hang[i].db.chiet_khau;
            //        model_lich_su_chi_tiet.thanh_tien_chiet_khau = model.list_mat_hang[i].db.thanh_tien_chiet_khau;
            //        model_lich_su_chi_tiet.is_thu_du = model.list_mat_hang[i].db.is_thu_du;
            //        model_lich_su_chi_tiet.is_xuat_du = model.list_mat_hang[i].db.is_xuat_du;
            //        model_lich_su_chi_tiet.vat = model.list_mat_hang[i].db.vat;
            //        model_lich_su_chi_tiet.tien_vat = model.list_mat_hang[i].db.tien_vat;
            //        model_lich_su_chi_tiet.ghi_chu = model.list_mat_hang[i].db.ghi_chu;
            //        model_lich_su_chi_tiet.tinh_trang_don_hang = model.list_mat_hang[i].db.tinh_trang_don_hang;
            //        model_lich_su_chi_tiet.ten_mat_hang = mat_hang.ten;
            //        model_lich_su_chi_tiet.id_loai_mat_hang = mat_hang.id_loai_mat_hang;
            //        model_lich_su_chi_tiet.ma_vach = mat_hang.ma_vach;

            //        await _context.sys_don_hang_ban_mat_hang_logs.InsertOneAsync(model_lich_su_chi_tiet);
            //    }
            //}
            //catch
            //{

            //}

            //await upset_doituong(model);

            return 1;
        }
        //public IQueryable<sys_don_hang_ban_mat_hang_model> FindAllDetail(IQueryable<sys_don_hang_ban_mat_hang_db> query)
        //{

        //    var result = (from d in query.OrderByDescending(d => d.id_don_hang)

        //                  join mh in _context.erp_mat_hangs.AsQueryable()
        //               on d.id_mat_hang equals mh.id into mhg

        //                  join dvt in _context.erp_don_vi_tinhs.AsQueryable()
        //                  on d.id_don_vi_tinh equals dvt.id into dvtg

        //                  from mh in mhg.DefaultIfEmpty()
        //                  from dvt in dvtg.DefaultIfEmpty()
        //                  select new sys_don_hang_ban_mat_hang_model
        //                  {
        //                      db = d,
        //                      id_mat_hang = mh.ma,
        //                      ten_don_vi_tinh = dvt.ten,
        //                      ten_mat_hang = mh.ten,
        //                      ma_mat_hang = mh.ma,
        //                      thuoc_tinh = mh.thuoc_tinh
        //                  });
        //    return result;

        //}


        public IQueryable<sys_don_hang_ban_model> FindAll(IQueryable<sys_don_hang_ban_col> query)
        {
            var result = (from d in query.OrderByDescending(d => d.ngay_dat_hang)

                          join u in _context.sys_user_col.AsQueryable()
                        on d.nguoi_cap_nhat equals u.id into uG

                          from u in uG.DefaultIfEmpty()

                              //join pxk in _context.erp_phieu_xuat_khos.AsQueryable()
                              //on d.id equals pxk.id_don_hang into lpxk
                              //from pxk in lpxk.DefaultIfEmpty()

                          select new sys_don_hang_ban_model
                          {
                              db = d,
                              ten_nguoi_cap_nhat = u.ho_va_ten,
                          });



            return result;
        }
        //public IQueryable<sys_don_hang_ban_log_model> FindAllLog(IQueryable<sys_don_hang_ban_log_db> query)
        //{
        //    var result = (from d in query.OrderByDescending(d => d.ngay_dat_hang)

        //                  join u in _context.Users.AsQueryable()
        //                on d.nguoi_cap_nhat equals u.Id into uG

        //                  from u in uG.DefaultIfEmpty()

        //                      //join pxk in _context.erp_phieu_xuat_khos.AsQueryable()
        //                      //on d.id equals pxk.id_don_hang into lpxk
        //                      //from pxk in lpxk.DefaultIfEmpty()

        //                  select new sys_don_hang_ban_log_model
        //                  {
        //                      db = d,
        //                      ten_nguoi_cap_nhat = u.full_name,
        //                      //is_xuat_kho = pxk != null ? true : false,
        //                      loai_giao_dich_str = d.loai_giao_dich == 1 ? "Hàng hóa" : "Dịch vụ",
        //                      kieu_ban_str = d.kieu_ban == 2 ? "Bán lẻ" : "Bán sỉ",
        //                      ten_ngan_hang = d.ma_ngan_hang + " - " + d.so_tai_khoan,
        //                      hinh_thuc_van_chuyen_str = d.hinh_thuc_van_chuyen == 1 ? "Tại chỗ" : "Giao hàng",
        //                      hinh_thuc_doi_tuong_str = d.hinh_thuc_doi_tuong == 1 ? "Cá nhân" :
        //                                                d.hinh_thuc_doi_tuong == 2 ? "Tổ chức" :
        //                                                d.hinh_thuc_doi_tuong == 3 ? "Phòng ban" : "Nhân viên",
        //                      tinh_trang_don_hang = d.tinh_trang_don_hang == "1" ? "Báo giá" : d.tinh_trang_don_hang == "2" ? "Hợp đồng" : d.tinh_trang_don_hang == "3" ? "Hoàn tất" : "",
        //                  });



        //    return result;
        //}
        //public IQueryable<sys_don_hang_ban_mat_hang_log_model> FindAllLogDetail(IQueryable<sys_don_hang_ban_mat_hang_log_db> query)
        //{

        //    var result = (from d in query.OrderByDescending(d => d.id_don_hang)

        //                  join mh in _context.erp_mat_hangs.AsQueryable()
        //               on d.id_mat_hang equals mh.id into mhg

        //                  join dvt in _context.erp_don_vi_tinhs.AsQueryable()
        //                  on d.id_don_vi_tinh equals dvt.id into dvtg

        //                  from mh in mhg.DefaultIfEmpty()
        //                  from dvt in dvtg.DefaultIfEmpty()
        //                  select new sys_don_hang_ban_mat_hang_log_model
        //                  {
        //                      db = d,
        //                      id_mat_hang = mh.ma,
        //                      ten_don_vi_tinh = dvt.ten,
        //                      ten_mat_hang = mh.ten,
        //                      ma_mat_hang = mh.ma,
        //                      thuoc_tinh = mh.thuoc_tinh
        //                  });
        //    return result;

        //}

        public async Task<int> update_status_del(string id, string userid, int status_del)
        {
            var update = Builders<sys_don_hang_ban_col>.Update
              .Set(x => x.status_del, status_del)
               .Set(x => x.nguoi_cap_nhat, userid)
                .Set(x => x.ngay_cap_nhat, DateTime.Now);

            // Create a filter to match the document to update
            var filter = Builders<sys_don_hang_ban_col>.Filter.Eq(x => x.id, id);
            _context.sys_don_hang_ban_col.UpdateOne(filter, update);
            //var filteDetail = Builders<sys_don_hang_ban_mat_hang_db>.Filter.Eq(x => x.id_don_hang, id);
            //var updateDetail = Builders<sys_don_hang_ban_mat_hang_db>.Update
            //  .Set(x => x.status_del, status_del)
            //   .Set(x => x.nguoi_cap_nhat, userid)
            //    .Set(x => x.ngay_cap_nhat, DateTime.Now);
            //_context.sys_don_hang_ban_mat_hangs.UpdateMany(filteDetail, updateDetail);
            //if (status_del == 2)
            //{
            //    await _common_trigger_don_hang_ban_repo.removeTriggerPhieu(id, userid);
            //}
            return 1;
        }
        //public int cap_nhap(string id, string userid, string tinh_trang_don_hang)
        //{
        //    var update = Builders<sys_don_hang_ban_db>.Update

        //       .Set(x => x.tinh_trang_don_hang, tinh_trang_don_hang)
        //        .Set(x => x.nguoi_cap_nhat, userid)
        //         .Set(x => x.ngay_cap_nhat, DateTime.Now);

        //    // Create a filter to match the document to update
        //    var filter = Builders<sys_don_hang_ban_db>.Filter.Eq(x => x.id, id);
        //    _context.sys_don_hang_bans.UpdateOne(filter, update);


        //    var filtermh = Builders<sys_don_hang_ban_mat_hang_db>.Filter.Eq(q => q.id_don_hang, id);

        //    var updatemh = Builders<sys_don_hang_ban_mat_hang_db>.Update
        //     .Set(x => x.tinh_trang_don_hang, tinh_trang_don_hang);
        //    _context.sys_don_hang_ban_mat_hangs.UpdateMany(filtermh, updatemh);
        //    return 1;
        //}


    }
}
