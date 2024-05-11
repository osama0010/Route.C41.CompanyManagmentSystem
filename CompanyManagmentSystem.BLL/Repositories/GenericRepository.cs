using CompanyManagmentSystem.BLL.Interfaces;
using CompanyManagmentSystem.DAL.Data;
using CompanyManagmentSystem.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;

namespace CompanyManagmentSystem.BLL.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : ModelBase
    {
        private protected readonly ApplicationDbContext _dbContext;
        public GenericRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void Add(T entity)
            => _dbContext.Set<T>().Add(entity); 

        #region MyRegion
        //_dbContext.Add(entity); // EF Core 3.1 NEW Feature
        #endregion
        public void Update(T entity)
            => _dbContext.Set<T>().Update(entity);
        public void Delete(T entity)
            => _dbContext.Set<T>().Remove(entity);
        public IEnumerable<T> GetAll()
        {
            if(typeof(T) == typeof(Employee))
            {
                return(IEnumerable<T>)_dbContext.Employees.Include(e => e.Department).ToList();
            }
            return _dbContext.Set<T>().AsNoTracking().ToList();

        }
        public T Get(int id)
            => _dbContext.Find<T>(id);
    }
}
