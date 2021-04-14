namespace BakeryMS.API.Common.DTOs.Master
{
    public class CustomerDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Contact { get; set; }
        public bool IsRetail { get; set; }
        public int Status { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public bool IsDeleted { get; set; }
    }
}