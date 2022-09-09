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
        private readonly ISPService<ResourceRecoveryDto, int> _service;
        private readonly IMapper _mapper;

        public ResourceRecoveryController(ISPService<ResourceRecoveryDto, int> service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet("FindByCombatPlayerId/{combatPlayerId}")]
        public async Task<IEnumerable<ResourceRecoveryModel>> Find(int combatPlayerId)
        {
            var resourceRecoveryes = await _service.GetByProcedureAsync(combatPlayerId);
            var map = _mapper.Map<IEnumerable<ResourceRecoveryModel>>(resourceRecoveryes);

            return map;
        }

        [HttpPost]
        public async Task Post(ResourceRecoveryModel value)
        {
            var map = _mapper.Map<ResourceRecoveryDto>(value);
            await _service.CreateByProcedureAsync(map);
        }

        [HttpDelete("DeleteByCombatPlayerId/{combatPlayerId}")]
        public async Task<int> Delete(int combatPlayerId)
        {
            var deletedId = await _service.DeleteByProcedureAsync(combatPlayerId);

            return deletedId;
        }
    }
}
