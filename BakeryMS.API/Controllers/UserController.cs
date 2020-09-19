using System.Linq;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using BakeryMS.API.Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BakeryMS.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        public IUserComponent _userComponent { get; set; }
        public UserController(IUserComponent userComponent)
        {
            _userComponent = userComponent;

        }

        [HttpGet("{id}", Name = "GetUser")]
        public async Task<IActionResult> GetUser(int id)
        {
            if (!User.FindAll(ClaimTypes.Role).Any(a => a.Value == "admin" || a.Value == "Admin"))
            {
                if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                    return Unauthorized();

            }

            var userToReturn = await _userComponent.GetUser(id);

            return Ok(userToReturn);
        }

        [HttpGet]
        [Authorize(Roles="Admin")]
        public async Task<IActionResult> GetUsers()
        {
            var usersToReturn = await _userComponent.GetUsers();

            return Ok(usersToReturn);
        }

    }
}