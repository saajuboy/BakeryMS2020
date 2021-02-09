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
            itemToCreate.ItemCategory = _invRepo.Get<ItemCategory>(itemForDetailDto.ItemCategory.Id);
            itemToCreate.Unit = _invRepo.Get<Unit>(itemForDetailDto.Unit.Id);

            _invRepo.Add<Item>(itemToCreate);

            if (await _invRepo.SaveAll())
                return Ok();

            throw new System.Exception($"Failed to Create user on save");
 
        }
    }
}