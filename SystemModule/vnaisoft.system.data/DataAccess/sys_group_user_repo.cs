using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;
using quan_ly_kho.DataBase.Mongodb;
using quan_ly_kho.DataBase.System;
using quan_ly_kho.system.data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace quan_ly_kho.system.data.DataAccess
{
    public class sys_group_user_repo
    {
        public MongoDBContext _context;

        public sys_group_user_repo(MongoDBContext context)
        {
            _context = context;
        }

        public async Task<sys_group_user_model> getElementById(string id)
        {
            var obj = await FindAll().FirstOrDefaultAsync(m => m.db.id == id);
            return obj;
        }

        public async Task<int> insert(sys_group_user_model model)
        {
            await _context.sys_group_user_col.InsertOneAsync(model.db);

            saveDetail(model);
            return 1;
        }

        public async Task<int> update(sys_group_user_model model)
        {
            var update = Builders<sys_group_user_db>.Update
               .Set(x => x.status_del, model.db.status_del)
               .Set(x => x.name, model.db.name)
               .Set(x => x.note, model.db.note)
               .Set(x => x.is_phan_quyen, model.db.is_phan_quyen)

               .Set(x => x.nguoi_cap_nhat, model.db.nguoi_cap_nhat)
               .Set(x => x.ngay_cap_nhat, model.db.ngay_cap_nhat);

            var filter = Builders<sys_group_user_db>.Filter.Eq(q => q.id, model.db.id);
            await _context.sys_group_user_col.UpdateOneAsync(filter, update);
            saveDetail(model);
            return 1;
        }
        public async void saveDetail(sys_group_user_model model)
        {
            var filter = Builders<sys_group_user_detail_db>.Filter.Eq(x => x.id_group_user, model.db.id);

            _context.sys_group_user_detail_col.DeleteMany(filter);


            var listdetail = model.list_item.Where(t => t.isCheck == true).ToList();
            var listinsert = new List<sys_group_user_detail_db>();
            for (int i = 0; i < listdetail.Count; i++)
            {
                var item = new sys_group_user_detail_db()
                {
                    id = ObjectId.GenerateNewId().ToString(),
                    id_group_user = model.db.id,
                    user_id = listdetail[i].user_id,
                };
                //listinsert.Add(item);
                await _context.sys_group_user_detail_col.InsertOneAsync(item);
            }

            var filter_role = Builders<sys_group_user_role_db>.Filter.Eq(x => x.id_group_user, model.db.id);

            _context.sys_group_user_role_col.DeleteMany(filter_role);



            model.list_role.ForEach(t =>
            {
                t.db.id = ObjectId.GenerateNewId().ToString();
                t.db.id_group_user = model.db.id;
            });
            var listInsert = model.list_role.Select(d => d.db).ToList();
            await _context.sys_group_user_role_col.InsertManyAsync(listInsert);



        }

        public IQueryable<sys_group_user_model> FindAll()
        {
            var result = from d in _context.sys_group_user_col.AsQueryable()

                         join u in _context.sys_user_col.AsQueryable()
                         on d.nguoi_cap_nhat equals u.id into uG

                         join sgud in _context.sys_group_user_detail_col.AsQueryable()
                         on d.id equals sgud.id_group_user into sgudG

                         //join gru in _context.sys_group_user_detail_col.AsQueryable()
                         //  on d.nguoi_cap_nhat equals gru.user_id into gruG
                         from u in uG.DefaultIfEmpty()
                             //from gru in gruG.DefaultIfEmpty()
                         select new sys_group_user_model
                         {
                             db = d,
                             ten_nguoi_cap_nhat = u.ho_va_ten,
                             count_user = sgudG.Count()
                             //count_user = gruG.Count()
                         };

            return result;



        }
        public IQueryable<sys_group_user_role_model> FindAllRole(string id)
        {
            var result = from d in _context.sys_group_user_role_col.AsQueryable()
                         where d.id_group_user == id

                         join gru in _context.sys_group_user_col.AsQueryable()
                         on d.id_group_user equals gru.id into gruG
                         from gru in gruG.DefaultIfEmpty()

                         select new sys_group_user_role_model()
                         {
                             db = d,
                             user_name = gru.name
                         };

            return result;
        }
        public IQueryable<sys_group_user_detail_model> FindAllItem()
        {
            var result = from d in _context.sys_user_col.AsQueryable()
                         where d.status_del == 1
                         select new sys_group_user_detail_model
                         {
                             user_name = d.Username,

                             user_id = d.id,
                             type_user = d.loai
                         };

            return result;
        }
        public int update_status_del(string id, string userid, int status_del)
        {
            var update = Builders<sys_group_user_db>.Update

                .Set(x => x.status_del, status_del)
                .Set(x => x.nguoi_cap_nhat, userid)
                 .Set(x => x.ngay_cap_nhat, DateTime.Now);
            // Create a filter to match the document to update
            var filter = Builders<sys_group_user_db>.Filter.Eq(x => x.id, id);
            _context.sys_group_user_col.UpdateOne(filter, update);
            return 1;
        }
    }
}
