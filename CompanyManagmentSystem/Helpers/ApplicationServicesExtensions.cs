using CompanyManagmentSystem.BLL;
using CompanyManagmentSystem.BLL.Interfaces;
using CompanyManagmentSystem.BLL.Repositories;
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

            return services;
        }
    }
}
