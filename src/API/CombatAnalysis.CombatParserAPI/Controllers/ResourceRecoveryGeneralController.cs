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
        private readonly IService<ResourceRecoveryGeneralDto> _service;
        private readonly IMapper _mapper;

        public ResourceRecoveryGeneralController(IService<ResourceRecoveryGeneralDto> service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet("FindByCombatPlayerId/{combatPlayerId}")]
        public async Task<IEnumerable<ResourceRecoveryGeneralModel>> Find(int combatPlayerId)
        {
            var resourceRecoveryGenerals = await _service.FindAllAsync(combatPlayerId);
            var map = _mapper.Map<IEnumerable<ResourceRecoveryGeneralModel>>(resourceRecoveryGenerals);

            return map;
        }

        [HttpPost]
        public async Task<int> Post(ResourceRecoveryGeneralModel value)
        {
            var map = _mapper.Map<ResourceRecoveryGeneralDto>(value);
            var createdCombatId = await _service.CreateAsync(map);

            return createdCombatId;
        }

        [HttpDelete("{id}")]
        public async Task<int> Delete(int id)
        {
            var combatLogresourceRecoveryGeneral = await _service.GetByIdAsync(id);
            var deletedId = await _service.DeleteAsync(combatLogresourceRecoveryGeneral);

            return deletedId;
        }
    }
}
