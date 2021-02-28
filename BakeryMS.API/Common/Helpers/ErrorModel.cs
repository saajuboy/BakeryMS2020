namespace BakeryMS.API.Common.Helpers
{
    public class ErrorModel
    {
        public ErrorModel(int code, int status, string message)
        {
            Code = code;
            Message = message;
            Status = status;
        }
        public int Code { get; set; }
        public string Message { get; set; }
        public int Status { get; set; }
    }
}