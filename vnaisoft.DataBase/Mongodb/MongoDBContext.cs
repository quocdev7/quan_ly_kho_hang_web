using MongoDB.Driver;
using vnaisoft.DataBase.Mongodb.Collection.HocAI;

using vnaisoft.DataBase.Mongodb.Collection.system;
using vnaisoft.DataBase.System;


namespace vnaisoft.DataBase.Mongodb
{

    public partial class MongoDBContext
    {
        public IMongoDatabase _database;
        public MongoDBContext(IMongoDatabase database)
        {
            _database = database;
            sys_user_col = database.GetCollection<User>("Users");

            sys_group_user_role_col = database.GetCollection<sys_group_user_role_db>("sys_group_user_role_db");
            sys_group_user_col = database.GetCollection<sys_group_user_db>("sys_group_user_db");
            sys_group_user_detail_col = database.GetCollection<sys_group_user_detail_db>("sys_group_user_detail_db");
            sys_notification_col = database.GetCollection<sys_notification_db>("sys_notification_col");
            sys_template_mail_col = database.GetCollection<sys_template_mail_col>("sys_template_mail_col");
            sys_search_col = database.GetCollection<sys_search_col>("sys_search_col");
            sys_send_otp_col = database.GetCollection<sys_send_otp_col>("sys_send_otp_col");
            sys_log_email_col = database.GetCollection<sys_log_email_col>("sys_log_email_col");
            sys_file_upload_col = database.GetCollection<sys_file_upload_col>("sys_file_upload_col");
            sys_cau_hinh_anh_mac_dinh_col = database.GetCollection<sys_cau_hinh_anh_mac_dinh_col>("sys_cau_hinh_anh_mac_dinh_col");
            sys_loai_mat_hang_col = database.GetCollection<sys_loai_mat_hang_col>("sys_loai_mat_hang_col");
            sys_don_vi_tinh_col = database.GetCollection<sys_don_vi_tinh_col>("sys_don_vi_tinh_col");
            sys_mat_hang_col = database.GetCollection<sys_mat_hang_col>("sys_mat_hang_col");
            sys_don_hang_mua_col = database.GetCollection<sys_don_hang_mua_col>("sys_don_hang_mua_col");
            sys_don_hang_ban_col = database.GetCollection<sys_don_hang_ban_col>("sys_don_hang_ban_col");
            sys_phieu_nhap_kho_col = database.GetCollection<sys_phieu_nhap_kho_col>("sys_phieu_nhap_kho_col");
            sys_phieu_xuat_kho_col = database.GetCollection<sys_phieu_xuat_kho_col>("sys_phieu_xuat_kho_col");
            sys_phieu_nhap_kho_chi_tiet_col = database.GetCollection<sys_phieu_nhap_kho_chi_tiet_col>("sys_phieu_nhap_kho_chi_tiet_col");
            sys_phieu_xuat_kho_chi_tiet_col = database.GetCollection<sys_phieu_xuat_kho_chi_tiet_col>("sys_phieu_xuat_kho_chi_tiet_col");
            sys_ton_kho_mat_hang_col = database.GetCollection<sys_ton_kho_mat_hang_col>("sys_ton_kho_mat_hang_col");
            sys_don_hang_ban_mat_hang_col = database.GetCollection<sys_don_hang_ban_mat_hang_col>("sys_don_hang_ban_mat_hang_col");
            sys_don_hang_mua_mat_hang_col = database.GetCollection<sys_don_hang_mua_mat_hang_col>("sys_don_hang_mua_mat_hang_col");
        }

        public readonly IMongoCollection<sys_don_hang_mua_mat_hang_col> sys_don_hang_mua_mat_hang_col;
        public readonly IMongoCollection<sys_don_hang_ban_mat_hang_col> sys_don_hang_ban_mat_hang_col;
        public readonly IMongoCollection<sys_ton_kho_mat_hang_col> sys_ton_kho_mat_hang_col;
        public readonly IMongoCollection<sys_phieu_xuat_kho_chi_tiet_col> sys_phieu_xuat_kho_chi_tiet_col;
        public readonly IMongoCollection<sys_phieu_nhap_kho_chi_tiet_col> sys_phieu_nhap_kho_chi_tiet_col;
        public readonly IMongoCollection<sys_phieu_xuat_kho_col> sys_phieu_xuat_kho_col;
        public readonly IMongoCollection<sys_phieu_nhap_kho_col> sys_phieu_nhap_kho_col;
        public readonly IMongoCollection<sys_don_hang_ban_col> sys_don_hang_ban_col;
        public readonly IMongoCollection<sys_don_hang_mua_col> sys_don_hang_mua_col;
        public readonly IMongoCollection<sys_mat_hang_col> sys_mat_hang_col;
        public readonly IMongoCollection<sys_don_vi_tinh_col> sys_don_vi_tinh_col;
        public readonly IMongoCollection<sys_loai_mat_hang_col> sys_loai_mat_hang_col;
        public readonly IMongoCollection<sys_cau_hinh_anh_mac_dinh_col> sys_cau_hinh_anh_mac_dinh_col;
        public readonly IMongoCollection<sys_file_upload_col> sys_file_upload_col;
        public readonly IMongoCollection<sys_log_email_col> sys_log_email_col;
        public readonly IMongoCollection<sys_template_mail_col> sys_template_mail_col;
        public readonly IMongoCollection<sys_send_otp_col> sys_send_otp_col;
        public readonly IMongoCollection<sys_search_col> sys_search_col;
        public readonly IMongoCollection<User> sys_user_col;
        public readonly IMongoCollection<sys_group_user_db> sys_group_user_col;
        public readonly IMongoCollection<sys_group_user_detail_db> sys_group_user_detail_col;
        public readonly IMongoCollection<sys_group_user_role_db> sys_group_user_role_col;
        public readonly IMongoCollection<sys_notification_db> sys_notification_col;
    }
}