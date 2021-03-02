namespace BakeryMS.API.Common.DTOs.Manufacturing
{
    public class IngredientForListDto
    {
        public int Id { get; set; }
        public string ItemName { get; set; }
        public string Description { get; set; }
        public decimal ServingSize { get; set; }
    }
}