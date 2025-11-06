//using Microsoft.EntityFrameworkCore;
//using MongoDB.Driver;
//using NPOI.SS.Util;
//using NPOI.XSSF.UserModel;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text.RegularExpressions;
//using System.Threading.Tasks;
//using System.Web;
//using vnaisoft.common.Common;
//using vnaisoft.common.Helpers;
//using vnaisoft.DataBase.commonFunc;
//using vnaisoft.DataBase.Helper;
//using vnaisoft.DataBase.Mongodb;
//using vnaisoft.DataBase.Mongodb.Collection.system;
//using vnaisoft.system.data.Models;
//using WS.CRM.Data.Helper;
//using static vnaisoft.common.BaseClass.BaseAuthenticationController;

//namespace vnaisoft.system.data.DataAccess
//{
//    public class bao_cao_ton_kho_mat_hang_repo
//    {
//        public MongoDBContext _context;
//        public common_mongo_repo _common_repo;

//        public bao_cao_ton_kho_mat_hang_repo(MongoDBContext context)
//        {
//            _context = context;
//            _common_repo = new common_mongo_repo(context);

//        }

//        public XSSFWorkbook exportExcelRepo(XSSFWorkbook workbook, string search, string id_kho, AppSettings _appsetting, string filename)
//        {
//            var excel = new ExcelHelper(_appsetting);

//            //KHỞI TẠO THÔNG TIN CHI TIẾT CỦA FILE

//            excel.InitializeWorkbook(workbook, filename);
//            string[] header = new string[] { };
//            string[] listKey = new string[] { };

//            search = search.Trim().ToLower();

//            var queryTable = _common_repo._context.erp_ton_kho_mat_hangs.AsQueryable()
//                             .Where(q => id_kho == "-1" || q.id_kho == id_kho)
//                               .Where(d => d.ma_mat_hang.ToLower().Contains(search) || d.ten_mat_hang.ToLower().Contains(search) ||

//                         search == ""
//                         );



//            var count = queryTable.Count();

//            var dataList = FindAll(queryTable).OrderBy(d => d.id_kho).ToList();
//            dataList.ForEach(q =>
//            {
//                q.ten_don_vi_tinh = _context.erp_don_vi_tinhs.AsQueryable().Where(d => d.id == q.id_don_vi_tinh).Select(d => d.ten).SingleOrDefault();
//                q.ma_loai_mat_hang = _context.erp_loai_mat_hangs.AsQueryable().Where(d => d.id == q.id_loai_mat_hang).Select(d => d.ma).SingleOrDefault();
//                q.ten_loai_mat_hang = _context.erp_loai_mat_hangs.AsQueryable().Where(d => d.id == q.id_loai_mat_hang).Select(d => d.ten).SingleOrDefault();
//                q.so_luong_ton_str = string.Format("{0:#,##0}", q.so_luong_ton) + " " + q.ten_don_vi_tinh;

//            });
//            header = new string[] {
//                "STT (No.)","Mã kho","Tên kho","Mã Loại mặt hàng","Tên loại mặt hàng","Mã mặt hàng","Tên mặt hàng","Số lượng","Đơn vị tính"
//            };

//            listKey = new string[]
//            {
//                "ma_kho","ten_kho","ma_loai_mat_hang","ten_loai_mat_hang","StrExcel_ma_mat_hang","ten_mat_hang","Num_so_luong_ton","ten_don_vi_tinh"
//            };

//            //return await exportFileExcel(_appsetting, header, listKey, dataList, "bao_cao_ban_hang_theo_hang_hoa");
//            var row_total = 1;

//            var header_excel = new List<row_excel_model> {
//             new row_excel_model(){ row_index =1,
//                 lst_col =new List<col_excel_model> {
//                     new col_excel_model() { name = "XEM TỒN MẶT HÀNG THEO TỪNG KHO",   col_index = 1, style = "styleCenterBoldNoBorder" },
//                 } },

//        new row_excel_model(){ row_index =2,
//                 lst_col =new List<col_excel_model> {
//                 } },
//            };

//            var listMerge = new List<CellRangeAddress>();
//            listMerge.Add(new CellRangeAddress(0, 0, 0, 7));
//            listMerge.Add(new CellRangeAddress(1, 1, 0, 7));

//            var sheet = workbook.CreateSheet(filename);
//            sheet.SetColumnWidth(0, 9 * 300);
//            sheet.SetColumnWidth(1, 9 * 400);
//            sheet.SetColumnWidth(2, 9 * 900);
//            sheet.SetColumnWidth(3, 9 * 400);
//            sheet.SetColumnWidth(4, 9 * 600);
//            sheet.SetColumnWidth(5, 9 * 400);
//            sheet.SetColumnWidth(6, 9 * 900);
//            sheet.SetColumnWidth(7, 9 * 400);

//            workbook = excel.exportExcelMultiHeader(sheet, workbook, filename, dataList, null, header_excel, new List<string[]> { header }, listKey, listMerge, true, row_total);

//            return workbook;
//        }
//        public IQueryable<bao_cao_ton_kho_mat_hang_model> FindAll(IQueryable<erp_ton_kho_mat_hang_db> query)
//        {

//            var result = (from d in query
//                          join mh in _context.erp_mat_hangs.AsQueryable()
//                                              on d.id_mat_hang equals mh.id into mhG

//                          join kho in _context.erp_khos.AsQueryable()
//                          on d.id_kho equals kho.id into khoG

//                          from kho in khoG.DefaultIfEmpty()

//                          from mh in mhG.DefaultIfEmpty()
//                          select new bao_cao_ton_kho_mat_hang_model
//                          {
//                              id_kho = d.id_kho,
//                              so_luong_ton = d.so_luong_ton / 1000 ?? 0,
//                              ma_mat_hang = mh.ma,
//                              ten_mat_hang = mh.ten,
//                              id_loai_mat_hang = mh.id_loai_mat_hang,
//                              id_don_vi_tinh = mh.id_don_vi_tinh,
//                              ten_kho = kho.ten,
//                              ma_kho = kho.ma,
//                              ngay_cap_nhat = d.ngay_cap_nhat


//                          });
//            return result;


//        }


//    }
//}