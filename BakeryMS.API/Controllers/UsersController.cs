using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BakeryMS.API.Data.Interfaces;
using BakeryMS.API.Common.DTOs;
using AutoMapper;
using System.Collections.Generic;

namespace BakeryMS.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _repository;
        private readonly IMapper _mapper;
        public UsersController(IUserRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;

        }

        [HttpGet("{id}", Name = "GetUser")]
        public async Task<IActionResult> GetUser(int id)
        {
            if (!User.FindAll(ClaimTypes.Role).Any(a => a.Value == "admin" || a.Value == "Admin"))
            {
                if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                    return Unauthorized();

            }

            var userFromRepository = await _repository.GetUser(id);

            var userToReturn = _mapper.Map<UserForDetailDto>(userFromRepository);

            return Ok(userToReturn);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUsers()
        {
            var usersFromRepository = await _repository.GetUsers();

            var usersToReturn = _mapper.Map<IEnumerable<UserForDetailDto>>(usersFromRepository);
            return Ok(usersToReturn);
        }

    }
}