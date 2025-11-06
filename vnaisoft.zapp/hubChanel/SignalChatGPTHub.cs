using Azure;
using Azure.AI.OpenAI;
using Google.Api.Gax.Grpc;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.AIPlatform.V1;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiktokenSharp;
using vnaisoft.common.Helpers;
using vnaisoft.DataBase.Mongodb;
using vnaisoft.DataBase.Mongodb.Collection.HocAI;

namespace vnaisoft.DataBase.hubChanel
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);




    public class ConverterVietNam
    {
        private char[] tcvnchars = {
            'µ', '¸', '¶', '·', '¹',
            '¨', '»', '¾', '¼', '½', 'Æ',
            '©', 'Ç', 'Ê', 'È', 'É', 'Ë',
            '®', 'Ì', 'Ð', 'Î', 'Ï', 'Ñ',
            'ª', 'Ò', 'Õ', 'Ó', 'Ô', 'Ö',
            '×', 'Ý', 'Ø', 'Ü', 'Þ',
            'ß', 'ã', 'á', 'â', 'ä',
            '«', 'å', 'è', 'æ', 'ç', 'é',
            '¬', 'ê', 'í', 'ë', 'ì', 'î',
            'ï', 'ó', 'ñ', 'ò', 'ô',
            '­', 'õ', 'ø', 'ö', '÷', 'ù',
            'ú', 'ý', 'û', 'ü', 'þ',
            '¡', '¢', '§', '£', '¤', '¥', '¦'};
        private char[] unichars = {
            'à', 'á', 'ả', 'ã', 'ạ',
            'ă', 'ằ', 'ắ', 'ẳ', 'ẵ', 'ặ',
            'â', 'ầ', 'ấ', 'ẩ', 'ẫ', 'ậ',
            'đ', 'è', 'é', 'ẻ', 'ẽ', 'ẹ',
            'ê', 'ề', 'ế', 'ể', 'ễ', 'ệ',
            'ì', 'í', 'ỉ', 'ĩ', 'ị',
            'ò', 'ó', 'ỏ', 'õ', 'ọ',
            'ô', 'ồ', 'ố', 'ổ', 'ỗ', 'ộ',
            'ơ', 'ờ', 'ớ', 'ở', 'ỡ', 'ợ',
            'ù', 'ú', 'ủ', 'ũ', 'ụ',
            'ư', 'ừ', 'ứ', 'ử', 'ữ', 'ự',
            'ỳ', 'ý', 'ỷ', 'ỹ', 'ỵ',
            'Ă', 'Â', 'Đ', 'Ê', 'Ô', 'Ơ', 'Ư'};
        private char[] convertTable;
        public ConverterVietNam()
        {
            convertTable = new char[256];
            for (int i = 0; i < 256; i++)
                convertTable[i] = (char)i;
            for (int i = 0; i < tcvnchars.Length; i++)
                convertTable[tcvnchars[i]] = unichars[i];
        }

    }
    public class HoiDap
    {
        public string code { get; set; }
        public string explanation { get; set; }
    }

    public class CauHoi
    {
        public string Question_Number { get; set; }
        public string Content { get; set; }
    }

    public class LstCauHoi
    {
        public List<CauHoi> data { get; set; }
    }



    public class Criterion
    {
        public string Element { get; set; }
    }

    public class Element
    {
        public string Description { get; set; }
        public string Element_Type { get; set; }
    }

    public class SectionR
    {
        public List<Section> Sections { get; set; }

    }
    public class TaskN
    {
        public string Question_Task { get; set; }
        public string Question_Number { get; set; }
        public List<Criterion> Criteria { get; set; }
        public object Scoring_Guide { get; set; }
        public string Additional_Scoring_Explanation { get; set; }
        public string Points_Total { get; set; }
        public string Partial_Credit_Allowed { get; set; }
        public string Question_Type { get; set; }
        public string Require_all_elements { get; set; }
    }
    public class Section
    {
        public string Section_ID { get; set; }
        public string Section_Title { get; set; }
        public string Points_Total { get; set; }
        public List<TaskN> Tasks { get; set; }
    }
    public class KetQua
    {

        public string Total_Score { get; set; }
    }


    public class TimeModel
    {
        public string id_bai_kiem_tra { get; set; }
        public int? thoi_gian_con_lai { get; set; }
    }

    public class SignalChatGPTHub : Hub
    {
        IDistributedCache _cache;
        public AppSettings _appsetting;
        MongoDBContext _contextMongo;

        public SignalChatGPTHub(IDistributedCache cache, MongoDBContext contextMongo, IOptions<AppSettings> appsetting)
        {
            _cache = cache;
            _contextMongo = contextMongo;
            _appsetting = appsetting.Value;
        }

        public override async Task OnConnectedAsync()
        {
            await addUserConnection();
            await base.OnConnectedAsync();
        }
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            Console.Write("ClientID " + Context.ConnectionId);
            await removeUserCacheConnection();
            await base.OnDisconnectedAsync(exception);
        }


        private async Task addUserConnection()
        {
            var connectionId = _cache.GetString("SignalChatGPT" + this.Context.User.Identity.Name);
            List<string> listconnection = new List<string>();
            if (!string.IsNullOrEmpty(connectionId))
            {
                listconnection.AddRange(connectionId.Split(","));
            }
            listconnection.Add(Context.ConnectionId);
            var listconnectionStore = listconnection.TakeLast(10).ToList();
            await _cache.SetStringAsync("SignalChatGPT" + this.Context.User.Identity.Name, string.Join(",", listconnectionStore));

            await Clients.All.SendAsync(ChatMethodEnum.Connected.ToString(), this.Context.User.Identity.Name);
        }

        private async Task removeUserCacheConnection()
        {
            var connectionId = _cache.GetString("SignalChatGPT" + this.Context.User.Identity.Name);
            List<string> listconnection = new List<string>();
            if (!string.IsNullOrEmpty(connectionId))
            {
                listconnection.AddRange(connectionId.Split(","));
            }
            listconnection.Remove(Context.ConnectionId);
            await _cache.SetStringAsync("SignalChatGPT" + this.Context.User.Identity.Name, string.Join(",", listconnection));
            if (listconnection.Count == 0)
            {
                await Clients.All.SendAsync(ChatMethodEnum.Disconnected.ToString(), this.Context.User.Identity.Name);
            }
        }

        private long getCurrentEpochMicrosecond()
        {
            var currentTime = DateTime.UtcNow;
            long unixTimeMilliseconds = new DateTimeOffset(currentTime).ToUnixTimeMilliseconds();
            return unixTimeMilliseconds * 1000;
        }







        private string ReplaceUserInfo(string noidungmau, string user_id)
        {

            var user = _contextMongo.sys_user_col.AsQueryable().Where(d => d.id == user_id).FirstOrDefault();
            //var ten_nganh_hoc = _contextMongo.sys_nganh_hocs.AsQueryable().Where(d => d.id == user.id_nganh_hoc).Select(t=>t.ten).FirstOrDefault()??"";
            //noidungmau= noidungmau.Replace("@@user_field@@", ten_nganh_hoc);

            //var id_lop = _contextMongo.sys_lop_hoc_sinh_col.AsQueryable().Where(d => d.id_hoc_sinh == user.id).Select(t => t.id_lop).FirstOrDefault() ?? "";
            //var ten_lop = _contextMongo.sys_lop_col.AsQueryable().Where(d => d.id == id_lop).Select(t => t.ten).FirstOrDefault() ?? "";
            noidungmau = noidungmau.Replace("@@user_class@@", "");
            return noidungmau;
        }





        public long? getDateTime(DateTime currentime)
        {
            long unixTimeMilliseconds = new DateTimeOffset(currentime).ToUnixTimeMilliseconds();
            return unixTimeMilliseconds * 1000;
        }





        private async Task sendUserMessageAsync(string user_id, string function_name, object data)
        {
            var connectionId = _cache.GetString("SignalChatGPT" + user_id);
            //var connectionId = _cache.GetString(user_id);
            List<string> listconnection = new List<string>();
            if (!string.IsNullOrEmpty(connectionId))
            {
                listconnection.AddRange(connectionId.Split(","));
            }
            for (int i = 0; i < listconnection.Count; i++)
            {
                if (!string.IsNullOrEmpty(listconnection[i].Trim())) await Clients.Client(listconnection[i]).SendAsync(function_name, data);
            }

        }





        public class sys_promt_model
        {
            public string giao_an { get; set; }
            public string cau_hoi { get; set; }
            public string qui_dinh { get; set; }
            public string hoc_sinh_lop { get; set; }
        }


        public static string ConvertToBase64(string imagePath)
        {
            byte[] imageBytes = File.ReadAllBytes(imagePath);
            return Convert.ToBase64String(imageBytes);
        }




    }


}
