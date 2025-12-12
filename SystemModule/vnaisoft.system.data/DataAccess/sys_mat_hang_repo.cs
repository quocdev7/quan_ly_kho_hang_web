using Microsoft.EntityFrameworkCore;
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
    public class sys_mat_hang_repo
    {
        public MongoDBContext _context;
        public common_mongo_repo _common_repo;

        public sys_mat_hang_repo(MongoDBContext context)
        {
            _context = context;
            _common_repo = new common_mongo_repo(context);
        }
        public string getCode()
        {
            var max = "";
            var config = _common_repo.get_code_config(true, "sys_mat_hang", "MH");
            var max_query = _context.sys_mat_hang_col.AsQueryable()
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


        public async Task<sys_mat_hang_model> getElementById(string id)
        {
            var query = _context.sys_mat_hang_col.AsQueryable();
            var obj = await FindAll(query).FirstOrDefaultAsync(m => m.db.id == id);


            return obj;
        }
        public async Task<int> insert(sys_mat_hang_model model)
        {
            model.db.ten_khong_dau = Regex.Replace(StringFunctions.NonUnicode(HttpUtility.HtmlDecode(model.db.ten ?? "")).ToLower().Normalize(), "<.*?>|&.*?;", string.Empty);

            await _context.sys_mat_hang_col.InsertOneAsync(model.db);


            return 1;
        }

        public async Task<int> update(sys_mat_hang_model model)
        {
            model.db.ten_khong_dau = Regex.Replace(StringFunctions.NonUnicode(HttpUtility.HtmlDecode(model.db.ten ?? "")).ToLower().Normalize(), "<.*?>|&.*?;", string.Empty);

            var update = Builders<sys_mat_hang_col>.Update
            //.Set(x => x.ma, model.db.ma)
                    .Set(x => x.ten, model.db.ten)
                    .Set(x => x.ten_khong_dau, model.db.ten_khong_dau)
                     .Set(x => x.id_loai_mat_hang, model.db.id_loai_mat_hang)
                       .Set(x => x.id_don_vi_tinh, model.db.id_don_vi_tinh)
                         .Set(x => x.gia_ban_le, model.db.gia_ban_le)
                           .Set(x => x.gia_ban_si, model.db.gia_ban_si)
                               .Set(x => x.vat, model.db.vat)
                                   .Set(x => x.ghi_chu, model.db.ghi_chu)
                                       .Set(x => x.nguoi_cap_nhat, model.db.nguoi_cap_nhat)
                                        .Set(x => x.ngay_cap_nhat, model.db.ngay_cap_nhat);

            // Create a filter to match the document to update
            var filter = Builders<sys_mat_hang_col>.Filter.Eq(x => x.id, model.db.id);
            // Create an update definition to set the "Name" property to a new value
            await _context.sys_mat_hang_col.UpdateOneAsync(filter, update);


            return 1;
        }

        public IQueryable<sys_mat_hang_model> FindAll(IQueryable<sys_mat_hang_col> query)
        {

            var result = from d in query.OrderByDescending(d => d.ma)
                         join u in _context.sys_user_col.AsQueryable()
                        on d.nguoi_cap_nhat equals u.id into uG


                         join lmh in _context.sys_loai_mat_hang_col.AsQueryable()
                      on d.id_loai_mat_hang equals lmh.id into lmhG

                         join dvt in _context.sys_don_vi_tinh_col.AsQueryable()
                         on d.id_don_vi_tinh equals dvt.id into dvtG

                         from u in uG.DefaultIfEmpty()
                         from lmh in lmhG.DefaultIfEmpty()
                         from dvt in dvtG.DefaultIfEmpty()

                         select new sys_mat_hang_model
                         {
                             db = d,
                             nguoi_cap_nhat = u.ho_va_ten,
                         };
            return result;
        }

        public int delete(string id)
        {
            var filter = Builders<sys_mat_hang_col>.Filter.Eq(x => x.id, id);
            _context.sys_mat_hang_col.DeleteOne(filter);
            return 1;
        }

        public int update_status_del(string id, string userid, int status_del)
        {

            var update = Builders<sys_mat_hang_col>.Update
               .Set(x => x.status_del, status_del)
                .Set(x => x.nguoi_cap_nhat, userid)
                 .Set(x => x.ngay_cap_nhat, DateTime.Now);

            // Create a filter to match the document to update
            var filter = Builders<sys_mat_hang_col>.Filter.Eq(x => x.id, id);
            _context.sys_mat_hang_col.UpdateOne(filter, update);
            return 1;
        }
    }
}
