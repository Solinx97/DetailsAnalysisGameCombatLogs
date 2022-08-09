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
    public class CombatLogController : ControllerBase
    {
        private readonly IService<CombatLogDto> _service;
        private readonly IMapper _mapper;

        public CombatLogController(IService<CombatLogDto> service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<CombatLogModel>> Get()
        {
            var combatLogs = await _service.GetAllAsync();
            var map = _mapper.Map<IEnumerable<CombatLogModel>>(combatLogs);

            return map;
        }

        [HttpPost]
        public async Task<int> Post(CombatLogModel value)
        {
            var map = _mapper.Map<CombatLogDto>(value);
            var createdCombatId = await _service.CreateAsync(map);

            return createdCombatId;
        }

        [HttpDelete("{id}")]
        public async Task<int> Delete(int id)
        {
            var combatLog = await _service.GetByIdAsync(id);
            var deletedId = await _service.DeleteAsync(combatLog);

            return deletedId;
        }
    }
}
