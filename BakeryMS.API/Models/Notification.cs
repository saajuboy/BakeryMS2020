using System;
using BakeryMS.API.Models.Profile;

namespace BakeryMS.API.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public DateTime DateTime { get; set; }
        public bool IsRead { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int Status { get; set; } // 0-active,1-inactive,2-deleted
    }
}