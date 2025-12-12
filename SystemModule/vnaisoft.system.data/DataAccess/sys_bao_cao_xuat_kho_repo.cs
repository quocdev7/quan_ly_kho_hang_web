using MongoDB.Driver;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using quan_ly_kho.common.Helpers;
using quan_ly_kho.DataBase.common;
using quan_ly_kho.DataBase.Mongodb;
using quan_ly_kho.DataBase.Mongodb.Collection.system;
using quan_ly_kho.system.data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using static quan_ly_kho.common.BaseClass.BaseAuthenticationController;

namespace quan_ly_kho.system.data.DataAccess
{
    public class bao_cao_xuat_kho_repo
    {
        public MongoDBContext _context;
        public common_mongo_repo _common_repo;

        public bao_cao_xuat_kho_repo(MongoDBContext context)
        {
            _context = context;
            _common_repo = new common_mongo_repo(context);

        }
        public XSSFWorkbook exportExcelRepo(XSSFWorkbook workbook, string search, DateTime tu_ngay, DateTime den_ngay, string id_loai_mat_hang, string id_kho, AppSettings _appsetting, string filename)
        {
            var excel = new ExcelHelper(_appsetting);

            //KHỞI TẠO THÔNG TIN CHI TIẾT CỦA FILE

            excel.InitializeWorkbook(workbook, filename);
            string[] header = new string[] { };
            string[] listKey = new string[] { };

            search = search.Trim().ToLower();


            var querytable = _context.sys_phieu_xuat_kho_chi_tiet_col.AsQueryable()
                     //.Where(q => lst_px.Contains(q.id_phieu_xuat_kho))
                     .Where(q => q.status_del == 1)
                     .Where(q => id_loai_mat_hang == "-1" || q.id_loai_mat_hang == id_loai_mat_hang)
                     .Where(q => tu_ngay <= q.ngay_xuat && q.ngay_xuat <= den_ngay)
                     .Where(q => q.id_mat_hang.ToLower().Contains(search) || q.ten_mat_hang.ToLower().Contains(search))
            ;

            var dataList = FindAll(querytable).ToList();
            dataList.ForEach(q =>
            {
                q.ten_loai_xuat = _context.sys_loai_nhap_xuat_col.AsQueryable().Where(d => d.ma == q.ma_loai_xuat).Select(d => d.ten).FirstOrDefault();
                q.id_loai_mat_hang = _context.sys_mat_hang_col.AsQueryable().Where(d => d.id == q.ma_mat_hang).Select(d => d.id_loai_mat_hang).FirstOrDefault();
                q.ma_loai_mat_hang = _context.sys_loai_mat_hang_col.AsQueryable().Where(d => d.id == q.id_loai_mat_hang).Select(d => d.ma).FirstOrDefault();
                q.ten_loai_mat_hang = _context.sys_loai_mat_hang_col.AsQueryable().Where(d => d.id == q.id_loai_mat_hang).Select(d => d.ten).FirstOrDefault();
                var id_don_vi_tinh = _context.sys_mat_hang_col.AsQueryable().Where(d => d.id == q.ma_mat_hang).Select(d => d.id_don_vi_tinh).FirstOrDefault();
                q.ten_don_vi_tinh = _context.sys_don_vi_tinh_col.AsQueryable().Where(d => d.id == id_don_vi_tinh).Select(d => d.ten).FirstOrDefault();
            });
            header = new string[] {
                "STT (No.)","Loại xuất","Mã phiếu xuất kho","Ngày xuất kho","Mã Loại mặt hàng","Tên loại mặt hàng","Mã mặt hàng","Tên mặt hàng","Số lượng","Đơn vị tính"
            };

            listKey = new string[]
            {
                "ten_loai_xuat","ma_phieu_xuat_kho","ngay_xuat_kho","ma_loai_mat_hang","ten_loai_mat_hang","StrExcel_ma_mat_hang","ten_mat_hang","Num_so_luong","ten_don_vi_tinh"
            };

            //return await exportFileExcel(_appsetting, header, listKey, dataList, "bao_cao_ban_hang_theo_hang_hoa");
            var row_total = 1;

            var header_excel = new List<row_excel_model> {
             new row_excel_model(){ row_index =1,
                 lst_col =new List<col_excel_model> {
                     new col_excel_model() { name = "BÁO CÁO XUẤT KHO",   col_index = 1, style = "styleCenterBoldNoBorder" },
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

        public IQueryable<bao_cao_xuat_kho_model> FindAll(IQueryable<sys_phieu_xuat_kho_chi_tiet_col> query)
        {

            var result = from d in query.OrderByDescending(q => q.ngay_xuat)


                         select new bao_cao_xuat_kho_model
                         {
                             status_del = d.status_del,
                             id_phieu_xuat_kho = d.id_phieu_xuat_kho,
                             ma_phieu_xuat_kho = d.id_phieu_xuat_kho,
                             so_luong = d.so_luong ?? 0,
                             don_gia = d.don_gia ?? 0,
                             gia_tri = d.gia_tri ?? 0,
                             ngay_xuat_kho = d.ngay_xuat,
                             ma_mat_hang = d.id_mat_hang,
                             ten_mat_hang = d.ten_mat_hang,
                             //ma_loai_xuat = d.id_loai_xuat,
                             id_don_vi_tinh = d.id_don_vi_tinh,
                         };
            return result;


        }


    }
}
