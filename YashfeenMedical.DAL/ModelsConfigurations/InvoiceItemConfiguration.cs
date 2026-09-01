using YashfeenMedical.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace YashfeenMedical.DAL.ModelsConfigurations
{
    public class InvoiceItemConfiguration : IEntityTypeConfiguration<InvoiceItem>
    {
        public void Configure(EntityTypeBuilder<InvoiceItem> builder)
        {
            builder.HasOne(ii => ii.Invoice)
                   .WithMany(i => i.Items)
                   .HasForeignKey(ii => ii.InvoiceId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Property(ii => ii.Type).HasConversion<string>().HasMaxLength(15);
            builder.Property(ii => ii.Description).IsRequired().HasMaxLength(300);
            builder.Property(ii => ii.UnitPrice).HasColumnType("decimal(10,2)");
            builder.Property(ii => ii.DiscountPercent).HasColumnType("decimal(5,2)");
            builder.Property(ii => ii.Total).HasColumnType("decimal(10,2)");

        }
    }
}
