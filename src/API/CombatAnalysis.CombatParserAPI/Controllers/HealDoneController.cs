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
        private readonly IService<HealDoneDto, int> _service;
        private readonly IMapper _mapper;

        public HealDoneController(IService<HealDoneDto, int> service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet("FindByCombatPlayerId/{combatPlayerId}")]
        public async Task<IEnumerable<HealDoneModel>> Find(int combatPlayerId)
        {
            var healDones = await _service.GetByParamAsync("CombatPlayerId", combatPlayerId);
            var map = _mapper.Map<IEnumerable<HealDoneModel>>(healDones);

            return map;
        }

        [HttpPost]
        public async Task Post(HealDoneModel value)
        {
            var map = _mapper.Map<HealDoneDto>(value);
            await _service.CreateAsync(map);
        }

        [HttpDelete]
        public async Task<int> Delete(HealDoneModel value)
        {
            var map = _mapper.Map<HealDoneDto>(value);
            var deletedId = await _service.DeleteAsync(map);

            return deletedId;
        }
    }
}
