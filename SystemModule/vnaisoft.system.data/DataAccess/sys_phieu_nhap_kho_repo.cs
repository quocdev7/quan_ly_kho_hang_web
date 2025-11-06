//using Microsoft.EntityFrameworkCore;
//using MongoDB.Driver;
//using NPOI.SS.Util;
//using NPOI.XSSF.UserModel;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text.RegularExpressions;
//using System.Threading.Tasks;
//using System.Web;
//using vnaisoft.common.Common;
//using vnaisoft.common.Helpers;
//using vnaisoft.DataBase.commonFunc;
//using vnaisoft.DataBase.Helper;
//using vnaisoft.DataBase.Mongodb;
//using vnaisoft.DataBase.Mongodb.Collection.system;
//using vnaisoft.system.data.Models;
//using WS.CRM.Data.Helper;
//using static vnaisoft.common.BaseClass.BaseAuthenticationController;

//namespace vnaisoft.system.data.DataAccess
//{
//    public class sys_phieu_nhap_kho_repo
//    {
//        public MongoDBContext _context;
//        public common_mongo_repo _common_repo;
//        public common_trigger_tonkho_repo _common_trigger_tonkho_repo;
//        public common_trigger_dinh_khoan_repo _common_trigger_dinh_khoan_repo;
//        public common_trigger_don_hang_mua_repo _common_trigger_don_hang_mua_repo;
//        public common_trigger_don_hang_ban_repo _common_trigger_don_hang_ban_repo;
//        //public Constant contant;

//        //       var data = new Constant();
//        //var lst = data.list_phuong_thuc_thanh_toan;
//        public sys_phieu_nhap_kho_repo(MongoDBContext context)
//        {
//            _context = context;
//            _common_repo = new common_mongo_repo(context);
//            _common_trigger_tonkho_repo = new common_trigger_tonkho_repo(context);
//            _common_trigger_dinh_khoan_repo = new common_trigger_dinh_khoan_repo(context);
//            _common_trigger_don_hang_mua_repo = new common_trigger_don_hang_mua_repo(context);
//            _common_trigger_don_hang_ban_repo = new common_trigger_don_hang_ban_repo(context);
//            //contant = new Constant();
//        }
//        public string getCode()
//        {
//            var max = "";
//            var config = _common_repo.get_code_config(false, "sys_phieu_nhap_kho", "PNK");
//            var max_query = _context.sys_phieu_nhap_kho_col.AsQueryable()
//             .Where(d => d.ma.StartsWith(config.prefix))
//             .Where(d => d.ma.Length == config.prefix.Length + config.numIncrease)
//             .Select(d => d.ma);
//            if (max_query.Count() > 0)
//            {
//                max = max_query.Max();
//            }
//            var code = _common_repo.generateCode(config.prefix, config.numIncrease, max);

//            return code;
//        }


//        public async Task<sys_phieu_nhap_kho_model> getElementById(string id)
//        {
//            var query = _context.sys_phieu_nhap_kho_col.AsQueryable().Where((m => m.id == id));
//            var obj = FindAll(query).SingleOrDefault();

//            var queryTableDetail = _context.sys_phieu_nhap_kho_chi_tiet_col.AsQueryable().Where(q => q.id_phieu_nhap_kho == id.Trim());
//            obj.list_mat_hang = FindAllDetail(queryTableDetail).AsQueryable().ToList();
//            obj.tong_so_luong = obj.list_mat_hang.Sum(q => q.db.so_luong) ?? 0;
//            //loai == 1 nhap
//            //loai == 2 xuat
//            var ma_loai_xuat = _context.erp_loai_nhap_xuats.AsQueryable().Where(q => q.id == obj.db.id_loai_nhap).Select(q => q.ma).SingleOrDefault();
//            if (obj.db.nguon == 2)
//            {
//                var don_hang_mua = _context.erp_don_hang_muas.AsQueryable().Where(q => q.id == obj.db.id_don_hang_mua).SingleOrDefault();
//                obj.loai_giao_dich = don_hang_mua == null ? null : don_hang_mua.loai_giao_dich;
//                obj.ma_don_hang = don_hang_mua == null ? null : don_hang_mua.ma;
//                obj.ten_hinh_thuc = don_hang_mua == null ? null : don_hang_mua.hinh_thuc_doi_tuong == 1 ? "Cá nhân" : "Tổ chức";
//            }
//            if (obj.db.nguon == 1)
//            {
//                var don_hang_ban = _context.erp_don_hang_bans.AsQueryable().Where(q => q.id == obj.db.id_don_hang_ban).SingleOrDefault();
//                obj.loai_giao_dich = don_hang_ban == null ? null : don_hang_ban.loai_giao_dich;
//                obj.ma_don_hang = don_hang_ban == null ? null : don_hang_ban.ma;
//                obj.ten_hinh_thuc = don_hang_ban == null ? null : don_hang_ban.hinh_thuc_doi_tuong == 1 ? "Cá nhân" : "Tổ chức";
//            }
//            //obj.db.ngay_nhap = DateTime.Parse(obj.db.ngay_nhap.Value.ToString());
//            return obj;
//        }
//        public async Task<sys_phieu_nhap_kho_log_model> getElementByIdLog(string id)
//        {
//            var query = _context.sys_phieu_nhap_kho_logs.AsQueryable().Where((m => m.id == id));
//            var obj = FindAllLog(query).SingleOrDefault();

