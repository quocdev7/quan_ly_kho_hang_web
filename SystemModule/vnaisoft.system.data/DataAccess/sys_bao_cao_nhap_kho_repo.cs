using MongoDB.Driver;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using vnaisoft.common.Helpers;
using vnaisoft.DataBase.commonFunc;
using vnaisoft.DataBase.Mongodb;
using vnaisoft.DataBase.Mongodb.Collection.system;
using vnaisoft.system.data.Models;
using WS.CRM.Data.Helper;
using static vnaisoft.common.BaseClass.BaseAuthenticationController;

namespace vnaisoft.system.data.DataAccess
{
    public class bao_cao_nhap_kho_repo
    {
        public MongoDBContext _context;
        public common_mongo_repo _common_repo;

        public bao_cao_nhap_kho_repo(MongoDBContext context)
        {
            _context = context;
            _common_repo = new common_mongo_repo(context);

        }
        public XSSFWorkbook exportExcelRepo(XSSFWorkbook workbook, string search, DateTime tu_ngay, DateTime den_ngay, string id_kho, string id_loai_mat_hang, AppSettings _appsetting, string filename)
        {
            var excel = new ExcelHelper(_appsetting);

            //KHỞI TẠO THÔNG TIN CHI TIẾT CỦA FILE

            excel.InitializeWorkbook(workbook, filename);
            string[] header = new string[] { };
            string[] listKey = new string[] { };
            search = search.Trim().ToLower();

            var querytable = _context.sys_phieu_nhap_kho_chi_tiet_col.AsQueryable()
                   //.Where(q => lst_pn.Contains(q.id_phieu_nhap_kho))
                   .Where(q => q.status_del == 1)
                   .Where(q => id_loai_mat_hang == "-1" || q.id_loai_mat_hang == id_loai_mat_hang)
                   .Where(d => tu_ngay <= d.ngay_nhap && d.ngay_nhap <= den_ngay)
                   .Where(d => d.id_mat_hang.ToLower().Contains(search) || d.ten_mat_hang.ToLower().Contains(search))
                    ;


            var count = querytable.Count();
            var dataList = FindAll(querytable).ToList();
            dataList.ForEach(q =>
            {
                //q.ten_loai_nhap = _context.sys_loai_nhap_xuats.AsQueryable().Where(d => d.ma == q.ma_loai_nhap).Select(d => d.ten).SingleOrDefault();
                q.id_loai_mat_hang = _context.sys_mat_hang_col.AsQueryable().Where(d => d.id == q.ma_mat_hang).Select(d => d.id_loai_mat_hang).FirstOrDefault();
                q.ten_mat_hang = _context.sys_mat_hang_col.AsQueryable().Where(d => d.id == q.ma_mat_hang).Select(d => d.ten).FirstOrDefault();
                q.ma_loai_mat_hang = _context.sys_loai_mat_hang_col.AsQueryable().Where(d => d.id == q.id_loai_mat_hang).Select(d => d.ma).SingleOrDefault();
                q.ten_loai_mat_hang = _context.sys_loai_mat_hang_col.AsQueryable().Where(d => d.id == q.id_loai_mat_hang).Select(d => d.ten).SingleOrDefault();
            });
            header = new string[] {
                       "STT (No.)","Loại nhập","Mã phiếu nhập kho","Ngày nhập kho","Mã kho","Tên kho","Mã Loại mặt hàng","Tên loại mặt hàng","Mã mặt hàng","Tên mặt hàng"
                       ,"Số lượng","Đơn vị tính","Đơn giá","Giá trị","Mã đối tượng","Tên đối tượng","Nội dung","Email","Địa chỉ"
                   };

            listKey = new string[]
            {
                       "ten_loai_nhap","ma_phieu_nhap_kho","ngay_nhap_kho","ma_kho","ten_kho","ma_loai_mat_hang","ten_loai_mat_hang","StrExcel_ma_mat_hang","ten_mat_hang"
                       ,"Num_so_luong","ten_don_vi_tinh","Num_don_gia","Num_gia_tri","StrExcel_ma_doi_tuong","ten_doi_tuong","noi_dung","email","dia_chi_doi_tuong"
            };

            //return await exportFileExcel(_appsetting, header, listKey, dataList, "bao_cao_ban_hang_theo_hang_hoa");
            var row_total = 1;

            var header_excel = new List<row_excel_model> {
                    new row_excel_model(){ row_index =1,
                        lst_col =new List<col_excel_model> {
                            new col_excel_model() { name = "BÁO CÁO NHẬP KHO",   col_index = 1, style = "styleCenterBoldNoBorder" },
                        } },

        new row_excel_model(){ row_index =2,
                        lst_col =new List<col_excel_model> {
                           new col_excel_model() { name = "Từ ngày " + tu_ngay.ToString("dd/MM/yyyy") + " đến ngày " + den_ngay.ToString("dd/MM/yyyy"),   col_index = 1, style = "styleCenterBoldNoBorder" },
                        } },
               new row_excel_model(){ row_index =3,
                        lst_col =new List<col_excel_model> {
                        } },
                   };

            var listMerge = new List<CellRangeAddress>();
            listMerge.Add(new CellRangeAddress(0, 0, 0, 19));
            listMerge.Add(new CellRangeAddress(1, 1, 0, 19));

            var sheet = workbook.CreateSheet(filename);
            //sheet.SetColumnWidth(0, 9 * 300);
            //sheet.SetColumnWidth(1, 9 * 400);
            //sheet.SetColumnWidth(2, 9 * 450);
            //sheet.SetColumnWidth(3, 9 * 900);
            //sheet.SetColumnWidth(4, 9 * 400);
            //sheet.SetColumnWidth(5, 9 * 400);
            //sheet.SetColumnWidth(6, 9 * 400);
            //sheet.SetColumnWidth(7, 9 * 400);
            //sheet.SetColumnWidth(8, 9 * 600);

            workbook = excel.exportExcelMultiHeader(sheet, workbook, filename, dataList, null, header_excel, new List<string[]> { header }, listKey, listMerge, true, row_total);

            return workbook;
        }

        public IQueryable<bao_cao_nhap_kho_model> FindAll(IQueryable<sys_phieu_nhap_kho_chi_tiet_col> query)
        {

            var result = (from d in query.OrderByDescending(q => q.ngay_nhap)

                          join dvt in _context.sys_don_vi_tinh_col.AsQueryable() on d.id_don_vi_tinh equals dvt.id into gdvt
                          from dvt in gdvt.DefaultIfEmpty()

                          join pn in _context.sys_phieu_nhap_kho_col.AsQueryable() on d.id_phieu_nhap_kho equals pn.id into gpx
                          from pn in gpx.DefaultIfEmpty()

                          select new bao_cao_nhap_kho_model
                          {
                              status_del = d.status_del,
                              id_phieu_nhap_kho = d.id_phieu_nhap_kho,
                              ma_phieu_nhap_kho = d.id_phieu_nhap_kho,
                              so_luong = d.so_luong ?? 0,
                              ngay_nhap_kho = d.ngay_nhap,
                              ma_mat_hang = d.id_mat_hang,
                              ten_mat_hang = d.ten_mat_hang,
                              id_don_vi_tinh = d.id_don_vi_tinh,
                              ngay_cap_nhat = d.ngay_cap_nhat,
                              ten_don_vi_tinh = dvt.ten,
                              ma_loai_nhap = pn.id_loai_nhap,
                              don_gia = d.don_gia ?? 0,
                          });
            return result;


        }


    }
}
