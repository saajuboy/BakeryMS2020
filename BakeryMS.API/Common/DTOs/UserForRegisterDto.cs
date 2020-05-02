using System;
using System.ComponentModel.DataAnnotations;

namespace BakeryMS.API.Common.DTOs
{
    public class UserForRegisterDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        [StringLength(20,MinimumLength=4, ErrorMessage="You must Specify Password between 20 and 4 characters")]
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int? ContactNumber { get; set; }  
        public string PhotoUrl { get; set; }
        public string PhotoPublicId { get; set; }
    }
}