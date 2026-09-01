using YashfeenMedical.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace YashfeenMedical.DAL.ModelsConfigurations
{
    public class InsurancePolicyConfiguration : IEntityTypeConfiguration<InsurancePolicy>
    {
        public void Configure(EntityTypeBuilder<InsurancePolicy> builder)
        {
            builder.HasOne(ip => ip.Patient)
                  .WithOne(p => p.InsurancePolicy)
                  .HasForeignKey<InsurancePolicy>(ip => ip.PatientId)
                  .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(ip => ip.PatientId).IsUnique();
            builder.HasIndex(ip => ip.PolicyNumber).IsUnique();

            builder.Property(ip => ip.InsuranceCompany).IsRequired().HasMaxLength(200);
            builder.Property(ip => ip.PolicyNumber).IsRequired().HasMaxLength(50);
            builder.Property(ip => ip.Status).HasConversion<string>().HasMaxLength(15);
            builder.Property(ip => ip.CoveragePercent).HasColumnType("decimal(5,2)");
            builder.Property(ip => ip.MaxAnnualCoverage).HasColumnType("decimal(10,2)");
            builder.Property(ip => ip.UsedCoverage).HasColumnType("decimal(10,2)");

        }
    }
}
