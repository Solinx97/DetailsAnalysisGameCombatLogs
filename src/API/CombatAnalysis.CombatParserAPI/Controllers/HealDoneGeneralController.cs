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
    public class HealDoneGeneralController : ControllerBase
    {
        private readonly ISPService<HealDoneGeneralDto, int> _service;
        private readonly IMapper _mapper;

        public HealDoneGeneralController(ISPService<HealDoneGeneralDto, int> service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet("FindByCombatPlayerId/{combatPlayerId}")]
        public async Task<IEnumerable<HealDoneGeneralModel>> Find(int combatPlayerId)
        {
            var healDoneGenerals = await _service.GetByProcedureAsync(combatPlayerId);
            var map = _mapper.Map<IEnumerable<HealDoneGeneralModel>>(healDoneGenerals);

            return map;
        }

        [HttpPost]
        public async Task<int> Post(HealDoneGeneralModel value)
        {
            var map = _mapper.Map<HealDoneGeneralDto>(value);
            var createdCombatId = await _service.CreateByProcedureAsync(map);

            return createdCombatId;
        }

        [HttpDelete("DeleteByCombatPlayerId/{combatPlayerId}")]
        public async Task<int> Delete(int combatPlayerId)
        {
            var deletedId = await _service.DeleteByProcedureAsync(combatPlayerId);

            return deletedId;
        }
    }
}
