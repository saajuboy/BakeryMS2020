using System;
using System.Collections.Generic;

namespace BakeryMS.API.Models.Inventory
{
    public class PurchaseOrderHeader
    {
        public int Id { get; set; }
        public int PONumber { get; set; }
        public int UserId { get; set; }
        public int? SupplierId { get; set; }
        public Supplier Supplier { get; set; }
        public string DeliveryMethod { get; set; }
        public DateTime DeliveryDate { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool Status { get; set; }
        public bool isFromOutlet { get; set; }

        public IList<PurchaseOrderDetail> PurchaseOrderDetail { get; set; }

    }
}