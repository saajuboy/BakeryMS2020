namespace BakeryMS.API.Common.DTOs.Master
{
    public class MachineryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Model { get; set; }
        public int Capacity { get; set; }
        public decimal Value { get; set; }
        public int BusinessPlaceId { get; set; }
        public string BusinessPlaceName { get; set; }
        public string PurchaseDate { get; set; }
    }
}