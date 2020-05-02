using System;

namespace BakeryMS.API.Models.Profile
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int? ContactNumber { get; set; }  
        public bool? Status { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? LastActive { get; set; }
        public string PhotoUrl { get; set; }
        public string PhotoPublicId { get; set; }
        

    }
}
