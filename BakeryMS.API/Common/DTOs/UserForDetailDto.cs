using System;

namespace BakeryMS.API.Common.DTOs
{
    public class UserForDetailDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public int? ContactNumber { get; set; }
        public string PhotoUrl { get; set; }
        public DateTime LastActive { get; set; }
    }
}