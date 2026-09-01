using YashfeenMedical.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace YashfeenMedical.DAL.ModelsConfigurations
{
    public class MedicalRecordConfiguration : IEntityTypeConfiguration<MedicalRecord>
    {
        public void Configure(EntityTypeBuilder<MedicalRecord> builder)
        {
            builder.HasOne(mr => mr.Appointment)
                   .WithOne(a => a.MedicalRecord)
                   .HasForeignKey<MedicalRecord>(mr => mr.AppointmentId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(mr => mr.AppointmentId).IsUnique();

            builder.Property(mr => mr.ChiefComplaint).IsRequired();
            builder.Property(mr => mr.Diagnosis).IsRequired();
            builder.Property(mr => mr.Temperature).HasColumnType("decimal(5,2)");
            builder.Property(mr => mr.Weight).HasColumnType("decimal(5,2)");
            builder.Property(mr => mr.Height).HasColumnType("decimal(5,2)");

        }
    }
}
