using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BakeryMS.API.Common.DTOs.Manufacturing
{
    public class ProdOrderHeaderForDetailDto
    {
        public int Id { get; set; }
        public int ProductionOrderNo { get; set; }
        [Required]
        [Range(1,int.MaxValue,ErrorMessage="Session Required")]
        public int SessionId { get; set; }
        public string Session { get; set; }
        [Required]
        [Range(1,int.MaxValue,ErrorMessage="User Required")]
        public int UserId { get; set; }
        public string User { get; set; }
        [Required]
        public string EnteredDate { get; set; }
        [Required]
        public string RequiredDate { get; set; }
        [Required]
        [Range(1,int.MaxValue,ErrorMessage="Business Place Required")]
        public int BusinessPlaceId { get; set; }
        public string BusinessPlace { get; set; }
        public bool IsNotEditable { get; set; }
        public bool IsDeleted { get; set; }
        public IList<ProdOrderDetailForDetailDto> ProductionOrderDetails { get; set; }
    }
}