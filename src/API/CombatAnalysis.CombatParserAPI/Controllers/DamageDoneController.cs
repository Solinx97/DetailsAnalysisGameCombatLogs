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
    public class DamageDoneController : ControllerBase
    {
        private readonly IService<DamageDoneDto> _service;
        private readonly IMapper _mapper;

        public DamageDoneController(IService<DamageDoneDto> service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet("FindByCombatPlayerId/{combatPlayerId}")]
        public async Task<IEnumerable<DamageDoneModel>> Find(int combatPlayerId)
        {
            var damageDones = await _service.FindAllAsync(combatPlayerId);
            var map = _mapper.Map<IEnumerable<DamageDoneModel>>(damageDones);

            return map;
        }

        [HttpPost]
        public async Task Post(DamageDoneModel value)
        {
            var map = _mapper.Map<DamageDoneDto>(value);
            await _service.CreateAsync(map);
        }
    }
}
