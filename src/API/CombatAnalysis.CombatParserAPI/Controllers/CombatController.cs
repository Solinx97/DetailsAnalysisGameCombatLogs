using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.CombatParserAPI.Helpers;
using CombatAnalysis.CombatParserAPI.Interfaces;
using CombatAnalysis.CombatParserAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
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
        private readonly ILogger _logger;
        private readonly SaveCombatDataHelper _saveCombatDataHelper;

        public CombatController(IService<CombatDto, int> service, IMapper mapper, IHttpClientHelper httpClient, ILogger logger)
        {
            _service = service;
            _mapper = mapper;
            _logger = logger;
            _saveCombatDataHelper = new SaveCombatDataHelper(mapper, httpClient, logger);
        }

        [HttpGet("{id:int:min(1)}")]
        public async Task<IActionResult> GetById(int id)
        {
            var combat = await _service.GetByIdAsync(id);
            var map = _mapper.Map<CombatModel>(combat);

            return Ok(map);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var combat = await _service.GetAllAsync();
            var map = _mapper.Map<IEnumerable<CombatModel>>(combat);

            return Ok(map);
        }

        [HttpGet("findByCombatLogId/{combatLogId:int:min(1)}")]
        public async Task<IActionResult> Find(int combatLogId)
        {
            var combats = await _service.GetByParamAsync("CombatLogId", combatLogId);
            var map = _mapper.Map<IEnumerable<CombatModel>>(combats);

            return Ok(map);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CombatModel model)
        {
            try
            {
                SaveCombatDataHelper.CombatData = model.Data;

                var map = _mapper.Map<CombatDto>(model);
                var createdItem = await _service.CreateAsync(map);
                var resultMap = _mapper.Map<CombatModel>(createdItem);

                return Ok(resultMap);
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, ex.Message);

                return BadRequest();
            }
        }

        [HttpPost("saveCombatPlayers")]
        public async Task<IActionResult> SaveCombatPlayers(List<CombatPlayerModel> combatPlayers)
        {
            var combat = await _service.GetByIdAsync(combatPlayers[0].CombatId);
            var map = _mapper.Map<CombatModel>(combat);
            map.Data = SaveCombatDataHelper.CombatData;

            await _saveCombatDataHelper.SaveCombatPlayerData(map, combatPlayers);

            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(CombatModel combat)
        {
            try
            {
                var map = _mapper.Map<CombatDto>(combat);
                var deletedId = await _service.DeleteAsync(map);

                return Ok(deletedId);
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, ex.Message);

                return BadRequest();
            }
        }
    }
}
