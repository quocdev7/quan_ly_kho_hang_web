using MongoDB.Driver;
using quan_ly_kho.common.Helpers;
using quan_ly_kho.DataBase.common;
using quan_ly_kho.DataBase.Mongodb;
using quan_ly_kho.DataBase.Mongodb.Collection.system;
using quan_ly_kho.system.data.Models;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace quan_ly_kho.system.data.DataAccess
{
    public class sys_don_hang_ban_repo
    {
        public MongoDBContext _context;
        public common_mongo_repo _common_repo;

        public sys_don_hang_ban_repo(MongoDBContext context)
        {
            _context = context;
            _common_repo = new common_mongo_repo(context);
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
        public async Task<sys_don_hang_ban_model> getElementById(string id)
        {
            var query = _context.sys_don_hang_ban_col.AsQueryable().Where(m => m.id == id);
            var obj = FindAll(query).SingleOrDefault();

            var queryTableDetail = _context.sys_don_hang_ban_mat_hang_col.AsQueryable().Where(q => q.id_don_hang == id.Trim());
            obj.list_mat_hang = FindAllDetail(queryTableDetail).ToList();

            return obj;
        }
        public async Task<int> insert(sys_don_hang_ban_model model)
        {
            //tinhTongTien(model);
            //model.db.ten = generate_ten(model);
            model.db.ten_khong_dau = Regex.Replace(StringFunctions.NonUnicode(HttpUtility.HtmlDecode(model.db.ten ?? "")).ToLower().Normalize(), "<.*?>|&.*?;", string.Empty);
            //model.db.id_file_upload = model.db.id + "sys_don_hang_ban";
            await _context.sys_don_hang_ban_col.InsertOneAsync(model.db);
            await upset_detail(model);

            return 1;
        }


        public async Task<int> upset_detail(sys_don_hang_ban_model model)
        {
            var filter = Builders<sys_don_hang_ban_mat_hang_col>.Filter.Eq(x => x.id_don_hang, model.db.id);
            await _context.sys_don_hang_ban_mat_hang_col.DeleteManyAsync(filter);
            for (int i = 0; i < model.list_mat_hang.Count(); i++)
            {
                var mat_hang = _context.sys_mat_hang_col.AsQueryable().Where(d => d.id == model.list_mat_hang[i].db.id_mat_hang).SingleOrDefault();
                var sotutang = 100 + i;
                model.list_mat_hang[i].db.id = model.db.ma + sotutang.ToString();
                model.list_mat_hang[i].db.ngay_dat_hang = model.db.ngay_dat_hang;
                model.list_mat_hang[i].db.id_don_hang = model.db.id;
                model.list_mat_hang[i].db.nguoi_cap_nhat = model.db.nguoi_cap_nhat;
                model.list_mat_hang[i].db.ngay_cap_nhat = DateTime.Now;
                model.list_mat_hang[i].db.status_del = 1;
                model.list_mat_hang[i].db.ten_mat_hang = mat_hang.ten;
                model.list_mat_hang[i].db.id_loai_mat_hang = mat_hang.id_loai_mat_hang;
                await _context.sys_don_hang_ban_mat_hang_col.InsertOneAsync(model.list_mat_hang[i].db);
            }
            return 1;
        }

        public async Task<int> update(sys_don_hang_ban_model model)
        {
            //tinhTongTien(model);
            // model.db.ten = generate_ten(model);
            model.db.ten_khong_dau = Regex.Replace(StringFunctions.NonUnicode(HttpUtility.HtmlDecode(model.db.ten ?? "")).ToLower().Normalize(), "<.*?>|&.*?;", string.Empty);
            var update = Builders<sys_don_hang_ban_col>.Update
                .Set(x => x.ma, model.db.ma)
                .Set(x => x.ten, model.db.ten)
                .Set(x => x.ten_khong_dau, model.db.ten_khong_dau)
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
            return 1;
        }
        public IQueryable<sys_don_hang_ban_mat_hang_model> FindAllDetail(IQueryable<sys_don_hang_ban_mat_hang_col> query)
        {

            var result = from d in query.OrderByDescending(d => d.id_don_hang)

                         join mh in _context.sys_mat_hang_col.AsQueryable()
                      on d.id_mat_hang equals mh.id into mhg

                         join dvt in _context.sys_don_vi_tinh_col.AsQueryable()
                         on d.id_don_vi_tinh equals dvt.id into dvtg

                         from mh in mhg.DefaultIfEmpty()
                         from dvt in dvtg.DefaultIfEmpty()
                         select new sys_don_hang_ban_mat_hang_model
                         {
                             db = d,
                             ten_don_vi_tinh = dvt.ten,
                             ten_mat_hang = mh.ten,
                         };
            return result;

        }


        public IQueryable<sys_don_hang_ban_model> FindAll(IQueryable<sys_don_hang_ban_col> query)
        {
            var result = from d in query.OrderByDescending(d => d.ngay_dat_hang)

                         join u in _context.sys_user_col.AsQueryable()
                       on d.nguoi_cap_nhat equals u.id into uG

                         from u in uG.DefaultIfEmpty()

                         select new sys_don_hang_ban_model
                         {
                             db = d,
                             ten_nguoi_cap_nhat = u.ho_va_ten,
                         };



            return result;
        }
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

    }
}
