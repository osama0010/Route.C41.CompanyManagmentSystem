﻿using CompanyManagmentSystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyManagmentSystem.BLL.Interfaces
{
    public interface IUnitOfWork: IAsyncDisposable
    {
        IGenericRepository<T> Repository<T>() where T : ModelBase;
        Task<int> Complete();
    }
}
