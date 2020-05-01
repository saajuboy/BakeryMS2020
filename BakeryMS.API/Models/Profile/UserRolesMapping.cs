namespace BakeryMS.API.Models.Profile
{
    public class UserRolesMapping
    {
        public int Id { get; set; }
        public Roles Roles { get; set; }
        public User User { get; set; }
    }
}
