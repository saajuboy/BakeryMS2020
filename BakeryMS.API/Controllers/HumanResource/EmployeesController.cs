using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BakeryMS.API.Common.DTOs.HumanResource;
using BakeryMS.API.Common.Helpers;
using BakeryMS.API.Data;
using BakeryMS.API.Data.Interfaces;
using BakeryMS.API.Models.HumanResource;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BakeryMS.API.Controllers.HumanResource
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class EmployeesController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IHumanResourceRepository _repository;
        private readonly IMapper _mapper;
        public EmployeesController(DataContext context, IHumanResourceRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
            _context = context;

        }

        [HttpGet("{id}", Name = "GetEmployee")]
        public async Task<IActionResult> GetEmployee(int id)
        {
            var employeeFromRepo = await _repository.GetEmployee(id);

            var employeeToReturn = _mapper.Map<EmployeeForDetailDto>(employeeFromRepo);

            return Ok(employeeToReturn);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,OutletManager,BakeryManager")]
        public async Task<IActionResult> GetEmployees()
        {
            var employeesFromRepo = await _repository.GetEmployees();
            var employeesToReturn = _mapper.Map<IEnumerable<EmployeeForListDto>>(employeesFromRepo);
            return Ok(employeesToReturn);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,OutletManager,BakeryManager,Cashier")]
        public async Task<IActionResult> CreateEmployee(EmployeeForDetailDto employeeForDetailDto)
        {
            if (employeeForDetailDto == null)
                return BadRequest(new ErrorModel(1, 400, "Empty Body"));

            var EmployeeToCreate = _mapper.Map<Employee>(employeeForDetailDto);

            await _repository.CreateEmployee(EmployeeToCreate);

            if (await _repository.SaveAll())
            {
                var employeeToReturn = _mapper.Map<EmployeeForDetailDto>(EmployeeToCreate);
                return CreatedAtRoute(nameof(GetEmployee), new { EmployeeToCreate.Id }, employeeToReturn);
            }

            return BadRequest("Could not create Employee");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, EmployeeForDetailDto employeeForDetailDto)
        {

            if (employeeForDetailDto == null)
                return BadRequest(new ErrorModel(1, 400, "Empty Body"));

            var employeeFromRepository = await _repository.GetEmployee(id);

            if (employeeFromRepository == null)
                return BadRequest(new ErrorModel(2, 400, "employee not available"));

            employeeFromRepository.Name = employeeForDetailDto.Name;

            employeeFromRepository.ContactNumber = employeeForDetailDto.ContactNumber;
            employeeFromRepository.NIC = employeeForDetailDto.NIC;
            employeeFromRepository.Address = employeeForDetailDto.Address;
            employeeFromRepository.Type = employeeForDetailDto.Type;
            employeeFromRepository.Salary = employeeForDetailDto.Salary;
            employeeFromRepository.Role = employeeForDetailDto.Role;

            if (await _repository.SaveAll())
            {
                return NoContent();
            }
            throw new System.Exception($"Updating employee {id} failed on save");
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await _repository.GetEmployee(id);
            if (employee == null)
                return BadRequest(new ErrorModel(2, 400, "employee not available"));

            employee.IsDeleted = true;

            if (await _repository.SaveAll())
                return Ok();

            throw new System.Exception($"Failed to delete employee {id}");
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<IActionResult> GetNextEmployeeNo()
        {
            int MaxNo = 0;
            if(!_context.Employees.Any())
                return Ok(MaxNo + 1);

           MaxNo = await _context.Employees.MaxAsync(a => a.EmployeeNumber) + 1;
            return Ok(MaxNo);
        }

        [HttpPatch("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PatchUser(int id, [FromQuery] int isNotActive)
        {
            var employeeFromRepository = await _repository.GetEmployee(id);

            employeeFromRepository.IsNotActive = isNotActive == 1 ? true : false;

            if (await _repository.SaveAll())
                return NoContent();

            throw new System.Exception($"Updating user {id} status failed on save");
        }
    }
}