using CompanyManagmentSystem.BLL;
using CompanyManagmentSystem.BLL.Interfaces;
using CompanyManagmentSystem.BLL.Repositories;
using CompanyManagmentSystem.DAL.Models;
using CompanyManagmentSystem.PL.Services.EmailSender;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace CompanyManagmentSystem.PL.Helpers
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            //services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            //services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IEmailSender, EmailSender>();


            return services;
        }
    }
}
