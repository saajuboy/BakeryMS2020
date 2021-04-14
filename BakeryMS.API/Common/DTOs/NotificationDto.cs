namespace BakeryMS.API.Common.DTOs
{
    public class NotificationDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public bool IsRead { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int Status { get; set; } // 0-active,1-inactive,2-deleted
    }
}