using System.Collections.Generic;

namespace BakeryMS.API.Common.DTOs
{
    public class ConfigDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int? UserId { get; set; }
        public string Value { get; set; }
    }
    public class ConfigListDto
    {
        public IList<ConfigDto> Configurations  { get; set; }
    }
}