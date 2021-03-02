using System.ComponentModel.DataAnnotations.Schema;

namespace BakeryMS.API.Models.Production
{
    public class IngredientDetail
    {
        public int Id { get; set; }
        public IngredientHeader IngredientsHeader { get; set; }
        public int ItemId { get; set; }
        public Item Item { get; set; } // always type 2
        [Column(TypeName = "decimal(18,2)")]
        public decimal Quantity { get; set; }
        public bool IsDeleted { get; set; }
    }
}