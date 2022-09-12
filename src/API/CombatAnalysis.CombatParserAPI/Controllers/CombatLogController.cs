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
        private readonly IService<CombatLogDto, int> _service;
        private readonly IMapper _mapper;
        private readonly SaveCombatDataHelper _saveCombatDataHelper;

        public CombatLogController(IService<CombatLogDto, int> service, IMapper mapper, IHttpClientHelper httpClient, ILogger logger)
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
        public async Task<CombatLogModel> Create(List<string> dungeonNames)
        {
            var combatLog = _saveCombatDataHelper.CreateCombatLog(dungeonNames);

            var map = _mapper.Map<CombatLogDto>(combatLog);
            var createdItem = await _service.CreateAsync(map);
            var resultMap = _mapper.Map<CombatLogModel>(createdItem);

            return resultMap;
        }

        [HttpPut]
        public async Task Update(CombatLogModel value)
        {
            var map = _mapper.Map<CombatLogDto>(value);
            await _service.UpdateAsync(map);
        }

        [HttpDelete]
        public async Task<int> Delete(CombatLogModel value)
        {
            var map = _mapper.Map<CombatLogDto>(value);
            var deletedId = await _service.DeleteAsync(map);

            return deletedId;
        }
    }
}
