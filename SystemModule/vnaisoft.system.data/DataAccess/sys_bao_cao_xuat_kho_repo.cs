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
//    public class bao_cao_xuat_kho_repo
//    {
//        public MongoDBContext _context;
//        public common_mongo_repo _common_repo;

//        public bao_cao_xuat_kho_repo(MongoDBContext context)
//        {
//            _context = context;
//            _common_repo = new common_mongo_repo(context);

//        }
//        public XSSFWorkbook exportExcelRepo(XSSFWorkbook workbook, string search, DateTime tu_ngay, DateTime den_ngay, string id_loai_mat_hang, string id_kho, AppSettings _appsetting, string filename)
//        {
//            var excel = new ExcelHelper(_appsetting);

//            //KHỞI TẠO THÔNG TIN CHI TIẾT CỦA FILE

//            excel.InitializeWorkbook(workbook, filename);
//            string[] header = new string[] { };
//            string[] listKey = new string[] { };

//            search = search.Trim().ToLower();


//            var querytable = _context.erp_phieu_xuat_kho_chi_tiets.AsQueryable()
//                     //.Where(q => lst_px.Contains(q.id_phieu_xuat_kho))
//                     .Where(q => q.is_vat != true)
//                     .Where(q => q.status_del == 1)
//                     .Where(q => id_loai_mat_hang == "-1" || q.id_loai_mat_hang == id_loai_mat_hang)
//                     .Where(q => id_kho == "-1" || q.id_kho == id_kho)
//                     .Where(q => tu_ngay <= q.ngay_xuat && q.ngay_xuat <= den_ngay)
//                     .Where(q => q.id_mat_hang.ToLower().Contains(search) || q.ten_mat_hang.ToLower().Contains(search))
//            ;

//            var dataList = FindAll(querytable).ToList();
//            dataList.ForEach(q =>
//            {
//                q.ten_loai_xuat = _context.erp_loai_nhap_xuats.AsQueryable().Where(d => d.ma == q.ma_loai_xuat).Select(d => d.ten).SingleOrDefault();
//                q.id_loai_mat_hang = _context.erp_mat_hangs.AsQueryable().Where(d => d.id == q.ma_mat_hang).Select(d => d.id_loai_mat_hang).FirstOrDefault();
//                q.ma_loai_mat_hang = _context.erp_loai_mat_hangs.AsQueryable().Where(d => d.id == q.id_loai_mat_hang).Select(d => d.ma).SingleOrDefault();
//                q.ten_loai_mat_hang = _context.erp_loai_mat_hangs.AsQueryable().Where(d => d.id == q.id_loai_mat_hang).Select(d => d.ten).SingleOrDefault();
//                var doi_tuong = _context.erp_phieu_xuat_khos.AsQueryable().Where(d => d.id == q.id_phieu_xuat_kho).SingleOrDefault();
//                q.ma_doi_tuong = doi_tuong.id_doi_tuong;
//                q.ten_doi_tuong = doi_tuong.ten_doi_tuong;
//                q.ma_so_thue = doi_tuong.ma_so_thue;
//                q.dien_thoai = doi_tuong.dien_thoai;
//                q.email = doi_tuong.email;
//                q.dia_chi_doi_tuong = doi_tuong.dia_chi_doi_tuong;
//                var id_don_vi_tinh = _context.erp_mat_hangs.AsQueryable().Where(d => d.id == q.ma_mat_hang).Select(d => d.id_don_vi_tinh).SingleOrDefault();
//                q.ten_don_vi_tinh = _context.erp_don_vi_tinhs.AsQueryable().Where(d => d.id == id_don_vi_tinh).Select(d => d.ten).SingleOrDefault();
//                q.noi_dung = doi_tuong.ghi_chu;
//                if (q.tai_khoan_no != null)
//                {
//                    if (q.tai_khoan_no.StartsWith("152") || q.tai_khoan_no.StartsWith("153") || q.tai_khoan_no.StartsWith("155") || q.tai_khoan_no.StartsWith("156"))
//                    {
//                        q.tai_khoan_kho = q.tai_khoan_no;
//                    }
//                }
//                ;
//                if (q.tai_khoan_co != null)
//                {
//                    if (q.tai_khoan_co.StartsWith("152") || q.tai_khoan_co.StartsWith("153") || q.tai_khoan_co.StartsWith("155") || q.tai_khoan_co.StartsWith("156"))
//                    {
//                        q.tai_khoan_kho = q.tai_khoan_co;
//                    }
//                }
//            });
//            header = new string[] {
//                "STT (No.)","Loại xuất","Mã phiếu xuất kho","Ngày xuất kho","Mã kho","Tên kho","Mã Loại mặt hàng","Tên loại mặt hàng","Mã mặt hàng","Tên mặt hàng","Tài khoản kho","Số lượng","Đơn vị tính"
//                ,"Đơn giá","Giá trị","Mã đối tượng","Tên đối tượng","Nội dung","Email","Địa chỉ"
//            };

