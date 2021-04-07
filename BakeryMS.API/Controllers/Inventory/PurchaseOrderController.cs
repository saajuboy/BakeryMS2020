using System.Text;
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
using BakeryMS.API.Models;
using BakeryMS.API.Models.Inventory;
using BakeryMS.API.Models.Profile;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MimeKit.Text;
using BakeryMS.API.Common.Helpers;
using Microsoft.EntityFrameworkCore;

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
            var filterParams = GetFilterParams();

            var purOrderFromRepo = await _repository.GetPurchaseOrder(id, filterParams);

            var purOrderToReturn = _mapper.Map<POHForDetailDto>(purOrderFromRepo);

            return Ok(purOrderToReturn);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,OutletManager,BakeryManager")]
        public async Task<IActionResult> GetPurchaseOrders()
        {
            var filterParams = GetFilterParams();

            var pOsFromRepo = await _repository.GetPurchaseOrders(filterParams);
            var pOsToReturn = _mapper.Map<IEnumerable<POForListDto>>(pOsFromRepo);

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
        public async Task<IActionResult> CreatePurchaseOrder(POHForDetailDto pOHForDetailDto, [FromQuery] bool isForSending)
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

            pOHToCreate.Status = 0;
            // pOHToCreate.Supplier = await _repository.GetSupplier(pOHForDetailDto.SupplierId);

            await _repository.CreatePurchaseOrder(pOHToCreate);

            if (await _repository.SaveAll())
            {
                //sending mail
                var filterParams = GetFilterParams();

                var pOHFromRepository = await _repository.GetPurchaseOrder(pOHToCreate.Id, filterParams);
                if (isForSending)
                {
                    var htmlBody = GenerateMailHtml(pOHFromRepository.PONumber, User.FindAll(ClaimTypes.Name).FirstOrDefault().ToString(),
                                                 pOHFromRepository.Supplier.Name, pOHFromRepository.DeliveryMethod, pOHFromRepository.OrderDate,
                                                 pOHFromRepository.DeliveryDate, pOHFromRepository.PurchaseOrderDetail);

                    if (await SendMail("hyeah227@gmail.com", "saajidhumar@gmail.com", "Purchase Order From Upland Bake house",
                                    htmlBody, "hyeah227@gmail.com", "hell123boy"))
                    {
                        pOHFromRepository.Status = 1;
                    }
                    else
                    {
                        pOHFromRepository.Status = 0;
                    }


                }

                if (isForSending == true && pOHFromRepository.Status == 0)
                {
                    return Ok(new { error = "Failed to send" });
                }

                await _repository.SaveAll();

                var pOHtoReturn = _mapper.Map<POHForDetailDto>(pOHToCreate);
                return CreatedAtRoute(nameof(GetPurchaseOrder), new { pOHToCreate.Id }, pOHtoReturn);
            }

            return BadRequest("Could not create Purchase Order");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePurchaseOrder(int id, POHForDetailDto pOHForDetailDto, [FromQuery] bool isForSending)
        {

            if (pOHForDetailDto == null)
                return BadRequest("Empty Body");

            var filterParams = GetFilterParams();

            var pOHFromRepository = await _repository.GetPurchaseOrder(id, filterParams);

            if (pOHFromRepository == null)
                return BadRequest("Purchase Order not available");

            var supFromRepo = await _repository.Get<Supplier>(pOHForDetailDto.SupplierId);
            var bPFromRepo = await _repository.Get<BusinessPlace>(pOHForDetailDto.BusinessPlaceId);


            if (User.FindAll(ClaimTypes.Role).Any(a => a.Value == "Admin"))
            {
                pOHFromRepository.isFromOutlet = pOHForDetailDto.isForOutlet;
            }
            else
            {
                pOHFromRepository.isFromOutlet = User.FindAll(ClaimTypes.Role).Any(a => a.Value == "OutletManager") ? true : false;
            }

            pOHFromRepository.UserId = pOHForDetailDto.UserId;
            pOHFromRepository.Supplier = supFromRepo;
            pOHFromRepository.BusinessPlace = bPFromRepo;
            pOHFromRepository.DeliveryMethod = pOHForDetailDto.DeliveryMethod;
            pOHFromRepository.DeliveryDate = DateTime.Parse(pOHForDetailDto.DeliveryDate);
            pOHFromRepository.OrderDate = DateTime.Parse(pOHForDetailDto.OrderDate);
            pOHFromRepository.ModifiedDate = pOHForDetailDto.ModifiedDate;
            pOHFromRepository.Status = 0;

            foreach (var pod in pOHFromRepository.PurchaseOrderDetail)
            {
                _repository.Delete(pod);
            }

            pOHFromRepository.PurchaseOrderDetail.Clear();

            foreach (var pod in pOHForDetailDto.PODetail)
            {

                pOHFromRepository.PurchaseOrderDetail.Add(new PurchaseOrderDetail
                {
                    DueDate = DateTime.Parse(pod.DueDate),
                    Item = await _repository.Get<Item>(pod.ItemId),
                    OrderQty = pod.OrderQty,
                    UnitPrice = (decimal)pod.UnitPrice,
                    LineTotal = (decimal)pod.LineTotal,
                    ModifiedTime = pod.ModifiedTime
                });
            }
            //sending mail
            if (isForSending)
            {
                var htmlBody = GenerateMailHtml(pOHFromRepository.PONumber, User.FindAll(ClaimTypes.Name).FirstOrDefault().ToString(),
                                             pOHFromRepository.Supplier.Name, pOHFromRepository.DeliveryMethod, pOHFromRepository.OrderDate,
                                             pOHFromRepository.DeliveryDate, pOHFromRepository.PurchaseOrderDetail);

                if (await SendMail("hyeah227@gmail.com", "saajidhumar@gmail.com", "Purchase Order From Upland Bake house",
                                htmlBody, "hyeah227@gmail.com", "hell123boy"))
                {
                    pOHFromRepository.Status = 1;
                }
                else
                {
                    pOHFromRepository.Status = 0;
                }
            }
            else
            {
                pOHFromRepository.Status = 0;
            }

            if (await _repository.SaveAll())
            {
                if (isForSending == true && pOHFromRepository.Status == 0)
                {
                    return Ok(new { error = "Failed to send" });
                }
                else
                {
                    return NoContent();
                }
            }


            throw new System.Exception($"Updating item {id} failed on save");
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<IActionResult> GetReorderPurchaseOrder(int placeId, int type)
        {
            if (placeId == 0)
                return BadRequest(new ErrorModel(1, 400, "Valid Business Place Required"));
            var place = await _context.BusinessPlaces.FindAsync(placeId);
            if (place == null)
                return BadRequest(new ErrorModel(1, 400, "Valid Business Place Required"));
            if (type != 1 && type != 2)
                return BadRequest(new ErrorModel(2, 400, "Valid Item Type Required"));

            IList<RawItems> Items = new List<RawItems>();

            switch (type)
            {
                case 1:
                    var compItemsAll = await _context.CompanyItems.Distinct()
                                    .Where(a => a.CurrentPlace == place && a.Item.IsDeleted == false)
                                    .Include(a => a.CurrentPlace)
                                    .Include(a => a.Item).ThenInclude(a => a.Unit).ToListAsync();

                    foreach (var item in compItemsAll)
                    {
                        if (!Items.Any(a => a.ItemId == item.ItemId))
                        {
                            var totalAvailQty = compItemsAll.Where(a => a.ItemId == item.ItemId).Sum(a => a.AvailableQuantity);
                            var reOrderLevel = compItemsAll.Where(a => a.ItemId == item.ItemId).FirstOrDefault().Item.ReOrderLevel;
                            if (reOrderLevel > totalAvailQty)
                            {
                                Items.Add(new RawItems
                                {
                                    Id = item.Id,
                                    ItemId = item.ItemId,
                                    Item = item.Item,
                                    AvailableQuantity = reOrderLevel,
                                    CostPrice = item.CostPrice
                                });
                            }

                        }
                    }

                    break;
                case 2:

                    var rawItemsAll = await _context.RawItems.Distinct()
                                        .Where(a => a.CurrentPlace == place && a.Item.IsDeleted == false)
                                        .Include(a => a.CurrentPlace)
                                        .Include(a => a.Item).ThenInclude(a => a.Unit).ToListAsync();

                    foreach (var item in rawItemsAll)
                    {
                        if (!Items.Any(a => a.ItemId == item.ItemId))
                        {
                            var totalAvailQty = rawItemsAll.Where(a => a.ItemId == item.ItemId).Sum(a => a.AvailableQuantity);
                            var reOrderLevel = rawItemsAll.Where(a => a.ItemId == item.ItemId).FirstOrDefault().Item.ReOrderLevel;
                            if (reOrderLevel > totalAvailQty)
                            {
                                Items.Add(new RawItems
                                {
                                    Id = item.Id,
                                    ItemId = item.ItemId,
                                    Item = item.Item,
                                    AvailableQuantity = reOrderLevel,
                                    CostPrice = item.CostPrice
                                });
                            }

                        }
                    }
                    break;
                default:
                    compItemsAll = await _context.CompanyItems.Distinct()
                                        .Where(a => a.CurrentPlace == place && a.Item.IsDeleted == false)
                                        .Include(a => a.CurrentPlace)
                                        .Include(a => a.Item).ThenInclude(a => a.Unit).ToListAsync();

                    foreach (var item in compItemsAll)
                    {
                        if (!Items.Any(a => a.ItemId == item.ItemId))
                        {
                            var totalAvailQty = compItemsAll.Where(a => a.ItemId == item.ItemId).Sum(a => a.AvailableQuantity);
                            var reOrderLevel = compItemsAll.Where(a => a.ItemId == item.ItemId).FirstOrDefault().Item.ReOrderLevel;
                            if (reOrderLevel > totalAvailQty)
                            {
                                Items.Add(new RawItems
                                {
                                    Id = item.Id,
                                    ItemId = item.ItemId,
                                    Item = item.Item,
                                    AvailableQuantity = reOrderLevel,
                                    CostPrice = item.CostPrice
                                });
                            }

                        }
                    }
                    break;
            }

            if (Items.Count == 0)
                return BadRequest(new ErrorModel(3, 400, "No Reorders available"));

            PurchaseOrderHeader purOrder = new PurchaseOrderHeader
            {
                BusinessPlace = place,
                BusinessPlaceId = place.Id,
                OrderDate = DateTime.Today,
                DeliveryMethod = "Direct",
                DeliveryDate = DateTime.Today.AddDays(1),
                isFromOutlet = type == 1 ? true : false,
                SupplierId = 0,
                PurchaseOrderDetail = new List<PurchaseOrderDetail>()
            };

            foreach (var item in Items)
            {
                purOrder.PurchaseOrderDetail.Add(new PurchaseOrderDetail
                {
                    Item = item.Item,
                    OrderQty = item.AvailableQuantity * 2,
                    UnitPrice = item.CostPrice,
                    LineTotal = item.AvailableQuantity * item.CostPrice * 2,
                    DueDate = DateTime.Today.AddDays(1)
                });
            }

            var purOrderToReturn = _mapper.Map<POHForDetailDto>(purOrder);

            return Ok(purOrderToReturn);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,OutletManager,BakeryManager,Cashier")]
        public async Task<IActionResult> DeletePurchaseOrder(int id)
        {
            var filterParams = GetFilterParams();
            var PO = await _repository.GetPurchaseOrder(id, filterParams);
            PO.IsDeleted = true;
            foreach (var pod in PO.PurchaseOrderDetail)
            {
                pod.IsDeleted = true;
            }
            if (await _repository.SaveAll())
                return Ok();

            throw new System.Exception($"Failed to delete item {id}");
        }

        private POFilterParams GetFilterParams()
        {
            POFilterParams filterParams = new POFilterParams()
            {
                isFromOutlet = 2,// containNotActive = false
            };
            if (User.FindAll(ClaimTypes.Role).Any(a => a.Value == "Admin"))
            {
                filterParams.isFromOutlet = 2;// filterParams.containNotActive = true;
            }
            else { filterParams.isFromOutlet = User.FindAll(ClaimTypes.Role).Any(a => a.Value == "OutletManager") ? 0 : 1; }

            return filterParams;
        }

        private async Task<bool> SendMail(string from, string to, string subject, string html, string username, string password)
        {
            // create message
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(from));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html) { Text = html };

            // send email
            using var smtp = new SmtpClient();
            await smtp.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            if (!smtp.IsConnected)
                return false;
            await smtp.AuthenticateAsync(username, password);
            if (!smtp.IsAuthenticated)
                return false;
            await smtp.SendAsync(email);

            await smtp.DisconnectAsync(true);

            return true;
        }

        private string GenerateMailHtml(int pONumber, string username, string supplier,
                                        string deliveryMethod, DateTime orderDate, DateTime deliveryDate,
                                        IEnumerable<PurchaseOrderDetail> poDetail)
        {
            StringBuilder html = new StringBuilder();
            html.AppendFormat(@"<link rel=""stylesheet"" href=""https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css"" integrity=""sha384-ggOyR0iXCbMQv3Xipma34MD+dH/1fQ784/j6cY/iJTQUOhcWr7x9JvoRxT2MZw1T"" crossorigin=""anonymous"">
            <div class=""row mt-3"">
          <div class=""col-md-6 font-weight-bold"">PO Number</div>
          <div class=""col-md-1"">-</div>
          <div class=""col-md-5"">{0}</div>
        </div>
        <div class=""row mt-2"">
          <div class=""col-md-6 font-weight-bold"">User</div>
          <div class=""col-md-1"">-</div>
          <div class=""col-md-5"">
            {1}
          </div>
        </div>
        <div class=""row mt-2"">
          <div class=""col-md-6 font-weight-bold"">Supplier</div>
          <div class=""col-md-1"">-</div>
          <div class=""col-md-5"">
            {2}
          </div>
        </div>
        <div class=""row mt-2"">
          <div class=""col-md-6 font-weight-bold"">Delivery Method</div>
          <div class=""col-md-1"">-</div>
          <div class=""col-md-5"">{3}</div>
        </div>
        <div class=""row mt-2"">
          <div class=""col-md-6 font-weight-bold"">Order Date</div>
          <div class=""col-md-1"">-</div>
          <div class=""col-md-5"">
            {4}
          </div>
        </div>
        <div class=""row mt-2"">
          <div class=""col-md-6 font-weight-bold"">Delivery Date</div>
          <div class=""col-md-1"">-</div>
          <div class=""col-md-5"">
            {5}
          </div>
        </div>
        <div class=""row mt-2 ml-3 mr-3"">
          <table class=""table"">
            <thead>
              <th>Item</th>
              <th>Due Date</th>
              <th>Quantity</th>
              <th>Price</th>
              <th>line Total</th>
            </thead>
            <tbody>", pONumber.ToString(), username, supplier, deliveryMethod, orderDate.ToShortDateString(), deliveryDate.ToShortDateString());

            foreach (var pod in poDetail)
            {
                html.AppendFormat(@"<tr>
                <td>{0}</td>
                <td>{1}</td>
                <td>{2}</td>
                <td>{3}</td>
                <td>{4}</td>
              </tr>", pod.Item.Name, pod.DueDate.ToShortDateString(), pod.OrderQty.ToString(), pod.UnitPrice.ToString(), pod.LineTotal.ToString());
            }


            html.AppendFormat(@"</tbody>
          </table>
        </div> ");


            return html.ToString();
        }
    }
}