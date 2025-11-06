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
    public class sys_cau_hinh_ma_he_thong_repo
    {
        public MongoDBContext _context;

        public sys_cau_hinh_ma_he_thong_repo(MongoDBContext context)
        {
            _context = context;
        }

        public async Task<sys_cau_hinh_ma_he_thong_model> getElementById(string id)
        {
            var obj = await FindAll().FirstOrDefaultAsync(m => m.db.id == id);
            return obj;
        }

        public async Task<int> save(sys_cau_hinh_ma_he_thong_model model, int action)
        {
            var db = new sys_cau_hinh_ma_he_thong_model();
            if (action == 1)
            {
                await insert(model);
            }
            else
            {
                await update(model);
            }
            return 1;
        }
        public async Task<int> insert(sys_cau_hinh_ma_he_thong_model model)
        {
            //is_ngay_gio = false > dd_MM_yy-

            model.db.controller = model.db.controller;
            model.db.tien_to = model.db.tien_to;
            model.db.so_chu_so_tu_tang = model.db.so_chu_so_tu_tang;
            model.db.is_ngay_gio = model.db.is_ngay_gio;
            model.db.status_del = 1;
            model.db.ngay_cap_nhat = DateTime.Now;
            await _context.sys_cau_hinh_ma_he_thong_col.InsertOneAsync(model.db);
            return 1;
        }


        public async Task<int> update(sys_cau_hinh_ma_he_thong_model model)
        {

            var update = Builders<sys_cau_hinh_ma_he_thong_col>.Update
            .Set(x => x.controller, model.db.controller)
             .Set(x => x.tien_to, model.db.tien_to)
              .Set(x => x.so_chu_so_tu_tang, model.db.so_chu_so_tu_tang)
               .Set(x => x.is_ngay_gio, model.db.is_ngay_gio)
                  .Set(x => x.is_ngay_gio, model.db.is_ngay_gio)
                     .Set(x => x.nguoi_cap_nhat, model.db.nguoi_cap_nhat)
                .Set(x => x.ngay_cap_nhat, model.db.ngay_cap_nhat);


            // Create a filter to match the document to update
            var filter = Builders<sys_cau_hinh_ma_he_thong_col>.Filter.Eq(x => x.id, model.db.id);

            // Create an update definition to set the "Name" property to a new value

            await _context.sys_cau_hinh_ma_he_thong_col.UpdateOneAsync(filter, update);
            return 1;
        }


        public IQueryable<sys_cau_hinh_ma_he_thong_model> FindAll()
        {
            var result = (from d in _context.sys_cau_hinh_ma_he_thong_col.AsQueryable()

                          join u in _context.sys_user_col.AsQueryable()
                         on d.nguoi_cap_nhat equals u.id into lu
                          from user in lu.DefaultIfEmpty()
                          select new sys_cau_hinh_ma_he_thong_model
                          {
                              db = d,
                              ten_nguoi_cap_nhat = user.ho_va_ten,
                          });



            return result;
        }

    }
}
