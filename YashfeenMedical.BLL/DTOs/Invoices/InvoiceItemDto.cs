using YashfeenMedical.DAL.Enums;
using YashfeenMedical.DAL.Shared.Entities;

namespace YashfeenMedical.BLL.DTOs.Invoices
{
    public class InvoiceItemDto : TIdType<int>
    {
        public int Id { get; set; }

        public int InvoiceId { get; set; }

        public InvoiceItemType Type { get; set; }

        public string Description { get; set; } = string.Empty;

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal? DiscountPercent { get; set; }

        public decimal Total { get; set; }
    }
}
