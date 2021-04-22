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
        [Route("[action]")]
        [HttpGet]
        public async Task<IActionResult> GetAvailableItemsForPOS(int placeId, int filter)//Filter(0-all,1-top,2-bread,3-buns,4-Biscuits)
        {
            if (placeId == 0)
                return BadRequest(new ErrorModel(1, 400, "Valid Business Place Required"));
            var place = await _context.BusinessPlaces.FindAsync(placeId);
            if (place == null)
                return BadRequest(new ErrorModel(1, 400, "Valid Business Place Required"));

            IEnumerable<AvailableItemsDtoForList> itemListtoReturn = new List<AvailableItemsDtoForList>();
            IList<ProductionItem> prodItems = new List<ProductionItem>();
            IList<CompanyItem> compItems = new List<CompanyItem>();

            var prodItemsQuery = _context.ProductionItems.OrderBy(a => a.ExpireDate)
            .Where(a => a.CurrentPlace == place && a.Item.IsDeleted == false && a.AvailableQuantity > 0)
            .Include(a => a.CurrentPlace)
            .Include(a => a.Item).ThenInclude(a => a.Unit).AsQueryable();

            var compItemsQuery = _context.CompanyItems.OrderBy(a => a.ExpireDate)
            .Where(a => a.CurrentPlace == place && a.Item.IsDeleted == false && a.AvailableQuantity > 0)
            .Include(a => a.CurrentPlace)
            .Include(a => a.Item).ThenInclude(a => a.Unit).AsQueryable();

            switch (filter)
            {
                case 0:
                    prodItems = await prodItemsQuery.ToListAsync();
                    itemListtoReturn = itemListtoReturn.Concat(_mapper.Map<IList<AvailableItemsDtoForList>>(prodItems));

                    compItems = await compItemsQuery.ToListAsync();
                    itemListtoReturn = itemListtoReturn.Concat(_mapper.Map<IList<AvailableItemsDtoForList>>(compItems));

                    break;
                case 1:
                    prodItems = await prodItemsQuery.ToListAsync();
                    itemListtoReturn = itemListtoReturn.Concat(_mapper.Map<IList<AvailableItemsDtoForList>>(prodItems));

                    compItems = await compItemsQuery.ToListAsync();
                    itemListtoReturn = itemListtoReturn.Concat(_mapper.Map<IList<AvailableItemsDtoForList>>(compItems));
                    // for now return all, later do this

                    break;
                case 2:
                    prodItemsQuery = prodItemsQuery.Where(a => a.Item.Code.Contains("BRD"));
                    prodItems = await prodItemsQuery.ToListAsync();
                    itemListtoReturn = _mapper.Map<IList<AvailableItemsDtoForList>>(prodItems);

                    break;
                case 3:
                    prodItemsQuery = prodItemsQuery.Where(a => a.Item.Code.Contains("BUN"));
                    prodItems = await prodItemsQuery.ToListAsync();
                    itemListtoReturn = _mapper.Map<IList<AvailableItemsDtoForList>>(prodItems);

                    break;
                case 4:
                    prodItemsQuery = prodItemsQuery.Where(a => a.Item.Code.Contains("BIS"));
                    prodItems = await prodItemsQuery.ToListAsync();
                    itemListtoReturn = itemListtoReturn.Concat(_mapper.Map<IList<AvailableItemsDtoForList>>(prodItems));

                    compItemsQuery = compItemsQuery.Where(a => a.Item.Code.Contains("BIS"));
                    compItems = await compItemsQuery.ToListAsync();
                    itemListtoReturn = itemListtoReturn.Concat(_mapper.Map<IList<AvailableItemsDtoForList>>(compItems));
                    break;

                default:
                    prodItems = await prodItemsQuery.ToListAsync();
                    itemListtoReturn = itemListtoReturn.Concat(_mapper.Map<IList<AvailableItemsDtoForList>>(prodItems));

                    compItems = await compItemsQuery.ToListAsync();
                    itemListtoReturn = itemListtoReturn.Concat(_mapper.Map<IList<AvailableItemsDtoForList>>(compItems));

                    break;
            }

            return Ok(itemListtoReturn);
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<IActionResult> GetReorderItems(int placeId, int itemType)//Filter(0-all,1-top,2-bread,3-buns,4-Biscuits)
        {
            if (placeId == 0)
                return BadRequest(new ErrorModel(1, 400, "Valid Business Place Required"));
            var place = await _context.BusinessPlaces.FindAsync(placeId);
            if (place == null)
                return BadRequest(new ErrorModel(1, 400, "Valid Business Place Required"));
            if (itemType != 0 && itemType != 1 && itemType != 2)
                return BadRequest(new ErrorModel(2, 400, "Valid Item Type Required"));

            IList<AvailableItemsDtoForList> itemListtoReturn = new List<AvailableItemsDtoForList>();
            IList<ProductionItem> prodItems = new List<ProductionItem>();
            IList<CompanyItem> compItems = new List<CompanyItem>();
            IList<RawItems> rawItems = new List<RawItems>();
            switch (itemType)
            {
                case 0:
                    var prodItemsAll = await _context.ProductionItems.Distinct()
                    .Where(a => a.CurrentPlace == place && a.Item.IsDeleted == false)
                    .Include(a => a.CurrentPlace)
                    .Include(a => a.Item).ThenInclude(a => a.Unit).ToListAsync();

                    foreach (var item in prodItemsAll)
                    {
                        if (!prodItems.Any(a => a.ItemId == item.ItemId))
                        {
                            prodItems.Add(new ProductionItem
                            {
                                Id = item.Id,
                                ItemId = item.ItemId,
                                Item = item.Item,
                                CostPrice = item.CostPrice,
                                CurrentPlace = item.CurrentPlace,
                                ExpireDate = item.ExpireDate,
                                ManufacturedDate = item.ManufacturedDate,
                                BatchNo = item.BatchNo,
                                StockedQuantity = prodItemsAll.Where(a => a.ItemId == item.ItemId).Sum(a => a.StockedQuantity),
                                UsedQuantity = prodItemsAll.Where(a => a.ItemId == item.ItemId).Sum(a => a.UsedQuantity),
                                AvailableQuantity = prodItemsAll.Where(a => a.ItemId == item.ItemId).Sum(a => a.AvailableQuantity)
                                // SellingPrice = item.SellingPrice
                            });
                        }
                    }

                    itemListtoReturn = _mapper.Map<IList<AvailableItemsDtoForList>>(prodItems);

                    break;

                case 1:
                    var compItemsAll = await _context.CompanyItems
                    .Where(a => a.CurrentPlace == place && a.Item.IsDeleted == false )
                    .Include(a => a.CurrentPlace)
                    .Include(a => a.Item).ThenInclude(a => a.Unit).ToListAsync();

                    foreach (var item in compItemsAll)
                    {
                        if (!compItems.Any(a => a.ItemId == item.ItemId))
                        {
                            compItems.Add(new CompanyItem
                            {
                                Id = item.Id,
                                ItemId = item.ItemId,
                                Item = item.Item,
                                CostPrice = item.CostPrice,
                                CurrentPlace = item.CurrentPlace,
                                ExpireDate = item.ExpireDate,
                                ManufacturedDate = item.ManufacturedDate,
                                BatchNo = item.BatchNo,
                                StockedQuantity = compItemsAll.Where(a => a.ItemId == item.ItemId).Sum(a => a.StockedQuantity),
                                UsedQuantity = compItemsAll.Where(a => a.ItemId == item.ItemId).Sum(a => a.UsedQuantity),
                                AvailableQuantity = compItemsAll.Where(a => a.ItemId == item.ItemId).Sum(a => a.AvailableQuantity),
                                SellingPrice = item.SellingPrice
                            });
                        }
                    }

                    itemListtoReturn = _mapper.Map<IList<AvailableItemsDtoForList>>(compItems);

                    break;
                case 2:
                    var rawItemsAll = await _context.RawItems
                    .Where(a => a.CurrentPlace == place && a.Item.IsDeleted == false)
                    .Include(a => a.CurrentPlace)
                    .Include(a => a.Item).ThenInclude(a => a.Unit).ToListAsync();

                    foreach (var item in rawItemsAll)
                    {
                        if (!rawItems.Any(a => a.ItemId == item.ItemId))
                        {
                            rawItems.Add(new RawItems
                            {
                                Id = item.Id,
                                ItemId = item.ItemId,
                                Item = item.Item,
                                CostPrice = item.CostPrice,
                                CurrentPlace = item.CurrentPlace,
                                ExpireDate = item.ExpireDate,
                                ManufacturedDate = item.ManufacturedDate,
                                BatchNo = item.BatchNo,
                                StockedQuantity = rawItemsAll.Where(a => a.ItemId == item.ItemId).Sum(a => a.StockedQuantity),
                                UsedQuantity = rawItemsAll.Where(a => a.ItemId == item.ItemId).Sum(a => a.UsedQuantity),
                                AvailableQuantity = rawItemsAll.Where(a => a.ItemId == item.ItemId).Sum(a => a.AvailableQuantity),
                                SellingPrice = item.SellingPrice
                            });
                        }
                    }

                    itemListtoReturn = _mapper.Map<IList<AvailableItemsDtoForList>>(rawItems);

                    break;

                default:
                    prodItemsAll = await _context.ProductionItems.Distinct()
                    .Where(a => a.CurrentPlace == place && a.Item.IsDeleted == false)
                    .Include(a => a.CurrentPlace)
                    .Include(a => a.Item).ThenInclude(a => a.Unit).ToListAsync();

                    foreach (var item in prodItemsAll)
                    {
                        if (!prodItems.Any(a => a.ItemId == item.ItemId))
                        {
                            prodItems.Add(new ProductionItem
                            {
                                Id = item.Id,
                                ItemId = item.ItemId,
                                Item = item.Item,
                                CostPrice = item.CostPrice,
                                CurrentPlace = item.CurrentPlace,
                                ExpireDate = item.ExpireDate,
                                ManufacturedDate = item.ManufacturedDate,
                                BatchNo = item.BatchNo,
                                StockedQuantity = prodItemsAll.Where(a => a.ItemId == item.ItemId).Sum(a => a.StockedQuantity),
                                UsedQuantity = prodItemsAll.Where(a => a.ItemId == item.ItemId).Sum(a => a.UsedQuantity),
                                AvailableQuantity = prodItemsAll.Where(a => a.ItemId == item.ItemId).Sum(a => a.AvailableQuantity)
                                // SellingPrice = item.SellingPrice
                            });
                        }
                    }

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