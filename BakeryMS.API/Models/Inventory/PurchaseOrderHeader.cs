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
        public int? BusinessPlaceId { get; set; }
        public BusinessPlace BusinessPlace { get; set; }
        public string DeliveryMethod { get; set; }
        public DateTime DeliveryDate { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int Status { get; set; } // null||0 - not sent,1-sent,2-received
        public bool isFromOutlet { get; set; }
        public bool IsDeleted { get; set; }

        public IList<PurchaseOrderDetail> PurchaseOrderDetail { get; set; }

    }
}