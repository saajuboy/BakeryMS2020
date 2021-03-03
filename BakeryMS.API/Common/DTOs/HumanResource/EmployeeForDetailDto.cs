using System.ComponentModel.DataAnnotations;

namespace BakeryMS.API.Common.DTOs.HumanResource
{
    public class EmployeeForDetailDto
    {
        public int Id { get; set; }
        public int? EmployeeNumber { get; set; }
        [Required]
        public string Name { get; set; }
        public string ContactNumber { get; set; }
        [Required]
        [RegularExpression("^([0-9]{9}[x|X|v|V]|[0-9]{12})$", ErrorMessage = "should be a valid NIC number")]
        public string NIC { get; set; }
        public string Address { get; set; }
        [Required]
        [Range(0, 2, ErrorMessage = "type Not available")]
        public int Type { get; set; }//0-permanent,1-daily,2-contract
        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Salary is required")]
        public decimal Salary { get; set; }
        [Required]
        [Range(0, 4, ErrorMessage = "Role Not available")]
        public int Role { get; set; }//0-Manager,1-Cashier,2-Baker,3-counter,4-waiter
        public bool IsNotActive { get; set; }
    }
}