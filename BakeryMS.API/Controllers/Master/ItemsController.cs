using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BakeryMS.API.Common.DTOs.Master;
using BakeryMS.API.Data.Interfaces;
using BakeryMS.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BakeryMS.API.Controllers.Master
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class ItemsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IInventoryRepository _invRepo;
        public ItemsController(IMapper mapper, IInventoryRepository invRepo)
        {
            _invRepo = invRepo;
            _mapper = mapper;

        }

        [HttpGet("{id}", Name = "GetItem")]
        public async Task<IActionResult> GetItem(int id)
        {
            var itemFromRepo = await _invRepo.GetItem(id);
            var itemToReturn = _mapper.Map<ItemForDetailDto>(itemFromRepo);

            return Ok(itemToReturn);

        }

        [HttpGet]
        public async Task<IActionResult> GetItems()
        {
            var itemsFromRepo = await _invRepo.GetItems();
            var itemsToReturn = _mapper.Map<IEnumerable<ItemForListDto>>(itemsFromRepo);

            return Ok(itemsToReturn);
        }

        [HttpPost]
        public async Task<IActionResult> CreateItem(ItemForDetailDto itemForDetailDto)
        {
            var itemToCreate = _mapper.Map<Item>(itemForDetailDto);
            itemToCreate.ItemCategory = await _invRepo.Get<ItemCategory>(itemForDetailDto.ItemCategory.Id);
            itemToCreate.Unit = await _invRepo.Get<Unit>(itemForDetailDto.Unit.Id);

            _invRepo.Add<Item>(itemToCreate);

            if (await _invRepo.SaveAll())
                return Ok();

            throw new System.Exception($"Failed to Create item on save");

        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItem(int id, ItemForDetailDto itemForDetailDto)
        {

            if (itemForDetailDto.Name == "" ||
                itemForDetailDto.Name == null ||
                itemForDetailDto.Code == null ||
                itemForDetailDto.Code == "" ||
                itemForDetailDto.ItemCategory == null ||
                itemForDetailDto.ItemCategory.Id == 0 ||
                itemForDetailDto.Unit == null ||
                itemForDetailDto.Unit.Id == 0)
            {
                return BadRequest("Insufficent data");

            }

            var itemFromRepository = await _invRepo.GetItem(id);
            if (itemFromRepository == null)
                return BadRequest("Item not available");

            var catFromRepo = await _invRepo.Get<ItemCategory>(itemForDetailDto.ItemCategory.Id);
            var unitFromRepo = await _invRepo.Get<Unit>(itemForDetailDto.Unit.Id);

            itemFromRepository.Name = itemForDetailDto.Name;
            itemFromRepository.Code = itemForDetailDto.Code;
            itemFromRepository.Description = itemForDetailDto.Description;
            itemFromRepository.ItemCategory = catFromRepo;
            itemFromRepository.Unit = unitFromRepo;
            itemFromRepository.Type = itemForDetailDto.Type;
            itemFromRepository.ReOrderLevel = itemForDetailDto.ReOrderLevel;

            if (itemForDetailDto.Type == 0)
            {
                if (itemForDetailDto.SellingPrice > 0)
                    itemFromRepository.SellingPrice = itemForDetailDto.SellingPrice;

                if (itemForDetailDto.ExpireDays > 0)
                    itemFromRepository.ExpireDays = itemForDetailDto.ExpireDays;
            }

            if (await _invRepo.SaveAll())
                return NoContent();

            throw new System.Exception($"Updating item {id} failed on save");
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,OutletManager,BakeryManager,Cashier")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var item = await _invRepo.GetItem(id);
            item.IsDeleted = true;
            if (await _invRepo.SaveAll())
                return Ok();

            throw new System.Exception($"Failed to delete item {id}");
        }
    }
}