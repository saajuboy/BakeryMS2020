using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BakeryMS.API.Models.Production
{
    public class IngredientHeader
    {
         public int Id { get; set; }
        public int ItemId { get; set; }
        public Item Item { get; set; } // always type 0
        public string Description { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal ServingSize { get; set; }
        public string Method { get; set; }
        public bool IsDeleted { get; set; }
        public IList<IngredientDetail> IngredientsDetail { get; set; }
    }
}