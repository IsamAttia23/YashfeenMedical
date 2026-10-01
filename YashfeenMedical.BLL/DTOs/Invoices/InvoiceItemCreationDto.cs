using System.ComponentModel.DataAnnotations;
using YashfeenMedical.DAL.Enums;

namespace YashfeenMedical.BLL.DTOs.Invoices
{
    public class InvoiceItemCreationDto
    {
        [Required]
        public InvoiceItemType Type { get; set; }

        public string Description { get; set; } = string.Empty;

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; } = 1;

        [Range(0.01, double.MaxValue)]
        public decimal UnitPrice { get; set; }

        [Range(0, 100)]
        public decimal? DiscountPercent { get; set; }
    }
}
