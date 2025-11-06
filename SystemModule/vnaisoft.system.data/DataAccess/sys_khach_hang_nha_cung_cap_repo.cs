using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using vnaisoft.DataBase.commonFunc;
using vnaisoft.DataBase.Helper;
using vnaisoft.DataBase.Mongodb;
using vnaisoft.DataBase.Mongodb.Collection.system;
using vnaisoft.system.data.Models;

namespace vnaisoft.system.data.DataAccess
{
    public class sys_khach_hang_nha_cung_cap_repo
    {
        public MongoDBContext _context;
        public common_mongo_repo _common_repo;

        public sys_khach_hang_nha_cung_cap_repo(MongoDBContext context)
        {
            _context = context;
            _common_repo = new common_mongo_repo(context);
        }
        public string getCode()
        {
            var max = "";
            var config = _common_repo.get_code_config(true, "sys_khach_hang_nha_cung_cap", "DT");
            var max_query = _context.sys_khach_hang_nha_cung_cap_col.AsQueryable()
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

        public async Task<sys_khach_hang_nha_cung_cap_model> getElementById(string id)
        {
            var obj = await FindAll().FirstOrDefaultAsync(m => m.db.id == id);
            return obj;
        }
        public async Task<int> insert(sys_khach_hang_nha_cung_cap_model model)
        {
            model.db.ten_khong_dau = Regex.Replace(StringFunctions.NonUnicode(HttpUtility.HtmlDecode(model.db.ten ?? "")).ToLower().Normalize(), "<.*?>|&.*?;", String.Empty);

            await _context.sys_khach_hang_nha_cung_cap_col.InsertOneAsync(model.db);

            insert_doi_tuong(model);

            return 1;
        }
        public async void insert_doi_tuong(sys_khach_hang_nha_cung_cap_model model)
        {
            var db = _context.sys_khach_hang_nha_cung_cap_col.AsQueryable().Where(q => q.dien_thoai == model.db.dien_thoai).FirstOrDefault();
            if (db == null)
            {
                var kh = new sys_khach_hang_nha_cung_cap_model();
                if (model.db.hinh_thuc == 1)
                {

                    kh.db.id = model.db.dien_thoai;
                    kh.db.ma = model.db.dien_thoai;
                }
                else
                {
                    kh.db.id = model.db.ma_so_thue;
                    kh.db.ma = model.db.ma_so_thue;

                }
                kh.db.ten = model.db.ten;
                kh.db.ma_so_thue = model.db.ma_so_thue;
                kh.db.dien_thoai = model.db.dien_thoai;
                kh.db.email = model.db.email;
                kh.db.hinh_thuc = model.db.hinh_thuc;
                kh.db.laKhachHang = true;
                kh.db.status_del = 1;
                await _context.sys_khach_hang_nha_cung_cap_col.InsertOneAsync(kh.db);
            }
            else
            {
                var update = Builders<sys_khach_hang_nha_cung_cap_col>.Update
                .Set(x => x.ten, model.db.ten)
                .Set(x => x.email, model.db.email)
                .Set(x => x.dia_chi, model.db.dia_chi);


                // Create a filter to match the document to update
                var filter = Builders<sys_khach_hang_nha_cung_cap_col>.Filter.Eq(x => x.id, model.db.id);

                // Create an update definition to set the "Name" property to a new value

                await _context.sys_khach_hang_nha_cung_cap_col.UpdateOneAsync(filter, update);
            }


        }

        public async Task<int> update(sys_khach_hang_nha_cung_cap_model model)
        {
            model.db.ten_khong_dau = Regex.Replace(StringFunctions.NonUnicode(HttpUtility.HtmlDecode(model.db.ten ?? "")).ToLower().Normalize(), "<.*?>|&.*?;", String.Empty);

            var update = Builders<sys_khach_hang_nha_cung_cap_col>.Update
                 .Set(x => x.ma, model.db.ma)
                    .Set(x => x.ten, model.db.ten)
                    .Set(x => x.ma, model.db.ma_so_thue)
                    .Set(x => x.id_ngan_hang, model.db.id_ngan_hang)
                    .Set(x => x.so_tai_khoan, model.db.so_tai_khoan)
                    .Set(x => x.ten_khong_dau, model.db.ten_khong_dau)
                    .Set(x => x.laKhachHang, model.db.laKhachHang)
                    .Set(x => x.laNhaCungCap, model.db.laNhaCungCap)
                    .Set(x => x.hinh_thuc, model.db.hinh_thuc)
                    .Set(x => x.ma_so_thue, model.db.ma_so_thue)
                     .Set(x => x.dien_thoai, model.db.dien_thoai)
                      .Set(x => x.email, model.db.email)
                       .Set(x => x.dia_chi, model.db.dia_chi)
                        .Set(x => x.nguoi_cap_nhat, model.db.nguoi_cap_nhat)
                         .Set(x => x.ngay_cap_nhat, model.db.ngay_cap_nhat);

            var filter = Builders<sys_khach_hang_nha_cung_cap_col>.Filter.Eq(x => x.id, model.db.id);
            await _context.sys_khach_hang_nha_cung_cap_col.UpdateOneAsync(filter, update);
            return 1;
        }

        public IQueryable<sys_khach_hang_nha_cung_cap_model> FindAll()
        {

            var result = (from d in _context.sys_khach_hang_nha_cung_cap_col.AsQueryable()

                          join u in _context.sys_user_col.AsQueryable()
                         on d.nguoi_cap_nhat equals u.id into lu


                          from user in lu.DefaultIfEmpty()
                          select new sys_khach_hang_nha_cung_cap_model
                          {
                              db = d,
                              nguoi_cap_nhat = user.ho_va_ten
                          });
            return result;

        }

        public int delete(string id)
        {
            var filter = Builders<sys_khach_hang_nha_cung_cap_col>.Filter.Eq(x => x.id, id);
            _context.sys_khach_hang_nha_cung_cap_col.DeleteOne(filter);
            return 1;
        }

        public int update_status_del(string id, string userid, int status_del)
        {

            var update = Builders<sys_khach_hang_nha_cung_cap_col>.Update
               .Set(x => x.status_del, status_del)
                .Set(x => x.nguoi_cap_nhat, userid)
                 .Set(x => x.ngay_cap_nhat, DateTime.Now);

            // Create a filter to match the document to update
            var filter = Builders<sys_khach_hang_nha_cung_cap_col>.Filter.Eq(x => x.id, id);
            _context.sys_khach_hang_nha_cung_cap_col.UpdateOne(filter, update);
            return 1;
        }
    }
}
