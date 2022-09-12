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
    public class DamageDoneGeneralController : ControllerBase
    {
        private readonly IService<DamageDoneGeneralDto, int> _service;
        private readonly IMapper _mapper;

        public DamageDoneGeneralController(IService<DamageDoneGeneralDto, int> service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet("findByCombatPlayerId/{combatPlayerId:int:min(1)}")]
        public async Task<IEnumerable<DamageDoneGeneralModel>> Find(int combatPlayerId)
        {
            var damageDoneGenerals = await _service.GetByParamAsync("CombatPlayerId", combatPlayerId);
            var map = _mapper.Map<IEnumerable<DamageDoneGeneralModel>>(damageDoneGenerals);

            return map;
        }

        [HttpPost]
        public async Task<DamageDoneGeneralModel> Post(DamageDoneGeneralModel model)
        {
            var map = _mapper.Map<DamageDoneGeneralDto>(model);
            var createdItem = await _service.CreateAsync(map);
            var resultMap = _mapper.Map<DamageDoneGeneralModel>(createdItem);

            return resultMap;
        }

        [HttpDelete]
        public async Task<int> Delete(DamageDoneGeneralModel model)
        {
            var map = _mapper.Map<DamageDoneGeneralDto>(model);
            var deletedId = await _service.DeleteAsync(map);

            return deletedId;
        }
    }
}
