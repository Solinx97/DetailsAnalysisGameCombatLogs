﻿using AutoMapper;
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
    public class DamageDoneGeneralController : ControllerBase
    {
        private readonly IService<DamageDoneGeneralDto> _service;
        private readonly IMapper _mapper;

        public DamageDoneGeneralController(IService<DamageDoneGeneralDto> service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet("FindByCombatPlayerId/{combatPlayerId}")]
        public async Task<IEnumerable<DamageDoneGeneralModel>> Find(int combatPlayerId)
        {
            var damageDoneGenerals = await _service.FindAllAsync(combatPlayerId);
            var map = _mapper.Map<IEnumerable<DamageDoneGeneralModel>>(damageDoneGenerals);

            return map;
        }

        [HttpPost]
        public async Task<int> Post(DamageDoneGeneralModel value)
        {
            var map = _mapper.Map<DamageDoneGeneralDto>(value);
            var createdCombatId = await _service.CreateAsync(map);

            return createdCombatId;
        }

        [HttpDelete("{id}")]
        public async Task<int> Delete(int id)
        {
            var damageDoneGeneral = await _service.GetByIdAsync(id);
            var deletedId = await _service.DeleteAsync(damageDoneGeneral);

            return deletedId;
        }
    }
}