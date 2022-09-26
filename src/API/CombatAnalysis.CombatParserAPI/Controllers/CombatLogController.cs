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
    public class CombatLogController : ControllerBase
    {
        private readonly IService<CombatLogDto, int> _service;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly SaveCombatDataHelper _saveCombatDataHelper;

        public CombatLogController(IService<CombatLogDto, int> service, IMapper mapper, IHttpClientHelper httpClient, ILogger logger)
        {
            _service = service;
            _mapper = mapper;
            _logger = logger;
            _saveCombatDataHelper = new SaveCombatDataHelper(mapper, httpClient, logger);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var combatLogs = await _service.GetAllAsync();
            var map = _mapper.Map<IEnumerable<CombatLogModel>>(combatLogs);

            return Ok(map);
        }

        [HttpGet("{id:int:min(1)}")]
        public async Task<IActionResult> GetById(int id)
        {
            var combatLog = await _service.GetByIdAsync(id);
            var map = _mapper.Map<CombatLogModel>(combatLog);

            return Ok(map);
        }

        [HttpPost]
        public async Task<IActionResult> Create(List<string> dungeonNames)
        {
            try
            {
                var combatLog = _saveCombatDataHelper.CreateCombatLog(dungeonNames);

                var map = _mapper.Map<CombatLogDto>(combatLog);
                var createdItem = await _service.CreateAsync(map);
                var resultMap = _mapper.Map<CombatLogModel>(createdItem);

                return Ok(resultMap);
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, ex.Message);

                return BadRequest();
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update(CombatLogModel value)
        {
            try
            {
                var map = _mapper.Map<CombatLogDto>(value);
                var rowsAffected = await _service.UpdateAsync(map);

                return Ok(rowsAffected);
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, ex.Message);

                return BadRequest();
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(CombatLogModel value)
        {
            try
            {
                var map = _mapper.Map<CombatLogDto>(value);
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
