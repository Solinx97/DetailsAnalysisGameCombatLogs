using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.CombatParserAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CombatAnalysis.CombatParserAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class HealDoneGeneralController : ControllerBase
    {
        private readonly IService<HealDoneGeneralDto, int> _service;
        private readonly IMapper _mapper;

        public HealDoneGeneralController(IService<HealDoneGeneralDto, int> service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet("findByCombatPlayerId/{combatPlayerId:int:min(1)}")]
        public async Task<IEnumerable<HealDoneGeneralModel>> Find(int combatPlayerId)
        {
            var healDoneGenerals = await _service.GetByParamAsync("CombatPlayerId", combatPlayerId);
            var map = _mapper.Map<IEnumerable<HealDoneGeneralModel>>(healDoneGenerals);

            return map;
        }

        [HttpPost]
        public async Task<HealDoneGeneralModel> Post(HealDoneGeneralModel model)
        {
            var map = _mapper.Map<HealDoneGeneralDto>(model);
            var createdItem = await _service.CreateAsync(map);
            var resultMap = _mapper.Map<HealDoneGeneralModel>(createdItem);

            return resultMap;
        }

        [HttpDelete]
        public async Task<int> Delete(HealDoneGeneralModel model)
        {
            var map = _mapper.Map<HealDoneGeneralDto>(model);
            var deletedId = await _service.DeleteAsync(map);

            return deletedId;
        }
    }
}
