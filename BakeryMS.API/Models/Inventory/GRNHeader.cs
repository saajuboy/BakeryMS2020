using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BakeryMS.API.Models.Inventory
{
    public class GRNHeader
    {
        public int Id { get; set; }
        public int PurchaseOrderHeaderId { get; set; }
        public PurchaseOrderHeader PurchaseOrderHeader { get; set; }
        public DateTime ReceivedDate { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal PaidAmount { get; set; }
        public int PaymentMode { get; set; }
        public IList<GRNDetail> GRNDetails { get; set; }
    }
}