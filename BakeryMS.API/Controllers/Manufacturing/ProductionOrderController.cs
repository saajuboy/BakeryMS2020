using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using BakeryMS.API.Common.DTOs.Manufacturing;
using BakeryMS.API.Data;
using BakeryMS.API.Data.Interfaces;
using BakeryMS.API.Models;
using BakeryMS.API.Models.Production;
using BakeryMS.API.Models.Profile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using BakeryMS.API.Common.Helpers;

namespace BakeryMS.API.Controllers.Manufacturing
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class ProductionOrderController : ControllerBase
    {
        private readonly IProductionRepository _repository;
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly IConfiguration _config;
        public ProductionOrderController(IProductionRepository repository,
                                        IMapper mapper,
                                        DataContext context,
                                        IConfiguration config)
        {
            _config = config;
            _context = context;
            _mapper = mapper;
            _repository = repository;

        }


        [HttpGet("{id}", Name = "GetProductionOrder")]
        public async Task<IActionResult> GetProductionOrder(int id)
        {
            var prodOrderFromRepo = await _repository.GetProductionOrder(id);

            var prodOrderToReturn = _mapper.Map<ProdOrderHeaderForDetailDto>(prodOrderFromRepo);

            return Ok(prodOrderToReturn);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,OutletManager,BakeryManager,Cashier")]
        public async Task<IActionResult> GetProductionOrders()
        {
            var ProdFromRepo = await _repository.GetProductionOrders();
            var prodsToReturn = _mapper.Map<IEnumerable<ProdOrderForListDto>>(ProdFromRepo);
            return Ok(prodsToReturn);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,OutletManager,BakeryManager,Cashier")]
        public async Task<IActionResult> CreateProductionOrder(ProdOrderHeaderForDetailDto prodOrderHeaderForDetailDto)
        {
            if (prodOrderHeaderForDetailDto == null)
                return BadRequest("Empty Body");

            var prodOrderHeaderToCreate = _mapper.Map<ProductionOrderHeader>(prodOrderHeaderForDetailDto);

            prodOrderHeaderToCreate.BusinessPlace = await _repository.Get<BusinessPlace>(prodOrderHeaderForDetailDto.BusinessPlaceId);
            prodOrderHeaderToCreate.Session = await _repository.Get<ProductionSession>(prodOrderHeaderForDetailDto.SessionId);
            prodOrderHeaderToCreate.User = await _repository.Get<User>(prodOrderHeaderForDetailDto.UserId);

            await _repository.CreateProductionOrder(prodOrderHeaderToCreate);

            if (await _repository.SaveAll())
            {
                var prodOrderHeaderToReturn = _mapper.Map<ProdOrderHeaderForDetailDto>(prodOrderHeaderToCreate);
                return CreatedAtRoute(nameof(GetProductionOrder), new { prodOrderHeaderToCreate.Id }, prodOrderHeaderToReturn);
            }

            return BadRequest("Could not create production Order");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProductionOrder(int id, ProdOrderHeaderForDetailDto prodOrderHeaderForDetailDto)
        {

            if (prodOrderHeaderForDetailDto == null)
                return BadRequest("Empty Body");

            var prodOrderHeaderFromRepository = await _repository.GetProductionOrder(id);

            if (prodOrderHeaderFromRepository == null)
                return BadRequest("Production Order not available");

            prodOrderHeaderFromRepository.UserId = prodOrderHeaderForDetailDto.UserId;

            prodOrderHeaderFromRepository.BusinessPlace = await _repository.Get<BusinessPlace>(prodOrderHeaderForDetailDto.BusinessPlaceId);
            prodOrderHeaderFromRepository.Session = await _repository.Get<ProductionSession>(prodOrderHeaderForDetailDto.SessionId);
            prodOrderHeaderFromRepository.User = await _repository.Get<User>(prodOrderHeaderForDetailDto.UserId);
            prodOrderHeaderFromRepository.RequiredDate = DateTime.Parse(prodOrderHeaderForDetailDto.RequiredDate);
            prodOrderHeaderFromRepository.EnteredDate = DateTime.Parse(prodOrderHeaderForDetailDto.EnteredDate);
            prodOrderHeaderFromRepository.IsNotEditable = false;

            foreach (var pod in prodOrderHeaderFromRepository.ProductionOrderDetails)
            {
                _repository.Delete(pod);
            }

            prodOrderHeaderFromRepository.ProductionOrderDetails.Clear();

            foreach (var pod in prodOrderHeaderForDetailDto.ProductionOrderDetails)
            {

                prodOrderHeaderFromRepository.ProductionOrderDetails.Add(new ProductionOrderDetail
                {
                    ItemId = pod.ItemId,
                    Quantity = (decimal)pod.Quantity,
                    Description = pod.Description
                });
            }

            if (await _repository.SaveAll())
            {
                return NoContent();
            }


            throw new System.Exception($"Updating production Order {id} failed on save");
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,OutletManager,BakeryManager,Cashier")]
        public async Task<IActionResult> DeleteProductionOrder(int id)
        {
            var PO = await _repository.GetProductionOrder(id);
            if (PO.IsNotEditable == true)
                return BadRequest("Order already review, cannot delete");

            PO.IsDeleted = true;
            foreach (var pod in PO.ProductionOrderDetails)
            {
                pod.IsDeleted = true;
            }
            if (await _repository.SaveAll())
                return Ok();

            throw new System.Exception($"Failed to delete Production order {id}");
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<IActionResult> GetAutoProductionOrder(int sessionId, int placeId, string requiredDate)
        {
            bool isRaining;
            bool isSaturday;
            float percentageCount = 100;
            bool isSpecialHoliday = false;

            DateTime reqDate;
            DateTime.TryParse(requiredDate, out reqDate);

            if (sessionId == 0)
                return BadRequest(new ErrorModel(1, 400, "session Required"));
            if (placeId == 0)
                return BadRequest(new ErrorModel(2, 400, "place Required"));
            if (reqDate == null || reqDate < DateTime.Today)
                return BadRequest(new ErrorModel(3, 400, "proper date Required"));

            var session = await _context.ProductionSessions.FindAsync(sessionId);
            if (session == null)
                return BadRequest(new ErrorModel(4, 400, "session Not Valid"));

            var place = await _context.BusinessPlaces.FindAsync(placeId);
            if (place == null)
                return BadRequest(new ErrorModel(5, 400, "Place Not valid"));

            if ((reqDate - DateTime.Today).Days > 7)
                return BadRequest(new ErrorModel(6, 400, "Date Too far ahead"));


            string appId = _config.GetSection("AppSettings:WeatherAPIKey").Value;
            string lat = _config.GetSection("AppSettings:Latitude").Value;
            string lon = _config.GetSection("AppSettings:longitude").Value;

            string url = string.Format("https://api.openweathermap.org/data/2.5/onecall?exclude=hourly,current,minutely,alerts&appid={0}&lat={1}&lon={2}", appId, lat, lon);

            WeatherInfo weatherInfo = new WeatherInfo();

            using (WebClient client = new WebClient())
            {
                string json = await client.DownloadStringTaskAsync(url);
                weatherInfo = JsonConvert.DeserializeObject<WeatherInfo>(json);
                foreach (var daily in weatherInfo.daily)
                {
                    daily.date = (DateTimeOffset.FromUnixTimeSeconds(daily.dt).LocalDateTime);
                }

            }

            isRaining = (weatherInfo.daily.FirstOrDefault(a => a.date.Date == reqDate.Date).rain > 0) ? true : false;

            isSpecialHoliday = 2 > 1 ? false : true; // to after employee model
            isSaturday = reqDate.DayOfWeek == DayOfWeek.Saturday ? true : false;

            percentageCount = isRaining ? percentageCount - 5 : percentageCount;
            percentageCount = isSpecialHoliday ? percentageCount + 10 : percentageCount;
            percentageCount = isSaturday ? percentageCount + 10 : percentageCount;

            var prodOrderFromRepo = await _context.ProductionOrderHeaders
            .Include(a => a.ProductionOrderDetails)
            .OrderByDescending(a => a.RequiredDate)
            .FirstOrDefaultAsync(a => a.Session == session && a.BusinessPlace == place && a.IsNotEditable == true);

            if (prodOrderFromRepo == null)
            {
                return BadRequest(new ErrorModel(7, 400, "No previous production order from this session and place available to auto generate "));
            }

            prodOrderFromRepo.RequiredDate = reqDate;
            prodOrderFromRepo.IsNotEditable = false;
            prodOrderFromRepo.EnteredDate = DateTime.Today;

            foreach (var item in prodOrderFromRepo.ProductionOrderDetails)
            {
                item.Quantity = Math.Ceiling(((item.Quantity * (decimal)percentageCount) / 100));
            }

            var prodOrderToReturn = _mapper.Map<ProdOrderHeaderForDetailDto>(prodOrderFromRepo);

            return Ok(prodOrderToReturn);
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<IActionResult> GetFilteredProductionOrders(int sessionId, int placeId, string requiredDate, bool isReviewed, int planId)//place and plan are optional
        {
            DateTime reqDate;
            DateTime.TryParse(requiredDate, out reqDate);

            if (sessionId == 0)
                return BadRequest(new ErrorModel(1, 400, "session Required"));
            // if (placeId == 0)
            //     return BadRequest(new ErrorModel(2, 400, "place Required"));
            if (reqDate == null || reqDate < DateTime.Today)
                return BadRequest(new ErrorModel(3, 400, "proper date Required"));

            var ProdOrdsQuery = _context.ProductionOrderHeaders
            .Where(a => a.RequiredDate == reqDate)
            .Include(a => a.BusinessPlace)
            .Include(a => a.ProductionOrderDetails).ThenInclude(a => a.Item)
            .AsQueryable();

            var session = await _context.ProductionSessions.FindAsync(sessionId);
            if (session == null)
                return BadRequest(new ErrorModel(4, 400, "session Not Valid"));

            ProdOrdsQuery = ProdOrdsQuery.Where(a => a.Session == session);

            // var place = await _context.BusinessPlaces.FindAsync(placeId);
            // if (place == null)
            //     return BadRequest(new ErrorModel(5, 400, "Place Not valid"));


            if (placeId > 0)
            {
                var place = await _context.BusinessPlaces.FindAsync(placeId);
                if (place != null)
                    ProdOrdsQuery = ProdOrdsQuery.Where(a => a.BusinessPlace == place);
            }

            if (isReviewed == false)
            {
                ProdOrdsQuery = ProdOrdsQuery.Where(a => a.IsNotEditable == false);
            }
            else
            {
                ProdOrdsQuery = ProdOrdsQuery.Where(a => a.IsNotEditable == true);
                if (planId > 0)
                {
                    ProdOrdsQuery = ProdOrdsQuery.Where(a => a.PlanId == planId);
                }
            }

            var prodOrds = await ProdOrdsQuery.ToListAsync();

            var ordersToReturn = _mapper.Map<IEnumerable<ProdOrderHeaderForDetailDto>>(prodOrds); ;

            return Ok(ordersToReturn);
        }
        [Route("[action]")]
        [HttpGet]
        public async Task<IActionResult> GetPlanProductionOrders(int planId)
        {
            var ProdOrdsQuery = _context.ProductionOrderHeaders
            .Include(a => a.BusinessPlace)
            .Include(a => a.ProductionOrderDetails).ThenInclude(a => a.Item)
            .AsQueryable();

            if (planId > 0)
            {
                ProdOrdsQuery = ProdOrdsQuery.Where(a => a.PlanId == planId && a.IsNotEditable == true && (a.isProcessed == null || a.isProcessed == 0));
            }

            var prodOrds = await ProdOrdsQuery.ToListAsync();
            var ordersToReturn = _mapper.Map<IEnumerable<ProdOrderHeaderForDetailDto>>(prodOrds); ;

            return Ok(ordersToReturn);
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> AcceptItems([FromQuery] int planId, [FromBody] ProdOrderHeaderForDetailDto prodOrderHeaderForDetailDto)
        {
            if (prodOrderHeaderForDetailDto == null)
                return BadRequest(new ErrorModel(1, 400, "Empty Body"));

            if (planId == 0)
                return BadRequest(new ErrorModel(2, 400, "Invalid Plan"));
            var plan = await _repository.GetProductionPlan(planId);
            if (plan == null)
                return BadRequest(new ErrorModel(2, 400, "Invalid Plan"));

            if (prodOrderHeaderForDetailDto.Id == 0)
                return BadRequest(new ErrorModel(3, 400, "Valid Prod order Id required"));
            var prodOrder = await _repository.GetProductionOrder(prodOrderHeaderForDetailDto.Id);
            if (prodOrder == null)
                return BadRequest(new ErrorModel(3, 400, "Valid Prod order Id required"));

            if (prodOrderHeaderForDetailDto.ProductionOrderDetails == null)
                return BadRequest(new ErrorModel(4, 400, "Items required"));


            var prodItems = await _context.ProductionItems
            .Where(a => a.CurrentPlace == prodOrder.BusinessPlace && a.BatchNo == planId).ToListAsync();

            var itemCost = await GetCostOfPlan(plan, 10);//changes

            foreach (var item in prodOrderHeaderForDetailDto.ProductionOrderDetails)
            {
                var itemFromPITEMSList = prodItems.FirstOrDefault(a => a.ItemId == item.ItemId);

                if (itemFromPITEMSList == null)
                {
                    ProductionItem pItem = new ProductionItem();
                    pItem.Id = 0;
                    pItem.Item = null;
                    pItem.ItemId = item.ItemId;
                    pItem.BatchNo = planId;
                    pItem.StockedQuantity = item.Quantity;
                    pItem.AvailableQuantity = item.Quantity;
                    pItem.UsedQuantity = 0;
                    pItem.CostPrice = itemCost.FirstOrDefault(a => a.ItemID == item.ItemId).CostPerUnit;//changes
                    pItem.CurrentPlace = prodOrder.BusinessPlace;
                    pItem.ManufacturedDate = DateTime.Now;

                    var expiryDays = prodOrder.ProductionOrderDetails.FirstOrDefault(a => a.ItemId == item.ItemId).Item.ExpireDays;
                    expiryDays = expiryDays == null ? 0 : expiryDays;
                    pItem.ExpireDate = DateTime.Now.AddDays(expiryDays.Value);

                    prodItems.Add(pItem);
                }
                else
                {
                    var existingQty = itemFromPITEMSList.StockedQuantity;

                    prodItems.FirstOrDefault(a => a.ItemId == item.ItemId).StockedQuantity = existingQty + item.Quantity;
                    prodItems.FirstOrDefault(a => a.ItemId == item.ItemId).AvailableQuantity += item.Quantity;
                }
            }

            _context.UpdateRange(prodItems);

            prodOrder.isProcessed = 1;
            _context.Update(prodOrder);

            Notification noti = new Notification
            {
                Title = "Items Sent for prod order " + prodOrder.ProductionOrderNo,
                Message = "Items for production order No - " + prodOrder.ProductionOrderNo + "  has been sent from " + plan.BusinessPlace.Name,
                DateTime = DateTime.Now,
                UserId = prodOrder.UserId,
                Status = 0
            };

            _context.Add(noti);

            if (await _context.SaveChangesAsync() > 0)
            {
                var prods = await _context.ProductionOrderHeaders.Where(a => a.PlanId == planId && (a.isProcessed == 0 || a.isProcessed == null)).ToListAsync();
                if (prods == null || prods.Count == 0)
                {
                    //changes
                    var availableRawMaterials = await _context.RawItems.Where(a => a.CurrentPlace == plan.BusinessPlace && a.AvailableQuantity > 0).ToListAsync();
                    //need more
                    foreach (var recipeitem in plan.ProductionPlanRecipes)
                    {
                        var quantityToUpdate = recipeitem.Quantity;
                        foreach (var item in availableRawMaterials.Where(a => a.ItemId == recipeitem.ItemId))
                        {
                            if (quantityToUpdate == 0)
                            {
                                break;
                            }

                            if (item.AvailableQuantity > quantityToUpdate)
                            {
                                item.UsedQuantity = item.UsedQuantity + quantityToUpdate;
                                item.AvailableQuantity = item.AvailableQuantity - quantityToUpdate;
                                quantityToUpdate = 0;
                            }
                            else
                            {
                                item.UsedQuantity = item.UsedQuantity + item.AvailableQuantity;
                                item.AvailableQuantity = item.AvailableQuantity - item.AvailableQuantity;
                                quantityToUpdate = quantityToUpdate - item.AvailableQuantity;

                            }
                        }
                    }

                    var planToUpdate = await _context.ProductionPlanHeaders.FirstOrDefaultAsync(a => a.Id == planId);
                    planToUpdate.IsNotEditable = true;
                    await _context.SaveChangesAsync();
                }
                return Ok();
            }

            return BadRequest(new ErrorModel(5, 400, "Couldn't Accept, Some error occured"));

        }
        //changes
        private async Task<IList<ItemCost>> GetCostOfPlan(ProductionPlanHeader plan, int itemId)
        {
            var availableRawMaterials = await _context.RawItems.Where(a => a.CurrentPlace == plan.BusinessPlace && a.AvailableQuantity > 0).ToListAsync();
            // calculate Total cost of eacch raw item used
            IList<ItemCost> rawItemCostList = new List<ItemCost>();
            IList<ItemCost> prodItemCostList = new List<ItemCost>();
            foreach (var recipeitem in plan.ProductionPlanRecipes)
            {
                var currentItemList = availableRawMaterials.Where(a => a.ItemId == recipeitem.ItemId);
                decimal accumulatedCost = 0;
                decimal accumulatedQty = 0;

                foreach (var rawItem in currentItemList)
                {
                    accumulatedCost = accumulatedCost + (rawItem.AvailableQuantity * rawItem.CostPrice);
                    accumulatedQty = accumulatedQty + rawItem.AvailableQuantity;
                }

                var costPerUnit = accumulatedCost / accumulatedQty;

                rawItemCostList.Add(new ItemCost { ItemID = recipeitem.ItemId, CostPerUnit = costPerUnit });
            }

            foreach (var item in plan.ProductionPlanDetails)
            {
                var planQuantity = item.Quantity;
                var recipe = await _context.IngredientHeaders.Include(a => a.IngredientsDetail).FirstOrDefaultAsync(a => a.ItemId == item.ItemId);
                decimal accumulatedCost = 0;
                foreach (var rawItem in recipe.IngredientsDetail)
                {
                    var costPerUnitItem = rawItemCostList.FirstOrDefault(a => a.ItemID == rawItem.ItemId).CostPerUnit;

                    accumulatedCost = accumulatedCost + (costPerUnitItem * rawItem.Quantity);
                }
                var costForOne = accumulatedCost / recipe.ServingSize;

                prodItemCostList.Add(new ItemCost { ItemID = item.ItemId, CostPerUnit = costForOne });

            }
            //cost of labour and other misc
            return prodItemCostList;
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<IActionResult> GetReorderProductionOrder(int placeId)
        {
            if (placeId == 0)
                return BadRequest(new ErrorModel(1, 400, "Valid Business Place Required"));
            var place = await _context.BusinessPlaces.FindAsync(placeId);
            if (place == null)
                return BadRequest(new ErrorModel(1, 400, "Valid Business Place Required"));

            IList<ProductionItem> prodItems = new List<ProductionItem>();
            var prodItemsAll = await _context.ProductionItems.Distinct()
                .Where(a => a.CurrentPlace == place && a.Item.IsDeleted == false)
                .Include(a => a.CurrentPlace)
                .Include(a => a.Item).ThenInclude(a => a.Unit).ToListAsync();

            foreach (var item in prodItemsAll)
            {
                if (!prodItems.Any(a => a.ItemId == item.ItemId))
                {
                    var totalAvailQty = prodItemsAll.Where(a => a.ItemId == item.ItemId).Sum(a => a.AvailableQuantity);
                    var reOrderLevel = prodItemsAll.Where(a => a.ItemId == item.ItemId).FirstOrDefault().Item.ReOrderLevel;
                    if (reOrderLevel > totalAvailQty)
                    {
                        prodItems.Add(new ProductionItem
                        {
                            Id = item.Id,
                            ItemId = item.ItemId,
                            Item = item.Item,
                            AvailableQuantity = reOrderLevel
                        });
                    }

                }
            }

            if (prodItems.Count == 0)
                return BadRequest(new ErrorModel(2, 400, "No Reorders available"));

            ProductionOrderHeader prodOrder = new ProductionOrderHeader
            {
                BusinessPlace = place,
                EnteredDate = DateTime.Today,
                Description = "Reorder",
                RequiredDate = DateTime.Today.AddDays(1),
                Session = await _context.ProductionSessions.OrderByDescending(a => a.Id).FirstOrDefaultAsync(a => a.StartTime > DateTime.Now.TimeOfDay),
                ProductionOrderDetails = new List<ProductionOrderDetail>()
            };

            foreach (var item in prodItems)
            {
                prodOrder.ProductionOrderDetails.Add(new ProductionOrderDetail
                {
                    Item = item.Item,
                    ItemId = item.ItemId,
                    Quantity = item.AvailableQuantity * 2
                });
            }

            var prodOrderToReturn = _mapper.Map<ProdOrderHeaderForDetailDto>(prodOrder);

            return Ok(prodOrderToReturn);
        }
    }


}