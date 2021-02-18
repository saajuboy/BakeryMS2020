using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using BakeryMS.API.Common.DTOs.Inventory;
using BakeryMS.API.Common.Params;
using BakeryMS.API.Data;
using BakeryMS.API.Data.Interfaces;
using BakeryMS.API.Models.Inventory;
using BakeryMS.API.Models.Profile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BakeryMS.API.Controllers.Inventory
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class PurchaseOrderController : ControllerBase
    {
        private readonly IInventoryRepository _repository;
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        public PurchaseOrderController(IInventoryRepository repository, IMapper mapper, DataContext context)
        {
            _context = context;
            _mapper = mapper;
            _repository = repository;

        }


        [HttpGet("{id}", Name = "GetPurchaseOrder")]
        public async Task<IActionResult> GetPurchaseOrder(int id)
        {
            POFilterParams filterParams = new POFilterParams()
            {
                isFromOutlet = 2,
                containNotActive = false
            };

            if (User.FindAll(ClaimTypes.Role).Any(a => a.Value == "Admin"))
            {
                filterParams.isFromOutlet = 2;
                filterParams.containNotActive = true;
            }
            else
            {
                filterParams.isFromOutlet = User.FindAll(ClaimTypes.Role).Any(a => a.Value == "OutletManager") ? 0 : 1;
            }

            var purOrderFromRepo = await _repository.GetPurchaseOrder(id, filterParams);

            var purOrderToReturn = _mapper.Map<POHForDetailDto>(purOrderFromRepo);

            return Ok(purOrderToReturn);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,OutletManager,BakeryManager")]
        public async Task<IActionResult> GetPurchaseOrders()
        {
            POFilterParams filterParams = new POFilterParams()
            {
                isFromOutlet = 2,
                containNotActive = false
            };

            if (User.FindAll(ClaimTypes.Role).Any(a => a.Value == "Admin"))
            {
                filterParams.isFromOutlet = 2;
                filterParams.containNotActive = true;
            }
            else
            {
                filterParams.isFromOutlet = User.FindAll(ClaimTypes.Role).Any(a => a.Value == "OutletManager") ? 0 : 1;
            }

            var pOsFromRepo = await _repository.GetPurchaseOrders(filterParams);
            var pOsToReturn = _mapper.Map<IEnumerable<POForListDto>>(pOsFromRepo); // check with list automapping

            var index = 0;
            var users = await _repository.GetAll<User>();
            foreach (var po in pOsFromRepo)
            {
                pOsToReturn.ElementAt(index).UserName = users.FirstOrDefault(a => a.Id == po.UserId).Username;
                index = index + 1;
            }
            return Ok(pOsToReturn);
        }


        [HttpPost]
        [Authorize(Roles = "Admin,OutletManager,BakeryManager")]
        public async Task<IActionResult> CreatePurchaseOrder(POHForDetailDto pOHForDetailDto)
        {
            if (pOHForDetailDto == null)
                return BadRequest("Empty Body");

            var pOHToCreate = _mapper.Map<PurchaseOrderHeader>(pOHForDetailDto);

            if (User.FindAll(ClaimTypes.Role).Any(a => a.Value == "Admin"))
            {
                pOHToCreate.isFromOutlet = pOHForDetailDto.isForOutlet;
            }
            else
            {
                pOHToCreate.isFromOutlet = User.FindAll(ClaimTypes.Role).Any(a => a.Value == "OutletManager") ? true : false;
            }

            // pOHToCreate.Supplier = await _repository.GetSupplier(pOHForDetailDto.SupplierId);

            await _repository.CreatePurchaseOrder(pOHToCreate);

            if (await _repository.SaveAll())
            {
                var pOHtoReturn = _mapper.Map<POHForDetailDto>(pOHToCreate);
                return CreatedAtRoute(nameof(GetPurchaseOrder), new { pOHToCreate.Id }, pOHtoReturn);
            }

            return BadRequest("Could not create Purchase Order");
        }
    }
}