//            var queryTableDetail = _context.sys_phieu_nhap_kho_chi_tiet_logs.AsQueryable().Where(q => q.id_phieu_nhap_kho == id.Trim());
//            obj.list_mat_hang = FindAllLogDetail(queryTableDetail).AsQueryable().ToList();
//            obj.tong_so_luong = obj.list_mat_hang.Sum(q => q.db.so_luong) ?? 0;
//            //loai == 1 nhap
//            //loai == 2 xuat
//            var ma_loai_xuat = _context.erp_loai_nhap_xuats.AsQueryable().Where(q => q.id == obj.db.id_loai_nhap).Select(q => q.ma).SingleOrDefault();
//            if (obj.db.nguon == 2)
//            {
//                var don_hang_mua = _context.erp_don_hang_muas.AsQueryable().Where(q => q.id == obj.db.id_don_hang_mua).SingleOrDefault();
//                obj.loai_giao_dich = don_hang_mua == null ? null : don_hang_mua.loai_giao_dich;
//                obj.ma_don_hang = don_hang_mua == null ? null : don_hang_mua.ma;
//                obj.ten_hinh_thuc = don_hang_mua == null ? null : don_hang_mua.hinh_thuc_doi_tuong == 1 ? "Cá nhân" : "Tổ chức";
//            }
//            if (obj.db.nguon == 1)
//            {
//                var don_hang_ban = _context.erp_don_hang_bans.AsQueryable().Where(q => q.id == obj.db.id_don_hang_ban).SingleOrDefault();
//                obj.loai_giao_dich = don_hang_ban == null ? null : don_hang_ban.loai_giao_dich;
//                obj.ma_don_hang = don_hang_ban == null ? null : don_hang_ban.ma;
//                obj.ten_hinh_thuc = don_hang_ban == null ? null : don_hang_ban.hinh_thuc_doi_tuong == 1 ? "Cá nhân" : "Tổ chức";
//            }
//            //obj.db.ngay_nhap = DateTime.Parse(obj.db.ngay_nhap.Value.ToString());
//            return obj;
//        }
//        public string generate_ten(sys_phieu_nhap_kho_model model)
//        {
//            var ten = "";
//            var ngay = model.db.ngay_nhap.Value.ToString("dd/MM/yyyy");
//            var loai_nhap = _context.erp_loai_nhap_xuats.AsQueryable().Where(q => q.id == model.db.id_loai_nhap).SingleOrDefault();
//            var dhm = _context.erp_don_hang_muas.AsQueryable().Where(q => q.id == model.db.id_don_hang_mua).SingleOrDefault();
//            var dhb = _context.erp_don_hang_bans.AsQueryable().Where(q => q.id == model.db.id_don_hang_ban).SingleOrDefault();
//            var kho = _context.erp_khos.AsQueryable().Where(q => q.id == model.db.id_kho).SingleOrDefault();
//            //đơn hàng bán
//            if (loai_nhap.nguon == "1")
//            {
//                ten = "Phiếu nhập " + kho.ten + "(" + kho.ma + ")" + " ngày " + ngay + " - " + model.db.ten_doi_tuong + "(" + model.db.id_doi_tuong + ")" + ", " + "Đơn hàng bán: " + dhb.ma + ", " + loai_nhap.ten + "(" + loai_nhap.ma + ")";
//            }
//            //đơn hàng mua
//            else if (loai_nhap.nguon == "2")
//            {
//                ten = "Phiếu nhập " + kho.ten + "(" + kho.ma + ")" + " ngày " + ngay + " - " + model.db.ten_doi_tuong + "(" + model.db.id_doi_tuong + ")" + ", " + "Đơn hàng mua: " + dhm.ma + ", " + loai_nhap.ten + "(" + loai_nhap.ma + ")";
//            }
//            else
//            {
//                ten = "Phiếu nhập " + kho.ten + "(" + kho.ma + ")" + " ngày " + ngay + " - " + model.db.ten_doi_tuong + "(" + model.db.id_doi_tuong + ")" + ", " + loai_nhap.ten + "(" + loai_nhap.ma + ")";
//            }

//            return ten;
//        }
//        public async Task<int> insert_import(sys_phieu_nhap_kho_model model)
//        {
//            if (model.db.id_loai_nhap == "NTRH")
//            {
//                var donHang = _context.erp_don_hang_bans.AsQueryable().Where(d => d.id == model.db.id_don_hang_ban).SingleOrDefault();
//                model.db.id_doi_tuong = donHang.id_doi_tuong;
//                model.db.dia_chi_doi_tuong = donHang.dia_chi_doi_tuong;
//                model.db.hinh_thuc_doi_tuong = donHang.hinh_thuc_doi_tuong;
//                model.db.ten_doi_tuong = donHang.ten_doi_tuong;
//                model.db.email = donHang.email;
//                model.db.dien_thoai = donHang.dien_thoai;
//                var list_mat_hang = _context.erp_don_hang_ban_mat_hangs.AsQueryable().Where(q => q.id_don_hang == model.db.id_don_hang_ban).Select(q => new sys_phieu_nhap_kho_chi_tiet_model
//                {
//                    id_mat_hang = q.id_mat_hang,
//                    so_luong = q.so_luong,
//                    ghi_chu = q.ghi_chu,
//                    id_don_vi_tinh = q.id_don_vi_tinh,
//                    don_gia = q.don_gia,
//                }).ToList();
//                if (list_mat_hang.Count() != 0)
//                {
//                    model.list_mat_hang = list_mat_hang;
//                }
//            }
//            if (model.db.id_loai_nhap == "NM")
//            {
//                var donHang = _context.erp_don_hang_muas.AsQueryable().Where(d => d.id == model.db.id_don_hang_mua).SingleOrDefault();
//                model.db.id_doi_tuong = donHang.id_doi_tuong;
//                model.db.dia_chi_doi_tuong = donHang.dia_chi_doi_tuong;
//                model.db.hinh_thuc_doi_tuong = donHang.hinh_thuc_doi_tuong;
//                model.db.ten_doi_tuong = donHang.ten_doi_tuong;
//                model.db.email = donHang.email;
//                model.db.dien_thoai = donHang.dien_thoai;
//                var list_mat_hang = _context.erp_don_hang_mua_mat_hangs.AsQueryable().Where(q => q.id_don_hang == model.db.id_don_hang_mua).Select(q => new sys_phieu_nhap_kho_chi_tiet_model
//                {
//                    id_mat_hang = q.id_mat_hang,
//                    so_luong = q.so_luong,
//                    ghi_chu = q.ghi_chu,
//                    id_don_vi_tinh = q.id_don_vi_tinh,
//                    don_gia = q.don_gia,
//                }).ToList();
//                if (list_mat_hang.Count() != 0)
//                {
//                    model.list_mat_hang = list_mat_hang;
//                }

//            }
//            model.db.ten = generate_ten(model);
//            model.db.ten_khong_dau = Regex.Replace(StringFunctions.NonUnicode(HttpUtility.HtmlDecode(model.db.ten ?? "")).ToLower().Normalize(), "<.*?>|&.*?;", String.Empty);
//            model.db.id_file_upload = model.db.id + "sys_phieu_nhap_kho";
//            await _context.sys_phieu_nhap_kho_col.InsertOneAsync(model.db);
//            //await _common_repo.insert_file(model.db.id, "sys_phieu_nhap_kho");
//            if (model.list_mat_hang.Count() > 0)
//            {
//                await upset_detail_import(model);
//            }
//            return 1;
//        }
//        public async Task<int> upset_detail_import(sys_phieu_nhap_kho_model model)
//        {
//            var filter_detail = Builders<sys_phieu_nhap_kho_chi_tiet_db>.Filter.Eq("id_phieu_nhap_kho", model.db.id);
//            await _context.sys_phieu_nhap_kho_chi_tiet_col.DeleteManyAsync(filter_detail);
//            await _common_trigger_dinh_khoan_repo.removePhieu(model.db.id, "sys_phieu_nhap_kho");

