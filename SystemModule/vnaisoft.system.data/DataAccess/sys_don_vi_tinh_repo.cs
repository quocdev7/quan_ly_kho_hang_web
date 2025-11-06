using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using System;
using System.Linq;
using System.Threading.Tasks;
using vnaisoft.DataBase.commonFunc;
using vnaisoft.DataBase.Mongodb;
using vnaisoft.DataBase.Mongodb.Collection.system;
using vnaisoft.system.data.Models;

namespace vnaisoft.system.data.DataAccess
{
    public class sys_don_vi_tinh_repo
    {
        public MongoDBContext _context;
        public common_mongo_repo _common_repo;

        public sys_don_vi_tinh_repo(MongoDBContext context)
        {
            _context = context;
            _common_repo = new common_mongo_repo(context);
        }
        public string getCode()
        {
            var max = "";
            var config = _common_repo.get_code_config(true, "sys_don_vi_tinh", "DVT");
            var max_query = _context.sys_don_vi_tinh_col.AsQueryable()
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

        public async Task<sys_don_vi_tinh_model> getElementById(string id)
        {
            var obj = await FindAll().FirstOrDefaultAsync(m => m.db.id == id);
            return obj;
        }

        public async Task<int> insert(sys_don_vi_tinh_model model)
        {
            await _context.sys_don_vi_tinh_col.InsertOneAsync(model.db);
            return 1;
        }


        public async Task<int> update(sys_don_vi_tinh_model model)
        {

            var update = Builders<sys_don_vi_tinh_col>.Update
                .Set(x => x.ma, model.db.ma)
                 .Set(x => x.ten, model.db.ten)
                  .Set(x => x.ghi_chu, model.db.ghi_chu)
                   .Set(x => x.nguoi_cap_nhat, model.db.nguoi_cap_nhat)
                    .Set(x => x.ngay_cap_nhat, model.db.ngay_cap_nhat);


            // Create a filter to match the document to update
            var filter = Builders<sys_don_vi_tinh_col>.Filter.Eq(x => x.id, model.db.id);

            // Create an update definition to set the "Name" property to a new value

            await _context.sys_don_vi_tinh_col.UpdateOneAsync(filter, update);
            return 1;
        }


        public IQueryable<sys_don_vi_tinh_model> FindAll()
        {
            var result = (from d in _context.sys_don_vi_tinh_col.AsQueryable()

                          join u in _context.sys_user_col.AsQueryable()
                         on d.nguoi_cap_nhat equals u.id into lu
                          from user in lu.DefaultIfEmpty()
                          select new sys_don_vi_tinh_model
                          {
                              db = d,
                              nguoi_cap_nhat = user.ho_va_ten,

                          });
            return result;
        }

        public int delete(string id)
        {
            var filter = Builders<sys_don_vi_tinh_col>.Filter.Eq(x => x.id, id);
            _context.sys_don_vi_tinh_col.DeleteOne(filter);
            return 1;
        }

        public int update_status_del(string id, string userid, int status_del)
        {

            var update = Builders<sys_don_vi_tinh_col>.Update
               .Set(x => x.status_del, status_del)
                .Set(x => x.nguoi_cap_nhat, userid)
                 .Set(x => x.ngay_cap_nhat, DateTime.Now);

            // Create a filter to match the document to update
            var filter = Builders<sys_don_vi_tinh_col>.Filter.Eq(x => x.id, id);
            _context.sys_don_vi_tinh_col.UpdateOne(filter, update);
            return 1;
        }
    }
}
