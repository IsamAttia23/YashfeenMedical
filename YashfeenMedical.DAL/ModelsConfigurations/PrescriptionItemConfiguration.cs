using YashfeenMedical.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace YashfeenMedical.DAL.ModelsConfigurations
{
    public class PrescriptionItemConfiguration : IEntityTypeConfiguration<PrescriptionItem>
    {
        public void Configure(EntityTypeBuilder<PrescriptionItem> builder)
        {
            builder.HasOne(pi => pi.Prescription)
                   .WithMany(p => p.Items)
                   .HasForeignKey(pi => pi.PrescriptionId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(pi => pi.Medication)
                  .WithMany(m => m.PrescriptionItems)
                  .HasForeignKey(pi => pi.MedicationId)
                  .OnDelete(DeleteBehavior.Restrict);

            builder.Property(pi => pi.Dosage).IsRequired().HasMaxLength(100);
            builder.Property(pi => pi.Frequency).IsRequired().HasMaxLength(100);
            builder.Property(pi => pi.Duration).IsRequired().HasMaxLength(50);

        }
    }
}
