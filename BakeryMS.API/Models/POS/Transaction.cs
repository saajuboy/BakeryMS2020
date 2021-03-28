using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BakeryMS.API.Models.POS
{
    public class Transaction
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public string Reference { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Debit { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Credit { get; set; }
    }
}