using System;

namespace BakeryMS.API.Models.Production
{
    public class ProductionOrderHeader
    {
        public int Id { get; set; }
        public int ProductionNo { get; set; }
        public DateTime? EnteredDate { get; set; }
        public DateTime? RequiredDate { get; set; }
        public ProductionSession Session { get; set; }
    }
}
