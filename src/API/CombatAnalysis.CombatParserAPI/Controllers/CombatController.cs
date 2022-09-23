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
        private readonly IService<CombatDto, int> _service;
        private readonly IMapper _mapper;
        private readonly SaveCombatDataHelper _saveCombatDataHelper;

        public CombatController(IService<CombatDto, int> service, IMapper mapper, IHttpClientHelper httpClient, ILogger logger)
        {
            _service = service;
            _mapper = mapper;
            _saveCombatDataHelper = new SaveCombatDataHelper(mapper, httpClient, logger);
        }

        [HttpGet("{id:int:min(1)}")]
        public async Task<CombatModel> GetById(int id)
        {
            var combat = await _service.GetByIdAsync(id);
            var map = _mapper.Map<CombatModel>(combat);

            return map;
        }

        [HttpGet]
        public async Task<IEnumerable<CombatModel>> GetAll()
        {
            var combat = await _service.GetAllAsync();
            var map = _mapper.Map<IEnumerable<CombatModel>>(combat);

            return map;
        }

        [HttpGet("findByCombatLogId/{combatLogId:int:min(1)}")]
        public async Task<IEnumerable<CombatModel>> Find(int combatLogId)
        {
            //var combats = await _service.GetByParamAsync("CombatLogId", combatLogId);
            //var map = _mapper.Map<IEnumerable<CombatModel>>(combats);

            return new List<CombatModel>();
        }

        [HttpPost]
        public async Task<CombatModel> Post(CombatModel model)
        {
            SaveCombatDataHelper.CombatData = model.Data;

            var map = _mapper.Map<CombatDto>(model);
            var createdItem = await _service.CreateAsync(map);
            var resultMap = _mapper.Map<CombatModel>(createdItem);

            return resultMap;
        }

        [HttpPost("saveCombatPlayers")]
        public async Task SaveCombatPlayers(List<CombatPlayerModel> combatPlayers)
        {
            var combat = await GetById(combatPlayers[0].CombatId);
            combat.Data = SaveCombatDataHelper.CombatData;

            await _saveCombatDataHelper.SaveCombatPlayerData(combat, combatPlayers);
        }

        [HttpDelete]
        public async Task<int> Delete(CombatModel combat)
        {
            var map = _mapper.Map<CombatDto>(combat);
            var deletedId = await _service.DeleteAsync(map);

            return deletedId;
        }
    }
}
