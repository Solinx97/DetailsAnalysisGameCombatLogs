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
    public class HealDoneController : ControllerBase
    {
        private readonly IService<HealDoneDto> _service;
        private readonly IMapper _mapper;

        public HealDoneController(IService<HealDoneDto> service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet("FindByCombatPlayerId/{combatPlayerId}")]
        public async Task<IEnumerable<HealDoneModel>> Find(int combatPlayerId)
        {
            var healDones = await _service.GetByProcedureAsync(combatPlayerId);
            var map = _mapper.Map<IEnumerable<HealDoneModel>>(healDones);

            return map;
        }

        [HttpPost]
        public async Task Post(HealDoneModel value)
        {
            var map = _mapper.Map<HealDoneDto>(value);
            await _service.CreateAsync(map);
        }

        [HttpDelete("DeleteByCombatPlayerId/{combatPlayerId}")]
        public async Task<int> Delete(int combatPlayerId)
        {
            //var healDone = await _service.GetByIdAsync(id);
            //var deletedId = await _service.DeleteAsync(healDone);
            var deletedId = await _service.DeleteByProcedureAsync(combatPlayerId);

            return deletedId;
        }
    }
}
