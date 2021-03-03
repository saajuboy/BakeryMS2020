namespace BakeryMS.API.Common.DTOs.HumanResource
{
    public class EmployeeForListDto
    {
        public int Id { get; set; }
        public int EmployeeNumber { get; set; }
        public string Name { get; set; }
        public string ContactNumber { get; set; }
        public string TypeName { get; set; }//0-permanent,1-daily,2-contract
        public decimal Salary { get; set; }
        public string RoleName { get; set; }//0-Manager,1-Cashier,2-Baker,3-counter,4-waiter
        public bool IsNotActive { get; set; }
    }
}