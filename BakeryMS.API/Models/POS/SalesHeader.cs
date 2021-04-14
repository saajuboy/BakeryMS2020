using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using BakeryMS.API.Models.Profile;

namespace BakeryMS.API.Models.POS
{
    public class SalesHeader
    {
        public int Id { get; set; }
        public int SalesNo { get; set; }
        public DateTime Date { get; set; }
        public int BusinessPlaceId { get; set; }
        public BusinessPlace BusinessPlace { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public string CustomerName { get; set; }
        public int? CustomerId { get; set; }
        public Customer Customer { get; set; }
        public TimeSpan? Time { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Total { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Discount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal ReceivedAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal ChangeAmount { get; set; }
        public bool IsHolded { get; set; }
        public bool IsCharged { get; set; }
        public IList<SalesDetail> SalesDetails { get; set; }
    }
}