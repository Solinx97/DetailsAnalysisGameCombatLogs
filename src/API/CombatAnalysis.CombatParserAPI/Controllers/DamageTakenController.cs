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
        private readonly IService<DamageTakenDto, int> _service;
        private readonly IMapper _mapper;

        public DamageTakenController(IService<DamageTakenDto, int> service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet("FindByCombatPlayerId/{combatPlayerId}")]
        public async Task<IEnumerable<DamageTakenModel>> Find(int combatPlayerId)
        {
            var damageTakens = await _service.GetByParamAsync("CombatPlayerId", combatPlayerId);
            var map = _mapper.Map<IEnumerable<DamageTakenModel>>(damageTakens);

            return map;
        }

        [HttpPost]
        public async Task<DamageTakenModel> Post(DamageTakenModel model)
        {
            var map = _mapper.Map<DamageTakenDto>(model);
            var createdItem = await _service.CreateAsync(map);
            var resultMap = _mapper.Map<DamageTakenModel>(createdItem);

            return resultMap;
        }

        [HttpDelete]
        public async Task<int> Delete(DamageTakenModel model)
        {
            var map = _mapper.Map<DamageTakenDto>(model);
            var deletedId = await _service.DeleteAsync(map);

            return deletedId;
        }
    }
}
