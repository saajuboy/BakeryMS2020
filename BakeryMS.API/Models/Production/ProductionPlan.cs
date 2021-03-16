using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using BakeryMS.API.Models.HumanResource;
using BakeryMS.API.Models.Profile;

namespace BakeryMS.API.Models.Production
{
    public class ProductionPlanHeader
    {
        public int Id { get; set; }
        public int ProductionSessionId { get; set; }
        public ProductionSession ProductionSession { get; set; }
        public int BusinessPlaceId { get; set; }
        public BusinessPlace BusinessPlace { get; set; }
        public DateTime Date { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public string Description { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsNotEditable { get; set; }

        public IList<ProductionPlanDetail> ProductionPlanDetails { get; set; }
        public IList<ProductionPlanWorker> ProductionPlanWorkers { get; set; }
        public IList<ProductionPlanMachine> ProductionPlanMachines { get; set; }
        public IList<ProductionPlanRecipe> ProductionPlanRecipes { get; set; }
    }
    public class ProductionPlanDetail
    {
        public int Id { get; set; }
        public ProductionPlanHeader ProductionPlanHeader { get; set; }
        public int ItemId { get; set; }
        public Item Item { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Quantity { get; set; }
        public string Description { get; set; }
        public bool IsDeleted { get; set; }
    }
    public class ProductionPlanWorker
    {
        public int Id { get; set; }
        public ProductionPlanHeader ProductionPlanHeader { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
    }
    public class ProductionPlanMachine
    {
        public int Id { get; set; }
        public ProductionPlanHeader ProductionPlanHeader { get; set; }
        public int MachineryId { get; set; }
        public Machinery Machinery { get; set; }
    }
    public class ProductionPlanRecipe
    {
        public int Id { get; set; }
        public ProductionPlanHeader productionPlanHeader { get; set; }
        public int ItemId { get; set; }
        public Item Item { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Quantity { get; set; }
        public string Description { get; set; }
    }
}