//            for (int i = 0; i < model.list_mat_hang.Count(); i++)
//            {
//                var data = model.list_mat_hang[i];
//                var id_don_vi_tinh = _context.erp_mat_hangs.AsQueryable().Where(q => q.id == data.id_mat_hang).Select(q => q.id_don_vi_tinh).SingleOrDefault();
//                var db = new sys_phieu_nhap_kho_chi_tiet_db();
//                var sotutang = 100 + i;
//                db.id = model.db.ma + sotutang.ToString();
//                db.status_del = 1;
//                db.id_kho = model.db.id_kho;
//                db.ngay_nhap = model.db.ngay_nhap;
//                db.id_phieu_nhap_kho = model.db.id;
//                db.id_mat_hang = data.id_mat_hang;
//                db.id_don_vi_tinh = id_don_vi_tinh;
//                db.so_luong = data.so_luong;
//                db.ghi_chu = data.ghi_chu;
//                db.nguoi_cap_nhat = model.db.nguoi_cap_nhat;
//                db.ngay_cap_nhat = model.db.ngay_cap_nhat;
//                db.doi_tuong_co = model.db.id_doi_tuong;
//                db.doi_tuong_no = model.db.id_doi_tuong;
//                db.is_dinh_khoan = false;
//                var mat_hang = _context.erp_mat_hangs.AsQueryable().Where(d => d.id == data.id_mat_hang).SingleOrDefault();

//                if (mat_hang != null)
//                {
//                    db.ten_mat_hang = mat_hang.ten;
//                    db.id_loai_mat_hang = mat_hang.id_loai_mat_hang;
//                    db.ma_vach = mat_hang.ma_vach;
//                    var loai_mat_hang = _context.erp_loai_mat_hangs.AsQueryable().Where(d => d.id == mat_hang.id_loai_mat_hang).SingleOrDefault();
//                    if (loai_mat_hang.id_loai_dinh_khoan_mat_hang != null)
//                    {
//                        var loai_dinh_khoan_mat_hang = _context.erp_loai_dinh_khoan_mat_hangs.AsQueryable().Where(d => d.id == loai_mat_hang.id_loai_dinh_khoan_mat_hang).SingleOrDefault();
//                        db.tai_khoan_co = loai_dinh_khoan_mat_hang.ma_tk_co_tien_mat;
//                        db.tai_khoan_no = loai_dinh_khoan_mat_hang.ma_tk_no_tien_mat;
//                    }

//                }
//                await _context.sys_phieu_nhap_kho_chi_tiet_col.InsertOneAsync(db);
//                await _common_trigger_tonkho_repo.updateNhapTonKhoAsync(data.so_luong, 0, model.db.id_kho, data.id_mat_hang, db.id_don_vi_tinh, model.db.ngay_nhap);

//                if (model.db.id_don_hang_ban != null)
//                {
//                    await _common_trigger_don_hang_ban_repo.InsertTriggerDaXuat(model.db.id_don_hang_ban, model.db.nguoi_cap_nhat, db.id_mat_hang);
//                }
//                if (model.db.id_don_hang_mua != null)
//                {
//                    await _common_trigger_don_hang_mua_repo.InsertTriggerDaNhap(model.db.id_don_hang_mua, model.db.nguoi_cap_nhat, db.id_mat_hang);
//                }
//            }
//            return 1;
//        }
//        public async Task<int> insert(sys_phieu_nhap_kho_model model)
//        {

//            if (model.db.id_loai_nhap == "NTRH")
//            {

//                var donHang = _context.erp_don_hang_bans.AsQueryable().Where(d => d.id == model.db.id_don_hang_ban).SingleOrDefault();
//                model.db.id_doi_tuong = donHang.id_doi_tuong;
//                model.db.dia_chi_doi_tuong = donHang.dia_chi_doi_tuong;
//                model.db.hinh_thuc_doi_tuong = donHang.hinh_thuc_doi_tuong;
//                model.db.ten_doi_tuong = donHang.ten_doi_tuong;
//                model.db.email = donHang.email;
//                model.db.dien_thoai = donHang.dien_thoai;
//            }
//            if (model.db.id_loai_nhap == "NM")
//            {
//                var donHang = _context.erp_don_hang_muas.AsQueryable().Where(d => d.id == model.db.id_don_hang_mua).SingleOrDefault();
//                model.db.id_doi_tuong = donHang.id_doi_tuong;
//                model.db.dia_chi_doi_tuong = donHang.dia_chi_doi_tuong;
//                model.db.hinh_thuc_doi_tuong = donHang.hinh_thuc_doi_tuong;
//                model.db.ten_doi_tuong = donHang.ten_doi_tuong;
//                model.db.email = donHang.email;
//                model.db.dien_thoai = donHang.dien_thoai;

//            }
//            model.db.ten = generate_ten(model);
//            model.db.ten_khong_dau = Regex.Replace(StringFunctions.NonUnicode(HttpUtility.HtmlDecode(model.db.ten ?? "")).ToLower().Normalize(), "<.*?>|&.*?;", String.Empty);
//            model.db.id_file_upload = model.db.id + "sys_phieu_nhap_kho";
//            await _context.sys_phieu_nhap_kho_col.InsertOneAsync(model.db);
//            await _common_repo.insert_file(model.db.id, "sys_phieu_nhap_kho");
//            if (model.list_mat_hang.Count() > 0)
//            {
//                await upset_detail(model);
//            }


//            //if(model.db.id_don_hang_ban != null)
//            //{
//            //    for (int i = 0; i < model.list_mat_hang.Count(); i++)
//            //    {
//            //        var itemNew = model.list_mat_hang[i];
//            //        var px = _context.erp_phieu_xuat_khos.AsQueryable().Where(q => q.id_don_hang_ban == model.db.id_don_hang_ban).Select(q => q.id).ToList();
//            //        var so_luong_mat_hang_px = _context.erp_phieu_xuat_kho_chi_tiets.AsQueryable()
//            //               .Where(q => px.Contains(q.id_phieu_xuat_kho) && q.status_del == 1 && q.id_mat_hang == itemNew.id_mat_hang).Sum(q => q.so_luong) ?? 0;
//            //        var pn = _context.sys_phieu_nhap_kho_col.AsQueryable().Where(q => q.id_don_hang_ban == model.db.id_don_hang_ban).Select(q => q.id).ToList();
//            //        var so_luong_mat_hang_pn = _context.sys_phieu_nhap_kho_chi_tiet_col.AsQueryable()
//            //           .Where(q => pn.Contains(q.id_phieu_nhap_kho) && q.status_del == 1 && q.id_mat_hang == itemNew.id_mat_hang).Sum(q => q.so_luong) ?? 0;
//            //        //var so_luong_da_xuat = 

//            //        //var tong_so_luong_px = _context.erp_phieu_xuat_kho_chi_tiets.AsQueryable()
//            //        //                    .Where(d => d.id_phieu_xuat_kho == model.db.id)
//            //        //                    .Where(d => d.status_del == 1)
//            //        //                    .Sum(d => d.so_luong) ?? 0;
//            //        //var tong_so_luong_pn = _context.sys_phieu_nhap_kho_chi_tiet_col.AsQueryable().Where(d => d.id_phieu_nhap_kho == model.db.id).Where(d => d.status_del == 1).Sum(d => d.so_luong) ?? 0;
//            //    }

