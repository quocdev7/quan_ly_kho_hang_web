using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using quan_ly_kho.DataBase.common;
using quan_ly_kho.DataBase.Mongodb;
using quan_ly_kho.DataBase.Mongodb.Collection.system;
using quan_ly_kho.system.data.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace quan_ly_kho.system.data.DataAccess
{
    public class sys_loai_nhap_xuat_repo
    {
        public MongoDBContext _context;
        public common_mongo_repo _common_repo;

        public sys_loai_nhap_xuat_repo(MongoDBContext context)
        {
            _context = context;
            _common_repo = new common_mongo_repo(context);

        }

        public async Task<sys_loai_nhap_xuat_model> getElementById(string id)
        {
            var obj = await FindAll().FirstOrDefaultAsync(m => m.db.id == id);
            return obj;
        }
        public async Task<int> insert(sys_loai_nhap_xuat_model model)
        {
            await _context.sys_loai_nhap_xuat_col.InsertOneAsync(model.db);

            return 1;
        }

        public async Task<int> update(sys_loai_nhap_xuat_model model)
        {
            var update = Builders<sys_loai_nhap_xuat_col>.Update
                 .Set(x => x.ma, model.db.ma)
                    .Set(x => x.ten, model.db.ten)
                    .Set(x => x.loai, model.db.loai)
                    .Set(x => x.ghi_chu, model.db.ghi_chu)
                    .Set(x => x.nguoi_cap_nhat, model.db.nguoi_cap_nhat)
                    .Set(x => x.ngay_cap_nhat, model.db.ngay_cap_nhat);
            // Create a filter to match the document to update
            var filter = Builders<sys_loai_nhap_xuat_col>.Filter.Eq(x => x.id, model.db.id);

            // Create an update definition to set the "Name" property to a new value

            await _context.sys_loai_nhap_xuat_col.UpdateOneAsync(filter, update);

            return 1;
        }

        public IQueryable<sys_loai_nhap_xuat_model> FindAll()
        {

            var result = from d in _context.sys_loai_nhap_xuat_col.AsQueryable()

                         join u in _context.sys_user_col.AsQueryable()
                        on d.nguoi_cap_nhat equals u.id into lu
                         from user in lu.DefaultIfEmpty()
                         select new sys_loai_nhap_xuat_model
                         {
                             db = d,
                             ten_nguoi_cap_nhat = user.ho_va_ten,
                         };
            return result;

        }
        public int update_status_del(string id, string userid, int status_del)
        {

            var update = Builders<sys_loai_nhap_xuat_col>.Update
               .Set(x => x.status_del, status_del)
                .Set(x => x.nguoi_cap_nhat, userid)
                 .Set(x => x.ngay_cap_nhat, DateTime.Now);

            // Create a filter to match the document to update
            var filter = Builders<sys_loai_nhap_xuat_col>.Filter.Eq(x => x.id, id);
            _context.sys_loai_nhap_xuat_col.UpdateOne(filter, update);
            return 1;
        }


    }
}
