using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BakeryMS.API.Common.DTOs.Master;
using BakeryMS.API.Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BakeryMS.API.Controllers.Master
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class SuppliersController : ControllerBase
    {
        private readonly IInventoryRepository _invRepo;
        private readonly IMapper _mapper;
        public SuppliersController(IInventoryRepository invrepo, IMapper mapper)
        {
            _mapper = mapper;
            _invRepo = invrepo;

        }

        [HttpGet("{id}", Name = "GetSupplier")]
        public async Task<IActionResult> GetSupplier(int id)
        {
            var supFromRepo = await _invRepo.GetSupplier(id);
            var supToReturn = _mapper.Map<SupplierDto>(supFromRepo);

            return Ok(supToReturn);
        }

        [HttpGet]
        public async Task<IActionResult> GetSuppliers()
        {
            var supsFromRepo = await _invRepo.GetSuppliers();
            var supsToReturn = _mapper.Map<IEnumerable<SupplierDto>>(supsFromRepo);

            return Ok(supsToReturn);
        }
    }
}