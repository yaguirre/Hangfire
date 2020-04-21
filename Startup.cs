using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Hangfire;
using Hangfire.SqlServer;
using RestSharp;
using API_CRON.Models;
using Newtonsoft.Json;

namespace API_CRON
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddControllers();
            services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(Configuration.GetConnectionString("HangfireConnection"), new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    UsePageLocksOnDequeue = true,
                    DisableGlobalLocks = true
                }));

            services.AddHangfireServer();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IBackgroundJobClient backgroundJobs, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseHangfireDashboard();
            app.UseHangfireServer();
            //backgroundJobs.Enqueue(() => Console.WriteLine("Hello world from Hangfire!"));
            //backgroundJobs.Schedule(() => Console.WriteLine("Hola Mundo en cuarentena"),TimeSpan.FromMinutes(2));

            //var client = new RestClient("https://api.covid19api.com/summary");
            //client.Timeout = -1;
            //var request = new RestRequest(Method.GET);
            //IRestResponse response = client.Execute(request);

            Global global = new Global(12, 3, 2, 33, 2, 3);
            //backgroundJobs.Schedule(() => Console.WriteLine(global), TimeSpan.FromMinutes(1));
            backgroundJobs.Schedule(() => callRestClient("Hello"), TimeSpan.FromMinutes(1));
            //backgroundJobs.Schedule(() => Console.WriteLine(global), TimeSpan.FromMinutes(2));
            //backgroundJobs.Schedule(() => Console.WriteLine(global), TimeSpan.FromMinutes(3));

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });            
        }

        public void callRestClient(string message)
        {
            var client = new RestClient("https://api.covid19api.com/summary");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            IRestResponse response = client.Execute(request);
        }
    }
}
