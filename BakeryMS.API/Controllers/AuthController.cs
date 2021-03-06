using System.Collections.Generic;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BakeryMS.API.Common.DTOs;
using BakeryMS.API.Data;
using BakeryMS.API.Data.Interfaces;
using BakeryMS.API.Models.Profile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using AutoMapper;

namespace BakeryMS.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repository;
        private readonly IConfiguration _config;
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public AuthController(IAuthRepository repository, IMapper mapper, IConfiguration configuration, DataContext context)
        {
            _mapper = mapper;
            _context = context;
            _config = configuration;
            _repository = repository;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
        {
            userForRegisterDto.Username = userForRegisterDto.Username.ToLower();

            if (await _repository.UserExists(userForRegisterDto.Username))
                return BadRequest("Username Already Exists");

            var userToCreate = _mapper.Map<User>(userForRegisterDto);

            //dynamic data
            userToCreate.Status = true;

            var createdUser = await _repository.Register(userToCreate, userForRegisterDto.Password);
            // TODO --> create a createdAtRoute Response
            // return CreatedAtRoute("GetUser", new { Controller = "Users", id = createdUser.Id }, userToReturn);
            return StatusCode(201);

        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {

            var userFromRepository = await _repository.Login(userForLoginDto.Username.ToLower(), userForLoginDto.Password);

            if (userFromRepository == null)
                return Unauthorized();

            if((bool)userFromRepository.Status == false)
                return Forbid();

            var userRoles = (from user in _context.Users
                             join roleMapping in _context.UserRolesMappings
                                 on user.Id equals roleMapping.User.Id
                             join role in _context.Roles
                                 on roleMapping.Roles.Id equals role.Id
                             where user.Username.ToUpper() == userFromRepository.Username.ToUpper()
                             select role.RoleName).ToArray();
            var claims = new List<Claim>
            {
                    new Claim(ClaimTypes.NameIdentifier,userFromRepository.Id.ToString()),
                    new Claim(ClaimTypes.Name,userFromRepository.Username),
            };

            foreach (var item in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, item));

            }

            var key = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(_config.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new
            {
                token = tokenHandler.WriteToken(token)
            });
        }
    }
}