namespace BakeryMS.API.Models.Profile
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int ContactNumber { get; set; }  
        public bool Status { get; set; }

    }
}
