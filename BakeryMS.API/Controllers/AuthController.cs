using System.Threading.Tasks;
using BakeryMS.API.Common.DTOs;
using BakeryMS.API.Data.Interfaces;
using BakeryMS.API.Models.Profile;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace BakeryMS.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repository;
        private readonly IConfiguration _configuration;
        public AuthController(IAuthRepository repository, IConfiguration configuration)
        {
            _configuration = configuration;
            _repository = repository;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
        {
            // validate request

            userForRegisterDto.Username = userForRegisterDto.Username.ToLower();

            if (await _repository.UserExists(userForRegisterDto.Username))
                return BadRequest("Username Already Exists");
            var usertoCreate = new User
            {
                Username = userForRegisterDto.Username
                //add user other details for registration

                //---end---
            };

            var createdUser = await _repository.Register(usertoCreate, userForRegisterDto.Password);
            return StatusCode(201);

        }
    }
}