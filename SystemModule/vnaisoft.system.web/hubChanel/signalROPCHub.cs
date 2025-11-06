using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Opc.UaFx;
using Opc.UaFx.Client;

namespace vnaisoft.system.web.hubChanel
{

    public class signalROPCHub : Hub
    {
     
        public override async Task OnConnectedAsync()
        {
            //var builder = new ConfigurationBuilder()
            //     .SetBasePath(Directory.GetCurrentDirectory())
            //     .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            //     .AddEnvironmentVariables();
            //IConfiguration config = builder.Build();
            //var opcClient = config["AppSettings:opcClient"];
            //var opcnode = config["AppSettings:opcnode"];
            //var variable = config["AppSettings:variable"];


            await base.OnConnectedAsync();
        }
        public async Task SendMessage(string user)
        {
          
        }
       
     
   
    }
}
