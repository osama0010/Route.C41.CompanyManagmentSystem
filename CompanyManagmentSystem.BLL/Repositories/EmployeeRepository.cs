using CompanyManagmentSystem.BLL.Interfaces;
using CompanyManagmentSystem.DAL.Models;
using CompanyManagmentSystem.DAL.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using System;

namespace CompanyManagmentSystem.BLL.Repositories
{
    public class EmployeeRepository : GenericRepository<Employee> , IEmployeeRepository
    {

        public EmployeeRepository(ApplicationDbContext dbContext)
            :base(dbContext) 
        {
        }
        public IQueryable<Employee> GetEmployeesByAddress(string address)
        {
            return _dbContext.Employees.Where(E => E.Address.ToLower() == address.ToLower());
        }

        public IQueryable<Employee> SearchByName(string name)
            => _dbContext.Employees.Where(E => E.Name.ToLower().Contains(name));
    }
}
