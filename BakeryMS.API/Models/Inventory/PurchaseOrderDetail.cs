using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BakeryMS.API.Models.Inventory
{
    public class PurchaseOrderDetail
    {
        public int Id { get; set; }

        public int POHeaderId { get; set; }
        public PurchaseOrderHeader POHeader { get; set; }

        public DateTime DueDate { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal OrderQty { get; set; }
        public Item Item { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal LineTotal { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? ReceivedQuantity { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? RejectedQuantity { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? StockedQuantity { get; set; }
        public DateTime? ModifiedTime { get; set; }


    }
}