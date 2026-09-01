using YashfeenMedical.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace YashfeenMedical.DAL.ModelsConfigurations
{
    public class DoctorConfiguration : IEntityTypeConfiguration<Doctor>
    {
        public void Configure(EntityTypeBuilder<Doctor> builder)
        {
            builder.HasOne(d => d.ApplicationUser)
                 .WithOne(u => u.Doctor)
                 .HasForeignKey<Doctor>(d => d.UserId)
                 .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(d => d.UserId).IsUnique();
            builder.HasIndex(d => d.LicenseNumber).IsUnique();

            builder.Property(d => d.FullName).IsRequired().HasMaxLength(200);
            builder.Property(d => d.LicenseNumber).IsRequired().HasMaxLength(50);
            builder.Property(d => d.ConsultationFee).HasColumnType("decimal(10,2)");
        }
    }
}
