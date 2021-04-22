using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using BakeryMS.API.Common.DTOs;
using BakeryMS.API.Common.DTOs.Inventory;
using BakeryMS.API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BakeryMS.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class DashboardController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public DashboardController(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        [HttpGet("{id}", Name = "GetDashboardData")]
        public async Task<IActionResult> GetDashboardData(int id)
        {
            var date = DateTime.Today.AddDays(-27).Date;

            var transactions = await _context.Transactions.Where(a => a.Date.Date >= date)
                .ToListAsync();

            DashboardDto dashboardDto = new DashboardDto()
            {
                Expense = new List<decimal>(),
                Income = new List<decimal>(),
                Net = new List<decimal>()
            };

            for (int i = 0; i < 28; i++)
            {
                var exp = transactions.Where(a => a.Date.Date == date.AddDays(i).Date).Sum(a => a.Credit);
                var inc = transactions.Where(a => a.Date.Date == date.AddDays(i).Date).Sum(a => a.Debit);
                var net = inc - exp;

                dashboardDto.Expense.Add(exp);
                dashboardDto.Income.Add(inc);
                dashboardDto.Net.Add(net);
            }

            var productionOrders = await _context.ProductionOrderHeaders.Where(a => a.RequiredDate == DateTime.Today.Date).ToListAsync();

            dashboardDto.OrderReceived = productionOrders.Where(a => !(a.isProcessed.HasValue && a.isProcessed.Value == 1)).Count();
            dashboardDto.MaxOrders = productionOrders.Count();

            dashboardDto.OrdersHandled = productionOrders.Where(a => a.IsNotEditable == true && (a.isProcessed.HasValue && a.isProcessed == 0)).Count();
            dashboardDto.MaxOrdersToHandle = productionOrders.Where(a => a.IsNotEditable == true).Count();
            var reOrderCount = 0;
            var reorderMax = 0;
            getReorderCount(out reOrderCount, out reorderMax);

            dashboardDto.MaxItems = reorderMax;
            dashboardDto.Reorders = reOrderCount;

            var workersmax = _context.Employees.Where(a => a.IsDeleted == false && a.IsNotActive == false).Count();
            var workersCount = _context.Routines.Where(a => a.Date.Date == DateTime.Today.Date
            && a.StartTime <= DateTime.Now.TimeOfDay
            && a.EndTime >= DateTime.Now.TimeOfDay).Count();

            dashboardDto.workersMax = workersmax;
            dashboardDto.workers = workersCount;
            
            return Ok(dashboardDto);
        }
        private void getReorderCount(out int reOrderCount, out int reorderMax)
        {
            var userid = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var configsFromRepo = _context.Configurations.Where(a => a.UserId == userid && a.Description.Contains("BusinessPlace"))
                                                               .FirstOrDefault();

            var placeId = int.Parse(configsFromRepo.Value);
            reOrderCount = 0;
            reorderMax = 0;

            var prodItemsAll = _context.ProductionItems
                    .Where(a => a.Item.IsDeleted == false && a.CurrentPlace.Id == placeId)
                    .Include(a => a.CurrentPlace)
                    .Include(a => a.Item).ThenInclude(a => a.Unit).ToList();
            var compItemsAll = _context.CompanyItems
                    .Where(a => a.Item.IsDeleted == false && a.CurrentPlace.Id == placeId)
                    .Include(a => a.CurrentPlace)
                    .Include(a => a.Item).ThenInclude(a => a.Unit).ToList();
            var rawItemsAll = _context.RawItems
                    .Where(a => a.Item.IsDeleted == false && a.CurrentPlace.Id == placeId)
                    .Include(a => a.CurrentPlace)
                    .Include(a => a.Item).ThenInclude(a => a.Unit).ToList();

            IList<AvailableItemsDtoForList> itemListtoReturn = new List<AvailableItemsDtoForList>();

            foreach (var item in prodItemsAll)
            {
                if (!itemListtoReturn.Any(a => a.Code == item.Item.Code))
                {
                    var qty = prodItemsAll.Where(a => a.Item == item.Item).Sum(a => a.AvailableQuantity);
                    if (item.Item.ReOrderLevel > qty)
                    {
                        itemListtoReturn.Add(new AvailableItemsDtoForList
                        {
                            Code = item.Item.Code,
                            AvailableQuantity = qty,
                            IsReorder = true
                        });
                    }

                }
            }
            foreach (var item in compItemsAll)
            {
                if (!itemListtoReturn.Any(a => a.Code == item.Item.Code))
                {
                    var qty = compItemsAll.Where(a => a.Item == item.Item).Sum(a => a.AvailableQuantity);
                    if (item.Item.ReOrderLevel > qty)
                    {
                        itemListtoReturn.Add(new AvailableItemsDtoForList
                        {
                            Code = item.Item.Code,
                            AvailableQuantity = qty,
                            IsReorder = true
                        });
                    }

                }
            }
            foreach (var item in rawItemsAll)
            {
                if (!itemListtoReturn.Any(a => a.Code == item.Item.Code))
                {
                    var qty = rawItemsAll.Where(a => a.Item == item.Item).Sum(a => a.AvailableQuantity);
                    if (item.Item.ReOrderLevel > qty)
                    {
                        itemListtoReturn.Add(new AvailableItemsDtoForList
                        {
                            Code = item.Item.Code,
                            AvailableQuantity = qty,
                            IsReorder = true
                        });
                    }

                }
            }

            reorderMax = _context.Items.Where(a => a.IsDeleted == false).Count();
            reOrderCount = itemListtoReturn.Count();
        }
    }


}