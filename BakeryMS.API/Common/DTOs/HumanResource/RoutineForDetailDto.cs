using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BakeryMS.API.Common.DTOs.HumanResource
{
    public class RoutineForDetailDto
    {
        public int Id { get; set; }
        [Required]
        public int EmployeeId { get; set; }
        public EmployeeForDetailDto Employee { get; set; }
        [Required]
        public string StartTime { get; set; }
        [Required]
        public string EndTime { get; set; }
        [Required]
        public int BusinessPlaceId { get; set; }
        [Required]
        public string Date { get; set; }
        public int RoleId { get; set; }
    }

    public class RoutineForListDto{
        public IList<RoutineForDetailDto> Routines { get; set; }
    }
}