//            //    //var tong_so_luong_dh = _context.erp_don_hang_ban_mat_hangs.AsQueryable().Where(d => d.id_don_hang == model.db.id_don_hang_ban).Where(d => d.status_del == 1).Sum(d => d.so_luong);

//            //    //var don_hang = _context.erp_don_hang_bans.AsQueryable().Where(d => d.id == model.db.id_don_hang_ban).Where(d => d.status_del == 1).SingleOrDefault();
//            //    //var id_dh = don_hang.id;
//            //    //if ((tong_so_luong + tong_so_luong_pn) == tong_so_luong_dh)
//            //    //{
//            //    //    var update = Builders<erp_don_hang_ban_db>.Update.Set(x => x.da_xuat_du, true);
//            //    //    var filter = Builders<erp_don_hang_ban_db>.Filter.Eq(x => x.id, id_dh);
//            //    //    await _context.erp_don_hang_bans.UpdateOneAsync(filter, update);
//            //    //}
//            //    //else
//            //    //{
//            //    //    var update = Builders<erp_don_hang_ban_db>.Update.Set(x => x.da_xuat_du, false);
//            //    //    var filter = Builders<erp_don_hang_ban_db>.Filter.Eq(x => x.id, id_dh);
//            //    //    await _context.erp_don_hang_bans.UpdateOneAsync(filter, update);
//            //    //}    
//            //}
//            //if (model.db.id_don_hang_mua != null)
//            //{
//            //    var tong_so_luong = model.list_mat_hang.Sum(d => d.db.so_luong);
//            //    var tong_so_luong_pn = _context.sys_phieu_nhap_kho_chi_tiet_col.AsQueryable().Where(d => d.id_phieu_nhap_kho == model.db.id).Where(d => d.status_del == 1).Sum(d => d.so_luong);
//            //    var tong_so_luong_dh = _context.erp_don_hang_mua_mat_hangs.AsQueryable().Where(d => d.id_don_hang == model.db.id_don_hang_mua).Where(d => d.status_del == 1).Sum(d => d.so_luong);

//            //    var don_hang = _context.erp_don_hang_muas.AsQueryable().Where(d => d.id == model.db.id_don_hang_mua).Where(d => d.status_del == 1).SingleOrDefault();
//            //    var id_dh = don_hang.id;
//            //    if ((tong_so_luong + tong_so_luong_pn) == tong_so_luong_dh)
//            //    {
//            //        var update = Builders<erp_don_hang_mua_db>.Update.Set(x => x.da_nhap_du, true);
//            //        var filter = Builders<erp_don_hang_mua_db>.Filter.Eq(x => x.id, id_dh);
//            //        await _context.erp_don_hang_muas.UpdateOneAsync(filter, update);
//            //    }
//            //    else
//            //    {
//            //        var update = Builders<erp_don_hang_mua_db>.Update.Set(x => x.da_nhap_du, false);
//            //        var filter = Builders<erp_don_hang_mua_db>.Filter.Eq(x => x.id, id_dh);
//            //        await _context.erp_don_hang_muas.UpdateOneAsync(filter, update);
//            //    }
//            //}

//            return 1;
//        }
//        public async Task removeOldPhieuAsync(string id_phieu)
//        {
//            var old_db = _context.sys_phieu_nhap_kho_col.AsQueryable().Where(d => d.id == id_phieu).FirstOrDefault();

//            var listOld = _context.sys_phieu_nhap_kho_chi_tiet_col.AsQueryable()
//              .Where(d => d.id_phieu_nhap_kho == id_phieu)
//              .Select(d => new
//              {
//                  id_mat_hang = d.id_mat_hang,
//                  id_don_vi_tinh = d.id_don_vi_tinh,
//                  so_luong = d.so_luong,
//                  gia_tri = d.gia_tri,
//              }).ToList();

//            for (int i = 0; i < listOld.Count(); i++)
//            {
//                await _common_trigger_tonkho_repo.updateNhapTonKhoAsync(-listOld[i].so_luong, -listOld[i].gia_tri, old_db.id_kho, listOld[i].id_mat_hang, listOld[i].id_don_vi_tinh, old_db.ngay_nhap);
//            }
//        }
//        public async Task<int> import_detail(sys_phieu_nhap_kho_model model)
//        {
//            for (int i = 0; i < model.list_mat_hang.Count(); i++)
//            {
//                var data = model.list_mat_hang[i];
//                var db = new sys_phieu_nhap_kho_chi_tiet_db();
//                var sotutang = 100 + i;
//                db.id = model.db.ma + sotutang.ToString();
//                db.status_del = 1;
//                db.id_kho = model.db.id_kho;
//                db.ngay_nhap = model.db.ngay_nhap;
//                db.id_phieu_nhap_kho = model.db.id;
//                db.id_mat_hang = data.db.id_mat_hang;
//                db.id_don_vi_tinh = data.db.id_don_vi_tinh;
//                db.so_luong = data.db.so_luong;
//                db.ghi_chu = data.db.ghi_chu;
//                db.nguoi_cap_nhat = model.db.nguoi_cap_nhat;
//                db.ngay_cap_nhat = model.db.ngay_cap_nhat;
//                db.doi_tuong_co = model.db.id_doi_tuong;
//                db.doi_tuong_no = model.db.id_doi_tuong;
//                db.is_dinh_khoan = false;
//                var mat_hang = _context.erp_mat_hangs.AsQueryable().Where(d => d.id == data.db.id_mat_hang).SingleOrDefault();

//                if (mat_hang != null)
//                {
//                    db.ten_mat_hang = mat_hang.ten;
//                    db.id_loai_mat_hang = mat_hang.id_loai_mat_hang;
//                    db.ma_vach = mat_hang.ma_vach;
//                    var loai_mat_hang = _context.erp_loai_mat_hangs.AsQueryable().Where(d => d.id == mat_hang.id_loai_mat_hang).SingleOrDefault();
//                    if (loai_mat_hang.id_loai_dinh_khoan_mat_hang != null)
//                    {
//                        var loai_dinh_khoan_mat_hang = _context.erp_loai_dinh_khoan_mat_hangs.AsQueryable().Where(d => d.id == loai_mat_hang.id_loai_dinh_khoan_mat_hang).SingleOrDefault();
//                        db.tai_khoan_co = loai_dinh_khoan_mat_hang.ma_tk_co_tien_mat;
//                        db.tai_khoan_no = loai_dinh_khoan_mat_hang.ma_tk_no_tien_mat;
//                    }

//                }
//                await _context.sys_phieu_nhap_kho_chi_tiet_col.InsertOneAsync(db);
//                await _common_trigger_tonkho_repo.updateNhapTonKhoAsync(data.db.so_luong, 0, model.db.id_kho, data.db.id_mat_hang, data.db.id_don_vi_tinh, model.db.ngay_nhap);

