using System;
using System.ComponentModel.DataAnnotations.Schema;
using BakeryMS.API.Models.Profile;

namespace BakeryMS.API.Models.POS
{
    public class Transaction
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan? Time { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public string Reference { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Debit { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Credit { get; set; }
    }
}