using System.Numerics;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BakeryMS.API.Common.DTOs.Inventory
{
    public class POHForDetailDto
    {
        public int Id { get; set; }
        public int PONumber { get; set; }
        [Required]
        [Range(1,int.MaxValue,ErrorMessage="User Required")]
        public int UserId { get; set; }
        [Required]
        [Range(1,int.MaxValue,ErrorMessage="Supplier Required")]
        public int SupplierId { get; set; }
        public int BusinessPlaceId { get; set; }
        public int Status { get; set; }
        public string DeliveryMethod { get; set; }
        [Required]
        public string OrderDate { get; set; }
        [Required]
        public string DeliveryDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public IList<PODForDetailDto> PODetail { get; set; }

        public bool isForOutlet { get; set; } // used only for creating
        public POHForDetailDto()
        {
            this.ModifiedDate = DateTime.Now;
        }

    }
}