using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMS.Context;
using CMS.Models.Db.Account;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CMS
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var host = CreateWebHostBuilder(args).Build();
            //var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<Program>>();

                try
                {
                    var context = services.GetRequiredService<CMSContext>();
                    var userManager = services.GetRequiredService<UserManager<User>>();

                    context.Database.Migrate();
                    Seed.SeedData(context, userManager).Wait();

                    logger.LogInformation("Migracja zako�czona pomy�lnie");

                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Wystapil problem przy migracji danych.");
                }
            }

            host.Run();
        }

        //public static IHostBuilder CreateHostBuilder(string[] args) =>
        //    Host.CreateDefaultBuilder(args)
        //        .ConfigureWebHostDefaults(webBuilder =>
        //        {
        //            webBuilder.UseStartup<Startup>();
        //        })
        //        .ConfigureAppConfiguration(configurationBuilder => { configurationBuilder.AddEnvironmentVariables("ASPNETCORE_CMS_"); }); 


        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args).UseStartup<Startup>();
    }
}

