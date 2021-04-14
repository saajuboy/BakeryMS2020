using System.Globalization;
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
    public class CustomersController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        public CustomersController(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetCustomers()
        {
            var customersFromRepo = await _context.Customers.Where(a => a.IsDeleted == false).ToListAsync();

            return Ok(customersFromRepo);
        }

        [HttpGet("{id}", Name = "GetCustomer")]
        public async Task<IActionResult> GetCustomer(int id)
        {
            var customerFromRepo = await _context.Customers.FirstOrDefaultAsync(a => a.IsDeleted == false && a.Id == id);

            return Ok(customerFromRepo);
        }
        [HttpPost]
        public async Task<IActionResult> CreateCustomer(CustomerDto customerDto)
        {
            if (await _context.Customers.AnyAsync(a => a.Name == customerDto.Name))
                return BadRequest(new ErrorModel(2, 400, "customer already exist"));

            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;

            var cusToCreate = _mapper.Map<Customer>(customerDto);
            cusToCreate.Name = textInfo.ToTitleCase(customerDto.Name.ToLower());

            await _context.AddAsync(cusToCreate);

            if (await _context.SaveChangesAsync() > 0)
                return Ok();

            return BadRequest(new ErrorModel(2, 400, "Failed to create Customer"));

        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(int id, CustomerDto customerDto)
        {


            var cusFromRepo = await _context.Customers.FirstOrDefaultAsync(a => a.IsDeleted == false && a.Id == id);
            if (cusFromRepo == null)
                return BadRequest(new ErrorModel(1, 400, "Customer not available"));

            if (await _context.Customers.AnyAsync(a => a.Name == customerDto.Name && a.Id != id))
                return BadRequest(new ErrorModel(2, 400, "Customer Already Exist"));

            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;

            cusFromRepo.Name = textInfo.ToTitleCase(customerDto.Name.ToLower());
            cusFromRepo.Contact = customerDto.Contact;
            cusFromRepo.Address = customerDto.Address;
            cusFromRepo.IsRetail = customerDto.IsRetail;
            cusFromRepo.Status = customerDto.Status;
            // cusFromRepo.Credit = customerDto.Credit;
            // cusFromRepo.Debit = customerDto.Debit;


            if (await _context.SaveChangesAsync() > 0)
                return NoContent();

            return BadRequest(new ErrorModel(3, 400, "Couldnt update Customer"));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,OutletManager,BakeryManager")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {

            // Delete
            var cusToDelete = await _context.Customers.FirstOrDefaultAsync(a => a.Id == id);

            cusToDelete.IsDeleted = true;

            if (await _context.SaveChangesAsync() > 0)
                return Ok();
            return BadRequest(new ErrorModel(2, 400, "Couldnt Delete Customer"));


        }
    }
}