//                if (model.db.id_don_hang_ban != null)
//                {
//                    await _common_trigger_don_hang_ban_repo.InsertTriggerDaXuat(model.db.id_don_hang_ban, model.db.nguoi_cap_nhat, db.id_mat_hang);
//                }
//                if (model.db.id_don_hang_mua != null)
//                {
//                    await _common_trigger_don_hang_mua_repo.InsertTriggerDaNhap(model.db.id_don_hang_mua, model.db.nguoi_cap_nhat, db.id_mat_hang);
//                }
//            }
//            return 1;
//        }
//        public async Task<int> upset_detail(sys_phieu_nhap_kho_model model)
//        {
//            var filter_detail = Builders<sys_phieu_nhap_kho_chi_tiet_db>.Filter.Eq("id_phieu_nhap_kho", model.db.id);
//            await _context.sys_phieu_nhap_kho_chi_tiet_col.DeleteManyAsync(filter_detail);
//            await _common_trigger_dinh_khoan_repo.removePhieu(model.db.id, "sys_phieu_nhap_kho");

//            for (int i = 0; i < model.list_mat_hang.Count(); i++)
//            {
//                var data = model.list_mat_hang[i];
//                var filter = Builders<sys_phieu_nhap_kho_chi_tiet_db>.Filter.Eq(x => x.id, data.id_deatils_nhap_kho);
//                await _context.sys_phieu_nhap_kho_chi_tiet_col.DeleteManyAsync(filter);
//                var db = new sys_phieu_nhap_kho_chi_tiet_db();
//                var sotutang = 100 + i;
//                db.id = model.db.ma + sotutang.ToString();
//                db.status_del = 1;
//                db.id_kho = model.db.id_kho;
//                db.ngay_nhap = model.db.ngay_nhap;
//                db.id_phieu_nhap_kho = model.db.id;
//                db.id_mat_hang = data.db.id_mat_hang;
//                db.id_don_vi_tinh = data.db.id_don_vi_tinh;
//                db.so_luong = data.db.so_luong;
//                db.ghi_chu = data.db.ghi_chu;
//                db.nguoi_cap_nhat = model.db.nguoi_cap_nhat;
//                db.ngay_cap_nhat = model.db.ngay_cap_nhat;
//                db.doi_tuong_co = model.db.id_doi_tuong;
//                db.doi_tuong_no = model.db.id_doi_tuong;
//                db.id_loai_nhap = model.db.id_loai_nhap;
//                db.is_dinh_khoan = false;
//                var mat_hang = _context.erp_mat_hangs.AsQueryable().Where(d => d.id == data.db.id_mat_hang).SingleOrDefault();

//                if (mat_hang != null)
//                {
//                    db.ten_mat_hang = mat_hang.ten;
//                    db.id_loai_mat_hang = mat_hang.id_loai_mat_hang;
//                    db.ma_vach = mat_hang.ma_vach;
//                    var loai_mat_hang = _context.erp_loai_mat_hangs.AsQueryable().Where(d => d.id == mat_hang.id_loai_mat_hang).SingleOrDefault();
//                    if (loai_mat_hang.id_loai_dinh_khoan_mat_hang != null)
//                    {
//                        var loai_dinh_khoan_mat_hang = _context.erp_loai_dinh_khoan_mat_hangs.AsQueryable().Where(d => d.id == loai_mat_hang.id_loai_dinh_khoan_mat_hang).SingleOrDefault();
//                        db.tai_khoan_co = loai_dinh_khoan_mat_hang.ma_tk_co_tien_mat;
//                        db.tai_khoan_no = loai_dinh_khoan_mat_hang.ma_tk_no_tien_mat;
//                    }

//                }
//                await _context.sys_phieu_nhap_kho_chi_tiet_col.InsertOneAsync(db);
//                await _common_trigger_tonkho_repo.updateNhapTonKhoAsync(data.db.so_luong, 0, model.db.id_kho, data.db.id_mat_hang, data.db.id_don_vi_tinh, model.db.ngay_nhap);

//                if (model.db.id_don_hang_ban != null)
//                {
//                    await _common_trigger_don_hang_ban_repo.InsertTriggerDaXuat(model.db.id_don_hang_ban, model.db.nguoi_cap_nhat, db.id_mat_hang);
//                }
//                if (model.db.id_don_hang_mua != null)
//                {
//                    await _common_trigger_don_hang_mua_repo.InsertTriggerDaNhap(model.db.id_don_hang_mua, model.db.nguoi_cap_nhat, db.id_mat_hang);
//                }
//            }
//            return 1;
//        }
//        public async Task<int> upset_detail_log(sys_phieu_nhap_kho_model model, sys_phieu_nhap_kho_log_model model_lich_su)
//        {
//            for (int i = 0; i < model.list_mat_hang.Count(); i++)
//            {
//                var data = model.list_mat_hang[i];
//                var db = new sys_phieu_nhap_kho_chi_tiet_log_db();
//                var sotutang = 100 + i;
//                db.id = model_lich_su.db.id + sotutang.ToString();
//                db.status_del = 1;
//                db.id_kho = model.db.id_kho;
//                db.ngay_nhap = model.db.ngay_nhap;
//                db.id_phieu_nhap_kho = model_lich_su.db.id;
//                db.id_mat_hang = data.db.id_mat_hang;
//                db.id_don_vi_tinh = data.db.id_don_vi_tinh;
//                db.so_luong = data.db.so_luong;
//                db.ghi_chu = data.db.ghi_chu;
//                db.nguoi_cap_nhat = model.db.nguoi_cap_nhat;
//                db.ngay_cap_nhat = model.db.ngay_cap_nhat;
//                db.doi_tuong_co = model.db.id_doi_tuong;
//                db.doi_tuong_no = model.db.id_doi_tuong;
//                db.is_dinh_khoan = false;
//                var mat_hang = _context.erp_mat_hangs.AsQueryable().Where(d => d.id == data.db.id_mat_hang).SingleOrDefault();

//                if (mat_hang != null)
//                {
//                    db.ten_mat_hang = mat_hang.ten;
//                    db.id_loai_mat_hang = mat_hang.id_loai_mat_hang;
//                    db.ma_vach = mat_hang.ma_vach;
//                    var loai_mat_hang = _context.erp_loai_mat_hangs.AsQueryable().Where(d => d.id == mat_hang.id_loai_mat_hang).SingleOrDefault();
//                    if (loai_mat_hang.id_loai_dinh_khoan_mat_hang != null)
//                    {
//                        var loai_dinh_khoan_mat_hang = _context.erp_loai_dinh_khoan_mat_hangs.AsQueryable().Where(d => d.id == loai_mat_hang.id_loai_dinh_khoan_mat_hang).SingleOrDefault();
//                        db.tai_khoan_co = loai_dinh_khoan_mat_hang.ma_tk_co_tien_mat;
//                        db.tai_khoan_no = loai_dinh_khoan_mat_hang.ma_tk_no_tien_mat;
//                    }

