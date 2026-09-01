using YashfeenMedical.DAL.Enums;
using YashfeenMedical.DAL.Shared.Entities;

namespace YashfeenMedical.DAL.Models;

public class InvoiceItem : TEntity<int>
{
    public int Id { get; set; }

    public int InvoiceId { get; set; }
    public Invoice Invoice { get; set; } = null!;

    public InvoiceItemType Type { get; set; }

    public string Description { get; set; } = string.Empty;

    public int Quantity { get; set; } // >= 1

    public decimal UnitPrice { get; set; }

    public decimal? DiscountPercent { get; set; } // 0-100

    public decimal Total { get; set; } // Quantity * UnitPrice * (1 - Discount%)
}
