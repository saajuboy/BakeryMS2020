namespace BakeryMS.API.Models.Production
{
    public class ProductionOrderDetail
    {
        public int Id { get; set; }
        public ProductionOrderHeader ProductionOrderHeader { get; set; }
        public Item Item { get; set; }
        public int? ItemCategory { get; set; }
        public string ItemCode { get; set; }
        public double Quantity { get; set; }
        public OrderType OrderType { get; set; }
        public string Remarks { get; set; }
    }
}
