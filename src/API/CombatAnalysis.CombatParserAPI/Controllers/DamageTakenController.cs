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
    public class DamageTakenController : ControllerBase
    {
        private readonly IService<DamageTakenDto, int> _service;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public DamageTakenController(IService<DamageTakenDto, int> service, IMapper mapper, ILogger logger)
        {
            _service = service;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("findByCombatPlayerId/{combatPlayerId:int:min(1)}")]
        public async Task<IActionResult> Find(int combatPlayerId)
        {
            var damageTakens = await _service.GetByParamAsync("CombatPlayerId", combatPlayerId);
            var map = _mapper.Map<IEnumerable<DamageTakenModel>>(damageTakens);

            return Ok(map);
        }

        [HttpPost]
        public async Task<IActionResult> Create(DamageTakenModel model)
        {
            try
            {
                var map = _mapper.Map<DamageTakenDto>(model);
                var createdItem = await _service.CreateAsync(map);
                var resultMap = _mapper.Map<DamageTakenModel>(createdItem);

                return Ok(resultMap);
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, ex.Message);

                return BadRequest();
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(DamageTakenModel model)
        {
            try
            {
                var map = _mapper.Map<DamageTakenDto>(model);
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
