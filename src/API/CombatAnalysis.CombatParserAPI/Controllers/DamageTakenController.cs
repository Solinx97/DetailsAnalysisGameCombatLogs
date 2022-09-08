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
    public class DamageTakenController : ControllerBase
    {
        private readonly IService<DamageTakenDto> _service;
        private readonly IMapper _mapper;

        public DamageTakenController(IService<DamageTakenDto> service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet("FindByCombatPlayerId/{combatPlayerId}")]
        public async Task<IEnumerable<DamageTakenModel>> Find(int combatPlayerId)
        {
            var damageTakens = await _service.GetByProcedureAsync(combatPlayerId);
            var map = _mapper.Map<IEnumerable<DamageTakenModel>>(damageTakens);

            return map;
        }

        [HttpPost]
        public async Task Post(DamageTakenModel value)
        {
            var map = _mapper.Map<DamageTakenDto>(value);
            await _service.CreateByProcedureAsync(map);
        }

        [HttpDelete("DeleteByCombatPlayerId/{combatPlayerId}")]
        public async Task<int> Delete(int combatPlayerId)
        {
            var deletedId = await _service.DeleteByProcedureAsync(combatPlayerId);

            return deletedId;
        }
    }
}
