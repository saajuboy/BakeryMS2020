using System.ComponentModel.DataAnnotations.Schema;

namespace BakeryMS.API.Models.HumanResource
{
    public class Employee
    {
        public int Id { get; set; }
        public int EmployeeNumber { get; set; }
        public string Name { get; set; }
        public string ContactNumber { get; set; }
        public string NIC { get; set; }
        public string Address { get; set; }
        public int Type { get; set; }//0-permanent,1-daily,2-contract
        [Column(TypeName = "decimal(18,2)")]
        public decimal Salary { get; set; }
        public int Role { get; set; }//0-Manager,1-Cashier,2-Baker,3-counter,4-waiter
        public bool IsNotActive { get; set; }
        public bool IsDeleted { get; set; }

    }
}