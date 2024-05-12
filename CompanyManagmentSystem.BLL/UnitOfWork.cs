using CompanyManagmentSystem.BLL.Interfaces;
using CompanyManagmentSystem.BLL.Repositories;
using CompanyManagmentSystem.DAL.Data;
using CompanyManagmentSystem.DAL.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyManagmentSystem.BLL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;
        private Hashtable _repositories; 
        public UnitOfWork(ApplicationDbContext dbContext)// Ask CLR for Creating Object from DbContext
        {
            _dbContext = dbContext;
            _repositories = new Hashtable();
        }
        public IGenericRepository<T> Repository<T>() /*Repository<Employee>()*/ where T : ModelBase
        {
            var key = typeof(T).Name;
            #region Important Note
            //This is Without Specification Design Pattern 
            #endregion
            if (!_repositories.ContainsKey(key))
            {
                if (key == nameof(Employee))
                {
                    var repository = new EmployeeRepository(_dbContext);
                    _repositories.Add(key, repository);
                }
                else
                {
                    var repository = new GenericRepository<T>(_dbContext);
                    _repositories.Add(key, repository);
                }

            }

            return _repositories[key] as IGenericRepository<T>;
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
