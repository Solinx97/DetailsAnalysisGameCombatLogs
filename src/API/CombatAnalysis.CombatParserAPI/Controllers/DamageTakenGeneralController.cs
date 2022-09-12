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
    public class DamageTakenGeneralController : ControllerBase
    {
        private readonly IService<DamageTakenGeneralDto, int> _service;
        private readonly IMapper _mapper;

        public DamageTakenGeneralController(IService<DamageTakenGeneralDto, int> service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet("FindByCombatPlayerId/{combatPlayerId}")]
        public async Task<IEnumerable<DamageTakenGeneralModel>> Find(int combatPlayerId)
        {
            var damageTakenGenerals = await _service.GetByParamAsync("CombatPlayerId", combatPlayerId);
            var map = _mapper.Map<IEnumerable<DamageTakenGeneralModel>>(damageTakenGenerals);

            return map;
        }

        [HttpPost]
        public async Task<DamageTakenGeneralModel> Post(DamageTakenGeneralModel model)
        {
            var map = _mapper.Map<DamageTakenGeneralDto>(model);
            var createdItem = await _service.CreateAsync(map);
            var resultMap = _mapper.Map<DamageTakenGeneralModel>(createdItem);

            return resultMap;
        }

        [HttpDelete]
        public async Task<int> Delete(DamageTakenGeneralModel model)
        {
            var map = _mapper.Map<DamageTakenGeneralDto>(model);
            var deletedId = await _service.DeleteAsync(map);

            return deletedId;
        }
    }
}
