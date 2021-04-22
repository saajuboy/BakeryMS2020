using System.Collections.Generic;
using System.Linq;
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
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class ControlProcedureController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        public ControlProcedureController(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;

        }

        [HttpGet("{id}", Name = "GetControlProcedure")]
        public async Task<IActionResult> GetControlProcedure(int id)
        {
            var conFromRepo = await _context.ControlProcedures.Include(a => a.BusinessPlace).FirstOrDefaultAsync(a => a.Id == id && a.IsDeleted == false);
            var conToReturn = _mapper.Map<ControlProcedureDto>(conFromRepo);

            return Ok(conToReturn);
        }

        [HttpGet]
        public async Task<IActionResult> GetControlProcedures(int placeId)
        {
            if (placeId == 0)
                return BadRequest(new ErrorModel(2, 400, "place Required"));
            var place = await _context.BusinessPlaces.FindAsync(placeId);

            var conQuery = _context.ControlProcedures.Where(a => a.IsDeleted == false).Include(a => a.BusinessPlace).AsQueryable();

            if (place != null)
            {
                conQuery = conQuery.Where(a => a.BusinessPlace == place);
            }

            var conFromRepo = await conQuery.ToListAsync();
            var consToReturn = _mapper.Map<IEnumerable<ControlProcedureDto>>(conFromRepo);

            return Ok(consToReturn);
        }

        [HttpPost]
        public async Task<IActionResult> CreateControlProcedure(ControlProcedureDto controlProcedureDto)
        {
            var conTOCreate = _mapper.Map<ControlProcedure>(controlProcedureDto);
            if (await _context.ControlProcedures.AnyAsync(a => a.Name == conTOCreate.Name))
                return BadRequest(new ErrorModel(1, 400, "Control Procedure already exist"));

            _context.Add(conTOCreate);

            if (await _context.SaveChangesAsync() > 0)
                return Ok();

            throw new System.Exception($"Failed to Create Control Procedure on save");

        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateControlProcedure(int id, ControlProcedureDto ConDto)
        {

            var conFromRepository = await _context.ControlProcedures.FirstOrDefaultAsync(a => a.Id == id);
            if (conFromRepository == null)
                return BadRequest("Control Procedure not available");

            if (await _context.ControlProcedures.AnyAsync(a => a.Name == ConDto.Name && a.Id != ConDto.Id))
                return BadRequest("Control Procedure already exist");

            conFromRepository.Name = ConDto.Name;
            conFromRepository.BusinessPlaceId = ConDto.BusinessPlaceId;
            conFromRepository.Description = ConDto.Description;

            if (await _context.SaveChangesAsync() > 0)
                return NoContent();

            return BadRequest(new ErrorModel(1, 400, "No changes Were made"));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,OutletManager,BakeryManager")]
        public async Task<IActionResult> DeleteControlProcedure(int id)
        {
            var Control = await _context.ControlProcedures.FirstOrDefaultAsync(a => a.Id == id);
            Control.IsDeleted = true;
            if (await _context.SaveChangesAsync() > 0)
                return Ok();

            return BadRequest(new ErrorModel(1, 400, "Unable To delete, contact admin"));
        }
    }
}