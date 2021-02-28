using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BakeryMS.API.Common.DTOs.Manufacturing;
using BakeryMS.API.Data.Interfaces;
using BakeryMS.API.Models.Production;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BakeryMS.API.Controllers.Manufacturing
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class ProductionSessionsController : ControllerBase
    {
        private readonly IProductionRepository _repository;
        private readonly IMapper _mapper;
        public ProductionSessionsController(IProductionRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;

        }

        [HttpGet("{id}", Name = "GetProductionSession")]
        public async Task<IActionResult> GetProductionSession(int id)
        {
            var sessionFromRepo = await _repository.Get<ProductionSession>(id);
            var sessionToReturn = _mapper.Map<ProdSessionForDetailDto>(sessionFromRepo);

            return Ok(sessionToReturn);
        }

        [HttpGet]
        public async Task<IActionResult> GetProductionSessions()
        {
            var sessionsFromRepo = await _repository.GetAll<ProductionSession>();
            var sessionListToReturn = _mapper.Map<IEnumerable<ProdSessionForDetailDto>>(sessionsFromRepo);

            return Ok(sessionListToReturn);
        }
    }
}