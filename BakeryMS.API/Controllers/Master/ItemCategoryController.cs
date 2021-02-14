using System;
using System.Linq;
using System.Security.AccessControl;
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
    [Route("[controller]")]
    [ApiController]
    public class ItemCategoryController : ControllerBase
    {
        private readonly IInventoryRepository _invRepo;
        private readonly DataContext _context;
        public ItemCategoryController(IInventoryRepository invRepo, DataContext context)
        {
            _context = context;
            _invRepo = invRepo;

        }

        [HttpGet]
        public async Task<IActionResult> GetItemCategories()
        {
            var categoriesFromRepo = await _invRepo.GetAll<ItemCategory>();

            return Ok(categoriesFromRepo);
        }

        [HttpGet("{id}", Name = "GetItemCategory")]
        public async Task<IActionResult> GetItemCategory(int id)
        {
            var categoryFromRepo = await _invRepo.Get<ItemCategory>(id);

            return Ok(categoryFromRepo);
        }
        [HttpPost]
        public async Task<IActionResult> CreateItemCategory(ItemCategoryForDetailDto itemCategoryForDetailDto)
        {
            if (await _context.ItemCategories.AnyAsync(a => a.Code == itemCategoryForDetailDto.Code))
                return BadRequest("item Category Exist");

            ItemCategory catToCreate = new ItemCategory();
            catToCreate.Code = itemCategoryForDetailDto.Code.ToUpper();
            catToCreate.Description = itemCategoryForDetailDto.Description;

            _invRepo.Add<ItemCategory>(catToCreate);

            if (await _invRepo.SaveAll())
                return Ok();

            throw new System.Exception($"Failed to Create Item category on save");

        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItemCategory(int id, ItemCategoryForDetailDto itemCategoryForDetailDto)
        {


            var itemCategoryFromRepository = await _invRepo.Get<ItemCategory>(id);
            if (itemCategoryFromRepository == null)
                return BadRequest("Item category not available");

            if (await _context.ItemCategories.AnyAsync(a => a.Code == itemCategoryForDetailDto.Code && a.Id != id))
                return BadRequest("item Category Exist");

            itemCategoryFromRepository.Code = itemCategoryForDetailDto.Code.ToUpper();
            itemCategoryFromRepository.Description = itemCategoryForDetailDto.Description;


            if (await _invRepo.SaveAll())
                return NoContent();

            throw new System.Exception($"Updating Item Category {id} failed on save");
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,OutletManager,BakeryManager,Cashier")]
        public async Task<IActionResult> DeleteItemCategory(int id)
        {
            try
            {
                // Delete
                var itemCategoryToDelete = await _invRepo.Get<ItemCategory>(id);
                _invRepo.Delete<ItemCategory>(itemCategoryToDelete);

                if (await _invRepo.SaveAll())
                    return Ok();
            }
            catch (SqlException ex) when (ex.Number == 547)
            {
                    return BadRequest("Item category has some dependencies");

            }
            throw new System.Exception($"Failed to delete item Category {id}");
        }

        [Route("[action]/{category}")]
        [HttpGet]
        public async Task<IActionResult> GetCode(int category)
        {
            var count = _context.Items.Count(cat => cat.ItemCategory.Id == category);

            var CatCode = await _context.ItemCategories.Where(a => a.Id == category).Select(a => a.Code).FirstOrDefaultAsync();

            if (CatCode == null)
                return BadRequest("Category does not Exists");

            var code = CatCode + String.Format("{0:00}", count + 1);

            return Ok(new { Code = code });

        }
    }
}