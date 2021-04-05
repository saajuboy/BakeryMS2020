using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using BakeryMS.API.Common.DTOs;
using BakeryMS.API.Common.Helpers;
using BakeryMS.API.Data;
using BakeryMS.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BakeryMS.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class ConfigurationsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public ConfigurationsController(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;

        }

        [HttpGet]
        public async Task<IActionResult> GetConfigs()
        {
            var userid = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var configsFromRepo = await _context.Configurations.Where(a => a.UserId == userid || a.UserId == null)
                                                               .ToListAsync();

            var configsToReturn = _mapper.Map<IList<ConfigDto>>(configsFromRepo);

            return Ok(configsToReturn);
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> UpdateConfig(ConfigListDto configListDto)
        {
            if (configListDto == null)
                return BadRequest(new ErrorModel(1, 400, "Empty Body"));
            if (configListDto.Configurations == null)
                return BadRequest(new ErrorModel(1, 400, "Empty Body"));

            var userid = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var configsFromRepo = await _context.Configurations.Where(a => a.UserId == userid || a.UserId == null)
                                                               .ToListAsync();

            foreach (var config in configListDto.Configurations)
            {
                var des = config.Description;
                var val = config.Value;
                var userId = config.UserId == null ? 0 : config.UserId;

                if (configsFromRepo.Any(a => a.Description == des && a.UserId == userId))
                {
                    configsFromRepo.Find(a => a.Description == des && a.UserId == userId).Value = val;
                }
                else
                {
                    configsFromRepo.Add(new Configuration
                    {
                        Description = des,
                        UserId = userid,
                        Value = val
                    });
                }
            }
            _context.UpdateRange(configsFromRepo);
            if (await _context.SaveChangesAsync() > 0)
            {
                return Ok();
            }

            return BadRequest(new ErrorModel(2, 400, "failed to update Config"));

        }

    }
}