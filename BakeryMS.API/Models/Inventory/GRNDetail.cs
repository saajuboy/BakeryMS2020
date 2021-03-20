using System.ComponentModel.DataAnnotations.Schema;

namespace BakeryMS.API.Models.Inventory
{
    public class GRNDetail
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public Item Item { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Quantity { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal LineTotal { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal SellingPrice { get; set; }
    }
}