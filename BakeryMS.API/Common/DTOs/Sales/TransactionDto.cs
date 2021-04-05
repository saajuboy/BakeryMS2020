namespace BakeryMS.API.Common.DTOs.Sales
{
    public class TransactionDto
    {
        public int Id { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Reference { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public int? BusinessPlaceId { get; set; }
        public string BusinessPlaceName { get; set; }
    }
}