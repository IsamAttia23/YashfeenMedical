using YashfeenMedical.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace YashfeenMedical.DAL.ModelsConfigurations
{
    public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
    {
        public void Configure(EntityTypeBuilder<Invoice> builder)
        {
            builder.HasOne(i => i.Appointment)
                  .WithOne(a => a.Invoice)
                  .HasForeignKey<Invoice>(i => i.AppointmentId)
                  .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(i => i.Patient)
                  .WithMany(p => p.Invoices)
                  .HasForeignKey(i => i.PatientId)
                  .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(i => i.AppointmentId).IsUnique();
            builder.HasIndex(i => i.InvoiceNumber).IsUnique();

            builder.Property(i => i.InvoiceNumber).IsRequired().HasMaxLength(30);
            builder.Property(i => i.PaymentStatus).HasConversion<string>().HasMaxLength(15);
            builder.Property(i => i.PaymentMethod).HasConversion<string>().HasMaxLength(15);

            foreach (var money in new[]
                   {
                         nameof(Invoice.SubTotal), nameof(Invoice.DiscountAmount),
                         nameof(Invoice.InsuranceCoverage), nameof(Invoice.PatientShare),
                         nameof(Invoice.TaxAmount), nameof(Invoice.TotalAmount), nameof(Invoice.PaidAmount)
                     })
            {
                builder.Property(money).HasColumnType("decimal(10,2)");
            }

        }
    }
}
