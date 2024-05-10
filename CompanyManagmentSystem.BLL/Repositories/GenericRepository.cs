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
        public int Add(T entity)
        {
            _dbContext.Set<T>().Add(entity);
            //_dbContext.Add(entity); // EF Core 3.1 NEW Feature
            return _dbContext.SaveChanges();
        }
        public int Update(T entity)
        {
            _dbContext.Set<T>().Update(entity);
            return _dbContext.SaveChanges();
        }
        public int Delete(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            return _dbContext.SaveChanges();
        }
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
