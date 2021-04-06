namespace BakeryMS.API.Common.DTOs.Inventory
{
    public class AvailableItemsDtoForList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Unit { get; set; }
        public int Type { get; set; }
        public string ManufacturedDate { get; set; }
        public string ExpireDate { get; set; }
        public int BatchNo { get; set; }
        public decimal CostPrice { get; set; }
        public decimal StockedQuantity { get; set; }
        public decimal UsedQuantity { get; set; }
        public decimal AvailableQuantity { get; set; }
        public int BusinessPlaceId { get; set; }
        public string BusinessPlaceName { get; set; }
        public decimal? SellingPrice { get; set; }
        public bool IsReorder { get; set; }
        public decimal ReorderLevel { get; set; }
    }
}