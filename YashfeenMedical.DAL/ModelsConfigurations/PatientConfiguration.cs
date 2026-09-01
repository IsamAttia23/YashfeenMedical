using YashfeenMedical.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace YashfeenMedical.DAL.ModelsConfigurations
{
    public class PatientConfiguration : IEntityTypeConfiguration<Patient>
    {
        public void Configure(EntityTypeBuilder<Patient> builder)
        {
            builder.HasOne(p => p.ApplicationUser)
                  .WithOne(u => u.Patient)
                  .HasForeignKey<Patient>(p => p.UserId)
                  .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(p => p.UserId).IsUnique();
            builder.HasIndex(p => p.NationalId).IsUnique();

            builder.Property(p => p.FullName).IsRequired().HasMaxLength(200);
            builder.Property(p => p.NationalId).IsRequired().HasMaxLength(50);
            builder.Property(p => p.Gender).HasConversion<string>().HasMaxLength(10);
            builder.Property(p => p.BloodType).HasConversion<string>().HasMaxLength(20);
        }
    }
}
