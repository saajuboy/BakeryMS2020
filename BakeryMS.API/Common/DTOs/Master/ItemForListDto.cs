namespace BakeryMS.API.Common.DTOs.Master
{
    public class ItemForListDto
    {
         public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string ItemCategory { get; set; } 
        public string Unit { get; set; }
    }
}