using System;

namespace BakeryMS.API.Common.DTOs.Inventory
{
    public class POForListDto
    {
        public int Id { get; set; }
        public int PONumber { get; set; }
        public string Type { get; set; }
        public string DeliveryMethod { get; set; }
        public string SupplierName { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public bool Status { get; set; }
    }
}