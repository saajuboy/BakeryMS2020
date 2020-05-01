using System.Collections.Generic;

namespace BakeryMS.API.Models
{
    public class ItemCategory
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public IList<Item> Items { get; set; }
    }
}
