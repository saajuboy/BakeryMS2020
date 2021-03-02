using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BakeryMS.API.Common.DTOs.Manufacturing;
using BakeryMS.API.Common.Helpers;
using BakeryMS.API.Data;
using BakeryMS.API.Data.Interfaces;
using BakeryMS.API.Models.Production;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BakeryMS.API.Controllers.Manufacturing
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class IngredientsController : ControllerBase
    {
        private readonly IProductionRepository _repository;
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        public IngredientsController(IProductionRepository repository, IMapper mapper, DataContext context)
        {
            _context = context;
            _mapper = mapper;
            _repository = repository;
        }

        [HttpGet("{id}", Name = "GetIngredient")]
        public async Task<IActionResult> GetIngredient(int id)
        {
            var IngredientFromRepo = await _repository.GetIngredient(id);

            var IngredientToReturn = _mapper.Map<IngredientHeaderForDetailDto>(IngredientFromRepo);

            return Ok(IngredientToReturn);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,BakeryManager")]
        public async Task<IActionResult> GetIngredients()
        {
            var IngredientFromRepo = await _repository.GetIngredients();
            var ingredientsToReturn = _mapper.Map<IEnumerable<IngredientForListDto>>(IngredientFromRepo);
            return Ok(ingredientsToReturn);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,BakeryManager")]
        public async Task<IActionResult> CreateIngredient(IngredientHeaderForDetailDto IngredientHeaderForDetailDto)
        {
            if (IngredientHeaderForDetailDto == null)
                return BadRequest(new ErrorModel(1, 400, "Empty Body"));

            var IngredientHeaderToCreate = _mapper.Map<IngredientHeader>(IngredientHeaderForDetailDto);

            await _repository.CreateIngredient(IngredientHeaderToCreate);

            if (await _repository.SaveAll())
            {
                var IngredientHeaderToReturn = _mapper.Map<IngredientHeaderForDetailDto>(IngredientHeaderToCreate);
                return CreatedAtRoute(nameof(GetIngredient), new { IngredientHeaderToCreate.Id }, IngredientHeaderToReturn);
            }

            return BadRequest(new ErrorModel(2, 400, "Could not create Ingredient"));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateIngredient(int id, IngredientHeaderForDetailDto ingredientHeaderForDetailDto)
        {

            if (ingredientHeaderForDetailDto == null)
                return BadRequest(new ErrorModel(1, 400, "empty body"));

            var ingredientHeaderFromRepository = await _repository.GetIngredient(id);

            if (ingredientHeaderFromRepository == null)
                return BadRequest(new ErrorModel(3, 400, "Ingredient not available"));

            ingredientHeaderFromRepository.ItemId = ingredientHeaderForDetailDto.ItemId;

            ingredientHeaderFromRepository.Description = ingredientHeaderForDetailDto.Description;
            ingredientHeaderFromRepository.Method = ingredientHeaderForDetailDto.Method;
            ingredientHeaderFromRepository.ServingSize = ingredientHeaderForDetailDto.ServingSize;

            foreach (var pod in ingredientHeaderFromRepository.IngredientsDetail)
            {
                _repository.Delete(pod);
            }

            ingredientHeaderFromRepository.IngredientsDetail.Clear();

            foreach (var pod in ingredientHeaderForDetailDto.IngredientDetails)
            {

                ingredientHeaderFromRepository.IngredientsDetail.Add(new IngredientDetail
                {
                    ItemId = pod.ItemId,
                    Quantity = (decimal)pod.Quantity
                });
            }

            if (await _repository.SaveAll())
            {
                return NoContent();
            }


            throw new System.Exception($"Updating Ingredient {id} failed on save");
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,OutletManager,BakeryManager,Cashier")]
        public async Task<IActionResult> DeleteIngredient(int id)
        {
            var ingredient = await _repository.GetIngredient(id);
            if (ingredient == null)
                return BadRequest(new ErrorModel(3,400,"ingredient not available"));

            ingredient.IsDeleted = true;
            foreach (var ing in ingredient.IngredientsDetail)
            {
                ing.IsDeleted = true;
            }
            if (await _repository.SaveAll())
                return Ok();

            throw new System.Exception($"Failed to delete Ingredient {id}");
        }
    }
}