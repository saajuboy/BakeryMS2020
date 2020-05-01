using System;

namespace BakeryMS.API.Models
{
    public class CompanyItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public Item Item { get; set; }
        public DateTime? ManufacturedDate { get; set; }
        public DateTime? ExpireDate { get; set; }
        public int BatchNo { get; set; }
        public decimal CostPrice { get; set; }
        public decimal Quantity { get; set; }

    }
}
