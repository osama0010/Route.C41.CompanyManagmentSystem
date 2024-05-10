using AutoMapper;
using CompanyManagmentSystem.DAL.Models;
using CompanyManagmentSystem.PL.ViewModels;

namespace CompanyManagmentSystem.PL.MappingProfiles
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile() 
        {
            CreateMap<EmployeeViewModel, Employee>().ReverseMap();
        }
    }
}
