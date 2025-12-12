using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using quan_ly_kho.common.BaseClass;
using quan_ly_kho.common.Models;
using quan_ly_kho.system.web.MenuAndRole;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace quan_ly_kho.zapp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var currrent = Directory.GetCurrentDirectory();
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
