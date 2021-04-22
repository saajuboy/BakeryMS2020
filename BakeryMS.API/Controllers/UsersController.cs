using System.Net;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BakeryMS.API.Data.Interfaces;
using BakeryMS.API.Common.DTOs;
using AutoMapper;
using System.Collections.Generic;
using BakeryMS.API.Data;
using Microsoft.EntityFrameworkCore;
using BakeryMS.API.Models.Profile;
using BakeryMS.API.Common.Helpers;

namespace BakeryMS.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _repository;
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        public UsersController(IUserRepository repository, IMapper mapper, DataContext context)
        {
            _context = context;
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


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserForRegisterDto userForUpdateDto)
        {
            if (!User.FindAll(ClaimTypes.Role).Any(a => a.Value == "admin" || a.Value == "Admin"))
            {
                if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                    return Unauthorized();

            }

            userForUpdateDto.Username = userForUpdateDto.Username.ToLower();

            if (await _repository.UserExists(userForUpdateDto.Username, id))
                return BadRequest("Username Already Exists");

            var UserFromRepository = await _repository.GetUser(id);

            var userToUpdate = _mapper.Map(userForUpdateDto, UserFromRepository);

            var user = await _repository.UpdateUser(userToUpdate, userForUpdateDto.Password);

            if (user != null)
                return NoContent();

            throw new System.Exception($"Updating user {id} failed on save");
        }

        [HttpPatch("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PatchUser(int id, UserForPatchDto userForPatchDto)
        {
            var UserFromRepository = await _repository.GetUser(id);
            UserFromRepository.Status = userForPatchDto.status;

            if (await _repository.SaveAll())
                return NoContent();

            throw new System.Exception($"Updating user {id} status failed on save");
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (await _repository.DeleteUser(id))
                return Ok();

            throw new System.Exception($"Failed to delet user {id}");
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<IActionResult> GetAvailableRoles()
        {
            var roles = await _context.Roles.ToListAsync();

            return Ok(roles);
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<IActionResult> GetRoles(int userId)
        {
            var rolesmapping = await _context.UserRolesMappings.Where(a => a.User.Id == userId).Include(a => a.Roles).ToListAsync();

            List<Roles> roles = new List<Roles>();
            foreach (var role in rolesmapping)
            {
                roles.Add(new Roles
                {
                    Id = role.Roles.Id,
                    RoleName = role.Roles.RoleName
                });
            }

            return Ok(roles);
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> UpdateRoles([FromQuery] int userId, [FromBody] RoleListDto roleList)
        {
            if (roleList.Roles == null)
                return BadRequest(new ErrorModel(1, 400, "Empty Body"));
                
            var user = await _context.Users.FirstOrDefaultAsync(a => a.Id == userId);
            if (user == null)
                return BadRequest(new ErrorModel(1, 400, "Invalid  User"));
                
            var roleMappingsFromRepo = await _context.UserRolesMappings.Where(a => a.User == user).ToListAsync();

            foreach (var role in roleMappingsFromRepo)
            {
                _context.Remove(role);
            }
            roleMappingsFromRepo.Clear();

            foreach (var item in roleList.Roles)
            {
                var role = await _context.Roles.FirstOrDefaultAsync(a => a.Id == item.Id);
                roleMappingsFromRepo.Add(new UserRolesMapping
                {
                    Roles = role,
                    User = user
                });
            }

            await _context.AddRangeAsync(roleMappingsFromRepo);

            if (await _context.SaveChangesAsync() > 0)
                return Ok();

            return BadRequest(new ErrorModel(1, 400, "No changes were made for Roles"));
        }
    }
}