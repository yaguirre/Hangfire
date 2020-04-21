using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Hangfire;

namespace API_CRON
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();

            var options = new BackgroundJobServerOptions
            {
                SchedulePollingInterval = TimeSpan.FromSeconds(10),
            };

            using (var server = new BackgroundJobServer(options))
            {
                Console.WriteLine("Hangfire Server started. Press any key to exit...");
                Console.ReadLine();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
