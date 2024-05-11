using CompanyManagmentSystem.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using System;

namespace CompanyManagmentSystem.BLL.Interfaces
{
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {
        IQueryable<Employee> GetEmployeesByAddress(string address);
        IQueryable<Employee> SearchByName(string name);
    }
}
