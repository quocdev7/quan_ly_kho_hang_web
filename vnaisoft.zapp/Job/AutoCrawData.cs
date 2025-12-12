using Microsoft.Extensions.Options;
using quan_ly_kho.common.Helpers;
using quan_ly_kho.DataBase.Mongodb;
using Quartz;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace quan_ly_kho.zapp.Job
{
    public class AutoCrawDataJob : IJob
    {

        public AppSettings _appsetting;
        public HttpClient client;
        public MongoDBContext _context;



        public AutoCrawDataJob(IOptions<AppSettings> appsetting, MongoDBContext context)
        {
            _context = context;
            _appsetting = appsetting.Value;
            client = new HttpClient();

        }


        public async Task Execute(IJobExecutionContext context)
        {
            //var log = new log_sync_hd_db();

            //if (_appsetting.auto_sync == true)
            //{
            //    string message = "Run SYNCTINTUC" + string.Format("Time: {0}", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"));
            //    message += Environment.NewLine;// + e.ToString();

            //    var filename = "Log" + DateTime.Now.Date.ToString("dd-MM-yyyy") + ".txt";
            //    var path = Path.Combine(_appsetting.folder_path, "Log");
            //    var path_save = Path.Combine(path, filename);
            //    if (!Directory.Exists(path))
            //        Directory.CreateDirectory(path);
            //    System.IO.File.AppendAllText(path_save, message);






            //    Thread.Sleep(2000);

            //}


        }



        public long? getDateTime(DateTime currentime)
        {
            long unixTimeMilliseconds = new DateTimeOffset(currentime).ToUnixTimeMilliseconds();
            return unixTimeMilliseconds * 1000;
        }
        public string getRssTinTuc(string CatalogNewsId)
        {
            var name = "";
            var tt1 = "https://moet.gov.vn/rss/Pages/index.aspx?ItemID=54";
            var tt2 = "https://moet.gov.vn/rss/Pages/index.aspx?ItemID=56";
            var tt3 = "https://moet.gov.vn/rss/Pages/index.aspx?ItemID=57";
            var tt4 = "https://moet.gov.vn/rss/Pages/index.aspx?ItemID=58";
            var tt5 = "https://moet.gov.vn/rss/Pages/index.aspx?ItemID=59";

            var tt = tt1 + ";" + tt2 + ";" + tt3 + ";" + tt4 + ";" + tt5;
            switch (CatalogNewsId)
            {
                case "tin_tuc":
                    name = tt;
                    break;
                case "van_ban_quy_pham":
                    name = "https://moet.gov.vn/rss/Pages/index.aspx?ItemID=56";
                    break;
                case "van_ban_chi_dao":
                    name = "https://moet.gov.vn/rss/Pages/index.aspx?ItemID=57";
                    break;


                default:
                    name = "";
                    break;
            }


            return name;
        }
        public string getRssVanBan(int? CatalogNewsId)
        {
            var name = "";

            switch (CatalogNewsId)
            {
                case 1:
                    name = "https://moet.gov.vn/rss/Pages/index.aspx?ItemID=55";
                    break;
                case 2:
                    name = "https://moet.gov.vn/rss/Pages/index.aspx?ItemID=60";
                    break;

                default:
                    name = "";
                    break;
            }


            return name;
        }
    }

}

