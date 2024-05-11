using CompanyManagmentSystem.BLL.Interfaces;
using CompanyManagmentSystem.BLL.Repositories;
using CompanyManagmentSystem.DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyManagmentSystem.BLL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;

        public IEmployeeRepository EmployeeRepository { get; set; } = null;
        public IDepartmentRepository DepartmentRepository { get; set; } = null;
        public UnitOfWork(ApplicationDbContext dbContext)// Ask CLR for Creating Object from DbContext
        {
            _dbContext = dbContext;
            EmployeeRepository = new EmployeeRepository(_dbContext);
            DepartmentRepository = new DepartmentRepository(_dbContext);
        }
        public int Complete()
        {
            return _dbContext.SaveChanges();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
