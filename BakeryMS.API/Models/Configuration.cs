using BakeryMS.API.Models.Profile;

namespace BakeryMS.API.Models
{
    public class Configuration
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int? UserId { get; set; }
        public User User { get; set; }
        public string Value { get; set; }
    }
}