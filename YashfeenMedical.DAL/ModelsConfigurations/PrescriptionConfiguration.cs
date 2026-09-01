using YashfeenMedical.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace YashfeenMedical.DAL.ModelsConfigurations
{
    internal class PrescriptionConfiguration : IEntityTypeConfiguration<Prescription>
    {
        public void Configure(EntityTypeBuilder<Prescription> builder)
        {
            builder.HasOne(p => p.MedicalRecord)
                   .WithMany(mr => mr.Prescriptions)
                   .HasForeignKey(p => p.MedicalRecordId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(p => p.PrescriptionNumber).IsUnique();
            builder.Property(p => p.PrescriptionNumber).IsRequired().HasMaxLength(30);
            builder.Property(p => p.Status).HasConversion<string>().HasMaxLength(15);

        }
    }
}
