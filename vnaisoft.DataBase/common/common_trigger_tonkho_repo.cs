using MongoDB.Bson;
using MongoDB.Driver;
using System;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using vnaisoft.DataBase.Mongodb;
using vnaisoft.DataBase.Mongodb.Collection.system;

namespace worldsoft.DataBase.commonFunc
{


    public class common_trigger_tonkho_repo
    {
        public MongoDBContext _context;

        public common_trigger_tonkho_repo(MongoDBContext context)
        {
            _context = context;
        }

        public  async Task<string> updateNhapTonKhoAsync(
          decimal? soLuongNhapMoi,
            decimal? giaTriNhapMoi,
          string id_mat_hang,
           string id_don_vi_tinh,
          DateTime? ngayHoachToan)
        {

            try
            {
                var mat_hang = _context.sys_mat_hang_col.AsQueryable().Where(d => d.id == id_mat_hang).Select(t => new
                {
                    id_loai_mat_hang = t.id_loai_mat_hang,
                    ten_mat_hang = t.ten,
                }).SingleOrDefault();
                if (mat_hang == null) return "";
                var erp_ton_kho_mat_hangs = _context.sys_ton_kho_mat_hang_col;
                //var erp_ton_kho_mat_hang_theo_thangs = _context.erp_ton_kho_mat_hang_theo_thangs;
                var thang = ngayHoachToan.Value.Month;
                var nam = ngayHoachToan.Value.Year;
                var tuNgay = new DateTime(nam, thang, 1);
                var denNgay = tuNgay.AddMonths(1).AddSeconds(-1);

                var idTonKho = id_mat_hang;
                //var idTonKhoThangNam = id_mat_hang + id_kho + nam + thang;
                var TangchenhLech = Convert.ToInt64(((soLuongNhapMoi ?? 0) ) * 1000);
                ;

                var TangchenhLechgiaTriNhapMoi = Convert.ToInt64(((giaTriNhapMoi ?? 0)) * 1000);
                ;

                var filter = Builders<sys_ton_kho_mat_hang_col>.Filter.And(
                    Builders<sys_ton_kho_mat_hang_col>.Filter.Eq(d => d.id, idTonKho)
                );
                var update = Builders<sys_ton_kho_mat_hang_col>.Update
                       .SetOnInsert(d => d.id, idTonKho)
                    .SetOnInsert(d => d.id_mat_hang, id_mat_hang)
                     .SetOnInsert(d => d.ten_mat_hang, mat_hang.ten_mat_hang)
                    .SetOnInsert(d => d.id_loai_mat_hang, mat_hang.id_loai_mat_hang)
                      .SetOnInsert(d => d.id_don_vi_tinh, id_don_vi_tinh)
                        .SetOnInsert(x => x.ngay_cap_nhat, DateTime.Now)
                    .Inc(d => d.so_luong_ton, TangchenhLech)
                        .Inc(d => d.gia_tri, TangchenhLechgiaTriNhapMoi);
                var options = new UpdateOptions { IsUpsert = true };

                var result = erp_ton_kho_mat_hangs.UpdateOne(filter, update, options);


                //var thangnow = DateTime.Now.Month;
                //var namnow = DateTime.Now.Year;
                //var thangcount = thang;
                //var namcount = nam;
                //var countNow = namnow * 100 + thangnow;
                //var countNamThang = namcount * 100 + thangcount;


                //var filterThangNam = Builders<erp_ton_kho_mat_hang_theo_thang_db>.Filter.And(
                //    Builders<erp_ton_kho_mat_hang_theo_thang_db>.Filter.Eq(d => d.id, idTonKhoThangNam)
                //);

                //var updateThangNam = Builders<erp_ton_kho_mat_hang_theo_thang_db>.Update
                //      .SetOnInsert(d => d.id, idTonKhoThangNam)
                //    .SetOnInsert(d => d.id_mat_hang, id_mat_hang)
                //       .SetOnInsert(d => d.ten_mat_hang, mat_hang.ten_mat_hang)
                //    .SetOnInsert(d => d.id_loai_mat_hang, mat_hang.id_loai_mat_hang)
                //        .SetOnInsert(d => d.namthang, countNamThang)
                //     .SetOnInsert(d => d.id_don_vi_tinh, id_don_vi_tinh)
                //     .SetOnInsert(d => d.thang, thang)
                //      .SetOnInsert(d => d.nam, nam)
                //        .SetOnInsert(d => d.ngay_bat_dau, tuNgay)
                //            .SetOnInsert(d => d.ngay_ket_thuc, denNgay)
                //    .SetOnInsert(d => d.id_kho, id_kho)
                //     .SetOnInsert(x => x.ngay_cap_nhat, DateTime.Now)

                //      .Inc(d => d.so_luong_nhap_trong_ky, TangchenhLech)
                //    .Inc(d => d.gia_tri_nhap_trong_ky, TangchenhLechgiaTriNhapMoi);
                //var resultThangNam = erp_ton_kho_mat_hang_theo_thangs.UpdateOne(filterThangNam, updateThangNam, new UpdateOptions { IsUpsert = true });

            }
            catch (Exception e)
            {
                Console.WriteLine("trigger" + id_mat_hang + " " + ngayHoachToan + " error");

            }

            Console.WriteLine("trigger"+id_mat_hang+" "+ngayHoachToan+" done");
            return "";

        }

