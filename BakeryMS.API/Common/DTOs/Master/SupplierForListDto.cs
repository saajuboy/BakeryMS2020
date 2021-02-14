using System.ComponentModel.DataAnnotations;

namespace BakeryMS.API.Common.DTOs.Master
{
    public class SupplierDto
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string ContactNumber { get; set; }
        public string Email { get; set; }
        [Required]
        public int? Type { get; set; }
        public string Address { get; set; }
    }
}