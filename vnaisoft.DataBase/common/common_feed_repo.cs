using MongoDB.Driver;
using System;using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using vnaisoft.DataBase.Mongodb;
using System.Text.RegularExpressions;
using System.Web;
using HtmlAgilityPack;

namespace vnaisoft.DataBase.commonFunc
{


    public class common_feed_repo
    {
        public MongoDBContext _context;

        public common_feed_repo(MongoDBContext context)
        {
            _context = context;
        }
          public class metata_model
        {
            public string id { get; set; }
            public string name { get; set; }
            public string content { get; set; }
            public string property { get; set; }

        }
        public string getCategoryBaoNongNghiep(int? CatalogNewsId)
        {
            var name = "";

            switch (CatalogNewsId)
            {
                case 1:
                    name = "Bản tin truyền hình";
                    break;
                case 2:
                    name = "Chăn nuôi";
                    break;
                case 3:
                    name = "Thú y";
                    break;
             
                case 4:
                    name = "Trồng trọt";
                    break;
               
                default:
                    name = "";
                    break;
            }


            return name;
        }
        public string getRss(int? CatalogNewsId)
        {
            var name = "";

            switch (CatalogNewsId)
            {
                case 1:
                    name = "https://nongnghiep.vn/ban-tin-truyen-hinh.rss";
                    break;
                case 2:
                    name = "https://nongnghiep.vn/chan-nuoi.rss";
                    break;
                case 3:
                    name = "https://nongnghiep.vn/thu-y.rss";
                    break;
            
                case 4:
                    name = "https://nongnghiep.vn/trong-trot.rss";
                    break;
                case 5:
                    name = "https://nongnghiep.vn/khoa-hoc---cong-nghe.rss";
                    break;
              
                    default:
                    name = "";
                    break;
            }


            return name;
        }
        public string getCategoryName(int? CatalogNewsId)
        {
            var name = "";

            switch (CatalogNewsId)
            {
                case 1:
                    name = "Tin tức nông nghiệp";
                    break;
                case 2:
                    name = "Tin tức lâm nghiệp";
                    break;
                case 3:
                    name = "Tin tức thủy lợi";
                    break;
                case 39:
                    name = "Tin tức phòng chống thiên tai";
                    break;
                case 4:
                    name = "Tin tức thủy sản";
                    break;
                case 5:
                    name = "Tin tức quản lí chất lượng";
                    break;
                case 7:
                    name = "Tin tức chế biến";
                    break;
                case 6:
                    name = "Tin tức phát triển nông thôn";
                    break;
                case 8:
                    name = "Tin tức khuyến nông";
                    break;
                default:
                    name = "";
                    break;
            }


            return name;
        }
        public string valid_url(string content)
        {
            var url = "";
            // Validate URL

            //Regex validateDateRegex = new Regex("^https?:\\/\\/(?:www\\.)?[-a-zA-Z0-9@:%._\\+~#=]{1,256}\\.[a-zA-Z0-9()]{1,6}\\b(?:[-a-zA-Z0-9()@:%_\\+.~#?&\\/=]*)$");
            //validateDateRegex.IsMatch("https://uibakery.io");

            // Extract URL from a string
            Regex extractDateRegex = new Regex("https?:\\/\\/(?:www\\.)?[-a-zA-Z0-9@:%._\\+~#=]{1,256}\\.[a-zA-Z0-9()]{1,6}\\b(?:[-a-zA-Z0-9()@:%_\\+.~#?&\\/=]*)");
            string[] extracted = extractDateRegex.Matches(content)
                .Cast<Match>()
                .Select(m => m.Value)
                .ToArray();

            url = String.Join(",", extracted);
            //Console.WriteLine(); // prints https://uibakery.io

            return url;
        }
    
    }
}
