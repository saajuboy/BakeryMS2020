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
        public int Type { get; set; } // 0:production,1:Company,2:Raw,3:Misc
        public bool IsDeleted { get; set; } 
        
    }
}
