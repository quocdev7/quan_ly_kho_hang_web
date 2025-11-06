using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using System;
using System.Linq;
using System.Threading.Tasks;
using vnaisoft.common.Common;
using vnaisoft.DataBase.commonFunc;
using vnaisoft.DataBase.Mongodb;
using vnaisoft.DataBase.System;
using vnaisoft.system.data.Models;

namespace vnaisoft.system.data.DataAccess
{
    public class sys_user_repo
    {
        //public vnaisoftDefautContext _context;
        public MongoDBContext _context;
        public common_mongo_repo _common_repo;
        private IMailService _mailService;
        public sys_user_repo(MongoDBContext context)
        {
            _context = context;
            _common_repo = new common_mongo_repo(context);

        }



        public async Task<sys_user_model> getElementById(string id)
        {

            var obj = await FindAll().FirstOrDefaultAsync(m => m.id == id);
            return obj;
        }


        public async Task<int> insert(sys_user_model model)
        {

            await _context.sys_user_col.InsertOneAsync(model.db);

            return 1;
        }
        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public async Task<int> update(sys_user_model model)
        {

            var update = Builders<User>.Update
              .Set(x => x.email, model.db.email)
              .Set(x => x.so_dien_thoai, model.db.so_dien_thoai)
              .Set(x => x.hinh_anh_dai_dien, model.db.hinh_anh_dai_dien)
              .Set(x => x.ho_va_ten, model.db.ho_va_ten)
              .Set(x => x.Username, model.db.Username)

              .Set(x => x.ngay_cap_nhat, model.db.ngay_cap_nhat)
              .Set(x => x.nguoi_cap_nhat, model.db.nguoi_cap_nhat)
              .Set(x => x.status_del, 1);

            var filter = Builders<User>.Filter.Eq(q => q.id, model.db.id);
            await _context.sys_user_col.UpdateOneAsync(filter, update);

            return 1;
        }

        public IQueryable<sys_user_model> FindAll()
        {
            var result = (from u in _context.sys_user_col.AsQueryable()


                          join us in _context.sys_user_col.AsQueryable()
                         on u.nguoi_cap_nhat equals us.id into usG


                          from us in usG.DefaultIfEmpty()
                          select new sys_user_model
                          {
                              db = u,
                              id = u.id,
                              nguoi_cap_nhat = us.ho_va_ten,
                              ho_va_ten = u.ho_va_ten,
                              email = u.email,
                              phone = u.so_dien_thoai,
                              nguoi_tao = us.ho_va_ten,
                              Username = u.Username,
                              hinh_anh_dai_dien = u.hinh_anh_dai_dien ?? "assets/images/logo/logo.png",
                          });
            return result;
        }


        public async Task<int> update_status_del(string id, string userid, int status_del)
        {

            var update = Builders<User>.Update
               .Set(x => x.status_del, status_del)
               .Set(x => x.nguoi_cap_nhat, userid)
               .Set(x => x.ngay_cap_nhat, DateTime.Now);

            // Create a filter to match the document to update
            var filter = Builders<User>.Filter.Eq(x => x.id, id);
            _context.sys_user_col.UpdateOne(filter, update);



            return 1;
        }
        public async Task<int> delete(string id)
        {
            var filter = Builders<User>.Filter.Eq(x => x.id, id);
            _context.sys_user_col.DeleteOne(filter);
            return 1;
        }
        public async Task<int> updatePassword(sys_user_model model)
        {
            if (!string.IsNullOrWhiteSpace(model.password))
            {

                var update = Builders<User>.Update
                  .Set(x => x.PasswordHash, model.db.PasswordHash)
                  .Set(x => x.PasswordSalt, model.db.PasswordSalt)
                   ;
                // Create a filter to match the document to update
                var filter = Builders<User>.Filter.Eq(x => x.id, model.db.id);
                _context.sys_user_col.UpdateOne(filter, update);
            }

            return 1;


        }


    }
}
