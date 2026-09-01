using YashfeenMedical.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace YashfeenMedical.DAL.ModelsConfigurations
{
    public class MedicalFileConfiguration : IEntityTypeConfiguration<MedicalFile>
    {
        public void Configure(EntityTypeBuilder<MedicalFile> builder)
        {
            builder.HasOne(mf => mf.Patient)
                    .WithMany(p => p.MedicalFiles)
                    .HasForeignKey(mf => mf.PatientId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(mf => mf.Appointment)
                  .WithMany(a => a.MedicalFiles)
                  .HasForeignKey(mf => mf.AppointmentId)
                  .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(mf => mf.UploadedByUser)
                  .WithMany(u => u.UploadedFiles)
                  .HasForeignKey(mf => mf.UploadedByUserId)
                  .OnDelete(DeleteBehavior.Restrict);

            builder.Property(mf => mf.FileType).HasConversion<string>().HasMaxLength(15);
            builder.Property(mf => mf.FileName).IsRequired().HasMaxLength(300);
            builder.Property(mf => mf.StoredFileName).IsRequired().HasMaxLength(300);
            builder.Property(mf => mf.FileUrl).IsRequired().HasMaxLength(500);
            builder.Property(mf => mf.MimeType).IsRequired().HasMaxLength(100);

        }
    }
}
