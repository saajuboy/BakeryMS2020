using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BakeryMS.API.Common.DTOs.Master;
using BakeryMS.API.Common.Helpers;
using BakeryMS.API.Data;
using BakeryMS.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BakeryMS.API.Controllers.Master
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class MachineriesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        public MachineriesController(IMapper mapper, DataContext context)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("{id}", Name = "GetMachinery")]
        public async Task<IActionResult> GetMachinery(int id)
        {
            var mchnFromRepo = await _context.Machineries.FirstOrDefaultAsync(a => a.Id == id);
            var mchnToReturn = _mapper.Map<MachineryDto>(mchnFromRepo);

            return Ok(mchnToReturn);
        }

        [HttpGet]
        public async Task<IActionResult> GetMachineries(int placeId)
        {
            if (placeId == 0)
                return BadRequest(new ErrorModel(2, 400, "place Required"));
            var place = await _context.BusinessPlaces.FindAsync(placeId);

            var mchnQuery = _context.Machineries.AsQueryable();

            if (place != null)
            {
                mchnQuery = mchnQuery.Where(a => a.BusinessPlace == place);
            }



            var mchnsFromRepo = await mchnQuery.ToListAsync();
            var mchnsToReturn = _mapper.Map<IEnumerable<MachineryDto>>(mchnsFromRepo);

            return Ok(mchnsToReturn);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMachinery(MachineryDto MachineryDto)
        {
            var mchnTOCreate = _mapper.Map<Machinery>(MachineryDto);
            if (await _context.Machineries.AnyAsync(a => a.Name == mchnTOCreate.Name))
                return BadRequest("Machinery already exist");

            _context.Add(mchnTOCreate);

            if (await _context.SaveChangesAsync() > 0)
                return Ok();

            throw new System.Exception($"Failed to Create Machinery on save");

        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMachinery(int id, MachineryDto MachineryDto)
        {

            var mchnFromRepository = await _context.Machineries.FirstOrDefaultAsync(a => a.Id == id);
            if (mchnFromRepository == null)
                return BadRequest("Machinery not available");

            if (await _context.Machineries.AnyAsync(a => a.Name == MachineryDto.Name && a.Id != MachineryDto.Id))
                return BadRequest("Machinery already exist");

            mchnFromRepository.Name = MachineryDto.Name;
            mchnFromRepository.BusinessPlaceId = MachineryDto.BusinessPlaceId;
            mchnFromRepository.Capacity = MachineryDto.Capacity;
            mchnFromRepository.PurchaseDate = DateTime.Parse(MachineryDto.PurchaseDate);
            mchnFromRepository.Model = MachineryDto.Model;
            mchnFromRepository.Value = MachineryDto.Value;

            if (await _context.SaveChangesAsync() > 0)
                return NoContent();

            throw new System.Exception($"Updating Machinery {id} failed on save");
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,OutletManager,BakeryManager,Cashier")]
        public async Task<IActionResult> DeleteMachinery(int id)
        {
            var Machinery = await _context.Machineries.FirstOrDefaultAsync(a => a.Id == id);
            _context.Remove(Machinery);
            if (await _context.SaveChangesAsync() > 0)
                return Ok();

            throw new System.Exception($"Failed to delete Machinery {id}");
        }
    }
}