//                }
//                await _context.sys_phieu_nhap_kho_chi_tiet_logs.InsertOneAsync(db);
//            }
//            return 1;
//        }
//        //public async Task<string> InsertTriggerDaXuat(sys_phieu_nhap_kho_model model, sys_phieu_nhap_kho_chi_tiet_db model_detail)
//        //{
//        //    var so_luong_mat_hang_dhb = _context.erp_don_hang_ban_mat_hangs.AsQueryable()
//        //                 .Where(q => q.status_del == 1 && q.id_don_hang == model.db.id_don_hang_ban && q.id_mat_hang == model_detail.id_mat_hang)
//        //                 .Sum(q => q.so_luong) ?? 0;
//        //    var px = _context.erp_phieu_xuat_khos.AsQueryable().Where(q => q.id_don_hang_ban == model.db.id_don_hang_ban).Select(q => q.id).ToList();
//        //    var so_luong_mat_hang_px = _context.erp_phieu_xuat_kho_chi_tiets.AsQueryable()
//        //       .Where(q => px.Contains(q.id_phieu_xuat_kho) && q.status_del == 1 && q.id_mat_hang == model_detail.id_mat_hang).Sum(q => q.so_luong) ?? 0;

//        //    var pn = _context.sys_phieu_nhap_kho_col.AsQueryable().Where(q => q.id_don_hang_ban == model.db.id_don_hang_ban).Select(q => q.id).ToList();
//        //    var so_luong_mat_hang_pn = _context.sys_phieu_nhap_kho_chi_tiet_col.AsQueryable()
//        //       .Where(q => pn.Contains(q.id_phieu_nhap_kho) && q.status_del == 1 && q.id_mat_hang == model_detail.id_mat_hang).Sum(q => q.so_luong) ?? 0;


//        //    var so_luong_xuat = so_luong_mat_hang_px - so_luong_mat_hang_pn;


//        //    if (so_luong_mat_hang_dhb <= so_luong_xuat)
//        //    {
//        //        var update = Builders<erp_don_hang_ban_db>.Update.Set(x => x.is_xuat_du, true);
//        //        var filter = Builders<erp_don_hang_ban_db>.Filter.Eq(x => x.id, model.db.id_don_hang_ban);
//        //        await _context.erp_don_hang_bans.UpdateOneAsync(filter, update);

//        //        var filteDetail = Builders<erp_don_hang_ban_mat_hang_db>.Filter.Eq(x => x.id_don_hang, model.db.id_don_hang_ban);
//        //        var updateDetail = Builders<erp_don_hang_ban_mat_hang_db>.Update
//        //            .Set(x => x.ngay_cap_nhat, DateTime.Now)
//        //            .Set(x => x.nguoi_cap_nhat, model.db.nguoi_cap_nhat)
//        //            .Set(x => x.is_chi_du, true)
//        //            ;
//        //        _context.erp_don_hang_ban_mat_hangs.UpdateMany(filteDetail, updateDetail);
//        //    }
//        //    else
//        //    {
//        //        var update = Builders<erp_don_hang_ban_db>.Update.Set(x => x.is_xuat_du, false);
//        //        var filter = Builders<erp_don_hang_ban_db>.Filter.Eq(x => x.id, model.db.id_don_hang_ban);
//        //        await _context.erp_don_hang_bans.UpdateOneAsync(filter, update);


//        //        var filteDetail = Builders<erp_don_hang_ban_mat_hang_db>.Filter.Eq(x => x.id_don_hang, model.db.id_don_hang_ban);
//        //        var updateDetail = Builders<erp_don_hang_ban_mat_hang_db>.Update
//        //            .Set(x => x.ngay_cap_nhat, DateTime.Now)
//        //            .Set(x => x.nguoi_cap_nhat, model.db.nguoi_cap_nhat)
//        //            .Set(x => x.is_chi_du, true)
//        //            ;
//        //        _context.erp_don_hang_ban_mat_hangs.UpdateMany(filteDetail, updateDetail);
//        //    }
//        //    return "";
//        //}

//        //public async Task<string> InsertTriggerDaNhap(sys_phieu_nhap_kho_model model, sys_phieu_nhap_kho_chi_tiet_db model_detail)
//        //{

//        //    var so_luong_mat_hang_dhb = _context.erp_don_hang_mua_mat_hangs.AsQueryable()
//        //           .Where(q => q.status_del == 1 && q.id_don_hang == model.db.id_don_hang_mua && q.id_mat_hang == model_detail.id_mat_hang)
//        //           .Sum(q => q.so_luong) ?? 0;
//        //    var px = _context.sys_phieu_nhap_kho_col.AsQueryable().Where(q => q.id_don_hang_mua == model.db.id_don_hang_mua).Select(q => q.id).ToList();
//        //    var so_luong_mat_hang_px = _context.erp_phieu_xuat_kho_chi_tiets.AsQueryable()
//        //       .Where(q => px.Contains(q.id_phieu_xuat_kho) && q.status_del == 1 && q.id_mat_hang == model_detail.id_mat_hang).Sum(q => q.so_luong) ?? 0;

//        //    var pn = _context.sys_phieu_nhap_kho_col.AsQueryable().Where(q => q.id_don_hang_mua == model.db.id_don_hang_mua).Select(q => q.id).ToList();
//        //    var so_luong_mat_hang_pn = _context.sys_phieu_nhap_kho_chi_tiet_col.AsQueryable()
//        //       .Where(q => pn.Contains(q.id_phieu_nhap_kho) && q.status_del == 1 && q.id_mat_hang == model_detail.id_mat_hang).Sum(q => q.so_luong) ?? 0;


//        //    var so_luong_nhap = so_luong_mat_hang_pn - so_luong_mat_hang_px;


//        //    if (so_luong_mat_hang_dhb <= so_luong_nhap)
//        //    {
//        //        var update = Builders<erp_don_hang_mua_db>.Update.Set(x => x.is_nhap_du, true);
//        //        var filter = Builders<erp_don_hang_mua_db>.Filter.Eq(x => x.id, model.db.id_don_hang_mua);
//        //        await _context.erp_don_hang_muas.UpdateOneAsync(filter, update);

//        //        var filteDetail = Builders<erp_don_hang_ban_mat_hang_db>.Filter.Eq(x => x.id_don_hang, model.db.id_don_hang_mua);
//        //        var updateDetail = Builders<erp_don_hang_ban_mat_hang_db>.Update
//        //            .Set(x => x.ngay_cap_nhat, DateTime.Now)
//        //            .Set(x => x.nguoi_cap_nhat, model.db.nguoi_cap_nhat)
//        //            .Set(x => x.is_chi_du, true)
//        //            ;
//        //        _context.erp_don_hang_ban_mat_hangs.UpdateMany(filteDetail, updateDetail);
//        //    }
//        //    else
//        //    {
//        //        var update = Builders<erp_don_hang_mua_db>.Update.Set(x => x.is_nhap_du, false);
//        //        var filter = Builders<erp_don_hang_mua_db>.Filter.Eq(x => x.id, model.db.id_don_hang_mua);
//        //        await _context.erp_don_hang_muas.UpdateOneAsync(filter, update);


