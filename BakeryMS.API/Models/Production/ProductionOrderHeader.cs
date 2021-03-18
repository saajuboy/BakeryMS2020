using System;
using System.Collections.Generic;
using BakeryMS.API.Models.Profile;

namespace BakeryMS.API.Models.Production
{
    public class ProductionOrderHeader
    {
        public int Id { get; set; }
        public int ProductionOrderNo { get; set; }
        public ProductionSession Session { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public DateTime? EnteredDate { get; set; }
        public DateTime? RequiredDate { get; set; }
        public BusinessPlace BusinessPlace { get; set; }
        public string Description { get; set; }
        public bool IsNotEditable { get; set; }
        public bool IsDeleted { get; set; }
        public IList<ProductionOrderDetail> ProductionOrderDetails { get; set; }
        public int? PlanId { get; set; }
        public int? isProcessed { get; set; }//0-reviewd,1-processed,2-completed

    }
}
