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
        private readonly IService<ResourceRecoveryDto> _service;
        private readonly IMapper _mapper;

        public ResourceRecoveryController(IService<ResourceRecoveryDto> service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet("FindByCombatPlayerId/{combatPlayerId}")]
        public async Task<IEnumerable<ResourceRecoveryModel>> Find(int combatPlayerId)
        {
            var resourceRecoveryes = await _service.FindAllAsync(combatPlayerId);
            var map = _mapper.Map<IEnumerable<ResourceRecoveryModel>>(resourceRecoveryes);

            return map;
        }

        [HttpPost]
        public async Task Post(ResourceRecoveryModel value)
        {
            var map = _mapper.Map<ResourceRecoveryDto>(value);
            await _service.CreateAsync(map);
        }

        [HttpDelete("{id}")]
        public async Task<int> Delete(int id)
        {
            var resourceRecovery = await _service.GetByIdAsync(id);
            var deletedId = await _service.DeleteAsync(resourceRecovery);

            return deletedId;
        }
    }
}
