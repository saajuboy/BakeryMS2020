using System.ComponentModel.DataAnnotations.Schema;

namespace BakeryMS.API.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Contact { get; set; }
        public bool IsRetail { get; set; }
        public int Status { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Debit { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Credit { get; set; }
        public bool IsDeleted { get; set; }
    }
}