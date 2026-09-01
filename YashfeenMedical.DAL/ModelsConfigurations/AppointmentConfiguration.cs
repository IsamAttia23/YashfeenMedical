using YashfeenMedical.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace YashfeenMedical.DAL.ModelsConfigurations
{
    public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
    {
        public void Configure(EntityTypeBuilder<Appointment> builder)
        {
            builder.HasOne(a => a.Patient)
                  .WithMany(p => p.Appointments)
                  .HasForeignKey(a => a.PatientId)
                  .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.Doctor)
                  .WithMany(d => d.Appointments)
                  .HasForeignKey(a => a.DoctorId)
                  .OnDelete(DeleteBehavior.Restrict);

            builder.Property(a => a.Status).HasConversion<string>().HasMaxLength(15);
            builder.Property(a => a.Type).IsRequired().HasMaxLength(50);
            builder.Property(a => a.ReasonForVisit).IsRequired().HasMaxLength(500);

            builder.HasIndex(a => new { a.DoctorId, a.AppointmentDate, a.StartTime });

        }
    }
}
