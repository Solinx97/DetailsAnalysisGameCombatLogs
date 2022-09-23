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
    public class ResourceRecoveryController : ControllerBase
    {
        private readonly IService<ResourceRecoveryDto, int> _service;
        private readonly IMapper _mapper;

        public ResourceRecoveryController(IService<ResourceRecoveryDto, int> service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet("findByCombatPlayerId/{combatPlayerId:int:min(1)}")]
        public async Task<IEnumerable<ResourceRecoveryModel>> Find(int combatPlayerId)
        {
            var resourceRecoveryes = await _service.GetByParamAsync("CombatPlayerId", combatPlayerId);
            var map = _mapper.Map<IEnumerable<ResourceRecoveryModel>>(resourceRecoveryes);

            return map;
        }

        [HttpPost]
        public async Task<ResourceRecoveryModel> Post(ResourceRecoveryModel model)
        {
            var map = _mapper.Map<ResourceRecoveryDto>(model);
            var createdItem = await _service.CreateAsync(map);
            var resultMap = _mapper.Map<ResourceRecoveryModel>(createdItem);

            return resultMap;
        }

        [HttpDelete]
        public async Task<int> Delete(ResourceRecoveryModel model)
        {
            var map = _mapper.Map<ResourceRecoveryDto>(model);
            var deletedId = await _service.DeleteAsync(map);

            return deletedId;
        }
    }
}
