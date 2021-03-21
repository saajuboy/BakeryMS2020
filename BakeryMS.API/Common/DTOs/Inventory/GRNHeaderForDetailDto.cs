using System;
using System.Collections.Generic;

namespace BakeryMS.API.Common.DTOs.Inventory
{
    public class GRNHeaderForDetailDto
    {
        public int Id { get; set; }
        public int PurchaseOrderHeaderId { get; set; }
        public string ReceivedDate { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public int PaymentMode { get; set; } // 0-full,1-part
        public IList<GRNDetailForDetailDto> GRNDetails { get; set; }
    }
}