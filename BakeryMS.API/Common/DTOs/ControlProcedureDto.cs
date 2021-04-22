using BakeryMS.API.Models;

namespace BakeryMS.API.Common.DTOs
{
    public class ControlProcedureDto
    {
        public int Id { get; set; }
        public int BusinessPlaceId { get; set; }
        public string BusinessPlaceName { get; set; } 
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsDeleted { get; set; }
    }
}