//            listKey = new string[]
//            {
//                "ten_loai_xuat","ma_phieu_xuat_kho","ngay_xuat_kho","ma_kho","ten_kho","ma_loai_mat_hang","ten_loai_mat_hang","StrExcel_ma_mat_hang","ten_mat_hang","StrExcel_tai_khoan_kho","Num_so_luong","ten_don_vi_tinh"
//                ,"Num_don_gia","Num_gia_tri","StrExcel_ma_doi_tuong","ten_doi_tuong","noi_dung","email","dia_chi_doi_tuong"
//            };

//            //return await exportFileExcel(_appsetting, header, listKey, dataList, "bao_cao_ban_hang_theo_hang_hoa");
//            var row_total = 1;

//            var header_excel = new List<row_excel_model> {
//             new row_excel_model(){ row_index =1,
//                 lst_col =new List<col_excel_model> {
//                     new col_excel_model() { name = "BÁO CÁO XUẤT KHO",   col_index = 1, style = "styleCenterBoldNoBorder" },
//                 } },

// new row_excel_model(){ row_index =2,
//                 lst_col =new List<col_excel_model> {
//                    new col_excel_model() { name = "Từ ngày " + tu_ngay.ToString("dd/MM/yyyy") + " đến ngày " + den_ngay.ToString("dd/MM/yyyy"),   col_index = 1, style = "styleCenterBoldNoBorder" },
//                 } },
//        new row_excel_model(){ row_index =3,
//                 lst_col =new List<col_excel_model> {
//                 } },
//            };

//            var listMerge = new List<CellRangeAddress>();
//            listMerge.Add(new CellRangeAddress(0, 0, 0, 19));
//            listMerge.Add(new CellRangeAddress(1, 1, 0, 19));

//            var sheet = workbook.CreateSheet(filename);
//            //sheet.SetColumnWidth(0, 9 * 300);
//            //sheet.SetColumnWidth(1, 9 * 400);
//            //sheet.SetColumnWidth(2, 9 * 450);
//            //sheet.SetColumnWidth(3, 9 * 900);
//            //sheet.SetColumnWidth(4, 9 * 400);
//            //sheet.SetColumnWidth(5, 9 * 400);
//            //sheet.SetColumnWidth(6, 9 * 400);
//            //sheet.SetColumnWidth(7, 9 * 400);
//            //sheet.SetColumnWidth(8, 9 * 600);

//            workbook = excel.exportExcelMultiHeader(sheet, workbook, filename, dataList, null, header_excel, new List<string[]> { header }, listKey, listMerge, true, row_total);

//            return workbook;
//        }

//        public IQueryable<bao_cao_xuat_kho_model> FindAll(IQueryable<erp_phieu_xuat_kho_chi_tiet_db> query)
//        {

//            var result = (from d in query.OrderByDescending(q => q.ngay_xuat)


//                          join kho in _context.erp_khos.AsQueryable() on d.id_kho equals kho.id into gkho
//                          from kho in gkho.DefaultIfEmpty()
//                          select new bao_cao_xuat_kho_model
//                          {
//                              status_del = d.status_del,
//                              id_phieu_xuat_kho = d.id_phieu_xuat_kho,
//                              ma_phieu_xuat_kho = d.id_phieu_xuat_kho,
//                              so_luong = d.so_luong ?? 0,
//                              don_gia = d.don_gia ?? 0,
//                              gia_tri = d.gia_tri ?? 0,
//                              id_kho = d.id_kho,
//                              ma_kho = kho.ma,
//                              ten_kho = kho.ten,
//                              ngay_xuat_kho = d.ngay_xuat,
//                              ma_mat_hang = d.id_mat_hang,
//                              ten_mat_hang = d.ten_mat_hang,
//                              ma_loai_xuat = d.id_loai_xuat,
//                              id_don_vi_tinh = d.id_don_vi_tinh,
//                              tai_khoan_co = d.tai_khoan_co,
//                              tai_khoan_no = d.tai_khoan_no,
//                          });
//            return result;


//        }


//    }
//}
