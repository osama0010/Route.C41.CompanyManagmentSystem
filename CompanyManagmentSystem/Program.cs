using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using CompanyManagmentSystem.PL.Helpers;
using CompanyManagmentSystem.DAL.Models;
using CompanyManagmentSystem.DAL.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using System;


namespace CompanyManagmentSystem
{
    public class Program
    {
        //Entry Point
        public static void Main(string[] args)
        {
            var WebApplicationBuilder = WebApplication.CreateBuilder(args);

            #region Configure Services
            WebApplicationBuilder.Services.AddControllersWithViews(); //Register Built-In Services Required by MVC

            WebApplicationBuilder.Services.AddDbContext<ApplicationDbContext>(optionsAction =>
            optionsAction.UseSqlServer(WebApplicationBuilder.Configuration.GetConnectionString("DefaultConnection"))
            ); // Default param is Scoped

            WebApplicationBuilder.Services.AddApplicationServices();

            WebApplicationBuilder.Services.AddAutoMapper(typeof(Program).Assembly);


            //To Register (apply DI) UserManager, SignInManager, RoleManager and to add Configurations =
            WebApplicationBuilder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
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

            WebApplicationBuilder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/SignIn";
                options.ExpireTimeSpan = TimeSpan.FromDays(1);
                options.AccessDeniedPath = "/Home/Error";
            });

            //called by default by AddIdentity but this is called explicitly for specific overloads
            //WebApplicationBuilder.Services.AddAuthentication();
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

            #endregion

            //Apply Migrations

            var app = WebApplicationBuilder.Build();

            #region Configure Kestrel Middleware

            if (app.Environment.IsDevelopment())
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
            #endregion

            app.Run(); // Application is Ready For Requests
        }
    }
}
