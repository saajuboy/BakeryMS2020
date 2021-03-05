using System;
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
    public class RoutinesController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IHumanResourceRepository _repository;
        private readonly IMapper _mapper;
        public RoutinesController(DataContext context, IHumanResourceRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
            _context = context;

        }

        [HttpGet("{id}", Name = "GetRoutine")]
        public async Task<IActionResult> GetRoutine(int id)
        {
            var routineFromRepo = await _repository.GetRoutine(id);

            var routineToReturn = _mapper.Map<RoutineForDetailDto>(routineFromRepo);

            return Ok(routineToReturn);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,OutletManager,BakeryManager")]
        public async Task<IActionResult> GetRoutines([FromQuery] DateTime date)
        {
            var routinesFromRepo = await _repository.GetRoutines(date);
            var routinesToReturn = _mapper.Map<IEnumerable<RoutineForDetailDto>>(routinesFromRepo);
            return Ok(routinesToReturn);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,OutletManager,BakeryManager,Cashier")]
        public async Task<IActionResult> CreateRoutine(IEnumerable<RoutineForDetailDto> routinesForDetailDto)
        {
            if (routinesForDetailDto == null)
                return BadRequest(new ErrorModel(1, 400, "Empty Body"));

            var RoutinesToCreate = _mapper.Map<IList<Routine>>(routinesForDetailDto);

            await _repository.CreateRoutine(RoutinesToCreate);


            if (await _repository.SaveAll())
            {
                return NoContent();
            }

            return BadRequest(new ErrorModel(2, 400, "Couldnt create Routines"));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateRoutine(RoutineForListDto routineList, [FromQuery] DateTime date)
        {

            if (routineList == null || routineList.Routines == null)
                return BadRequest(new ErrorModel(1, 400, "Empty Body"));
                
            var routinesForDetailDto = routineList.Routines;

            var routineFromRepository = await _repository.GetRoutines(date);

            if (routineFromRepository == null)
                return BadRequest(new ErrorModel(3, 400, "routines not available"));

            foreach (var routine in routineFromRepository)
            {
                _repository.Delete(routine);
            }
            await _repository.SaveAll();
            routineFromRepository.Clear();

            foreach (var rout in routinesForDetailDto)
            {
                var routToAdd = _mapper.Map<Routine>(rout);
                _repository.Add(routToAdd);
            }

            if (await _repository.SaveAll())
            {
                return NoContent();
            }
            throw new System.Exception($"Updating routines failed on save");
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteRoutine(int id)
        {
            var routine = await _repository.GetRoutine(id);
            if (routine == null)
                return BadRequest(new ErrorModel(3, 400, "routine not available"));

            _repository.Delete(routine);

            if (await _repository.SaveAll())
                return Ok();

            throw new System.Exception($"Failed to delete routine {id}");
        }


        [Route("[action]")]
        [HttpGet]
        public async Task<IActionResult> CreateAutoRoutine([FromQuery] DateTime date)
        {
            if (date == null || date == DateTime.MinValue)
                return BadRequest(new ErrorModel(4, 400, "Invalid Date"));

            var routines = await _repository.GetRoutines(date);
            if(routines.Any())
                return BadRequest(new ErrorModel(5,400,"Routine Already availabe for the date"));

            var previousRoutine = await _repository.GetRoutines(date.AddDays(-1));


            if (previousRoutine.Any())
            { // SOLVE FOR NEW EMPLOYEES CREATED
                foreach (var routine in previousRoutine)
                {
                    Routine routineToCreate = new Routine()
                    {
                        BusinessPlace = routine.BusinessPlace,
                        BusinessPlaceId = routine.BusinessPlaceId,
                        Date = date,
                        Employee = routine.Employee,
                        EmployeeId = routine.EmployeeId,
                        EndTime = routine.EndTime,
                        StartTime = routine.StartTime
                    };

                    _repository.Add(routineToCreate);
                }
            }
            else
            {
                var employees = await _repository.GetEmployees();
                var businessPlace = await _context.BusinessPlaces.FirstOrDefaultAsync();
                foreach (var emp in employees)
                {
                    Routine routineToCreate = new Routine()
                    {
                        BusinessPlace = businessPlace,
                        BusinessPlaceId = businessPlace.Id,
                        Date = date,
                        Employee = emp,
                        EmployeeId = emp.Id,
                        EndTime = new TimeSpan(12, 0, 0),
                        StartTime = new TimeSpan(6, 0, 0)
                    };

                    _repository.Add(routineToCreate);
                }

            }

            if (await _repository.SaveAll())
                return NoContent();

            throw new System.Exception($"Creating routines failed on save");
        }

        // function For retreiving routine base on session
        //


    }
}