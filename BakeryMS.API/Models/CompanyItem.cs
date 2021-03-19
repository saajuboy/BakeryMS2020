using System;

namespace BakeryMS.API.Models
{
    public class CompanyItem
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public Item Item { get; set; }
        public DateTime? ManufacturedDate { get; set; }
        public DateTime? ExpireDate { get; set; }
        public int BatchNo { get; set; }
        public decimal CostPrice { get; set; }
        public decimal StockedQuantity { get; set; }
        public decimal UsedQuantity { get; set; }
        public decimal AvailableQuantity { get; set; }
        public BusinessPlace CurrentPlace { get; set; }

    }
}
