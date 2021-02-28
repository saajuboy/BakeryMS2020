using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BakeryMS.API.Common.DTOs.Master;
using BakeryMS.API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BakeryMS.API.Controllers.Master
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class BusinessPlacesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        public BusinessPlacesController(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;

        }

        [HttpGet("{id}", Name = "GetBusinessPlace")]
        public async Task<IActionResult> GetBusinessPlace(int id)
        {

            var businessPlaceFromRepo = await _context.BusinessPlaces.FindAsync(id);
            var businessPlaceToReturn = _mapper.Map<BusinessPlaceForDetailDto>(businessPlaceFromRepo);

            return Ok(businessPlaceToReturn);
        }

        [HttpGet]
        public async Task<IActionResult> GetBusinessPlaces()
        {
            var businessPlacesFromRepo = await _context.BusinessPlaces.ToListAsync();
            var businessPlaceListToReturn = _mapper.Map<IEnumerable<BusinessPlaceForDetailDto>>(businessPlacesFromRepo);
            
            return Ok(businessPlaceListToReturn);
        }
    }
}