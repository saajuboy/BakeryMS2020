using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BakeryMS.API.Common.DTOs.Manufacturing;
using BakeryMS.API.Common.Helpers;
using BakeryMS.API.Data;
using BakeryMS.API.Data.Interfaces;
using BakeryMS.API.Models;
using BakeryMS.API.Models.Production;
using BakeryMS.API.Models.Profile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BakeryMS.API.Controllers.Manufacturing
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class ProductionPlansController : ControllerBase
    {
        private readonly IProductionRepository _repository;
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly IConfiguration _config;
        public ProductionPlansController(IProductionRepository repository, IMapper mapper, DataContext context, IConfiguration config)
        {
            _config = config;
            _context = context;
            _mapper = mapper;
            _repository = repository;
        }

        [HttpGet("{id}", Name = "GetProductionPlan")]
        public async Task<IActionResult> GetProductionPlan(int id)
        {
            var prodPlanFromRepo = await _repository.GetProductionPlan(id);

            var prodPlanToReturn = _mapper.Map<ProdPlanHeaderForDetailDto>(prodPlanFromRepo);

            prodPlanToReturn.ProdOrdrIds = await _context.ProductionOrderHeaders.Where(a => a.IsNotEditable == true && a.PlanId == id).Select(a => a.Id).ToListAsync();

            return Ok(prodPlanToReturn);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,BakeryManager")]
        public async Task<IActionResult> GetProductionPlans()
        {
            var ProdPlansFromRepo = await _repository.GetProductionPlans();
            var prodsToReturn = _mapper.Map<IEnumerable<ProdPlanForListDto>>(ProdPlansFromRepo);
            return Ok(prodsToReturn);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,BakeryManager")]
        public async Task<IActionResult> CreateProductionPlan(ProdPlanHeaderForDetailDto ProdPlanHeaderForDetailDto)
        {
            if (ProdPlanHeaderForDetailDto == null)
                return BadRequest(new ErrorModel(1, 400, "Empty Body"));
            if (ProdPlanHeaderForDetailDto.ProdOrdrIds == null)
                return BadRequest(new ErrorModel(2, 400, "Production orders needed"));

            if (ProdPlanHeaderForDetailDto.ProductionPlanDetails == null
            || ProdPlanHeaderForDetailDto.ProductionPlanRecipes == null
            || ProdPlanHeaderForDetailDto.ProductionPlanWorkers == null
            || ProdPlanHeaderForDetailDto.ProductionPlanMachines == null)
            {
                return BadRequest(new ErrorModel(3, 400, "all nested details required"));
            }

            var planHeaderToCreate = _mapper.Map<ProductionPlanHeader>(ProdPlanHeaderForDetailDto);

            planHeaderToCreate.BusinessPlace = await _repository.Get<BusinessPlace>(ProdPlanHeaderForDetailDto.BusinessPlaceId);
            planHeaderToCreate.ProductionSession = await _repository.Get<ProductionSession>(ProdPlanHeaderForDetailDto.ProductionSessionId);
            planHeaderToCreate.User = await _repository.Get<User>(ProdPlanHeaderForDetailDto.UserId);



            await _repository.CreateProductionPlan(planHeaderToCreate);

            if (await _repository.SaveAll())
            {

                foreach (var prodOrderId in ProdPlanHeaderForDetailDto.ProdOrdrIds)
                {
                    var prodOrder = await _repository.GetProductionOrder(prodOrderId);
                    prodOrder.IsNotEditable = true;
                    prodOrder.PlanId = planHeaderToCreate.Id;
                }

                if (await _repository.SaveAll())
                {
                    var planHeaderToReturn = _mapper.Map<ProdPlanHeaderForDetailDto>(planHeaderToCreate);
                    return CreatedAtRoute(nameof(GetProductionPlan), new { planHeaderToCreate.Id }, planHeaderToReturn);
                }
                return BadRequest(new ErrorModel(4, 400, "created production plan, but some error occured"));
            }
            return BadRequest(new ErrorModel(4, 400, "Could not create production plan"));
        }


        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,BakeryManager")]
        public async Task<IActionResult> UpdateProductionPlan(int id, ProdPlanHeaderForDetailDto ProdPlanHeaderForDetailDto)
        {
            if (ProdPlanHeaderForDetailDto == null)
                return BadRequest(new ErrorModel(1, 400, "Empty Body"));
            if (ProdPlanHeaderForDetailDto.ProdOrdrIds == null)
                return BadRequest(new ErrorModel(2, 400, "Production orders needed"));

            if (ProdPlanHeaderForDetailDto.ProductionPlanDetails == null
            || ProdPlanHeaderForDetailDto.ProductionPlanRecipes == null
            || ProdPlanHeaderForDetailDto.ProductionPlanWorkers == null
            || ProdPlanHeaderForDetailDto.ProductionPlanMachines == null)
            {
                return BadRequest(new ErrorModel(3, 400, "all nested details required"));
            }


            var planHeaderToUpdate = await _repository.GetProductionPlan(id);
            planHeaderToUpdate.UserId = ProdPlanHeaderForDetailDto.UserId;
            planHeaderToUpdate.Description = ProdPlanHeaderForDetailDto.Description;

            foreach (var detail in planHeaderToUpdate.ProductionPlanDetails)
            {
                _repository.Delete(detail);
            }

            planHeaderToUpdate.ProductionPlanDetails.Clear();

            foreach (var detail in ProdPlanHeaderForDetailDto.ProductionPlanDetails)
            {

                planHeaderToUpdate.ProductionPlanDetails.Add(new ProductionPlanDetail
                {
                    ItemId = detail.ItemId,
                    Quantity = (decimal)detail.Quantity,
                    Description = detail.Description
                });
            }

            foreach (var recipe in planHeaderToUpdate.ProductionPlanRecipes)
            {
                _repository.Delete(recipe);
            }

            planHeaderToUpdate.ProductionPlanRecipes.Clear();

            foreach (var recipe in ProdPlanHeaderForDetailDto.ProductionPlanRecipes)
            {

                planHeaderToUpdate.ProductionPlanRecipes.Add(new ProductionPlanRecipe
                {
                    ItemId = recipe.ItemId,
                    Quantity = (decimal)recipe.Quantity,
                    Description = recipe.Description
                });
            }

            foreach (var emp in planHeaderToUpdate.ProductionPlanWorkers)
            {
                _repository.Delete(emp);
            }

            planHeaderToUpdate.ProductionPlanWorkers.Clear();

            foreach (var emp in ProdPlanHeaderForDetailDto.ProductionPlanWorkers)
            {

                planHeaderToUpdate.ProductionPlanWorkers.Add(new ProductionPlanWorker
                {
                    EmployeeId = emp.EmployeeId
                });
            }

            foreach (var mchn in planHeaderToUpdate.ProductionPlanMachines)
            {
                _repository.Delete(mchn);
            }

            planHeaderToUpdate.ProductionPlanMachines.Clear();

            foreach (var mchn in ProdPlanHeaderForDetailDto.ProductionPlanMachines)
            {

                planHeaderToUpdate.ProductionPlanMachines.Add(new ProductionPlanMachine
                {
                    MachineryId = mchn.MachineryId
                });
            }

            if (await _repository.SaveAll())
            {
                var prodsUpdate = await _context.ProductionOrderHeaders.Where(a => a.PlanId == planHeaderToUpdate.Id).ToListAsync();
                foreach (var prod in prodsUpdate)
                {
                    prod.IsNotEditable = false;
                    prod.PlanId = null;
                }
                await _context.SaveChangesAsync();

                foreach (var prodOrderId in ProdPlanHeaderForDetailDto.ProdOrdrIds)
                {
                    var prodOrder = await _context.ProductionOrderHeaders.FirstOrDefaultAsync(a => a.Id == prodOrderId);
                    prodOrder.IsNotEditable = true;
                    prodOrder.PlanId = planHeaderToUpdate.Id;
                }

                if (await _context.SaveChangesAsync() > 0)
                {
                    return NoContent();
                }
                return BadRequest(new ErrorModel(4, 400, "Updated production plan, but some error occured"));
            }
            return BadRequest(new ErrorModel(5, 400, "Could not update production plan"));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,BakeryManager")]
        public async Task<IActionResult> DeleteProductionPlan(int id)
        {
            var Plan = await _repository.GetProductionPlan(id);
            if (Plan.IsNotEditable == true)
                return BadRequest("Plan already finalized, cannot delete");

            Plan.IsDeleted = true;
            foreach (var detail in Plan.ProductionPlanDetails)
            {
                detail.IsDeleted = true;
            }
            if (await _repository.SaveAll())
            {
                var prodOrders = await _context.ProductionOrderHeaders.Where(a => a.PlanId == id).ToListAsync();
                foreach (var prod in prodOrders)
                {
                    prod.IsNotEditable = false;
                    prod.PlanId = null;
                }
                if (await _context.SaveChangesAsync() > 0)
                {
                    return Ok();
                }
                return BadRequest(new ErrorModel(6, 400, "Deleted production plan, but some error occured"));
            }
            throw new System.Exception($"Failed to delete Production order {id}");
        }
    }
}