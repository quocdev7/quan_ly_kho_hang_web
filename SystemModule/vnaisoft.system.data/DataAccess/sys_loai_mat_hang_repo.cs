using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using System;
using System.Linq;
using System.Threading.Tasks;
using vnaisoft.common.Common;
using vnaisoft.DataBase.commonFunc;
using vnaisoft.DataBase.Mongodb;
using vnaisoft.DataBase.Mongodb.Collection.system;
using vnaisoft.system.data.Models;

namespace vnaisoft.system.data.DataAccess
{
    public class sys_loai_mat_hang_repo
    {
        //public vnaisoftDefautContext _context;
        public MongoDBContext _context;
        public common_mongo_repo _common_repo;
        private IMailService _mailService;
        public sys_loai_mat_hang_repo(MongoDBContext context)
        {
            _context = context;
            _common_repo = new common_mongo_repo(context);

        }
        public string getCode()
        {
            var max = "";
            var config = _common_repo.get_code_config(true, "sys_loai_mat_hang", "LMH");
            var max_query = _context.sys_loai_mat_hang_col.AsQueryable()
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

        public async Task<sys_loai_mat_hang_model> getElementById(string id)
        {
            var query = _context.sys_loai_mat_hang_col.AsQueryable().Where(m => m.id == id);
            var obj = await FindAll(query).FirstOrDefaultAsync();
            return obj;
        }
        public async Task<int> insert(sys_loai_mat_hang_model model)
        {
            await _context.sys_loai_mat_hang_col.InsertOneAsync(model.db);

            return 1;
        }

        public async Task<int> update(sys_loai_mat_hang_model model)
        {
            var update = Builders<sys_loai_mat_hang_col>.Update
                    //.Set(x => x.ma, model.db.ma)
                    .Set(x => x.ten, model.db.ten)
                     .Set(x => x.ghi_chu, model.db.ghi_chu)
                      .Set(x => x.nguoi_cap_nhat, model.db.nguoi_cap_nhat)
                       .Set(x => x.ngay_cap_nhat, model.db.ngay_cap_nhat);
            // Create a filter to match the document to update
            var filter = Builders<sys_loai_mat_hang_col>.Filter.Eq(x => x.id, model.db.id);

            // Create an update definition to set the "Name" property to a new value

            await _context.sys_loai_mat_hang_col.UpdateOneAsync(filter, update);

            return 1;
        }

        public IQueryable<sys_loai_mat_hang_model> FindAll(IQueryable<sys_loai_mat_hang_col> query)
        {

            var result = (from d in query.OrderByDescending(d => d.ma)

                          join u in _context.sys_user_col.AsQueryable()
                         on d.nguoi_cap_nhat equals u.id into lu
                          from user in lu.DefaultIfEmpty()
                          select new sys_loai_mat_hang_model
                          {
                              db = d,
                              ten_nguoi_cap_nhat = user.ho_va_ten,
                          });
            return result;

        }
        public int update_status_del(string id, string userid, int status_del)
        {

            var update = Builders<sys_loai_mat_hang_col>.Update
               .Set(x => x.status_del, status_del)
                .Set(x => x.nguoi_cap_nhat, userid)
                 .Set(x => x.ngay_cap_nhat, DateTime.Now);

            // Create a filter to match the document to update
            var filter = Builders<sys_loai_mat_hang_col>.Filter.Eq(x => x.id, id);
            _context.sys_loai_mat_hang_col.UpdateOne(filter, update);
            return 1;
        }


    }
}
