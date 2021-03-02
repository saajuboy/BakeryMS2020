using System.ComponentModel.DataAnnotations;

namespace BakeryMS.API.Common.DTOs.Manufacturing
{
    public class IngredientDetailForDetailDto
    {
         public int Id { get; set; }
        [Required]
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Quantity should be greater than 0")]
        public decimal Quantity { get; set; }
        public bool IsDeleted { get; set; }
    }
}