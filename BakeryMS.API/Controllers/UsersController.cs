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
            if(await _repository.DeleteUser(id))
            return Ok();

            throw new System.Exception($"Failed to delet user {id}");
        }
    }
}