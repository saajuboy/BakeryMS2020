using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BakeryMS.API.Models
{
    public class RawItems
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public Item Item { get; set; }
        public DateTime? ManufacturedDate { get; set; }
        public DateTime? ExpireDate { get; set; }
        public int BatchNo { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal CostPrice { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal StockedQuantity { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal UsedQuantity { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal AvailableQuantity { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal SellingPrice { get; set; }
        public BusinessPlace CurrentPlace { get; set; }
    }
}