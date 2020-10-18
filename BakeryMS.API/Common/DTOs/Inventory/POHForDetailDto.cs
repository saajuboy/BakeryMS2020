using System;
using System.Collections.Generic;

namespace BakeryMS.API.Common.DTOs.Inventory
{
    public class POHForDetailDto
    {
        public int Id { get; set; }
        public int PONumber { get; set; }
        public int UserId { get; set; }
        public string Supplier { get; set; }
        public string Type { get; set; }
        public string DeliveryMethod { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime DeliveryDate { get; set; }

        public IList<PODForDetailDto> PODetail { get; set; }
       
    }
}