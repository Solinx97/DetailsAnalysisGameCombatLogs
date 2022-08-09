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
        private readonly IService<CombatDto> _service;
        private readonly IMapper _mapper;

        public CombatController(IService<CombatDto> service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet("FindByCombatLogId/{combatLogId}")]
        public async Task<IEnumerable<CombatModel>> Find(int combatLogId)
        {
            var combats = await _service.GetByProcedureAsync(combatLogId);
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

        [HttpDelete("{id}")]
        public async Task<int> Delete(int id)
        {
            var combat = await _service.GetByIdAsync(id);
            var deletedId = await _service.DeleteAsync(combat);

            return deletedId;
        }
    }
}
