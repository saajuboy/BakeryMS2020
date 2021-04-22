using System.Collections.Generic;

namespace BakeryMS.API.Models
{
    public class ControlProcedure
    {
        public int Id { get; set; }
        public int BusinessPlaceId { get; set; }
        public BusinessPlace BusinessPlace { get; set; } // always type 0
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsDeleted { get; set; }
    }
}