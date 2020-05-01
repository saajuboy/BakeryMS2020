using System;

namespace BakeryMS.API.Models
{
    public class BusinessPlace
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string RegistrationNumber { get; set; }
        public DateTime? RegistrationStartDate { get; set; }
        public DateTime? RegistrationEndDate { get; set; }
        public DateTime? EstablishedDate { get; set; }
        public string Address { get; set; }



    }
}
