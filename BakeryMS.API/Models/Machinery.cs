using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BakeryMS.API.Models
{
    public class Machinery
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Model { get; set; }
        public int Capacity { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Value { get; set; }
        public int BusinessPlaceId { get; set; }
        public BusinessPlace BusinessPlace { get; set; }
        public DateTime? PurchaseDate { get; set; }
    }
}