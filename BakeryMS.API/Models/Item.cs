using System.Collections.Generic;

namespace BakeryMS.API.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public ItemCategory ItemCategory { get; set; }
        public Unit Unit { get; set; }
        
    }
}
