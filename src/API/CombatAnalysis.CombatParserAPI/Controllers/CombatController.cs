using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.CombatParserAPI.Helpers;
using CombatAnalysis.CombatParserAPI.Interfaces;
using CombatAnalysis.CombatParserAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CombatAnalysis.CombatParserAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CombatController : ControllerBase
    {
        private readonly IService<CombatDto> _service;
        private readonly IMapper _mapper;
        private readonly SaveCombatDataHelper _saveCombatDataHelper;

        public CombatController(IService<CombatDto> service, IMapper mapper, IHttpClientHelper httpClient, ILogger logger)
        {
            _service = service;
            _mapper = mapper;
            _saveCombatDataHelper = new SaveCombatDataHelper(mapper, httpClient, logger);
        }

        [HttpGet("{id}")]
        public async Task<CombatModel> GetById(int id)
        {
            var combatLog = await _service.GetByIdAsync(id);
            var map = _mapper.Map<CombatModel>(combatLog);

            return map;
        }

        [HttpGet("FindByCombatLogId/{combatLogId}")]
        public async Task<IEnumerable<CombatModel>> Find(int combatLogId)
        {
            var combats = await _service.GetByProcedureAsync(combatLogId);
            var map = _mapper.Map<IEnumerable<CombatModel>>(combats);

            return map;
        }

        [HttpPost]
        public async Task<int> Post(CombatModel combat)
        {
            SaveCombatDataHelper.CombatData = combat.Data;

            var map = _mapper.Map<CombatDto>(combat);
            var createdCombatId = await _service.CreateAsync(map);

            return createdCombatId;
        }

        [HttpPost("SaveCombatPlayers")]
        public async Task SaveCombatPlayers(List<CombatPlayerDataModel> combatPlayers)
        {
            var combat = await GetById(combatPlayers[0].CombatId);
            combat.Data = SaveCombatDataHelper.CombatData;

            await _saveCombatDataHelper.SaveCombatPlayerData(combat, combatPlayers);
        }

        [HttpDelete("{id}")]
        public async Task<int> Delete(int id)
        {
            var combat = await _service.GetByIdAsync(id);
            var deletedId = await _service.DeleteAsync(combat);

            return deletedId;
        }
    }
}
