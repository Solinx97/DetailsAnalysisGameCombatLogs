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
    public class ResourceRecoveryGeneralController : ControllerBase
    {
        private readonly IService<ResourceRecoveryGeneralDto, int> _service;
        private readonly IMapper _mapper;

        public ResourceRecoveryGeneralController(IService<ResourceRecoveryGeneralDto, int> service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet("findByCombatPlayerId/{combatPlayerId:int:min(1)}")]
        public async Task<IEnumerable<ResourceRecoveryGeneralModel>> Find(int combatPlayerId)
        {
            var resourceRecoveryGenerals = await _service.GetByParamAsync("CombatPlayerId", combatPlayerId);
            var map = _mapper.Map<IEnumerable<ResourceRecoveryGeneralModel>>(resourceRecoveryGenerals);

            return map;
        }

        [HttpPost]
        public async Task<ResourceRecoveryGeneralModel> Post(ResourceRecoveryGeneralModel model)
        {
            var map = _mapper.Map<ResourceRecoveryGeneralDto>(model);
            var createdItem = await _service.CreateAsync(map);
            var resultMap = _mapper.Map<ResourceRecoveryGeneralModel>(createdItem);

            return resultMap;
        }

        [HttpDelete]
        public async Task<int> Delete(ResourceRecoveryGeneralModel model)
        {
            var map = _mapper.Map<ResourceRecoveryGeneralDto>(model);
            var deletedId = await _service.DeleteAsync(map);

            return deletedId;
        }
    }
}
