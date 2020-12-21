using System;

namespace BakeryMS.API.Common.DTOs.Inventory
{
    public class PODForDetailDto
    {
        public int Id { get; set; }
        public DateTime DueDate { get; set; }
        public decimal OrderQty { get; set; }
        public string Item { get; set; }
        public int ItemId { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal LineTotal { get; set; }
        public decimal ReceivedQuantity { get; set; }
        public decimal RejectedQuantity { get; set; }
        public decimal? StockedQuantity { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }
} 