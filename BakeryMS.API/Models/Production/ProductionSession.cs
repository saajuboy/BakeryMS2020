using System;
namespace BakeryMS.API.Models.Production
{
    public class ProductionSession
    {
        public int Id { get; set; }
        public string Session { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
    }
}
