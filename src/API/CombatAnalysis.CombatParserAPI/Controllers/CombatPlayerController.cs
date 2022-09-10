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
        private readonly ISPService<CombatPlayerDto, int> _spService;
        private readonly IService<CombatPlayerDto, int> _service;
        private readonly IMapper _mapper;

        public CombatPlayerController(ISPService<CombatPlayerDto, int> spService, IService<CombatPlayerDto, int> service, IMapper mapper)
        {
            _spService = spService;
            _service = service;
            _mapper = mapper;
        }

        [HttpGet("FindByCombatId/{combatId}")]
        public async Task<IEnumerable<CombatPlayerDto>> Find(int combatId)
        {
            var players = await _spService.GetByProcedureAsync(combatId);
            var map = _mapper.Map<IEnumerable<CombatPlayerDto>>(players);

            return map;
        }

        [HttpGet("{id}")]
        public async Task<CombatPlayerDto> GetById(int id)
        {
            var combatLog = await _service.GetByIdAsync(id);
            var map = _mapper.Map<CombatPlayerDto>(combatLog);

            return map;
        }

        [HttpPost]
        public async Task<int> Post(CombatPlayerModel value)
        {
            var map = _mapper.Map<CombatPlayerDto>(value);
            var createdCombatId = await _spService.CreateByProcedureAsync(map);

            return createdCombatId;
        }

        [HttpDelete("DeleteByCombatId/{combatId}")]
        public async Task<int> Delete(int combatId)
        {
            var deletedId = await _spService.DeleteByProcedureAsync(combatId);

            return deletedId;
        }
    }
}
