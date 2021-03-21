using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BakeryMS.API.Common.DTOs.Inventory;
using BakeryMS.API.Common.Helpers;
using BakeryMS.API.Data;
using BakeryMS.API.Data.Interfaces;
using BakeryMS.API.Models;
using BakeryMS.API.Models.Inventory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BakeryMS.API.Controllers.Inventory
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class GRNController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IInventoryRepository _repository;
        private readonly IMapper _mapper;
        public GRNController(IInventoryRepository repository, IMapper mapper, DataContext context)
        {
            _context = context;
            _mapper = mapper;
            _repository = repository;
        }

        [HttpGet("{id}", Name = "GetGRN")]
        public async Task<IActionResult> GetGRN(int id)
        {
            var GRNFromRepo = await _repository.GetGRN(id);

            var GRNToReturn = _mapper.Map<GRNHeaderForDetailDto>(GRNFromRepo);

            return Ok(GRNToReturn);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,OutletManager,BakeryManager")]
        public async Task<IActionResult> GetGRNs()
        {
            var grnFromRepo = await _repository.GetGRNs();
            var grnToReturn = _mapper.Map<IEnumerable<GRNHeaderForDetailDto>>(grnFromRepo);

            return Ok(grnToReturn);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,OutletManager,BakeryManager")]
        public async Task<IActionResult> CreateGRN(GRNHeaderForDetailDto gRNHeaderForDetailDto)
        {
            if (gRNHeaderForDetailDto == null)
                return BadRequest(new ErrorModel(1, 400, "Empty Body"));
            if (gRNHeaderForDetailDto.GRNDetails == null)
                return BadRequest(new ErrorModel(2, 400, "received items needed"));

            var purchaseOrder = await _context.PurchaseOrderHeaders.Include(a => a.Supplier).Include(a => a.BusinessPlace).FirstOrDefaultAsync(a => a.Id == gRNHeaderForDetailDto.PurchaseOrderHeaderId);
            if (purchaseOrder == null)
                return BadRequest(new ErrorModel(3, 400, "Invalid purchase order"));

            GRNHeader grnToCreate = new GRNHeader
            {
                PurchaseOrderHeaderId = gRNHeaderForDetailDto.PurchaseOrderHeaderId,
                ReceivedDate = DateTime.Parse(gRNHeaderForDetailDto.ReceivedDate),
                PaymentMode = gRNHeaderForDetailDto.PaymentMode,
                PaidAmount = gRNHeaderForDetailDto.PaidAmount
            };

            IList<GRNDetail> grnDetailList = new List<GRNDetail>();
            IList<CompanyItem> compItemList = new List<CompanyItem>();
            IList<RawItems> rawItemList = new List<RawItems>();

            var items = await _context.Items.Where(a => a.IsDeleted == false).ToListAsync();

            decimal total = 0;

            foreach (var detail in gRNHeaderForDetailDto.GRNDetails)
            {
                decimal lineTotal = 0;
                if (detail.Quantity > 0)
                {
                    lineTotal = detail.Quantity * detail.UnitPrice;
                    grnDetailList.Add(new GRNDetail
                    {
                        ItemId = detail.ItemId,
                        Quantity = detail.Quantity,
                        UnitPrice = detail.UnitPrice,
                        SellingPrice = detail.SellingPrice,
                        LineTotal = lineTotal
                    });

                    total = total + lineTotal;

                    if (items.Find(a => a.Id == detail.ItemId).Type == 1)//company
                    {
                        compItemList.Add(CreatCompanyItem(detail, purchaseOrder.BusinessPlace, gRNHeaderForDetailDto.PurchaseOrderHeaderId));
                    }
                    if (items.Find(a => a.Id == detail.ItemId).Type == 2)//raw
                    {
                        rawItemList.Add(CreatRawItem(detail, purchaseOrder.BusinessPlace, gRNHeaderForDetailDto.PurchaseOrderHeaderId));
                    }
                }
            }

            grnToCreate.TotalAmount = total;
            grnToCreate.GRNDetails = grnDetailList;

            await _context.AddRangeAsync(grnToCreate);

            await _context.AddRangeAsync(rawItemList);

            await _context.AddRangeAsync(compItemList);

            purchaseOrder.Status = 2;

            if (await _context.SaveChangesAsync() > 0)
                return Ok();


            return BadRequest(new ErrorModel(3, 400, "Failed to Save GRN"));

        }

        private RawItems CreatRawItem(GRNDetailForDetailDto detail, BusinessPlace businessPlace, int purchaseOrderId)
        {
            RawItems itemToReturn = new RawItems()
            {
                ItemId = detail.ItemId,
                BatchNo = purchaseOrderId,
                CurrentPlace = businessPlace,
                ManufacturedDate = DateTime.Parse(detail.ManufacturedDate),
                ExpireDate = DateTime.Parse(detail.ExpiredDate),
                StockedQuantity = detail.Quantity,
                UsedQuantity = 0,
                AvailableQuantity = detail.Quantity,
                CostPrice = detail.UnitPrice
            };

            return itemToReturn;
        }

        private CompanyItem CreatCompanyItem(GRNDetailForDetailDto detail, BusinessPlace businessPlace, int purchaseOrderId)
        {
            CompanyItem itemToReturn = new CompanyItem()
            {
                ItemId = detail.ItemId,
                BatchNo = purchaseOrderId,
                CurrentPlace = businessPlace,
                ManufacturedDate = DateTime.Parse(detail.ManufacturedDate),
                ExpireDate = DateTime.Parse(detail.ExpiredDate),
                StockedQuantity = detail.Quantity,
                UsedQuantity = 0,
                AvailableQuantity = detail.Quantity,
                CostPrice = detail.UnitPrice,
                SellingPrice = detail.SellingPrice
            };

            return itemToReturn;
        }
    }
}