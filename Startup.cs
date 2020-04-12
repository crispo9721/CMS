using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMS.Context;
using CMS.Models.Db.Account;
using CMS.Services;
using CMS.Services.interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CMS
{
    public class Startup
    {
        // Wstrzykiwanie pliku konfiguracyjnego do naszej aplikacji
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // ��czenie z baz� danych
            services.AddDbContext<CMSContext>(builder =>
            {
                // Dodajemy dostawc� do obs�ugi MySql'a i przekazujemy connection string pobrany z pliku konfiguracyjnego z naszej aplikacji
                builder.UseMySql(Configuration.GetConnectionString("DefaultConnection"));
            });

            // Wstrzykujemy zale�no�ci o identifykacji u�ytkownik�w
            services.AddIdentity<User, IdentityRole>(options =>
            {
                // Opcje dotycz�ce has�a
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 2;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;

            }).AddEntityFrameworkStores<CMSContext>();

            // Dodajemy obs�ug� silnika razor oraz controller�w
            services.AddRazorPages();
            services.AddControllersWithViews();

            // Tutaj dodajemy zale�no�ci do wstrzykiwania
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ITagService, TagService>();
            services.AddScoped<IArticleService, ArticleService>();
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // dodajemy obs�ug� plik�w statycznych z ~/wwwroot
            app.UseStaticFiles();

            // w��czamy u�ywanie autoryzacji i autentykacji z .NET Core Identity
            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                // ustawiamy routing na domy�lny Model - View - Controller
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
