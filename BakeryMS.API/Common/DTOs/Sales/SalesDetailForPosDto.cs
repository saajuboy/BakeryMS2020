namespace BakeryMS.API.Common.DTOs.Sales
{
    public class SalesDetailForPosDto
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public int Type { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal LineTotal { get; set; }
        public string ItemName { get; set; }
    }
}