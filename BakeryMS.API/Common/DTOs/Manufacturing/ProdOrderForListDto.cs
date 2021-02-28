namespace BakeryMS.API.Common.DTOs.Manufacturing
{
    public class ProdOrderForListDto
    {
        public int Id { get; set; }
        public int ProductionOrderNo { get; set; }
        public string SessionName { get; set; }
        public string UserName { get; set; }
        public string EnteredDate { get; set; }
        public string RequiredDate { get; set; }
        public string BusinessPlaceName { get; set; }
        public bool IsNotEditable { get; set; }
    }
}