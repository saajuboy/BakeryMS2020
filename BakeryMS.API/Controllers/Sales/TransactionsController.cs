using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BakeryMS.API.Common.DTOs.Sales;
using BakeryMS.API.Common.Helpers;
using BakeryMS.API.Data;
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

            var transQuery = _context.Transactions.Where(a=>a.BusinessPlace == place)
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
    }
}