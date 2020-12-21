namespace BakeryMS.API.Models.Inventory
{
    public class Supplier
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
        public int? Type { get; set; }
        public string Address { get; set; }
    }
}