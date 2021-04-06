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
    public class TransactionsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public TransactionsController(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        [HttpGet("{id}", Name = "GetTransaction")]
        public async Task<IActionResult> GetTransaction(int id)
        {

            var transaction = await _context.Transactions
            .Include(a => a.BusinessPlace)
            .Include(a => a.User).FirstOrDefaultAsync(a => a.Id == id);

            var transToReturn = _mapper.Map<TransactionDto>(transaction);

            return Ok(transToReturn);
        }

        [HttpGet]
        public async Task<IActionResult> GetTransactions(int placeId, string fromDate, string toDate)
        {
            if (placeId == 0)
                return BadRequest(new ErrorModel(1, 400, "Valid business place required"));

            var place = await _context.BusinessPlaces.FirstOrDefaultAsync(a => a.Id == placeId);

            if (place == null)
                return BadRequest(new ErrorModel(1, 400, "Valid business place required"));

            var transQuery = _context.Transactions.Where(a => a.BusinessPlace == place)
            .Include(a => a.BusinessPlace)
            .Include(a => a.User)
            .OrderByDescending(a => a.Date).AsQueryable();

            DateTime from;
            DateTime to;

            if (DateTime.TryParse(fromDate, out from))
            {
                transQuery = transQuery.Where(a => a.Date >= from);
            }

            if (DateTime.TryParse(toDate, out to))
            {
                transQuery = transQuery.Where(a => a.Date <= to);
            }

            var Transactions = await transQuery.ToListAsync();

            var transToReturn = _mapper.Map<IList<TransactionDto>>(Transactions);
            return Ok(transToReturn);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTransaction(TransactionDto transDto)
        {
            if (transDto == null)
                return BadRequest(new ErrorModel(1, 400, "Empty Body"));

            if (transDto.BusinessPlaceId == 0 || transDto.BusinessPlaceId == null)
                return BadRequest(new ErrorModel(2, 400, "Valid business place required"));
            var place = await _context.BusinessPlaces.FirstOrDefaultAsync(a => a.Id == transDto.BusinessPlaceId);
            if (place == null)
                return BadRequest(new ErrorModel(2, 400, "Valid business place required"));

            if (transDto.UserId == 0)
                return BadRequest(new ErrorModel(3, 400, "Valid user required"));
            var userid = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (transDto.UserId != userid)
                return BadRequest(new ErrorModel(3, 400, "Valid user required"));

            if (transDto.Description == "" || (transDto.Debit == 0 && transDto.Credit == 0))
                return BadRequest(new ErrorModel(3, 400, "Empty values"));

            var transToCreate = _mapper.Map<Transaction>(transDto);
            transToCreate.Date = DateTime.Today;

            var timeDate = DateTime.Now;
            transToCreate.Time = new TimeSpan(timeDate.Hour, timeDate.Minute, timeDate.Second);

            if (await _context.Transactions.AnyAsync(a => a.Reference.Contains(transToCreate.Reference)
                && a.Date == transToCreate.Date
                && a.BusinessPlaceId == transToCreate.BusinessPlaceId))
            {
                var transaction = await _context.Transactions.FirstOrDefaultAsync(a => a.Reference.Contains(transToCreate.Reference)
                && a.Date == transToCreate.Date
                && a.BusinessPlaceId == transToCreate.BusinessPlaceId);

                transaction.Debit += transToCreate.Debit;
                transaction.Credit += transToCreate.Credit;
            }
            else
            {
                await _context.AddRangeAsync(transToCreate);
            }

            if (await _context.SaveChangesAsync() > 0)
            {
                return Ok();
            }

            return BadRequest(new ErrorModel(4, 400, "Failed to create transaction"));
        }
    }
}