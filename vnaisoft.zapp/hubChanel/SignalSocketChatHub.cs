using Azure;
using Azure.AI.OpenAI;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using TiktokenSharp;
using vnaisoft.common.Helpers;
using vnaisoft.DataBase.Mongodb;
using vnaisoft.DataBase.Mongodb.Collection.HocAI;
using vnaisoft.DataBase.System;

using static System.Net.WebRequestMethods;



namespace vnaisoft.DataBase.hubChanel
{



    //class ChatMethodReceive
    //{
    //    static final Connected = "Connected";
    //    static final Disconnected = "Disconnected";
    //    static final sendMessage = "sendMessage";
    //    static final reactionMessage = "reactionMessage";
    //    static final change_convertsation = "change_convertsation";
    //    static final remove_user_convertsation = "remove_user_convertsation";

    //    static final ErrorSendMessage = "ErrorSendMessage";
    //}

    //class ChatMethodSend
    //{
    //    static final SendMessage = "SendMessage";
    //    static final EditMessage = "EditMessage";
    //    static final SendMessageFile = "SendMessageFile";
    //    static final create_convertsation = "create_convertsation";
    //    static final update_convertsation = "update_convertsation";
    //    static final invate_user_group = "invate_user_group";
    //    static final leave_group = "leave_group";
    //    static final ReactionMessage = "ReactionMessage";
    //    static final removeReactionMessage = "removeReactionMessage";
    //}




    public enum ChatMethodEnum
    {
      Connected,
      Disconnected,
      sendMessage,
       reactionMessage,
      change_convertsation,
       remove_user_convertsation,
        ErrorSendMessage,

    }





    public class SignalSocketChatHub : Hub
    {
        IDistributedCache _cache;
        public AppSettings _appsetting;
        MongoDBContext _contextMongo;

        public SignalSocketChatHub(IDistributedCache cache, MongoDBContext contextMongo, IOptions<AppSettings> appsetting)
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
            var connectionId = _cache.GetString("SignalSocketChat" + this.Context.User.Identity.Name);
            List<string> listconnection = new List<string>();
            if (!string.IsNullOrEmpty(connectionId))
            {
                listconnection.AddRange(connectionId.Split(','));
            }
            listconnection.Add(Context.ConnectionId);
            var  listconnectionStore = listconnection.Take(10).ToList();
            await _cache.SetStringAsync("SignalSocketChat" + this.Context.User.Identity.Name, string.Join(",", listconnectionStore));

            await Clients.All.SendAsync(ChatMethodEnum.Connected.ToString(), this.Context.User.Identity.Name);
        }

        private async Task removeUserCacheConnection()
        {
            var connectionId = _cache.GetString("SignalSocketChat" + this.Context.User.Identity.Name);
            List<string> listconnection = new List<string>();
            if (!string.IsNullOrEmpty(connectionId))
            {
                listconnection.AddRange(connectionId.Split(','));
            }
            listconnection.Remove(Context.ConnectionId);
            await _cache.SetStringAsync("SignalSocketChat" + this.Context.User.Identity.Name, string.Join(",", listconnection));
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
        private async Task sendUserMessageAsync(string user_id, string function_name, object data)
        {
            var connectionId = _cache.GetString("SignalSocketChat" + user_id);
            List<string> listconnection = new List<string>();
            if (!string.IsNullOrEmpty(connectionId))
            {
                listconnection.AddRange(connectionId.Split(','));
            }
            for (int i = 0; i < listconnection.Count; i++)


            {
                if (!string.IsNullOrEmpty(listconnection[i].Trim())) await Clients.Client(listconnection[i]).SendAsync(function_name, data);
            }
        }

    }



}
