namespace BakeryMS.API.Common.DTOs.Inventory
{
    public class GRNDetailForDetailDto
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string ItemCode { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal LineTotal { get; set; }
        public decimal SellingPrice { get; set; }

        public string ManufacturedDate { get; set; }
        public string ExpiredDate { get; set; }
    }
}