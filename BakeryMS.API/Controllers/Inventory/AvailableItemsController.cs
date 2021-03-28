using System.Xml.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BakeryMS.API.Common.Helpers;
using BakeryMS.API.Data;
using BakeryMS.API.Data.Interfaces;
using BakeryMS.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using BakeryMS.API.Common.DTOs.Inventory;

namespace BakeryMS.API.Controllers.Inventory
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class AvailableItemsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IInventoryRepository _invRepo;
        private readonly DataContext _context;
        public AvailableItemsController(IMapper mapper, IInventoryRepository invRepo, DataContext context)
        {
            _context = context;
            _invRepo = invRepo;
            _mapper = mapper;

        }

        [HttpGet("{id}", Name = "GetAvailableItem")]
        public async Task<IActionResult> GetAvailableItem(int id, [FromQuery] int itemType)
        {
            AvailableItemsDtoForList itemToReturn = new AvailableItemsDtoForList();
            switch (itemType)
            {
                case 0:
                    var prodItems = await _context.ProductionItems.Include(a => a.CurrentPlace)
                    .Include(a => a.Item).ThenInclude(a => a.Unit).FirstOrDefaultAsync(a => a.Id == id && a.Item.IsDeleted == false);

                    itemToReturn = _mapper.Map<AvailableItemsDtoForList>(prodItems);

                    break;

                case 1:
                    var compItems = await _context.CompanyItems.Include(a => a.CurrentPlace)
                    .Include(a => a.Item).ThenInclude(a => a.Unit).FirstOrDefaultAsync(a => a.Id == id && a.Item.IsDeleted == false);

                    itemToReturn = _mapper.Map<AvailableItemsDtoForList>(compItems);

                    break;
                case 2:
                    var rawItems = await _context.RawItems.Include(a => a.CurrentPlace)
                    .Include(a => a.Item).ThenInclude(a => a.Unit).FirstOrDefaultAsync(a => a.Id == id && a.Item.IsDeleted == false);

                    itemToReturn = _mapper.Map<AvailableItemsDtoForList>(rawItems);

                    break;

                default:
                    prodItems = await _context.ProductionItems.Include(a => a.CurrentPlace)
                    .Include(a => a.Item).ThenInclude(a => a.Unit).FirstOrDefaultAsync(a => a.Id == id && a.Item.IsDeleted == false);

                    itemToReturn = _mapper.Map<AvailableItemsDtoForList>(prodItems);
                    break;
            }
            return Ok(itemToReturn);

        }

        [HttpGet]
        public async Task<IActionResult> GetAvailableItems(int placeId, int itemType)
        {
            if (placeId == 0)
                return BadRequest(new ErrorModel(1, 400, "Valid Business Place Required"));
            var place = await _context.BusinessPlaces.FindAsync(placeId);
            if (place == null)
                return BadRequest(new ErrorModel(1, 400, "Valid Business Place Required"));
            if (itemType != 0 && itemType != 1 && itemType != 2)
                return BadRequest(new ErrorModel(2, 400, "Valid Item Type Required"));

            IList<AvailableItemsDtoForList> itemListtoReturn = new List<AvailableItemsDtoForList>();
            switch (itemType)
            {
                case 0:
                    var prodItems = await _context.ProductionItems
                    .Where(a => a.CurrentPlace == place && a.Item.IsDeleted == false && a.AvailableQuantity > 0)
                    .Include(a => a.CurrentPlace)
                    .Include(a => a.Item).ThenInclude(a => a.Unit).ToListAsync();

                    itemListtoReturn = _mapper.Map<IList<AvailableItemsDtoForList>>(prodItems);

                    break;

                case 1:
                    var compItems = await _context.CompanyItems
                    .Where(a => a.CurrentPlace == place && a.Item.IsDeleted == false && a.AvailableQuantity > 0)
                    .Include(a => a.CurrentPlace)
                    .Include(a => a.Item).ThenInclude(a => a.Unit).ToListAsync();

                    itemListtoReturn = _mapper.Map<IList<AvailableItemsDtoForList>>(compItems);

                    break;
                case 2:
                    var rawItems = await _context.RawItems
                    .Where(a => a.CurrentPlace == place && a.Item.IsDeleted == false && a.AvailableQuantity > 0)
                    .Include(a => a.CurrentPlace)
                    .Include(a => a.Item).ThenInclude(a => a.Unit).ToListAsync();

                    itemListtoReturn = _mapper.Map<IList<AvailableItemsDtoForList>>(rawItems);

                    break;

                default:
                    prodItems = await _context.ProductionItems
                       .Where(a => a.CurrentPlace == place && a.Item.IsDeleted == false && a.AvailableQuantity > 0)
                       .Include(a => a.CurrentPlace)
                       .Include(a => a.Item).ThenInclude(a => a.Unit).ToListAsync();

                    itemListtoReturn = _mapper.Map<IList<AvailableItemsDtoForList>>(prodItems);
                    break;
            }

            return Ok(itemListtoReturn);
        }

        // [HttpPost]
        // public async Task<IActionResult> CreateItem(ItemForDetailDto itemForDetailDto)
        // {
        //     var itemToCreate = _mapper.Map<Item>(itemForDetailDto);
        //     itemToCreate.ItemCategory = await _invRepo.Get<ItemCategory>(itemForDetailDto.ItemCategory.Id);
        //     itemToCreate.Unit = await _invRepo.Get<Unit>(itemForDetailDto.Unit.Id);

        //     _invRepo.Add<Item>(itemToCreate);

        //     if (await _invRepo.SaveAll())
        //         return Ok();

        //     throw new System.Exception($"Failed to Create item on save");

        // }
    }
}