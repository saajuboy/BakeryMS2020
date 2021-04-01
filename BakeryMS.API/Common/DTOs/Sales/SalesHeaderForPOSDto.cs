using System.Collections.Generic;
using BakeryMS.API.Common.DTOs.Master;

namespace BakeryMS.API.Common.DTOs.Sales
{
    public class SalesHeaderForPOSDto
    {
        public int Id { get; set; }
        public int SalesNo { get; set; }
        public string Date { get; set; }
        public int BusinessPlaceId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string BusinessPlaceName { get; set; }
        public string CustomerName { get; set; }
        public string Time { get; set; }
        public decimal Total { get; set; }
        public decimal Discount { get; set; }
        public decimal ReceivedAmount { get; set; }
        public decimal ChangeAmount { get; set; }
        public bool IsHolded { get; set; }
        public bool IsCharged { get; set; }
        public IList<SalesDetailForPosDto> SalesDetails { get; set; }
        public BusinessPlaceForDetailDto BusinessPlace { get; set; }
    }
}