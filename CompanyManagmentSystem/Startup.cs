using CompanyManagmentSystem.BLL.Interfaces;
using CompanyManagmentSystem.BLL.Repositories;
using CompanyManagmentSystem.DAL.Data;
using CompanyManagmentSystem.DAL.Models;
using CompanyManagmentSystem.PL.Helpers;
using CompanyManagmentSystem.PL.MappingProfiles;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyManagmentSystem
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the DI container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews(); //Register Built-In Services Required by MVC

            services.AddDbContext<ApplicationDbContext>(optionsAction =>
            optionsAction.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
            ); // Default param is Scoped
            
            services.AddApplicationServices();

            services.AddAutoMapper(typeof(Program).Assembly);


            //To Register (apply DI) UserManager, SignInManager, RoleManager and to add Configurations =
            services.AddIdentity<ApplicationUser, IdentityRole>(options => 
            {
                options.Password.RequiredUniqueChars = 2;
                options.Password.RequireDigit = true;
                options.Password.RequireNonAlphanumeric = true; //@%$
				options.Password.RequireUppercase = true;
				options.Password.RequiredLength = 5;

                options.Lockout.AllowedForNewUsers = true;
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromDays(5);

                options.User.RequireUniqueEmail = true;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/SignIn";
                options.ExpireTimeSpan = TimeSpan.FromDays(1);
                options.AccessDeniedPath = "/Home/Error";
            });

            //called by default by AddIdentity but this is called explicitly for specific overloads
            //services.AddAuthentication();
            #region MyRegion
            //		services.AddAuthentication(options =>
            //		{
            //			options.DefaultAuthenticateScheme = "AnotherSchema";
            //		})
            //.AddCookie("AnotherSchema", options =>
            //{
            //	options.LoginPath = "/Account/SignIn";
            //	options.ExpireTimeSpan = TimeSpan.FromDays(1);
            //	options.AccessDeniedPath = "/Home/Error";
            //}); 
            #endregion


            #region MyRegion
            //services.AddAutoMapper(m => m.AddProfile(new EmployeeProfile()));
            #endregion
        }

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
