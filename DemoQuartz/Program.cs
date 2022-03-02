using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using DemoQuartz.Quartz;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using DemoQuartz.Ip;
using DemoQuartz.DB;

namespace DemoQuartz
{
    public class Program
    {
        static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
           Host.CreateDefaultBuilder(args)
               .ConfigureServices((hostContext, services) =>
               {
                   IConfiguration configuration = hostContext.Configuration;
                   var dbOption = new DbOptions();
                   var configs = new Configs();
                   configuration.Bind("ConnectionStrings:SqlServer", dbOption);
                   configuration.Bind("Config", configs);
                   services.AddSingleton(dbOption);
                   services.AddSingleton(configs);
                   services.AddSingleton<IDatabase, Database>();
                   services.AddQuartz();
                   services.AddHostedService<QuartzHostedService>();
                   services.AddHostedService<Worker>();
                   services.AddIP();
               });
    }
}
