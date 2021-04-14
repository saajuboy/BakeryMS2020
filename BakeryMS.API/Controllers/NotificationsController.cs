using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using BakeryMS.API.Common.DTOs;
using BakeryMS.API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BakeryMS.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class NotificationsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public NotificationsController(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        [HttpGet("{id}", Name = "GetNotification")]
        public async Task<IActionResult> GetNotification(int id)
        {
            var notiFromRepo = await _context.Notifications.Include(a => a.User)
                .FirstOrDefaultAsync(a => a.Id == id && a.Status != 2);

            var notiToReturn = _mapper.Map<NotificationDto>(notiFromRepo);

            notiFromRepo.IsRead = true;

            if (await _context.SaveChangesAsync() > 0)
                return Ok(notiToReturn);

            return Ok(notiToReturn);
        }

        [HttpGet]
        public async Task<IActionResult> GetNotifications()
        {
            var userid = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var notificationsFromRepo = await _context.Notifications.Where(a => (a.Status == 0
            || a.Status == 1) && a.UserId == userid)
                .Include(a => a.User)
                .OrderByDescending(a => a.DateTime)
                .ToListAsync();

            var notisToReturn = _mapper.Map<IList<NotificationDto>>(notificationsFromRepo);

            return Ok(notisToReturn);
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<IActionResult> GetRecentNotifications()
        {
            var userid = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var dateToCompare = DateTime.Now.AddDays(-3);

            var notRecentnotis = await _context.Notifications.Where(a => a.Status == 0
            && a.DateTime < dateToCompare && a.UserId == userid).Include(a => a.User).ToListAsync();

            foreach (var noti in notRecentnotis)
            {
                noti.Status = 1;
            }

            await _context.SaveChangesAsync();

            var Recentnotis = await _context.Notifications.Where(a => a.Status == 0)
                .OrderByDescending(a => a.DateTime)
                .Include(a => a.User)
                .ToListAsync();

            var notisToReturn = _mapper.Map<IList<NotificationDto>>(Recentnotis);

            return Ok(notisToReturn);
        }
    }
}