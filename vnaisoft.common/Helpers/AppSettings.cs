namespace quan_ly_kho.common.Helpers
{
    public class AppSettings
    {
         public string Secret { get; set; }
        public string host { get; set; }
        public string folder_path { get; set; }
        public string folder_sync { get; set; }
        public string help_phone { get; set; }
        public string help_mail { get; set; }
        public string logo { get; set; }
        public string avatar { get; set; }
        public string cover { get; set; }
        public string domain { get; set; }

        public string iosBundleId { get; set; }
        public string CertP8FilePath { get; set; }
        public string KeyId { get; set; }
        public string TeamId { get; set; }
        public string ServerType { get; set; }
        public string redis_client_name { get; set; }
        public string redis_server { get; set; }
        public string redis_password { get; set; }
        public string mongodb_database { get; set; }
        public string mongodb_database_hoccungai { get; set; }
        public string key_excel { get; set; }
        public string folder_path_sach_dien_tu { get; set; }
        
        public bool? auto_sync { get; set; }
        


    }
}