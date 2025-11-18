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
    public class sys_phieu_nhap_kho_repo
    {
        public MongoDBContext _context;
        public common_mongo_repo _common_repo;
        //public common_trigger_tonkho_repo _common_trigger_tonkho_repo;
        //public common_trigger_dinh_khoan_repo _common_trigger_dinh_khoan_repo;
        //public common_trigger_don_hang_mua_repo _common_trigger_don_hang_mua_repo;
        //public common_trigger_don_hang_ban_repo _common_trigger_don_hang_ban_repo;
        //public Constant contant;

        //       var data = new Constant();
        //var lst = data.list_phuong_thuc_thanh_toan;
        public sys_phieu_nhap_kho_repo(MongoDBContext context)
        {
            _context = context;
            _common_repo = new common_mongo_repo(context);
            //_common_trigger_tonkho_repo = new common_trigger_tonkho_repo(context);
            //_common_trigger_dinh_khoan_repo = new common_trigger_dinh_khoan_repo(context);
            //_common_trigger_don_hang_mua_repo = new common_trigger_don_hang_mua_repo(context);
            //_common_trigger_don_hang_ban_repo = new common_trigger_don_hang_ban_repo(context);
            //contant = new Constant();
        }
        public string getCode()
        {
            var max = "";
            var config = _common_repo.get_code_config(false, "sys_phieu_nhap_kho", "PNK");
            var max_query = _context.sys_phieu_nhap_kho_col.AsQueryable()
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


        public async Task<sys_phieu_nhap_kho_model> getElementById(string id)
        {
            var query = _context.sys_phieu_nhap_kho_col.AsQueryable().Where((m => m.id == id));
            var obj = FindAll(query).SingleOrDefault();

            var queryTableDetail = _context.sys_phieu_nhap_kho_chi_tiet_col.AsQueryable().Where(q => q.id_phieu_nhap_kho == id.Trim());
            obj.list_mat_hang = FindAllDetail(queryTableDetail).AsQueryable().ToList();
            obj.tong_so_luong = obj.list_mat_hang.Sum(q => q.db.so_luong) ?? 0;
            //loai == 1 nhap
            //loai == 2 xuat
            //var ma_loai_xuat = _context.sys_loai_nhap_xuats.AsQueryable().Where(q => q.id == obj.db.id_loai_nhap).Select(q => q.ma).SingleOrDefault();
            //if (obj.db.nguon == 2)
            //{
            //    var don_hang_mua = _context.sys_don_hang_muas.AsQueryable().Where(q => q.id == obj.db.id_don_hang_mua).SingleOrDefault();
            //    obj.loai_giao_dich = don_hang_mua == null ? null : don_hang_mua.loai_giao_dich;
            //    obj.ma_don_hang = don_hang_mua == null ? null : don_hang_mua.ma;
            //    obj.ten_hinh_thuc = don_hang_mua == null ? null : don_hang_mua.hinh_thuc_doi_tuong == 1 ? "Cá nhân" : "Tổ chức";
            //}
            //if (obj.db.nguon == 1)
            //{
            //    var don_hang_ban = _context.sys_don_hang_bans.AsQueryable().Where(q => q.id == obj.db.id_don_hang_ban).SingleOrDefault();
            //    obj.loai_giao_dich = don_hang_ban == null ? null : don_hang_ban.loai_giao_dich;
            //    obj.ma_don_hang = don_hang_ban == null ? null : don_hang_ban.ma;
            //    obj.ten_hinh_thuc = don_hang_ban == null ? null : don_hang_ban.hinh_thuc_doi_tuong == 1 ? "Cá nhân" : "Tổ chức";
            //}
            //obj.db.ngay_nhap = DateTime.Parse(obj.db.ngay_nhap.Value.ToString());
            return obj;
        }
        //public string generate_ten(sys_phieu_nhap_kho_model model)
        //{
        //    var ten = "";
        //    var ngay = model.db.ngay_nhap.Value.ToString("dd/MM/yyyy");
        //    var loai_nhap = _context.sys_loai_nhap_xuats.AsQueryable().Where(q => q.id == model.db.id_loai_nhap).SingleOrDefault();
        //    var dhm = _context.sys_don_hang_muas.AsQueryable().Where(q => q.id == model.db.id_don_hang_mua).SingleOrDefault();
        //    var dhb = _context.sys_don_hang_bans.AsQueryable().Where(q => q.id == model.db.id_don_hang_ban).SingleOrDefault();
        //    //đơn hàng bán
        //    if (loai_nhap.nguon == "1")
        //    {
        //        ten = "Phiếu nhập " + "ngày " + ngay + " - " + ", " + "Đơn hàng bán: " + dhb.ma + ", " + loai_nhap.ten + "(" + loai_nhap.ma + ")";
        //    }
        //    //đơn hàng mua
        //    else if (loai_nhap.nguon == "2")
        //    {
        //        ten = "Phiếu nhập " + kho.ten + "(" + kho.ma + ")" + " ngày " + ngay + " - " + model.db.ten_doi_tuong + "(" + model.db.id_doi_tuong + ")" + ", " + "Đơn hàng mua: " + dhm.ma + ", " + loai_nhap.ten + "(" + loai_nhap.ma + ")";
        //    }
        //    else
        //    {
        //        ten = "Phiếu nhập " + kho.ten + "(" + kho.ma + ")" + " ngày " + ngay + " - " + model.db.ten_doi_tuong + "(" + model.db.id_doi_tuong + ")" + ", " + loai_nhap.ten + "(" + loai_nhap.ma + ")";
        //    }

        //    return ten;
        //}
        
        public async Task<int> insert(sys_phieu_nhap_kho_model model)
        {
            //model.db.ten = generate_ten(model);
            model.db.ten_khong_dau = Regex.Replace(StringFunctions.NonUnicode(HttpUtility.HtmlDecode(model.db.ten ?? "")).ToLower().Normalize(), "<.*?>|&.*?;", String.Empty);
            await _context.sys_phieu_nhap_kho_col.InsertOneAsync(model.db);
            if (model.list_mat_hang.Count() > 0)
            {
                await upset_detail(model);
            }


            //if(model.db.id_don_hang_ban != null)
            //{
            //    for (int i = 0; i < model.list_mat_hang.Count(); i++)
            //    {
            //        var itemNew = model.list_mat_hang[i];
            //        var px = _context.sys_phieu_xuat_khos.AsQueryable().Where(q => q.id_don_hang_ban == model.db.id_don_hang_ban).Select(q => q.id).ToList();
            //        var so_luong_mat_hang_px = _context.sys_phieu_xuat_kho_chi_tiets.AsQueryable()
            //               .Where(q => px.Contains(q.id_phieu_xuat_kho) && q.status_del == 1 && q.id_mat_hang == itemNew.id_mat_hang).Sum(q => q.so_luong) ?? 0;
            //        var pn = _context.sys_phieu_nhap_kho_col.AsQueryable().Where(q => q.id_don_hang_ban == model.db.id_don_hang_ban).Select(q => q.id).ToList();
            //        var so_luong_mat_hang_pn = _context.sys_phieu_nhap_kho_chi_tiet_col.AsQueryable()
            //           .Where(q => pn.Contains(q.id_phieu_nhap_kho) && q.status_del == 1 && q.id_mat_hang == itemNew.id_mat_hang).Sum(q => q.so_luong) ?? 0;
            //        //var so_luong_da_xuat = 

            //        //var tong_so_luong_px = _context.sys_phieu_xuat_kho_chi_tiets.AsQueryable()
            //        //                    .Where(d => d.id_phieu_xuat_kho == model.db.id)
            //        //                    .Where(d => d.status_del == 1)
            //        //                    .Sum(d => d.so_luong) ?? 0;
            //        //var tong_so_luong_pn = _context.sys_phieu_nhap_kho_chi_tiet_col.AsQueryable().Where(d => d.id_phieu_nhap_kho == model.db.id).Where(d => d.status_del == 1).Sum(d => d.so_luong) ?? 0;
            //    }

            //    //var tong_so_luong_dh = _context.sys_don_hang_ban_mat_hangs.AsQueryable().Where(d => d.id_don_hang == model.db.id_don_hang_ban).Where(d => d.status_del == 1).Sum(d => d.so_luong);

            //    //var don_hang = _context.sys_don_hang_bans.AsQueryable().Where(d => d.id == model.db.id_don_hang_ban).Where(d => d.status_del == 1).SingleOrDefault();
            //    //var id_dh = don_hang.id;
            //    //if ((tong_so_luong + tong_so_luong_pn) == tong_so_luong_dh)
            //    //{
            //    //    var update = Builders<sys_don_hang_ban_db>.Update.Set(x => x.da_xuat_du, true);
            //    //    var filter = Builders<sys_don_hang_ban_db>.Filter.Eq(x => x.id, id_dh);
            //    //    await _context.sys_don_hang_bans.UpdateOneAsync(filter, update);
            //    //}
            //    //else
            //    //{
            //    //    var update = Builders<sys_don_hang_ban_db>.Update.Set(x => x.da_xuat_du, false);
            //    //    var filter = Builders<sys_don_hang_ban_db>.Filter.Eq(x => x.id, id_dh);
            //    //    await _context.sys_don_hang_bans.UpdateOneAsync(filter, update);
            //    //}    
            //}
            //if (model.db.id_don_hang_mua != null)
            //{
            //    var tong_so_luong = model.list_mat_hang.Sum(d => d.db.so_luong);
            //    var tong_so_luong_pn = _context.sys_phieu_nhap_kho_chi_tiet_col.AsQueryable().Where(d => d.id_phieu_nhap_kho == model.db.id).Where(d => d.status_del == 1).Sum(d => d.so_luong);
            //    var tong_so_luong_dh = _context.sys_don_hang_mua_mat_hangs.AsQueryable().Where(d => d.id_don_hang == model.db.id_don_hang_mua).Where(d => d.status_del == 1).Sum(d => d.so_luong);

            //    var don_hang = _context.sys_don_hang_muas.AsQueryable().Where(d => d.id == model.db.id_don_hang_mua).Where(d => d.status_del == 1).SingleOrDefault();
            //    var id_dh = don_hang.id;
            //    if ((tong_so_luong + tong_so_luong_pn) == tong_so_luong_dh)
            //    {
            //        var update = Builders<sys_don_hang_mua_db>.Update.Set(x => x.da_nhap_du, true);
            //        var filter = Builders<sys_don_hang_mua_db>.Filter.Eq(x => x.id, id_dh);
            //        await _context.sys_don_hang_muas.UpdateOneAsync(filter, update);
            //    }
            //    else
            //    {
            //        var update = Builders<sys_don_hang_mua_db>.Update.Set(x => x.da_nhap_du, false);
            //        var filter = Builders<sys_don_hang_mua_db>.Filter.Eq(x => x.id, id_dh);
            //        await _context.sys_don_hang_muas.UpdateOneAsync(filter, update);
            //    }
            //}

            return 1;
        }
        //public async Task removeOldPhieuAsync(string id_phieu)
        //{
        //    var old_db = _context.sys_phieu_nhap_kho_col.AsQueryable().Where(d => d.id == id_phieu).FirstOrDefault();

        //    var listOld = _context.sys_phieu_nhap_kho_chi_tiet_col.AsQueryable()
        //      .Where(d => d.id_phieu_nhap_kho == id_phieu)
        //      .Select(d => new
        //      {
        //          id_mat_hang = d.id_mat_hang,
        //          id_don_vi_tinh = d.id_don_vi_tinh,
        //          so_luong = d.so_luong,
        //          gia_tri = d.gia_tri,
        //      }).ToList();

        //    for (int i = 0; i < listOld.Count(); i++)
        //    {
        //        await _common_trigger_tonkho_repo.updateNhapTonKhoAsync(-listOld[i].so_luong, -listOld[i].gia_tri, old_db.id_kho, listOld[i].id_mat_hang, listOld[i].id_don_vi_tinh, old_db.ngay_nhap);
        //    }
        //}
        public async Task<int> upset_detail(sys_phieu_nhap_kho_model model)
        {
            var filter_detail = Builders<sys_phieu_nhap_kho_chi_tiet_col>.Filter.Eq("id_phieu_nhap_kho", model.db.id);
            await _context.sys_phieu_nhap_kho_chi_tiet_col.DeleteManyAsync(filter_detail);
            //await _common_trigger_dinh_khoan_repo.removePhieu(model.db.id, "sys_phieu_nhap_kho");

            for (int i = 0; i < model.list_mat_hang.Count(); i++)
            {
                var data = model.list_mat_hang[i];
                var filter = Builders<sys_phieu_nhap_kho_chi_tiet_col>.Filter.Eq(x => x.id, data.id_deatils_nhap_kho);
                await _context.sys_phieu_nhap_kho_chi_tiet_col.DeleteManyAsync(filter);
                var db = new sys_phieu_nhap_kho_chi_tiet_col();
                var sotutang = 100 + i;
                db.id = model.db.ma + sotutang.ToString();
                db.status_del = 1;
                db.ngay_nhap = model.db.ngay_nhap;
                db.id_phieu_nhap_kho = model.db.id;
                db.id_mat_hang = data.db.id_mat_hang;
                db.id_don_vi_tinh = data.db.id_don_vi_tinh;
                db.so_luong = data.db.so_luong;
                db.ghi_chu = data.db.ghi_chu;
                db.nguoi_cap_nhat = model.db.nguoi_cap_nhat;
                db.ngay_cap_nhat = model.db.ngay_cap_nhat;
                var mat_hang = _context.sys_mat_hang_col.AsQueryable().Where(d => d.id == data.db.id_mat_hang).SingleOrDefault();

                if (mat_hang != null)
                {
                    db.ten_mat_hang = mat_hang.ten;
                    db.id_loai_mat_hang = mat_hang.id_loai_mat_hang;
                }
                await _context.sys_phieu_nhap_kho_chi_tiet_col.InsertOneAsync(db);
                //await _common_trigger_tonkho_repo.updateNhapTonKhoAsync(data.db.so_luong, 0, model.db.id_kho, data.db.id_mat_hang, data.db.id_don_vi_tinh, model.db.ngay_nhap);

                //if (model.db.id_don_hang_ban != null)
                //{
                //    await _common_trigger_don_hang_ban_repo.InsertTriggerDaXuat(model.db.id_don_hang_ban, model.db.nguoi_cap_nhat, db.id_mat_hang);
                //}
                //if (model.db.id_don_hang_mua != null)
                //{
                //    await _common_trigger_don_hang_mua_repo.InsertTriggerDaNhap(model.db.id_don_hang_mua, model.db.nguoi_cap_nhat, db.id_mat_hang);
                //}
            }
            return 1;
        }
        public async Task<int> update(sys_phieu_nhap_kho_model model)
        {
            //model.db.ten = generate_ten(model);
            model.db.ten_khong_dau = Regex.Replace(StringFunctions.NonUnicode(HttpUtility.HtmlDecode(model.db.ten ?? "")).ToLower().Normalize(), "<.*?>|&.*?;", String.Empty);

            var update = Builders<sys_phieu_nhap_kho_col>.Update
                  //.Set(x => x.ma, model.db.ma)
                  //.Set(x => x.loai, model.db.loai)
                  .Set(x => x.ten, model.db.ten)
                  .Set(x => x.ten_khong_dau, model.db.ten_khong_dau)
                  .Set(x => x.nguon, model.db.nguon)
                       .Set(x => x.ngay_nhap, model.db.ngay_nhap)
                           .Set(x => x.id_loai_nhap, model.db.id_loai_nhap)
                             .Set(x => x.nguoi_cap_nhat, model.db.nguoi_cap_nhat)
                               .Set(x => x.ngay_cap_nhat, model.db.ngay_cap_nhat)
                                   .Set(x => x.status_del, model.db.status_del)
                                       .Set(x => x.nguoi_cap_nhat, model.db.nguoi_cap_nhat)
                                        .Set(x => x.ngay_cap_nhat, model.db.ngay_cap_nhat);


            // Create a filter to match the document to update
            var filter = Builders<sys_phieu_nhap_kho_col>.Filter.Eq(x => x.id, model.db.id);


            //await removeOldPhieuAsync(model.db.id);

            // Create an update definition to set the "Name" property to a new value
            await _context.sys_phieu_nhap_kho_col.UpdateOneAsync(filter, update);

            if (model.list_mat_hang.Count() > 0)
            {
                await upset_detail(model);
            }
            return 1;
        }

        public IQueryable<sys_phieu_nhap_kho_model> FindAll(IQueryable<sys_phieu_nhap_kho_col> query)
        {
            //var vat = contant.list_vat.AsQueryable().Select(q => q.name).ToList();
            var result = (from d in query.OrderByDescending(d => d.ngay_nhap)

                          join u in _context.sys_user_col.AsQueryable()
                          on d.nguoi_cap_nhat equals u.id into uG

                          from u in uG.DefaultIfEmpty()

                          select new sys_phieu_nhap_kho_model
                          {
                              db = d,
                              ten_nguoi_cap_nhat = u.ho_va_ten,
                          });
            return result;
        }

        public IQueryable<sys_phieu_nhap_kho_chi_tiet_model> FindAllDetail(IQueryable<sys_phieu_nhap_kho_chi_tiet_col> query)
        {

            var result = (from d in query.OrderByDescending(d => d.id_phieu_nhap_kho)
                          join mh in _context.sys_mat_hang_col.AsQueryable()
                           on d.id_mat_hang equals mh.id into mhG

                          join dvt in _context.sys_don_vi_tinh_col.AsQueryable()
                          on d.id_don_vi_tinh equals dvt.id into dvtG

                          from dvt in dvtG.DefaultIfEmpty()
                          from mh in mhG.DefaultIfEmpty()

                          select new sys_phieu_nhap_kho_chi_tiet_model
                          {
                              db = d,
                              ten_don_vi_tinh = dvt.ten,
                              ten_mat_hang = mh.ten,
                              ma_mat_hang = mh.ma,
                              id_mat_hang = mh.id,
                              id_deatils_nhap_kho = d.id,
                          });
            return result;
        }
        //public IQueryable<sys_phieu_nhap_kho_log_model> FindAllLog(IQueryable<sys_phieu_nhap_kho_log_db> query)
        //{
        //    //var vat = contant.list_vat.AsQueryable().Select(q => q.name).ToList();
        //    var result = (from d in query.OrderByDescending(d => d.ngay_nhap)

        //                  join u in _context.Users.AsQueryable()
        //                  on d.nguoi_cap_nhat equals u.Id into uG

        //                  join tk in _context.sys_khos.AsQueryable()
        //                  on d.id_kho equals tk.id into tkG

        //                  join ln in _context.sys_loai_nhap_xuats.AsQueryable()
        //                  on d.id_loai_nhap equals ln.id into lnG

        //                  join pck in _context.sys_phieu_chuyen_khos.AsQueryable()
        //                  on d.id_phieu_chuyen_kho equals pck.id into pckG

        //                  from tk in tkG.DefaultIfEmpty()
        //                  from u in uG.DefaultIfEmpty()
        //                  from ln in lnG.DefaultIfEmpty()
        //                  from pck in pckG.DefaultIfEmpty()

        //                  select new sys_phieu_nhap_kho_log_model
        //                  {
        //                      db = d,
        //                      ten_nguoi_cap_nhat = u.full_name,
        //                      ten_loai_nhap = ln.ten,
        //                      ten_kho = tk.ten,
        //                      ma_chuyen_kho = pck.ma,
        //                      is_sinh_tu_dong = d.ma.StartsWith("PNDHM") ? true : false
        //                  });
        //    return result;
        //}

        //public IQueryable<sys_phieu_nhap_kho_chi_tiet_log_model> FindAllLogDetail(IQueryable<sys_phieu_nhap_kho_chi_tiet_log_db> query)
        //{

        //    var result = (from d in query.OrderByDescending(d => d.id_phieu_nhap_kho)
        //                  join mh in _context.sys_mat_hang_col.AsQueryable()
        //                   on d.id_mat_hang equals mh.id into mhG

        //                  join dvt in _context.sys_don_vi_tinh_col.AsQueryable()
        //                  on d.id_don_vi_tinh equals dvt.id into dvtG

        //                  from dvt in dvtG.DefaultIfEmpty()
        //                  from mh in mhG.DefaultIfEmpty()

        //                  select new sys_phieu_nhap_kho_chi_tiet_log_model
        //                  {
        //                      db = d,
        //                      ten_don_vi_tinh = dvt.ten,
        //                      ten_mat_hang = mh.ten,
        //                      ma_mat_hang = mh.ma,
        //                      id_mat_hang = mh.id,
        //                      id_deatils_nhap_kho = d.id,
        //                  });
        //    return result;
        //}

        //public IQueryable<sys_phieu_nhap_kho_chi_tiet_ref_model> FindAllDetailMatHang(IQueryable<sys_phieu_nhap_kho_chi_tiet_col> query)
        //{
        //    var result = (from d in query.OrderByDescending(d => d.id_phieu_nhap_kho)

        //                  join mh in _context.sys_mat_hang_col.AsQueryable()
        //                  on d.id_mat_hang equals mh.id into lmh

        //                  join dvt in _context.sys_don_vi_tinh_col.AsQueryable()
        //                  on d.id_don_vi_tinh equals dvt.id into ldvt



        //                  from dvt in ldvt.DefaultIfEmpty()
        //                  from mh in lmh.DefaultIfEmpty()
        //                  select new sys_phieu_nhap_kho_chi_tiet_ref_model
        //                  {
        //                      id = d.id,
        //                      id_phieu_nhap_kho = d.id_phieu_nhap_kho,
        //                      id_mat_hang = mh.id,
        //                      ten_don_vi_tinh = dvt.ten,
        //                      ten_mat_hang = mh.ten,
        //                      ma_mat_hang = mh.ma,
        //                      don_gia = mh.gia_ban_si,
        //                      so_luong = d.so_luong
        //                  });
        //    return result;
        //}

        public async Task<int> update_status_del(string id, string userid, int status_del)
        {

            var update = Builders<sys_phieu_nhap_kho_col>.Update
               .Set(x => x.status_del, status_del)
                .Set(x => x.nguoi_cap_nhat, userid)
                 .Set(x => x.ngay_cap_nhat, DateTime.Now);

            // Create a filter to match the document to update

            if (status_del == 2)
            {
                //await removeOldPhieuAsync(id);
                //await _common_trigger_dinh_khoan_repo.removePhieu(id, "sys_phieu_nhap_kho");
            }
            else if (status_del == 1)
            {

            }
            var filter = Builders<sys_phieu_nhap_kho_col>.Filter.Eq(x => x.id, id);
            _context.sys_phieu_nhap_kho_col.UpdateOne(filter, update);
            var filteDetail = Builders<sys_phieu_nhap_kho_chi_tiet_col>.Filter.Eq(x => x.id_phieu_nhap_kho, id);
            var updateDetail = Builders<sys_phieu_nhap_kho_chi_tiet_col>.Update
              .Set(x => x.status_del, status_del)
               .Set(x => x.nguoi_cap_nhat, userid)
                .Set(x => x.ngay_cap_nhat, DateTime.Now);
            _context.sys_phieu_nhap_kho_chi_tiet_col.UpdateMany(filteDetail, updateDetail);



            var query = _context.sys_phieu_nhap_kho_col.AsQueryable().Where(m => m.id == id);
            var model = FindAll(query).SingleOrDefault();

            var list_detail = _context.sys_phieu_nhap_kho_chi_tiet_col.AsQueryable().Where(q => q.id_phieu_nhap_kho == id.Trim());

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
