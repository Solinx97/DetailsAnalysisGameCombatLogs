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
    public class CombatLogController : ControllerBase
    {
        private readonly IService<CombatLogDto> _service;
        private readonly IMapper _mapper;
        private readonly SaveCombatDataHelper _saveCombatDataHelper;

        public CombatLogController(IService<CombatLogDto> service, IMapper mapper, IHttpClientHelper httpClient, ILogger logger)
        {
            _service = service;
            _mapper = mapper;
            _saveCombatDataHelper = new SaveCombatDataHelper(mapper, httpClient, logger);
        }

        [HttpGet]
        public async Task<IEnumerable<CombatLogModel>> Get()
        {
            var combatLogs = await _service.GetAllAsync();
            var map = _mapper.Map<IEnumerable<CombatLogModel>>(combatLogs);

            return map;
        }

        [HttpGet("{id}")]
        public async Task<CombatLogModel> GetById(int id)
        {
            var combatLog = await _service.GetByIdAsync(id);
            var map = _mapper.Map<CombatLogModel>(combatLog);

            return map;
        }

        [HttpPost]
        public async Task<int> Post(List<string> dungeonNames)
        {
            var combatLog = _saveCombatDataHelper.CreateCombatLog(dungeonNames);
            var map = _mapper.Map<CombatLogDto>(combatLog);
            var createdCombatId = await _service.CreateAsync(map);

            return createdCombatId;
        }

        [HttpPut]
        public async Task Put(CombatLogModel value)
        {
            var map = _mapper.Map<CombatLogDto>(value);
            await _service.UpdateAsync(map);
        }

        [HttpDelete("{id}")]
        public async Task<int> Delete(int id)
        {
            var combatLog = await _service.GetByIdAsync(id);
            var deletedId = await _service.DeleteAsync(combatLog);

            return deletedId;
        }
    }
}
