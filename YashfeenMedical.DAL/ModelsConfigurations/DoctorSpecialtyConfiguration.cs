using YashfeenMedical.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;

namespace YashfeenMedical.DAL.ModelsConfigurations
{
    public class DoctorSpecialtyConfiguration : IEntityTypeConfiguration<DoctorSpecialty>
    {
        public void Configure(EntityTypeBuilder<DoctorSpecialty> builder)
        {
            builder.HasKey(ds => new { ds.DoctorId, ds.SpecialtyId });

            builder.HasOne(ds => ds.Doctor)
                      .WithMany(d => d.DoctorSpecialties)
                      .HasForeignKey(ds => ds.DoctorId)
                      .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(ds => ds.Specialty)
                      .WithMany(s => s.DoctorSpecialties)
                      .HasForeignKey(ds => ds.SpecialtyId)
                      .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