//        //        var filteDetail = Builders<erp_don_hang_ban_mat_hang_db>.Filter.Eq(x => x.id_don_hang, model.db.id_don_hang_mua);
//        //        var updateDetail = Builders<erp_don_hang_ban_mat_hang_db>.Update
//        //            .Set(x => x.ngay_cap_nhat, DateTime.Now)
//        //            .Set(x => x.nguoi_cap_nhat, model.db.nguoi_cap_nhat)
//        //            .Set(x => x.is_chi_du, false)
//        //            ;
//        //        _context.erp_don_hang_ban_mat_hangs.UpdateMany(filteDetail, updateDetail);
//        //    }
//        //    return "";
//        //}
//        public async Task<int> update(sys_phieu_nhap_kho_model model)
//        {
//            model.db.ten = generate_ten(model);
//            model.db.ten_khong_dau = Regex.Replace(StringFunctions.NonUnicode(HttpUtility.HtmlDecode(model.db.ten ?? "")).ToLower().Normalize(), "<.*?>|&.*?;", String.Empty);

//            var update = Builders<sys_phieu_nhap_kho_db>.Update
//                  //.Set(x => x.ma, model.db.ma)
//                  //.Set(x => x.loai, model.db.loai)
//                  .Set(x => x.ten, model.db.ten)
//                  .Set(x => x.ten_khong_dau, model.db.ten_khong_dau)
//                  .Set(x => x.nguon, model.db.nguon)
//                  .Set(x => x.id_doi_tuong, model.db.id_doi_tuong)
//                   .Set(x => x.id_phieu_chuyen_kho, model.db.id_phieu_chuyen_kho)
//                    .Set(x => x.id_don_hang_ban_thuc_hien, model.db.id_don_hang_ban_thuc_hien)
//                       .Set(x => x.ngay_nhap, model.db.ngay_nhap)
//                         .Set(x => x.id_kho, model.db.id_kho)
//                           .Set(x => x.id_loai_nhap, model.db.id_loai_nhap)
//                             .Set(x => x.nguoi_cap_nhat, model.db.nguoi_cap_nhat)
//                               .Set(x => x.ngay_cap_nhat, model.db.ngay_cap_nhat)
//                                   .Set(x => x.status_del, model.db.status_del)
//                                       .Set(x => x.nguoi_cap_nhat, model.db.nguoi_cap_nhat)
//                                        .Set(x => x.ngay_cap_nhat, model.db.ngay_cap_nhat);


//            // Create a filter to match the document to update
//            var filter = Builders<sys_phieu_nhap_kho_db>.Filter.Eq(x => x.id, model.db.id);


//            await removeOldPhieuAsync(model.db.id);

//            // Create an update definition to set the "Name" property to a new value
//            await _context.sys_phieu_nhap_kho_col.UpdateOneAsync(filter, update);

//            if (model.list_mat_hang.Count() > 0)
//            {
//                await upset_detail(model);
//            }
//            var model_lich_su = new sys_phieu_nhap_kho_log_model();
//            model_lich_su.db.id = ObjectId.GenerateNewId().ToString();
//            model_lich_su.db.is_sinh_tu_dong = model.db.is_sinh_tu_dong;
//            model_lich_su.db.ma = model.db.ma;
//            model_lich_su.db.ten = model.db.ten;
//            model_lich_su.db.ten_khong_dau = model.db.ten_khong_dau;
//            model_lich_su.db.nguon = model.db.nguon;
//            model_lich_su.db.id_doi_tuong = model.db.id_doi_tuong;
//            model_lich_su.db.id_phieu_chuyen_kho = model.db.id_phieu_chuyen_kho;
//            model_lich_su.db.id_don_hang_ban_thuc_hien = model.db.id_don_hang_ban_thuc_hien;
//            model_lich_su.db.ngay_nhap = model.db.ngay_nhap;
//            model_lich_su.db.id_kho = model.db.id_kho;
//            model_lich_su.db.id_loai_nhap = model.db.id_loai_nhap;
//            model_lich_su.db.nguoi_cap_nhat = model.db.nguoi_cap_nhat;
//            model_lich_su.db.ngay_cap_nhat = model.db.ngay_cap_nhat;
//            model_lich_su.db.status_del = model.db.status_del;
//            model_lich_su.db.nguoi_cap_nhat = model.db.nguoi_cap_nhat;
//            model_lich_su.db.ngay_cap_nhat = model.db.ngay_cap_nhat;



//            model_lich_su.db.ly_do_chinh_sua = model.ly_do_chinh_sua;

//            await _context.sys_phieu_nhap_kho_logs.InsertOneAsync(model_lich_su.db);


//            await upset_detail_log(model, model_lich_su);

//            return 1;
//        }

//        public IQueryable<sys_phieu_nhap_kho_model> FindAll(IQueryable<sys_phieu_nhap_kho_db> query)
//        {
//            //var vat = contant.list_vat.AsQueryable().Select(q => q.name).ToList();
//            var result = (from d in query.OrderByDescending(d => d.ngay_nhap)

//                          join u in _context.Users.AsQueryable()
//                          on d.nguoi_cap_nhat equals u.Id into uG

//                          join tk in _context.erp_khos.AsQueryable()
//                          on d.id_kho equals tk.id into tkG

//                          join ln in _context.erp_loai_nhap_xuats.AsQueryable()
//                          on d.id_loai_nhap equals ln.id into lnG

//                          join pck in _context.erp_phieu_chuyen_khos.AsQueryable()
//                          on d.id_phieu_chuyen_kho equals pck.id into pckG

//                          from tk in tkG.DefaultIfEmpty()
//                          from u in uG.DefaultIfEmpty()
//                          from ln in lnG.DefaultIfEmpty()
//                          from pck in pckG.DefaultIfEmpty()

//                          select new sys_phieu_nhap_kho_model
//                          {
//                              db = d,
//                              ten_nguoi_cap_nhat = u.full_name,
//                              ten_loai_nhap = ln.ten,
//                              ten_kho = tk.ten,
//                              ma_chuyen_kho = pck.ma,
//                              is_sinh_tu_dong = d.ma.StartsWith("PNDHM") ? true : false
//                          });
//            return result;
//        }

//        public IQueryable<sys_phieu_nhap_kho_chi_tiet_model> FindAllDetail(IQueryable<sys_phieu_nhap_kho_chi_tiet_db> query)
//        {

//            var result = (from d in query.OrderByDescending(d => d.id_phieu_nhap_kho)
//                          join mh in _context.erp_mat_hangs.AsQueryable()
//                           on d.id_mat_hang equals mh.id into mhG

