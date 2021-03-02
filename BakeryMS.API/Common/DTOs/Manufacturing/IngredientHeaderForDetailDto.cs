using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BakeryMS.API.Common.DTOs.Manufacturing
{
    public class IngredientHeaderForDetailDto
    {
        public int Id { get; set; }
        [Required]
        [Range(1,int.MaxValue,ErrorMessage="Item Required")]
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Serving Size should be greater than 0")]
        public decimal ServingSize { get; set; }
        public string Description { get; set; }
        public string Method { get; set; }
        public bool IsDeleted { get; set; }
        public IList<IngredientDetailForDetailDto> IngredientDetails { get; set; }
    }
}