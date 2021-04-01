using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using BakeryMS.API.Common.DTOs.Sales;
using BakeryMS.API.Common.Helpers;
using BakeryMS.API.Data;
using BakeryMS.API.Models.POS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BakeryMS.API.Controllers.Sales
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class PointOfSalesController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public PointOfSalesController(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        [HttpGet("{id}", Name = "GetSale")]
        public async Task<IActionResult> GetSale(int id)
        {
            var prodItems = await _context.ProductionItems.Include(a => a.Item).ToListAsync();
            var compItems = await _context.CompanyItems.Include(a => a.Item).ToListAsync();

            var sale = await _context.SalesHeaders
            .Include(a => a.BusinessPlace)
            .Include(a => a.User).Include(a => a.SalesDetails).FirstOrDefaultAsync(a => a.Id == id);

            var saleToReturn = _mapper.Map<SalesHeaderForPOSDto>(sale);

            foreach (var item in saleToReturn.SalesDetails)
            {
                if (item.Type == 0)
                {
                    item.ItemName = prodItems.Find(a => a.Id == item.ItemId).Item.Name;
                }
                if (item.Type == 1)
                {
                    item.ItemName = compItems.Find(a => a.Id == item.ItemId).Item.Name;
                }
            }

            return Ok(saleToReturn);
        }

        [HttpGet]
        public async Task<IActionResult> GetSales(int placeId, string fromDate, string toDate)
        {
            if (placeId == 0)
                return BadRequest(new ErrorModel(1, 400, "Valid business place required"));

            var place = await _context.BusinessPlaces.FirstOrDefaultAsync(a => a.Id == placeId);

            if (place == null)
                return BadRequest(new ErrorModel(1, 400, "Valid business place required"));

            var salesQuery = _context.SalesHeaders
            .Include(a => a.BusinessPlace)
            .Include(a => a.User)
            .OrderByDescending(a => a.Date).AsQueryable();

            DateTime from;
            DateTime to;

            if (DateTime.TryParse(fromDate, out from))
            {
                salesQuery = salesQuery.Where(a => a.Date >= from);
            }

            if (DateTime.TryParse(toDate, out to))
            {
                salesQuery = salesQuery.Where(a => a.Date <= to);
            }

            var sales = await salesQuery.ToListAsync();

            var saleToReturn = _mapper.Map<IList<SalesHeaderForPOSDto>>(sales);
            return Ok(saleToReturn);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSale(SalesHeaderForPOSDto salesDto)
        {
            if (salesDto == null)
                return BadRequest(new ErrorModel(1, 400, "Empty Body"));

            if (salesDto.SalesDetails == null)
                return BadRequest(new ErrorModel(1, 400, "Empty Body"));
            if (salesDto.BusinessPlaceId == 0)
                return BadRequest(new ErrorModel(2, 400, "Valid business place required"));

            var place = await _context.BusinessPlaces.FirstOrDefaultAsync(a => a.Id == salesDto.BusinessPlaceId);

            if (place == null)
                return BadRequest(new ErrorModel(2, 400, "Valid business place required"));

            if (salesDto.ReceivedAmount == 0 || salesDto.ReceivedAmount < 0)
                return BadRequest(new ErrorModel(3, 400, "Received amount required"));

            if (salesDto.CustomerName == "")
                return BadRequest(new ErrorModel(4, 400, "Customer name required required"));

            var saleToCreate = _mapper.Map<SalesHeader>(salesDto);

            saleToCreate.Date = DateTime.Today;
            var date = DateTime.Now;
            saleToCreate.Time = new TimeSpan(date.Hour, date.Minute, date.Second);
            saleToCreate.IsCharged = true;

            int MaxNo = 0;
            if (!_context.SalesHeaders.Any())
            { MaxNo = 1; }
            else
            {
                MaxNo = await _context.SalesHeaders.MaxAsync(a => a.SalesNo) + 1;
            }

            saleToCreate.SalesNo = MaxNo;
            saleToCreate.Total = saleToCreate.SalesDetails.Sum(a => a.LineTotal) - saleToCreate.Discount;
            saleToCreate.ChangeAmount =
            (saleToCreate.ReceivedAmount < saleToCreate.Total ? saleToCreate.Total : saleToCreate.ReceivedAmount) - saleToCreate.Total;

            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            saleToCreate.UserId = int.Parse(userid);


            // update items table

            var availableProdItems = await _context.ProductionItems
                    .Where(a => a.CurrentPlace == place && a.Item.IsDeleted == false && a.AvailableQuantity > 0).Include(a => a.Item).ToListAsync();
            var availableCompItems = await _context.CompanyItems
                    .Where(a => a.CurrentPlace == place && a.Item.IsDeleted == false && a.AvailableQuantity > 0).Include(a => a.Item).ToListAsync();

            foreach (var item in saleToCreate.SalesDetails)
            {
                if (item.Type == 0)
                {
                    var prod = availableProdItems.Find(a => a.Id == item.ItemId);
                    prod.UsedQuantity += item.Quantity;
                    prod.AvailableQuantity -= item.Quantity;
                }
                if (item.Type == 1)
                {
                    var comp = availableCompItems.Find(a => a.Id == item.ItemId);
                    comp.UsedQuantity += item.Quantity;
                    comp.AvailableQuantity -= item.Quantity;
                }
            }

            await _context.AddRangeAsync(saleToCreate);

            if (await _context.SaveChangesAsync() > 0)
            {
                //update transaction
                if (await _context.Transactions.AnyAsync(a => a.Reference.Contains("Sales") && a.Date == saleToCreate.Date))
                {
                    var transaction = await _context.Transactions.FirstOrDefaultAsync(a => a.Reference.Contains("Sales") && a.Date == saleToCreate.Date);
                    transaction.Debit += saleToCreate.Total;
                }
                else
                {
                    await _context.AddRangeAsync(new Transaction
                    {
                        Date = saleToCreate.Date,
                        Reference = "Sales",
                        Description = "Sale For " + saleToCreate.Date.ToShortDateString(),
                        Debit = saleToCreate.Total,
                        UserId = saleToCreate.UserId,
                        Credit = 0,
                        Time = saleToCreate.Time
                    });
                }

                if (await _context.SaveChangesAsync() > 0)
                {
                    saleToCreate.User = await _context.Users.FirstOrDefaultAsync(a => a.Id == saleToCreate.UserId);
                    // return receipt
                    var saleToReturn = _mapper.Map<SalesHeaderForPOSDto>(saleToCreate);

                    foreach (var item in saleToReturn.SalesDetails)
                    {
                        if (item.Type == 0)
                        {
                            var prod = availableProdItems.Find(a => a.Id == item.ItemId);
                            item.ItemName = prod.Item.Name;
                        }
                        if (item.Type == 1)
                        {
                            var comp = availableCompItems.Find(a => a.Id == item.ItemId);
                            item.ItemName = comp.Item.Name;
                        }
                    }

                    return Ok(saleToReturn);
                }
                return BadRequest(new ErrorModel(6, 400, "Created Sale,But some server error occured"));
            }

            return BadRequest(new ErrorModel(5, 400, "Unable to create sale"));
        }
    }
}