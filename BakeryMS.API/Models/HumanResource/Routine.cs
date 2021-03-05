using System;

namespace BakeryMS.API.Models.HumanResource
{
    public class Routine
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public int BusinessPlaceId { get; set; }
        public BusinessPlace BusinessPlace { get; set; }
        public DateTime Date { get; set; }
    }
}