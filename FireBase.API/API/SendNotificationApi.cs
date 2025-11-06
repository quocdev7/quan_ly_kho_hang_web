using Google.Cloud.Firestore;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using vnaisoft.DataBase.Mongodb;
using vnaisoft.DataBase.System;
using vnaisoft.fireBase.models;

namespace vnaisoft.fireBase.API
{
    public class SendNotificationApi
    {
        FirestoreDb fireStoreDb;
        public MongoDBContext _context;
        public SendNotificationApi(MongoDBContext context)
        {

            //fireStoreDb = getFirebase();
            _context = context;
        }
        public async Task update_fireBase_noti(List<string> listIdUser)
        {

            //listIdUser.RemoveAll(r => r == idUserSend);

            try
            {
                if (listIdUser.Count != 0)
                {

                    var userQuery = fireStoreDb.Collection("user_firebase")
                                                        .WhereIn("user_id", listIdUser);
                    var users = await userQuery.GetSnapshotAsync();
                    var listUser = users.ToList();

                    for (int i = 0; i < listUser.Count; i++)
                    {

                        //var colectionNoti = fireStoreDb.Collection("notification");
                        var user = JsonConvert.DeserializeObject<TokenNotiUserModel>(JsonConvert.SerializeObject(listUser[i].ToDictionary()));
                        //Update count notification
                        var update_noti = new Dictionary<string, object>
                        {
                            {"count_notification", FieldValue.Increment(1) },
                        };
                        await fireStoreDb.Collection("user_firebase")
                                            .Document(user.user_id).UpdateAsync(update_noti);

                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        //public FirestoreDb getFirebase()
        //{
        //    var currentpath = Directory.GetCurrentDirectory();
        //    string newPath = Path.Combine(currentpath);
        //    if (!Directory.Exists(newPath))
        //        Directory.CreateDirectory(newPath);
        //    //AppDomain.CurrentDomain.BaseDirectory
        //    var pathgoogle = newPath + @"\\firebaseShungo.json";

        //    Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", pathgoogle);
        //    FirestoreDb fireStoreDb;
        //    fireStoreDb = FirestoreDb.Create("shungo-982a5");

        //    return fireStoreDb;
        //}

        //public async Task send_notificatin_web(string useridSend, string type, dynamic model, List<string> listIdUser)
        //{
        //    var desc = "";
        //    var title = "";
        //    var link = "";
        //    var id_type = _context.sys_type_notifications.Where(q => q.code == type).Select(q => q.id).FirstOrDefault();
        //    var temp = _context.sys_template_notifications.Where(q => q.id_type == id_type).FirstOrDefault();
        //    var userid = useridSend;

        //    //su
        //    var su_kien = type == "13" || type == "14" || type == "15" || type == "16" || type == "17" || type == "18" || type == "19";
        //    if (su_kien == true)
        //    {
        //        string id_su_kien = model.id_su_kien;
        //        link = _context.sys_events.Where(q => q.id == id_su_kien).Select(q => q.logo).FirstOrDefault();

        //    }

        //    if (type == "1")
        //    {
        //        desc = temp.template;
        //    }
        //    else
        //    {
        //        //ABC.replace(A) = BC => BC
        //        //ABC.replace(A)
        //        desc = temp.template.Replace("@@ly_do_khong_duyet_profile@@", model.ly_do_khong_duyet_profile ?? "");
        //        desc = desc.Replace("@@ho_va_ten_nguoi_gui_loi_moi_ket_ban@@", model.ho_va_ten_nguoi_gui_loi_moi_ket_ban ?? "");

        //        desc = desc.Replace("@@ho_va_ten_nguoi_nhan_loi_moi_ket_ban@@", model.ho_va_ten_nguoi_nhan_loi_moi_ket_ban ?? "");
        //        desc = desc.Replace("@@ten_tin_tuc@@", model.ten_tin_tuc ?? "");
        //        desc = desc.Replace("@@ly_do_khong_duyet_tin@@", model.ly_do_khong_duyet_tin ?? "");
        //        desc = desc.Replace("@@ten_khoa@@", model.ten_khoa ?? "");
        //        desc = desc.Replace("@@ten_loai_tin_tuc@@", model.ten_loai_tin_tuc);

        //        desc = desc.Replace("@@ly_do_xoa_binh_luan@@", model.ly_do_xoa_binh_luan);
        //        desc = desc.Replace("@@ho_va_ten_nguoi_gui_loi_moi_tham_gia_su_kien@@", model.ho_va_ten_nguoi_gui_loi_moi_tham_gia_su_kien);
        //        desc = desc.Replace("@@ten_su_kien@@", model.ten_su_kien);
        //        desc = desc.Replace("@@ly_do_khong_duyet_dang_ky_tham_gia@@", model.ly_do_khong_duyet_dang_ky_tham_gia);
        //        desc = desc.Replace("@@ho_va_ten_nguoi_nhan_loi_moi_tham_gia_su_kien@@", model.ho_va_ten_nguoi_nhan_loi_moi_tham_gia_su_kien);
        //        desc = desc.Replace("@@ho_va_ten_nguoi_tham_gia_su_kien@@", model.ho_va_ten_nguoi_tham_gia_su_kien);
        //        desc = desc.Replace("@@ly_do_huy_bo_su_kien@@ ", model.ly_do_huy_bo_su_kien);

        //    }


        //    if (id_type != null)
        //    {

        //        for (int i = 0; i < listIdUser.Count; i++)
        //        {
        //            var model_noti_web = new sys_notification_db();
        //            model_noti_web.title = title;
        //            model_noti_web.description = desc;
        //            model_noti_web.user_id_send = useridSend;
        //            model_noti_web.param = type;
        //            model_noti_web.date_send = DateTime.Now;
        //            model_noti_web.title = title;
        //            model_noti_web.link = link;
        //            model_noti_web.menu = type;
        //            model_noti_web.status_del = 1;
        //            model_noti_web.user_id = listIdUser[i];
        //            model_noti_web.id = Guid.NewGuid().ToString();
        //            if (type == "4")
        //            {
        //                model_noti_web.link = "/portal-profile-user/" + useridSend;

        //            }
        //            _context.sys_notifications.Add(model_noti_web);
        //            _context.SaveChanges();


        //        }
        //    }
        //    sendNotification(userid, listIdUser, title, desc);



        //}
        public async Task send_notificatin_web(string useridSend, string menu, string title, string description, string link, string logo, DateTime? ngaydang, string param, string id_thong_bao, List<string> listIdUser)
        {
            var userid = useridSend;

            for (int i = 0; i < listIdUser.Count; i++)
            {
                var model_noti_web = new sys_notification_db();
                model_noti_web.title = title;
                model_noti_web.description = description;
                model_noti_web.user_id_send = useridSend;
                model_noti_web.param = param;
                model_noti_web.id_thong_bao = id_thong_bao;
                // 1 chưa xem 2 đã xem
                model_noti_web.check_xem = 1;
                model_noti_web.logo = logo;
                model_noti_web.date_send = ngaydang;
                model_noti_web.link = link;
                model_noti_web.menu = menu;
                model_noti_web.status_del = 1;
                model_noti_web.user_id = listIdUser[i];
                model_noti_web.id = ObjectId.GenerateNewId().ToString();
                _context.sys_notification_col.InsertOne(model_noti_web);
            }
            await sendNotification(userid, listIdUser, title, description);
        }
        public async Task sendNotification(string idUserSend, List<string> listIdUser, string title, string desc)
        {
            try
            {
                if (listIdUser.Count != 0)
                {
                    var userQuery = fireStoreDb.Collection("user_firebase")
                                                        .WhereIn("user_id", listIdUser);
                    var users = await userQuery.GetSnapshotAsync();
                    var listUser = users.ToList();

                    //Add list user
                    //for (int i = 0; i < listUser.Count; i++)
                    //{
                    //    string json = JsonConvert.SerializeObject(listUser[i].ToDictionary());
                    //    var userModel = JsonConvert.DeserializeObject<TokenNotiUserModel>(json);
                    //    var listDeviceQuery = await fireStoreDb.Collection($"token_noti_user/{domain + userModel.user_id}/sub_device").GetSnapshotAsync();
                    //    var listDeviceSnap = listDeviceQuery.ToList();
                    //    for (int j = 0; j < listDeviceSnap.Count; j++)
                    //    {
                    //        string listDeviceSnapJson = JsonConvert.SerializeObject(listDeviceSnap[j].ToDictionary());
                    //        userModel.listDevice.Add(JsonConvert.DeserializeObject<TokenNotiDeviceUser>(listDeviceSnapJson));
                    //    }
                    //    listUserModel.Add(userModel);
                    //}

                    var now = DateTime.Now;
                    TimeZone localZone = TimeZone.CurrentTimeZone;
                    TimeSpan offset = localZone.GetUtcOffset(DateTime.Now);
                    var currentDate = now.AddHours(offset.TotalHours);
                    var listnotify = new List<NotificationModel>();
                    for (int i = 0; i < listUser.Count; i++)
                    {

                        //var colectionNoti = fireStoreDb.Collection("notification");
                        var user = JsonConvert.DeserializeObject<TokenNotiUserModel>(JsonConvert.SerializeObject(listUser[i].ToDictionary()));
                        //Update count notification
                        var update_noti = new Dictionary<string, object>
                        {
                            {"count_notification", FieldValue.Increment(1) },
                        };
                        await fireStoreDb.Collection("user_firebase")
                                            .Document(user.user_id).UpdateAsync(update_noti);
                        await NotifyAsync(user.token_firebase ?? "", new { }, title, "");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        public String getSubstring(String text)
        {
            var start = ">";
            var end = "</";
            int Pos1 = text.IndexOf(start) + start.Length;
            int Pos2 = text.IndexOf(end);
            if (Pos1 == -1 || Pos2 == -1)
            {
                return text;
            }
            var FinalString = text.Substring(Pos1, Pos2 - Pos1);

            return FinalString;
        }
        private static string HtmlToPlainText(string html)
        {
            const string tagWhiteSpace = @"(>|$)(\W|\n|\r)+<";//matches one or more (white space or line breaks) between '>' and '<'
            const string stripFormatting = @"<[^>]*(>|$)";//match any character between '<' and '>', even when end tag is missing
            const string lineBreak = @"<(br|BR)\s{0,1}\/{0,1}>";//matches: <br>,<br/>,<br />,<BR>,<BR/>,<BR />
            var lineBreakRegex = new Regex(lineBreak, RegexOptions.Multiline);
            var stripFormattingRegex = new Regex(stripFormatting, RegexOptions.Multiline);
            var tagWhiteSpaceRegex = new Regex(tagWhiteSpace, RegexOptions.Multiline);

            var text = html;
            //Decode html specific characters
            text = System.Net.WebUtility.HtmlDecode(text);
            //Remove tag whitespace/line breaks
            text = tagWhiteSpaceRegex.Replace(text, "><");
            //Replace <br /> with line breaks
            text = lineBreakRegex.Replace(text, Environment.NewLine);
            //Strip formatting
            text = stripFormattingRegex.Replace(text, string.Empty);

            return text;
        }
        public async Task<bool> NotifyAsync(string token, dynamic data, string title, string msg)
        {
            try
            {
                // Get the server key from FCM console
                var serverKey = string.Format("key={0}", "AAAANHVpYME:APA91bHYL9H7bgiE0GVD5TRNdHZ4fiIQJ1pc5JFjAl2GaCCdqCBQ1kYPCwHi9bE8pkyFTJC_sn6-OqOOX6iyEVu6LP7XSVDwiaNd8lYYRlhPcEcaCTfaRxUWxtCZNb4KTmBCBZu4Ae5Q");

                // Get the sender id from FCM console
                var senderId = string.Format("id={0}", "225308139713");

                var jsonBody1 = new
                {
                    notification = new
                    {
                        body = HtmlToPlainText(msg),
                        title = HtmlToPlainText(title)
                    },
                    priority = "high",
                    data = new { },
                    to = token
                };

                // Using Newtonsoft.Json
                var jsonBody = JsonConvert.SerializeObject(jsonBody1);

                using (var httpRequest = new HttpRequestMessage(HttpMethod.Post, "https://fcm.googleapis.com/fcm/send"))
                {
                    httpRequest.Headers.TryAddWithoutValidation("Authorization", serverKey);
                    httpRequest.Headers.TryAddWithoutValidation("Sender", senderId);
                    httpRequest.Content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                    using (var httpClient = new HttpClient())
                    {
                        var result = await httpClient.SendAsync(httpRequest);

                        if (result.IsSuccessStatusCode)
                        {
                            return true;
                        }
                        else
                        {
                            // Use result.StatusCode to handle failure

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return false;
        }

    }
}
