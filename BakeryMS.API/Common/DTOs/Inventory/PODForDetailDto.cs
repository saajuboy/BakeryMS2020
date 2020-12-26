using System;
using System.ComponentModel.DataAnnotations;

namespace BakeryMS.API.Common.DTOs.Inventory
{
    public class PODForDetailDto
    {
        public int Id { get; set; }
        public DateTime DueDate { get; set; }
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Order Quantity should be greater than 0")]
        public decimal OrderQty { get; set; }
        public string Item { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Item is required")]
        public int ItemId { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? LineTotal { get; set; }
        public decimal? ReceivedQuantity { get; set; }
        public decimal? RejectedQuantity { get; set; }
        public decimal? StockedQuantity { get; set; }
        public DateTime? ModifiedTime { get; set; }

        public PODForDetailDto()
        {
            this.ModifiedTime = DateTime.Now;
        }
    }
}