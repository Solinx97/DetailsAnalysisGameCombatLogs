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
        private readonly IService<CombatPlayerDto, int> _service;
        private readonly IMapper _mapper;

        public CombatPlayerController(IService<CombatPlayerDto, int> service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet("findByCombatId/{combatId:int:min(1)}")]
        public async Task<IEnumerable<CombatPlayerDto>> Find(int combatId)
        {
            var players = await _service.GetByParamAsync("CombatId", combatId);
            var map = _mapper.Map<IEnumerable<CombatPlayerDto>>(players);

            return map;
        }

        [HttpGet("{id:int:min(1)}")]
        public async Task<CombatPlayerDto> GetById(int id)
        {
            var combatLog = await _service.GetByIdAsync(id);
            var map = _mapper.Map<CombatPlayerDto>(combatLog);

            return map;
        }

        [HttpPost]
        public async Task<CombatPlayerModel> Post(CombatPlayerModel model)
        {
            var map = _mapper.Map<CombatPlayerDto>(model);
            var createdItem = await _service.CreateAsync(map);
            var resultMap = _mapper.Map<CombatPlayerModel>(createdItem);

            return resultMap;
        }

        [HttpDelete]
        public async Task<int> Delete(CombatPlayerModel model)
        {
            var map = _mapper.Map<CombatPlayerDto>(model);
            var deletedId = await _service.DeleteAsync(map);

            return deletedId;
        }
    }
}
