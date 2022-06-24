using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.CombatParserAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CombatAnalysis.CombatParserAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ResourceRecoveryGeneralController : ControllerBase
    {
        private readonly IService<ResourceRecoveryGeneralDto> _service;
        private readonly IMapper _mapper;

        public ResourceRecoveryGeneralController(IService<ResourceRecoveryGeneralDto> service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [HttpPost]
        public async Task<int> Post(ResourceRecoveryGeneralModel value)
        {
            var map = _mapper.Map<ResourceRecoveryGeneralDto>(value);
            var createdCombatId = await _service.CreateAsync(map);

            return createdCombatId;
        }

        [HttpPut("{id}")]
        public void Put(int id, string value)
        {
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
