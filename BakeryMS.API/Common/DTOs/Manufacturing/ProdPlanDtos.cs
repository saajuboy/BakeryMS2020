using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BakeryMS.API.Common.DTOs.Manufacturing
{
    public class ProdPlanHeaderForDetailDto
    {
        public int Id { get; set; }
        [Required]
        public int ProductionSessionId { get; set; }
        [Required]
        public int BusinessPlaceId { get; set; }
        [Required]
        public string Date { get; set; }
        [Required]
        public int UserId { get; set; }
        public string Description { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsNotEditable { get; set; }

        public IList<ProdPlanDetailForDetailDto> ProductionPlanDetails { get; set; }
        public IList<ProdPlanWorkerForDetailDto> ProductionPlanWorkers { get; set; }
        public IList<ProdPlanMachineForDetailDto> ProductionPlanMachines { get; set; }
        public IList<ProdPlanRecipeForDetailDto> ProductionPlanRecipes { get; set; }

        public IList<int> ProdOrdrIds { get; set; }
    }
    public class ProdPlanDetailForDetailDto
    {
        public int Id { get; set; }
        [Required]
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Quantity should be greater than 0")]
        public decimal Quantity { get; set; }
        public string Description { get; set; }
    }
    public class ProdPlanWorkerForDetailDto
    {
        public int Id { get; set; }
        [Required]
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
    }
    public class ProdPlanMachineForDetailDto
    {
        public int Id { get; set; }
        [Required]
        public int MachineryId { get; set; }
        public string MachineryName { get; set; }
    }
    public class ProdPlanRecipeForDetailDto
    {
        public int Id { get; set; }
        [Required]
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Quantity should be greater than 0")]
        public decimal Quantity { get; set; }
        public string Description { get; set; }
    }

    public class ProdPlanForListDto
    {
        public int Id { get; set; }
        public string SessionName { get; set; }
        public string UserName { get; set; }
        public string Date { get; set; }
        public string BusinessPlaceName { get; set; }
        public bool IsNotEditable { get; set; }
    }

    public class ProductionPlanDetailListDto
    {
        public IList<ProdPlanDetailForDetailDto> ProductionPlanDetails { get; set; }
    }
}