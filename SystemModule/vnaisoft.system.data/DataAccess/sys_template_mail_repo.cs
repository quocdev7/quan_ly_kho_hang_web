using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using System;
using System.Linq;
using System.Threading.Tasks;
using vnaisoft.DataBase.commonFunc;
using vnaisoft.DataBase.Mongodb;
using vnaisoft.DataBase.Mongodb.Collection.system;
using vnaisoft.DataBase.System;
using vnaisoft.system.data.Models;

namespace vnaisoft.system.data.DataAccess
{
    public class sys_template_mail_repo
    {
        //public vnaisoftDefautContext _context;
        public MongoDBContext _context;
        public common_mongo_repo _common_repo;

        public sys_template_mail_repo(MongoDBContext context)
        {
            _context = context;
            _common_repo = new common_mongo_repo(context);
        }

        public async Task<sys_template_mail_model> getElementById(string id)
        {
            var obj = await FindAll().FirstOrDefaultAsync(m => m.db.id == id);
            return obj;
        }

        public async Task<int> insert(sys_template_mail_model model)
        {
            model.db.nguoi_cap_nhat = model.db.nguoi_cap_nhat;
            model.db.ngay_cap_nhat = model.db.ngay_cap_nhat;
            //await _context.sys_template_mail_col.AddAsync(model.db);
            //_context.SaveChanges();
            await _context.sys_template_mail_col.InsertOneAsync(model.db);
            return 1;
        }

        public async Task<int> update(sys_template_mail_model model)
        {
            //var db = await _context.sys_template_mail_col.Where(d => d.id == model.db.id).FirstOrDefaultAsync();
            //db.name = model.db.name;
            //db.template = model.db.template;
            //db.id_type = model.db.id_type;
            //db.id_nhom_mail = model.db.id_nhom_mail;
            //db.update_by = model.db.update_by;
            //db.update_date = model.db.update_date;
            //db.status_del = model.db.status_del;
            //_context.SaveChanges();
            //return 1;
            var update = Builders<sys_template_mail_col>.Update
               .Set(x => x.name, model.db.name)
               .Set(x => x.template, model.db.template)
                .Set(x => x.nguoi_cap_nhat, model.db.nguoi_cap_nhat)
                 .Set(x => x.ngay_cap_nhat, model.db.ngay_cap_nhat)
                  .Set(x => x.status_del, model.db.status_del)
                  ;

            // Create a filter to match the document to update
            var filter = Builders<sys_template_mail_col>.Filter.Eq(x => x.id, model.db.id);

            // Create an update definition to set the "Name" property to a new value

            await _context.sys_template_mail_col.UpdateOneAsync(filter, update);
            return 1;
        }

        public IQueryable<sys_template_mail_model> FindAll()
        {
            var result = (from q in _context.sys_template_mail_col.AsQueryable()

                          join u in _context.sys_user_col.AsQueryable()
                          on q.nguoi_cap_nhat equals u.id into uG
                          from u in uG.DefaultIfEmpty()

                          select new sys_template_mail_model
                          {
                              db = q,
                              nguoi_cap_nhat = u.ho_va_ten,
                          });
            return result;
        }
        public async Task<int> update_status_del(string id, string userid, int status_del)
        {
            var update = Builders<sys_template_mail_col>.Update
               .Set(x => x.status_del, status_del)
                .Set(x => x.nguoi_cap_nhat, userid)
                 .Set(x => x.ngay_cap_nhat, DateTime.Now);
            // Create a filter to match the document to update
            var filter = Builders<sys_template_mail_col>.Filter.Eq(x => x.id, id);
            await _context.sys_template_mail_col.UpdateOneAsync(filter, update);
            return 1;
        }
    }
}
