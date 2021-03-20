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
        public string UserName { get; set; }
        public string OrderDate { get; set; }
        public string DeliveryDate { get; set; }
        public int Status { get; set; }
        public bool isFromOutlet { get; set; }
    }
}