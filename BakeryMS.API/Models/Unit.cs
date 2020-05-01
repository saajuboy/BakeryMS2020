using System.Collections.Generic;

namespace BakeryMS.API.Models
{
    public class Unit
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public IList<Item> Items { get; set; }
    }
}
