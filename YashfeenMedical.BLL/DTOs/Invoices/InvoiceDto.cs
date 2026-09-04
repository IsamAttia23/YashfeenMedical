using System;
using System.Collections.Generic;
using YashfeenMedical.DAL.Enums;
using YashfeenMedical.DAL.Shared.Entities;

namespace YashfeenMedical.BLL.DTOs.Invoices
{
    public class InvoiceDto : TIdType<int>
    {
        public int Id { get; set; }

        public int AppointmentId { get; set; }

        public int PatientId { get; set; }

        public string InvoiceNumber { get; set; } = string.Empty;

        public decimal SubTotal { get; set; }

        public decimal DiscountAmount { get; set; }

        public decimal InsuranceCoverage { get; set; }

        public decimal PatientShare { get; set; }

        public decimal TaxAmount { get; set; }

        public decimal TotalAmount { get; set; }

        public decimal PaidAmount { get; set; }

        public PaymentStatus PaymentStatus { get; set; }

        public PaymentMethod? PaymentMethod { get; set; }

        public DateTimeOffset IssuedAt { get; set; }

        public DateTimeOffset? PaidAt { get; set; }

        public string? Notes { get; set; }

        // Keep items as IDs to avoid deep nesting in this DTO
        public IList<int>? ItemIds { get; set; }
    }
}
