using System;
using System.Collections.Generic;
using System.Text;
using YashfeenMedical.DAL.Repositories;

namespace YashfeenMedical.DAL.IRepositories
{
    public interface IUnitOfWork : IDisposable
    {
        IPatientRepository Patients { get; }
        Task<int> SaveChangesAsync();

        Task BeginTransactionAsync();

        Task CommitTransactionAsync();

        Task RollbackTransactionAsync();
    }
}
