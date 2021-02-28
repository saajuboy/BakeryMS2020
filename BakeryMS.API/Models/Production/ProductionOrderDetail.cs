using System.ComponentModel.DataAnnotations.Schema;

namespace BakeryMS.API.Models.Production
{
    public class ProductionOrderDetail
    {
        public int Id { get; set; }
        public ProductionOrderHeader ProductionOrderHeader { get; set; }
        public int ItemId { get; set; }
        public Item Item { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Quantity { get; set; }
        public string Description { get; set; }
        public bool IsDeleted { get; set; }
    }
}
