using System.ComponentModel.DataAnnotations.Schema;

namespace BakeryMS.API.Models.POS
{
    public class SalesDetail
    {
        public int Id { get; set; }
        public SalesHeader SalesHeader { get; set; }
        public int ItemId { get; set; }//can be prod or comp
        public int Type { get; set; }//0-prod,1-comp

        [Column(TypeName = "decimal(18,2)")]
        public decimal Quantity { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal LineTotal { get; set; }
    }
}