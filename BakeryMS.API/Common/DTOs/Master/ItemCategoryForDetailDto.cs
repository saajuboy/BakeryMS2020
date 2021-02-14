using System.ComponentModel.DataAnnotations;

namespace BakeryMS.API.Common.DTOs.Master
{
    public class ItemCategoryForDetailDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage="Code Required")]
        [RegularExpression("^[a-zA-Z]{4}",ErrorMessage="Code should be only 4 alpha characters eg:- ABCD")]

        public string Code { get; set; }
        [Required]
        public string Description { get; set; }
    }
}