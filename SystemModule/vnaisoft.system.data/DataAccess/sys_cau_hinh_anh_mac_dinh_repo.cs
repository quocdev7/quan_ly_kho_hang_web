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
    public class sys_cau_hinh_anh_mac_dinh_repo
    {
        //public vnaisoftDefautContext _context;
        public MongoDBContext _context;
        public common_mongo_repo _common_repo;
        private IMailService _mailService;
        public sys_cau_hinh_anh_mac_dinh_repo(MongoDBContext context)
        {
            _context = context;
            _common_repo = new common_mongo_repo(context);

        }



        public async Task<sys_cau_hinh_anh_mac_dinh_model> getElementById(string id)
        {

            var obj = await FindAll().FirstOrDefaultAsync(m => m.db.id == id);



            return obj;
        }
        public async Task<int> insert(sys_cau_hinh_anh_mac_dinh_model model)
        {

            await _context.sys_cau_hinh_anh_mac_dinh_col.InsertOneAsync(model.db);

            return 1;
        }

        public async Task<int> update(sys_cau_hinh_anh_mac_dinh_model model)
        {

            var update = Builders<sys_cau_hinh_anh_mac_dinh_col>.Update
              .Set(x => x.type, model.db.type)
              .Set(x => x.image, model.db.image)
              .Set(x => x.avatar, model.db.avatar)

              .Set(x => x.ngay_cap_nhat, model.db.ngay_cap_nhat)
              .Set(x => x.ngay_cap_nhat, model.db.ngay_cap_nhat)
              ;
            var filter = Builders<sys_cau_hinh_anh_mac_dinh_col>.Filter.Eq(q => q.id, model.db.id);
            await _context.sys_cau_hinh_anh_mac_dinh_col.UpdateOneAsync(filter, update);
            return 1;
        }

        public IQueryable<sys_cau_hinh_anh_mac_dinh_model> FindAll()
        {
            var result = (from u in _context.sys_cau_hinh_anh_mac_dinh_col.AsQueryable()


                          join us in _context.sys_cau_hinh_anh_mac_dinh_col.AsQueryable()
                         on u.nguoi_cap_nhat equals us.id into usG


                          from us in usG.DefaultIfEmpty()
                          select new sys_cau_hinh_anh_mac_dinh_model
                          {
                              db = u,
                              nguoi_cap_nhat = us.nguoi_cap_nhat

                          });
            return result;
        }


        public int update_status_del(string id, string userid, int status_del)
        {

            var update = Builders<sys_cau_hinh_anh_mac_dinh_col>.Update
               .Set(x => x.status_del, status_del)
               .Set(x => x.nguoi_cap_nhat, userid)
               .Set(x => x.ngay_cap_nhat, DateTime.Now);

            // Create a filter to match the document to update
            var filter = Builders<sys_cau_hinh_anh_mac_dinh_col>.Filter.Eq(x => x.id, id);
            _context.sys_cau_hinh_anh_mac_dinh_col.UpdateOne(filter, update);
            return 1;
        }



    }
}