//                          join dvt in _context.erp_don_vi_tinhs.AsQueryable()
//                          on d.id_don_vi_tinh equals dvt.id into dvtG

//                          from dvt in dvtG.DefaultIfEmpty()
//                          from mh in mhG.DefaultIfEmpty()

//                          select new sys_phieu_nhap_kho_chi_tiet_model
//                          {
//                              db = d,
//                              ten_don_vi_tinh = dvt.ten,
//                              ten_mat_hang = mh.ten,
//                              ma_mat_hang = mh.ma,
//                              id_mat_hang = mh.id,
//                              id_deatils_nhap_kho = d.id,
//                          });
//            return result;
//        }
//        public IQueryable<sys_phieu_nhap_kho_log_model> FindAllLog(IQueryable<sys_phieu_nhap_kho_log_db> query)
//        {
//            //var vat = contant.list_vat.AsQueryable().Select(q => q.name).ToList();
//            var result = (from d in query.OrderByDescending(d => d.ngay_nhap)

//                          join u in _context.Users.AsQueryable()
//                          on d.nguoi_cap_nhat equals u.Id into uG

//                          join tk in _context.erp_khos.AsQueryable()
//                          on d.id_kho equals tk.id into tkG

//                          join ln in _context.erp_loai_nhap_xuats.AsQueryable()
//                          on d.id_loai_nhap equals ln.id into lnG

//                          join pck in _context.erp_phieu_chuyen_khos.AsQueryable()
//                          on d.id_phieu_chuyen_kho equals pck.id into pckG

//                          from tk in tkG.DefaultIfEmpty()
//                          from u in uG.DefaultIfEmpty()
//                          from ln in lnG.DefaultIfEmpty()
//                          from pck in pckG.DefaultIfEmpty()

//                          select new sys_phieu_nhap_kho_log_model
//                          {
//                              db = d,
//                              ten_nguoi_cap_nhat = u.full_name,
//                              ten_loai_nhap = ln.ten,
//                              ten_kho = tk.ten,
//                              ma_chuyen_kho = pck.ma,
//                              is_sinh_tu_dong = d.ma.StartsWith("PNDHM") ? true : false
//                          });
//            return result;
//        }

//        public IQueryable<sys_phieu_nhap_kho_chi_tiet_log_model> FindAllLogDetail(IQueryable<sys_phieu_nhap_kho_chi_tiet_log_db> query)
//        {

//            var result = (from d in query.OrderByDescending(d => d.id_phieu_nhap_kho)
//                          join mh in _context.erp_mat_hangs.AsQueryable()
//                           on d.id_mat_hang equals mh.id into mhG

//                          join dvt in _context.erp_don_vi_tinhs.AsQueryable()
//                          on d.id_don_vi_tinh equals dvt.id into dvtG

//                          from dvt in dvtG.DefaultIfEmpty()
//                          from mh in mhG.DefaultIfEmpty()

//                          select new sys_phieu_nhap_kho_chi_tiet_log_model
//                          {
//                              db = d,
//                              ten_don_vi_tinh = dvt.ten,
//                              ten_mat_hang = mh.ten,
//                              ma_mat_hang = mh.ma,
//                              id_mat_hang = mh.id,
//                              id_deatils_nhap_kho = d.id,
//                          });
//            return result;
//        }

//        public IQueryable<sys_phieu_nhap_kho_chi_tiet_ref_model> FindAllDetailMatHang(IQueryable<sys_phieu_nhap_kho_chi_tiet_db> query)
//        {
//            var result = (from d in query.OrderByDescending(d => d.id_phieu_nhap_kho)

//                          join mh in _context.erp_mat_hangs.AsQueryable()
//                          on d.id_mat_hang equals mh.id into lmh

//                          join dvt in _context.erp_don_vi_tinhs.AsQueryable()
//                          on d.id_don_vi_tinh equals dvt.id into ldvt



//                          from dvt in ldvt.DefaultIfEmpty()
//                          from mh in lmh.DefaultIfEmpty()
//                          select new sys_phieu_nhap_kho_chi_tiet_ref_model
//                          {
//                              id = d.id,
//                              id_phieu_nhap_kho = d.id_phieu_nhap_kho,
//                              id_mat_hang = mh.id,
//                              ten_don_vi_tinh = dvt.ten,
//                              ten_mat_hang = mh.ten,
//                              ma_mat_hang = mh.ma,
//                              don_gia = mh.gia_ban_si,
//                              so_luong = d.so_luong
//                          });
//            return result;
//        }
//        public async Task<int> update_status_del(string id, string userid, int status_del)
//        {

//            var update = Builders<sys_phieu_nhap_kho_db>.Update
//               .Set(x => x.status_del, status_del)
//                .Set(x => x.nguoi_cap_nhat, userid)
//                 .Set(x => x.ngay_cap_nhat, DateTime.Now);

//            // Create a filter to match the document to update

//            if (status_del == 2)
//            {
//                await removeOldPhieuAsync(id);
//                await _common_trigger_dinh_khoan_repo.removePhieu(id, "sys_phieu_nhap_kho");
//            }
//            else if (status_del == 1)
//            {

//            }
//            var filter = Builders<sys_phieu_nhap_kho_db>.Filter.Eq(x => x.id, id);
//            _context.sys_phieu_nhap_kho_col.UpdateOne(filter, update);
//            var filteDetail = Builders<sys_phieu_nhap_kho_chi_tiet_db>.Filter.Eq(x => x.id_phieu_nhap_kho, id);
//            var updateDetail = Builders<sys_phieu_nhap_kho_chi_tiet_db>.Update
//              .Set(x => x.status_del, status_del)
//               .Set(x => x.nguoi_cap_nhat, userid)
//                .Set(x => x.ngay_cap_nhat, DateTime.Now);
//            _context.sys_phieu_nhap_kho_chi_tiet_col.UpdateMany(filteDetail, updateDetail);



//            var query = _context.sys_phieu_nhap_kho_col.AsQueryable().Where(m => m.id == id);
//            var model = FindAll(query).SingleOrDefault();

//            var list_detail = _context.sys_phieu_nhap_kho_chi_tiet_col.AsQueryable().Where(q => q.id_phieu_nhap_kho == id.Trim());

//            foreach (var item in list_detail)
//            {
//                if (model.db.id_don_hang_ban != null)
//                {
//                    await _common_trigger_don_hang_ban_repo.InsertTriggerDaXuat(model.db.id_don_hang_ban, model.db.nguoi_cap_nhat, item.id_mat_hang);
//                }
//                if (model.db.id_don_hang_mua != null)
//                {
//                    await _common_trigger_don_hang_mua_repo.InsertTriggerDaNhap(model.db.id_don_hang_ban, model.db.nguoi_cap_nhat, item.id_mat_hang);
//                }
//            }
//            return 1;
//        }


//    }
//}
