using System;
using System.Collections.Generic;
using System.Text;
using YashfeenMedical.DAL.Repositories;

namespace YashfeenMedical.DAL.IRepositories
{
    public interface IUnitOfWork : IDisposable
    {
        IPatientRepository Patients { get; }
        IDoctorRepository Doctors { get; }
        ISpecialtyRepository Specialties { get; }
        IDoctorScheduleRepository DoctorSchedules { get; } 
        IAppointmentRepository Appointments { get; }
        IInvoiceRepository Invoices { get; }
        IMedicalRecordRepository MedicalRecords { get; }

        Task<int> SaveChangesAsync();

        Task BeginTransactionAsync();

        Task CommitTransactionAsync();

        Task RollbackTransactionAsync();
    }
}
