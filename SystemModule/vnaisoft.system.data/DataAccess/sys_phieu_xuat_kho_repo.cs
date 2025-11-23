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
    public class sys_phieu_xuat_kho_repo
    {
        public MongoDBContext _context;
        public common_mongo_repo _common_repo;
        public common_trigger_tonkho_repo _common_trigger_tonkho_repo;
        public sys_phieu_xuat_kho_repo(MongoDBContext context)
        {
            _context = context;
            _common_repo = new common_mongo_repo(context);
            _common_trigger_tonkho_repo = new common_trigger_tonkho_repo(context);
        }
        public string getCode()
        {
            var max = "";
            var config = _common_repo.get_code_config(false, "sys_phieu_xuat_kho", "PXK");
            var max_query = _context.sys_phieu_xuat_kho_col.AsQueryable()
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

        public async Task<sys_phieu_xuat_kho_model> getElementById(string id)
        {
            var query = _context.sys_phieu_xuat_kho_col.AsQueryable().Where(m => m.id == id);
            var obj = FindAll(query).SingleOrDefault();

            var queryTableDetail = _context.sys_phieu_xuat_kho_chi_tiet_col.AsQueryable().Where(q => q.id_phieu_xuat_kho == id.Trim());
            try
            {
                obj.list_mat_hang = FindAllDetail(queryTableDetail).ToList();
                obj.list_mat_hang.ForEach(q =>
                {
                    var thanh_tien = (q.db.don_gia * q.db.so_luong) ?? 0;
                    q.thanh_tien = thanh_tien;
                });
                obj.tong_so_luong = obj.list_mat_hang.Sum(q => q.db.so_luong) ?? 0;
            }
            catch (Exception e)
            {

            }

            //loai == 1 nhap
            //loai == 2 xuat
            if (obj.db.nguon == 2)
            {
                var don_hang_mua = _context.sys_don_hang_mua_col.AsQueryable().Where(q => q.id == obj.db.id_don_hang_mua).SingleOrDefault();
                obj.ma_don_hang = don_hang_mua.ma;
            }
            if (obj.db.nguon == 1)
            {
                var don_hang_ban = _context.sys_don_hang_ban_col.AsQueryable().Where(q => q.id == obj.db.id_don_hang_ban).SingleOrDefault();
                obj.ma_don_hang = don_hang_ban.ma;
            }
            return obj;
        }
        public async Task<int> insert(sys_phieu_xuat_kho_model model)
        {
            model.db.ten_khong_dau = Regex.Replace(StringFunctions.NonUnicode(HttpUtility.HtmlDecode(model.db.ten ?? "")).ToLower().Normalize(), "<.*?>|&.*?;", String.Empty);
            await _context.sys_phieu_xuat_kho_col.InsertOneAsync(model.db);
            if (model.list_mat_hang.Count() > 0)
            {
                await upset_detail(model);
            }

            return 1;
        }
        public async Task<int> upset_detail(sys_phieu_xuat_kho_model model)
        {
            var filter_detail = Builders<sys_phieu_xuat_kho_chi_tiet_col>.Filter.Eq("id_phieu_xuat_kho", model.db.id);
            await _context.sys_phieu_xuat_kho_chi_tiet_col.DeleteManyAsync(filter_detail);
            for (int i = 0; i < model.list_mat_hang.Count(); i++)
            {
                var data = model.list_mat_hang[i];
                var filter = Builders<sys_phieu_xuat_kho_chi_tiet_col>.Filter.Eq(x => x.id, data.id_deatils_xuat_kho);
                await _context.sys_phieu_xuat_kho_chi_tiet_col.DeleteManyAsync(filter);
                var db = new sys_phieu_xuat_kho_chi_tiet_col();
                var sotutang = 100 + i;
                db.id = model.db.ma + sotutang.ToString();
                db.status_del = 1;
                db.ngay_xuat = model.db.ngay_xuat;
                db.id_phieu_xuat_kho = model.db.id;
                db.id_mat_hang = data.db.id_mat_hang;
                db.id_don_vi_tinh = data.db.id_don_vi_tinh;
                db.so_luong = data.db.so_luong;
                db.don_gia = data.db.don_gia;
                db.gia_tri = (data.db.so_luong ?? 0) * (data.db.don_gia ?? 0);
                db.ghi_chu = data.db.ghi_chu;
                db.nguoi_cap_nhat = model.db.nguoi_cap_nhat;
                db.ngay_cap_nhat = model.db.ngay_cap_nhat;
                var mat_hang = _context.sys_mat_hang_col.AsQueryable().Where(d => d.id == data.db.id_mat_hang).SingleOrDefault();
                if (mat_hang != null)
                {
                    db.ten_mat_hang = mat_hang.ten;
                    db.id_loai_mat_hang = mat_hang.id_loai_mat_hang;
                }
                await _context.sys_phieu_xuat_kho_chi_tiet_col.InsertOneAsync(db);
                await _common_trigger_tonkho_repo.updateXuatTonKhoAsync(data.db.so_luong, (data.db.so_luong ?? 0) * (data.db.don_gia ?? 0), data.db.id_mat_hang, data.db.id_don_vi_tinh, model.db.ngay_xuat);
            }
            return 1;
        }
        public async Task<int> delete_detail(sys_phieu_xuat_kho_model model)
        {
            var filter = Builders<sys_phieu_xuat_kho_chi_tiet_col>.Filter.Eq("id_phieu_xuat_kho", model.db.id);
            await _context.sys_phieu_xuat_kho_chi_tiet_col.DeleteManyAsync(filter);
            return 1;
        }

        public async Task<int> update(sys_phieu_xuat_kho_model model)
        {
            model.db.ten_khong_dau = Regex.Replace(StringFunctions.NonUnicode(HttpUtility.HtmlDecode(model.db.ten ?? "")).ToLower().Normalize(), "<.*?>|&.*?;", String.Empty);
            var update = Builders<sys_phieu_xuat_kho_col>.Update
                  .Set(x => x.ten, model.db.ten)
                  .Set(x => x.ten_khong_dau, model.db.ten_khong_dau)
                  .Set(x => x.nguon, model.db.nguon)
                  .Set(x => x.ngay_xuat, model.db.ngay_xuat)
                  .Set(x => x.id_loai_xuat, model.db.id_loai_xuat)
                  .Set(x => x.nguoi_cap_nhat, model.db.nguoi_cap_nhat)
                  .Set(x => x.ngay_cap_nhat, model.db.ngay_cap_nhat)
                  .Set(x => x.status_del, model.db.status_del)
                  .Set(x => x.nguoi_cap_nhat, model.db.nguoi_cap_nhat)
                  .Set(x => x.ngay_cap_nhat, model.db.ngay_cap_nhat);
            // Creae a filter to match the document to update
            var filter = Builders<sys_phieu_xuat_kho_col>.Filter.Eq(x => x.id, model.db.id);

            await removeOldPhieuAsync(model.db.id);
            // Create an update definition to set the "Name" property to a new value
            await _context.sys_phieu_xuat_kho_col.UpdateOneAsync(filter, update);
            if (model.list_mat_hang.Count() > 0)
            {
                await upset_detail(model);
            }

            return 1;
        }

        public IQueryable<sys_phieu_xuat_kho_model> FindAll(IQueryable<sys_phieu_xuat_kho_col> query)
        {
            var result = (from d in query.OrderByDescending(d => d.ngay_xuat)

                          join u in _context.sys_user_col.AsQueryable()
                          on d.nguoi_cap_nhat equals u.id into uG

                          join lx in _context.sys_loai_nhap_xuat_col.AsQueryable()
                          on d.id_loai_xuat equals lx.id into lxG

                          from u in uG.DefaultIfEmpty()
                          from lx in lxG.DefaultIfEmpty()

                          select new sys_phieu_xuat_kho_model
                          {
                              db = d,
                              ten_nguoi_cap_nhat = u.ho_va_ten,
                              ten_loai_xuat = lx.ten,
                          });
            return result;
        }
        public IQueryable<sys_phieu_xuat_kho_chi_tiet_model> FindAllDetail(IQueryable<sys_phieu_xuat_kho_chi_tiet_col> query)
        {
            var result = (from d in query.OrderByDescending(d => d.id_phieu_xuat_kho)

                          join mh in _context.sys_mat_hang_col.AsQueryable()
                          on d.id_mat_hang equals mh.id into lmh

                          join dvt in _context.sys_don_vi_tinh_col.AsQueryable()
                          on d.id_don_vi_tinh equals dvt.id into ldvt



                          from dvt in ldvt.DefaultIfEmpty()
                          from mh in lmh.DefaultIfEmpty()
                          select new sys_phieu_xuat_kho_chi_tiet_model
                          {
                              db = d,
                              id_mat_hang = mh.id,
                              ten_don_vi_tinh = dvt.ten,
                              ten_mat_hang = mh.ten,
                              ma_mat_hang = mh.ma,

                          });
            return result;
        }

        public async Task removeOldPhieuAsync(string id_phieu)
        {
            var old_db = _context.sys_phieu_xuat_kho_col.AsQueryable().Where(d => d.id == id_phieu).FirstOrDefault();
            var listOld = _context.sys_phieu_xuat_kho_chi_tiet_col.AsQueryable()
              .Where(d => d.id_phieu_xuat_kho == id_phieu)
              .Select(d => new
              {
                  id_mat_hang = d.id_mat_hang,
                  so_luong = d.so_luong,
                  id_don_vi_tinh = d.id_don_vi_tinh,
                  don_gia = d.don_gia,
              }).ToList();

            for (int i = 0; i < listOld.Count(); i++)
            {
                await _common_trigger_tonkho_repo.updateXuatTonKhoAsync(-listOld[i].so_luong, (-listOld[i].so_luong ?? 0) * (listOld[i].don_gia ?? 0), listOld[i].id_mat_hang, listOld[i].id_don_vi_tinh, old_db.ngay_xuat);
            }
        }
        public async Task<int> update_status_del(string id, string userid, int status_del)
        {

            var update = Builders<sys_phieu_xuat_kho_col>.Update
               .Set(x => x.status_del, status_del)
                .Set(x => x.nguoi_cap_nhat, userid)
                 .Set(x => x.ngay_cap_nhat, DateTime.Now);

            // Create a filter to match the document to update
            if (status_del == 2)
            {
                await removeOldPhieuAsync(id);
                //await _common_trigger_dinh_khoan_repo.removePhieu(id, "sys_phieu_xuat_kho");
            }
            else if (status_del == 1)
            {

            }
            var filter = Builders<sys_phieu_xuat_kho_col>.Filter.Eq(x => x.id, id);
            _context.sys_phieu_xuat_kho_col.UpdateOne(filter, update);
            var filteDetail = Builders<sys_phieu_xuat_kho_chi_tiet_col>.Filter.Eq(x => x.id_phieu_xuat_kho, id);
            var updateDetail = Builders<sys_phieu_xuat_kho_chi_tiet_col>.Update
              .Set(x => x.status_del, status_del)
               .Set(x => x.nguoi_cap_nhat, userid)
                .Set(x => x.ngay_cap_nhat, DateTime.Now);
            _context.sys_phieu_xuat_kho_chi_tiet_col.UpdateMany(filteDetail, updateDetail);

            var query = _context.sys_phieu_xuat_kho_col.AsQueryable().Where(m => m.id == id);
            var model = FindAll(query).SingleOrDefault();

            //var list_detail = _context.sys_phieu_xuat_kho_chi_tiet_col.AsQueryable().Where(q => q.id_phieu_xuat_kho == id.Trim());

            //foreach (var item in list_detail)
            //{
            //    if (model.db.id_don_hang_ban != null)
            //    {
            //        await _common_trigger_don_hang_ban_repo.InsertTriggerDaXuat(model.db.id_don_hang_ban, model.db.nguoi_cap_nhat, item.id_mat_hang);
            //    }
            //    if (model.db.id_don_hang_mua != null)
            //    {
            //        await _common_trigger_don_hang_mua_repo.InsertTriggerDaNhap(model.db.id_don_hang_ban, model.db.nguoi_cap_nhat, item.id_mat_hang);
            //    }
            //}
            return 1;
        }
    }
}
