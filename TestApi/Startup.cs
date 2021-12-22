using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.EntityFrameworkCore;
using TestApi.Data;
using TestApi.Auth;
using Hangfire.Dashboard;
using TestApi.Services.Interfaces;
using TestApi.Repositories.Interfaces;
using TestApi.Repositories;
using TestApi.Services;

namespace TestApi
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
            // Add Hangfire services.
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
                    DisableGlobalLocks = true
                }));

            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IAuthorizationService, AuthorizationService>();

            var connection = Configuration.GetConnectionString("DefaultDatabase");
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connection));

            // Add the processing server as IHostedService
            services.AddHangfireServer();

            services.AddSingleton<PrintJob>();

            // Add framework services.
            services.AddMvc();

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IBackgroundJobClient backgroundJobs,
            IRecurringJobManager recurringJobManager, IServiceProvider serviceProvider, IConfiguration configuration)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseHangfireDashboard("/hangfireAdmin", new DashboardOptions
            {
                Authorization = new[] { new DashboardAuthFilter(configuration) }
            });

            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = new[] { new DashboardAuthFilter(configuration) },
                IsReadOnlyFunc = (DashboardContext context) => true
            });

            backgroundJobs.Enqueue(() => Console.WriteLine("Hello world from Hangfire!"));
            recurringJobManager.AddOrUpdate(
                "Run every minute",
                () => serviceProvider.GetService<PrintJob>().Print(),
                "* * * * *"
                );


            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHangfireDashboard();
            });
        }
    }
}
