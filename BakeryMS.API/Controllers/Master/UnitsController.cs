using System.Threading.Tasks;
using BakeryMS.API.Common.DTOs.Master;
using BakeryMS.API.Data;
using BakeryMS.API.Data.Interfaces;
using BakeryMS.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace BakeryMS.API.Controllers.Master
{
    [ApiController]
    [Route("[controller]")]
    public class UnitsController : ControllerBase
    {
        private readonly IInventoryRepository _invRepo;
        private readonly DataContext _context;
        public UnitsController(IInventoryRepository invRepo,DataContext context)
        {
            _context = context;
            _invRepo = invRepo;

        }

        [HttpGet("{id}", Name = "GetUnit")]
        public async Task<IActionResult> GetUnit(int id)
        {
            var unitFromRepo = await _invRepo.Get<Unit>(id);

            return Ok(unitFromRepo);
        }

        [HttpGet]
        public async Task<IActionResult> GetUnits()
        {
            var unitsFromRepo = await _invRepo.GetAll<Unit>();

            return Ok(unitsFromRepo);

        }

        [HttpPost]
        public async Task<IActionResult> CreateUnit(UnitForDetailDto unitForDetailDto)
        {
            if (await _context.Units.AnyAsync(a => a.Description == unitForDetailDto.Description))
                return BadRequest("Unit Exist");

            Unit unitToCreate = new Unit();
            unitToCreate.Description = unitForDetailDto.Description;

            _invRepo.Add<Unit>(unitToCreate);

            if (await _invRepo.SaveAll())
                return Ok();

            throw new System.Exception($"Failed to Create unit on save");

        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUnit(int id, UnitForDetailDto unitForDetailDto)
        {


            var unitFromRepository = await _invRepo.Get<Unit>(id);
            if (unitFromRepository == null)
                return BadRequest("Unit not available");

            if (await _context.Units.AnyAsync(a => a.Description == unitForDetailDto.Description && a.Id != id))
                return BadRequest("Unit Exist");

            unitFromRepository.Description = unitForDetailDto.Description;


            if (await _invRepo.SaveAll())
                return NoContent();

            throw new System.Exception($"Updating unit {id} failed on save");
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,OutletManager,BakeryManager,Cashier")]
        public async Task<IActionResult> DeleteUnit(int id)
        {
            try
            {
                var unitToDelete = await _invRepo.Get<Unit>(id);
                _invRepo.Delete<Unit>(unitToDelete);

                if (await _invRepo.SaveAll())
                    return Ok();
            }
            catch (SqlException ex) when (ex.Number == 547)
            {
                    return BadRequest("Unit has some dependencies");

            }
            throw new System.Exception($"Failed to delete Unit {id}");
        }
        
    }
}