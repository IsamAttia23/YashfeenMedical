using YashfeenMedical.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using YashfeenMedical.DAL.ModelsConfigurations;

namespace YashfeenMedical.DAL
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Specialty> Specialties { get; set; }
        public DbSet<DoctorSpecialty> DoctorSpecialties { get; set; }
        public DbSet<DoctorSchedule> DoctorSchedules { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<MedicalRecord> MedicalRecords { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<PrescriptionItem> PrescriptionItems { get; set; }
        public DbSet<Medication> Medications { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceItem> InvoiceItems { get; set; }
        public DbSet<InsurancePolicy> InsurancePolicies { get; set; }
        public DbSet<MedicalFile> MedicalFiles { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>().ToTable("Users", "security")
                .Property(p => p.PhoneNumber).IsRequired();
            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims", "security");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles", "security");
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins", "security");
            builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens", "security");
            builder.Entity<IdentityRole>().ToTable("Roles", "security");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims", "security");

            new AppointmentConfiguration().Configure(builder.Entity<Appointment>());
            new DoctorConfiguration().Configure(builder.Entity<Doctor>());
            new DoctorScheduleConfiguration().Configure(builder.Entity<DoctorSchedule>());
            new DoctorSpecialtyConfiguration().Configure(builder.Entity<DoctorSpecialty>());
            new InsurancePolicyConfiguration().Configure(builder.Entity<InsurancePolicy>());
            new InvoiceConfiguration().Configure(builder.Entity<Invoice>());
            new InvoiceItemConfiguration().Configure(builder.Entity<InvoiceItem>());
            new MedicalFileConfiguration().Configure(builder.Entity<MedicalFile>());
            new MedicalRecordConfiguration().Configure(builder.Entity<MedicalRecord>());
            new MedicationConfiguration().Configure(builder.Entity<Medication>());
            new PatientConfiguration().Configure(builder.Entity<Patient>());
            new PrescriptionConfiguration().Configure(builder.Entity<Prescription>());
            new PrescriptionItemConfiguration().Configure(builder.Entity<PrescriptionItem>());
            new SpecialtyConfiguration().Configure(builder.Entity<Specialty>());
        }

    }
}
