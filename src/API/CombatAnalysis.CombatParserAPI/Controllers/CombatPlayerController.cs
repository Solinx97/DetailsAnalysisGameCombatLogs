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
    public class CombatPlayerController : ControllerBase
    {
        private readonly IService<CombatPlayerDataDto> _service;
        private readonly IMapper _mapper;

        public CombatPlayerController(IService<CombatPlayerDataDto> service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet("FindByCombatId/{combatId}")]
        public async Task<IEnumerable<CombatPlayerDataDto>> Find(int combatId)
        {
            var players = await _service.GetByProcedureAsync(combatId);
            var map = _mapper.Map<IEnumerable<CombatPlayerDataDto>>(players);

            return map;
        }

        [HttpGet("{id}")]
        public async Task<CombatPlayerDataDto> GetById(int id)
        {
            var combatLog = await _service.GetByIdAsync(id);
            var map = _mapper.Map<CombatPlayerDataDto>(combatLog);

            return map;
        }

        [HttpPost]
        public async Task<int> Post(CombatPlayerDataModel value)
        {
            var map = _mapper.Map<CombatPlayerDataDto>(value);
            var createdCombatId = await _service.CreateAsync(map);

            return createdCombatId;
        }

        [HttpDelete("{id}")]
        public async Task<int> Delete(int id)
        {
            var player = await _service.GetByIdAsync(id);
            var deletedId = await _service.DeleteAsync(player);

            return deletedId;
        }
    }
}
