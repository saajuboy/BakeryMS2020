using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BakeryMS.API.Common.DTOs.Master;
using BakeryMS.API.Data;
using BakeryMS.API.Data.Interfaces;
using BakeryMS.API.Models.Inventory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BakeryMS.API.Controllers.Master
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class SuppliersController : ControllerBase
    {
        private readonly IInventoryRepository _invRepo;
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        public SuppliersController(IInventoryRepository invrepo, IMapper mapper, DataContext context)
        {
            _context = context;
            _mapper = mapper;
            _invRepo = invrepo;

        }

        [HttpGet("{id}", Name = "GetSupplier")]
        public async Task<IActionResult> GetSupplier(int id)
        {
            var supFromRepo = await _invRepo.GetSupplier(id);
            var supToReturn = _mapper.Map<SupplierDto>(supFromRepo);

            return Ok(supToReturn);
        }

        [HttpGet]
        public async Task<IActionResult> GetSuppliers()
        {
            var supsFromRepo = await _invRepo.GetSuppliers();
            var supsToReturn = _mapper.Map<IEnumerable<SupplierDto>>(supsFromRepo);

            return Ok(supsToReturn);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSupplier(SupplierDto supplierDto)
        {
            var supTOCreate = _mapper.Map<Supplier>(supplierDto);
            if (await _context.Suppliers.AnyAsync(a => a.Name == supTOCreate.Name))
                return BadRequest("Supplier already exist");

            _invRepo.Add<Supplier>(supTOCreate);

            if (await _invRepo.SaveAll())
                return Ok();

            throw new System.Exception($"Failed to Create Supplier on save");

        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSupplier(int id, SupplierDto supplierDto)
        {

            var supFromRepository = await _invRepo.GetSupplier(id);
            if (supFromRepository == null)
                return BadRequest("supplier not available");

             if (await _context.Suppliers.AnyAsync(a => a.Name == supplierDto.Name && a.Id != supplierDto.Id))
                return BadRequest("Supplier already exist");

            supFromRepository.Name = supplierDto.Name;
            supFromRepository.ContactNumber = supplierDto.ContactNumber;
            supFromRepository.Email = supplierDto.Email;
            supFromRepository.Address = supplierDto.Address;
            supFromRepository.Type = supplierDto.Type;

            if (await _invRepo.SaveAll())
                return NoContent();

            throw new System.Exception($"Updating supplier {id} failed on save");
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,OutletManager,BakeryManager,Cashier")]
        public async Task<IActionResult> DeleteSupplier(int id)
        {
            var supplier = await _invRepo.GetSupplier(id);
            supplier.IsDeleted = true;
            if (await _invRepo.SaveAll())
                return Ok();

            throw new System.Exception($"Failed to delete supplier {id}");
        }
    }
}