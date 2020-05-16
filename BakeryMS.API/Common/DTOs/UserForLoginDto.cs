using System.ComponentModel.DataAnnotations;

namespace BakeryMS.API.Common.DTOs
{
    public class UserForLoginDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        [StringLength(20,MinimumLength=4, ErrorMessage="You must Specify Password between 20 and 4 characters")]
        public string Password { get; set; }
    }
}