        public async Task<string> updateXuatTonKhoAsync(
         decimal? soLuongXuatMoi,
          decimal? giaTriXuatMoi,
         string id_mat_hang,
            string id_don_vi_tinh,
         DateTime? ngayHoachToan)
        {

            try
            {
                var mat_hang = _context.sys_mat_hang_col.AsQueryable().Where(d => d.id == id_mat_hang).Select(t => new
                {
                    id_loai_mat_hang=  t.id_loai_mat_hang,
                    ten_mat_hang = t.ten,
                }).SingleOrDefault();
                if (mat_hang == null) return "";
                var erp_ton_kho_mat_hangs = _context.sys_ton_kho_mat_hang_col;
                //var erp_ton_kho_mat_hang_theo_thangs = _context.erp_ton_kho_mat_hang_theo_thangs;
                var thang = ngayHoachToan.Value.Month;
                var nam = ngayHoachToan.Value.Year;
                var tuNgay = new DateTime(nam, thang, 1);
                var denNgay = tuNgay.AddMonths(1).AddSeconds(-1);

                var idTonKho = id_mat_hang;
                var idTonKhoThangNam = id_mat_hang + nam + thang;
                var GiamchenhLech = Convert.ToInt64(((soLuongXuatMoi ?? 0)) * 1000);
                ; var GiamchenhLechgiaTriNhapMoi = Convert.ToInt64(((giaTriXuatMoi ?? 0)) * 1000);
                ;

                var filter = Builders<sys_ton_kho_mat_hang_col>.Filter.And(
                    Builders<sys_ton_kho_mat_hang_col>.Filter.Eq(d => d.id, idTonKho)
                );
                var update = Builders<sys_ton_kho_mat_hang_col>.Update
                       .SetOnInsert(d => d.id, idTonKho)
                    .SetOnInsert(d => d.id_mat_hang, id_mat_hang)
                       .SetOnInsert(d => d.ten_mat_hang, mat_hang.ten_mat_hang)
                    .SetOnInsert(d => d.id_loai_mat_hang, mat_hang.id_loai_mat_hang)
                     .SetOnInsert(d => d.id_don_vi_tinh, id_don_vi_tinh)
                        .SetOnInsert(x => x.ngay_cap_nhat, DateTime.Now)
                    .Inc(d => d.so_luong_ton, -GiamchenhLech)
                      .Inc(d => d.gia_tri, -GiamchenhLechgiaTriNhapMoi);
                var options = new UpdateOptions { IsUpsert = true };

                var result = erp_ton_kho_mat_hangs.UpdateOne(filter, update, options);




                //var thangnow = DateTime.Now.Month;
                //var namnow = DateTime.Now.Year;
                //var thangcount = thang;
                //var namcount = nam;
                //var countNow = namnow * 100 + thangnow;
                //var countNamThang = namcount * 100 + thangcount;
                //var filterThangNam = Builders<erp_ton_kho_mat_hang_theo_thang_db>.Filter.And(
                //    Builders<erp_ton_kho_mat_hang_theo_thang_db>.Filter.Eq(d => d.id, idTonKhoThangNam)
                //);
                //var updateThangNam = Builders<erp_ton_kho_mat_hang_theo_thang_db>.Update
                //      .SetOnInsert(d => d.id, idTonKhoThangNam)
                //    .SetOnInsert(d => d.id_mat_hang, id_mat_hang)
                //           .SetOnInsert(d => d.ten_mat_hang, mat_hang.ten_mat_hang)
                //    .SetOnInsert(d => d.id_loai_mat_hang, mat_hang.id_loai_mat_hang)
                //          .SetOnInsert(d => d.namthang, countNamThang)
                //     .SetOnInsert(d => d.id_don_vi_tinh, id_don_vi_tinh)
                //     .SetOnInsert(d => d.thang, thang)
                //      .SetOnInsert(d => d.nam, nam)
                //        .SetOnInsert(d => d.ngay_bat_dau, tuNgay)
                //            .SetOnInsert(d => d.ngay_ket_thuc, denNgay)
                //    .SetOnInsert(d => d.id_kho, id_kho)
                //     .SetOnInsert(x => x.ngay_cap_nhat, DateTime.Now)
                //      //.Inc(d => d.so_luong_ton_dau_ky, 0)
                //       .Inc(d => d.so_luong_xuat_trong_ky, GiamchenhLech)
                //        .Inc(d => d.gia_tri_xuat_trong_ky, GiamchenhLechgiaTriNhapMoi)
                //       ;
                //var resultThangNam = erp_ton_kho_mat_hang_theo_thangs.UpdateOne(filterThangNam, updateThangNam, new UpdateOptions { IsUpsert = true });

            }
            catch (Exception e)
            {
                Console.WriteLine("trigger" + id_mat_hang + " " + ngayHoachToan + " error");

            }

            Console.WriteLine("trigger" + id_mat_hang + " " + ngayHoachToan + " done");
            return "";

        }

    }
}
