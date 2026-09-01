using YashfeenMedical.DAL.Enums;
using YashfeenMedical.DAL.Shared.Entities;

namespace YashfeenMedical.DAL.Models;

public class Invoice : TEntity<int>
{
    public int Id { get; set; }

    public int AppointmentId { get; set; } // UNIQUE — علاقة 1:1
    public Appointment Appointment { get; set; } = null!;

    public int PatientId { get; set; }
    public Patient Patient { get; set; } = null!;

    public string InvoiceNumber { get; set; } = string.Empty; // INV-YYYYMMDD-{Id}

    public decimal SubTotal { get; set; } // مجموع البنود قبل الخصم والتأمين

    public decimal DiscountAmount { get; set; } = 0;

    public decimal InsuranceCoverage { get; set; } // ما يغطيه التأمين

    public decimal PatientShare { get; set; } // SubTotal - Discount - Insurance

    public decimal TaxAmount { get; set; }

    public decimal TotalAmount { get; set; }

    public decimal PaidAmount { get; set; } // قد يكون جزئياً

    public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;

    public PaymentMethod? PaymentMethod { get; set; }

    public DateTimeOffset IssuedAt { get; set; } = DateTimeOffset.UtcNow;

    public DateTimeOffset? PaidAt { get; set; }

    public string? Notes { get; set; }


    public ICollection<InvoiceItem> Items { get; set; } = new List<InvoiceItem>();
}
