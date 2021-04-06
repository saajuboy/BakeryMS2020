namespace BakeryMS.API.Common.DTOs.Master
{
    public class ItemForDetailDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public ItemCategoryForDetailDto ItemCategory { get; set; }
        public UnitForDetailDto Unit { get; set; }
        public int Type { get; set; }
        public decimal SellingPrice { get; set; }
        public decimal ReOrderLevel { get; set; }
        public int ExpireDays { get; set; }
    }
}