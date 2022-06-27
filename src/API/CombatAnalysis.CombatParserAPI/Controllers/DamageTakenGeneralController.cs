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
    public class DamageTakenGeneralController : ControllerBase
    {
        private readonly IService<DamageTakenGeneralDto> _service;
        private readonly IMapper _mapper;

        public DamageTakenGeneralController(IService<DamageTakenGeneralDto> service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet("FindByCombatPlayerId/{combatPlayerId}")]
        public async Task<IEnumerable<DamageTakenGeneralModel>> Find(int combatPlayerId)
        {
            var damageTakenGenerals = await _service.FindAllAsync(combatPlayerId);
            var map = _mapper.Map<IEnumerable<DamageTakenGeneralModel>>(damageTakenGenerals);

            return map;
        }

        [HttpPost]
        public async Task<int> Post(DamageTakenGeneralModel value)
        {
            var map = _mapper.Map<DamageTakenGeneralDto>(value);
            var createdCombatId = await _service.CreateAsync(map);

            return createdCombatId;
        }
    }
}
