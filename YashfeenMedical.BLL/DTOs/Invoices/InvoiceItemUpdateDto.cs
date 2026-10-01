using System.ComponentModel.DataAnnotations;
using YashfeenMedical.DAL.Enums;
using YashfeenMedical.DAL.Shared.Entities;

namespace YashfeenMedical.BLL.DTOs.Invoices
{
    public class InvoiceItemUpdateDto : TIdType<int>
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int InvoiceId { get; set; }

        [Required]
        public InvoiceItemType Type { get; set; }

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Required]
        [Range(0.0, double.MaxValue)]
        public decimal UnitPrice { get; set; }

        [Range(0.0, 100.0)]
        public decimal? DiscountPercent { get; set; }
    }
}
