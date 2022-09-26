using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces;
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
    public class CombatPlayerController : ControllerBase
    {
        private readonly IService<CombatPlayerDto, int> _service;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public CombatPlayerController(IService<CombatPlayerDto, int> service, IMapper mapper, ILogger logger)
        {
            _service = service;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("findByCombatId/{combatId:int:min(1)}")]
        public async Task<IActionResult> Find(int combatId)
        {
            var players = await _service.GetByParamAsync("CombatId", combatId);
            var map = _mapper.Map<IEnumerable<CombatPlayerDto>>(players);

            return Ok(map);
        }

        [HttpGet("{id:int:min(1)}")]
        public async Task<IActionResult> GetById(int id)
        {
            var combatLog = await _service.GetByIdAsync(id);
            var map = _mapper.Map<CombatPlayerDto>(combatLog);

            return Ok(map);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CombatPlayerModel model)
        {
            try
            {
                var map = _mapper.Map<CombatPlayerDto>(model);
                var createdItem = await _service.CreateAsync(map);
                var resultMap = _mapper.Map<CombatPlayerModel>(createdItem);

                return Ok(resultMap);
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, ex.Message);

                return BadRequest();
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(CombatPlayerModel model)
        {
            try
            {
                var map = _mapper.Map<CombatPlayerDto>(model);
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
