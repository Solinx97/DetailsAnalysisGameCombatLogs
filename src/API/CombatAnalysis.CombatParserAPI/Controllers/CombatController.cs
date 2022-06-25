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
    public class CombatController : ControllerBase
    {
        private readonly ICombatService<CombatDto> _service;
        private readonly IMapper _mapper;

        public CombatController(ICombatService<CombatDto> service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<CombatModel>> Get()
        {
            var combats = await _service.GetAllAsync();
            var map = _mapper.Map<IEnumerable<CombatModel>>(combats);

            return map;
        }

        [HttpGet("FindByCombatLogId/{combatLogId}")]
        public async Task<IEnumerable<CombatModel>> Get(int combatLogId)
        {
            var combats = await _service.FindAllAsync(combatLogId);
            var map = _mapper.Map<IEnumerable<CombatModel>>(combats);

            return map;
        }

        [HttpPost]
        public async Task<int> Post(CombatModel value)
        {
            var map = _mapper.Map<CombatDto>(value);
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
