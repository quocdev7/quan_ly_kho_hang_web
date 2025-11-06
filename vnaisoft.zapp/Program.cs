using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using vnaisoft.common.BaseClass;
using vnaisoft.common.Models;
using vnaisoft.system.web.MenuAndRole;

namespace vnaisoft.zapp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var currrent = System.IO.Directory.GetCurrentDirectory();
            var pathgoogle = Path.Combine(currrent, "firebaseShungo.json");
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", pathgoogle);
            ListControlller.list = new List<ControllerAppModel>();



            ListControlller.list.AddRange(SystemListController.listController);


            ListControlller.listpublicactioncontroller = new List<string>();
            ListControlller.listnonloginpublicactioncontroller = new List<string>();
            for (int i = 0; i < ListControlller.list.Count; i++)
            {
                ListControlller.listpublicactioncontroller.AddRange(ListControlller.list[i].list_controller_action_public.Select(d => d.ToLower()));
                ListControlller.listnonloginpublicactioncontroller.AddRange(ListControlller.list[i].list_controller_action_publicNonLogin.Select(d => d.ToLower()));
            }
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
