using YashfeenMedical.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace YashfeenMedical.DAL.ModelsConfigurations
{
    public class MedicationConfiguration : IEntityTypeConfiguration<Medication>
    {
        public void Configure(EntityTypeBuilder<Medication> builder)
        {
            builder.HasIndex(m => m.Name).IsUnique();
            builder.Property(m => m.Name).IsRequired().HasMaxLength(200);
            builder.Property(m => m.GenericName).IsRequired().HasMaxLength(200);

        }
    }
}
