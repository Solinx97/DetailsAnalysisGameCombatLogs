using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Exceptions;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Helpers;
using CombatAnalysis.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CombatAnalysis.BL.Services
{
    internal class HealDoneGeneralService : ISPService<HealDoneGeneralDto, int>
    {
        private readonly ISPGenericRepository<HealDoneGeneral> _repository;
        private readonly IMapper _mapper;

        public HealDoneGeneralService(ISPGenericRepository<HealDoneGeneral> userRepository, IMapper mapper)
        {
            _repository = userRepository;
            _mapper = mapper;
        }

        Task<int> IService<HealDoneGeneralDto, int>.CreateAsync(HealDoneGeneralDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return CreateInternalAsync(item);
        }

        async Task<int> ISPService<HealDoneGeneralDto, int>.CreateByProcedureAsync(HealDoneGeneralDto item)
        {
            var paramNames = new string[] { nameof(item.Value), nameof(item.HealPerSecond), nameof(item.SpellOrItem),
                nameof(item.CritNumber), nameof(item.CastNumber), nameof(item.MinValue), nameof(item.MaxValue),
                nameof(item.AverageValue), nameof(item.CombatPlayerDataId) };
            var paramValues = new object[] { item.Value, item.HealPerSecond, item.SpellOrItem,
                item.CritNumber, item.CastNumber, item.MinValue, item.MaxValue,
                item.AverageValue, item.CombatPlayerDataId };

            var response = await _repository.ExecuteStoredProcedureAsync(DbProcedureHelper.InsertIntoHealDoneGeneral, paramNames, paramValues);
            return response;
        }

        Task<int> IService<HealDoneGeneralDto, int>.DeleteAsync(HealDoneGeneralDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return DeleteInternalAsync(item);
        }

        async Task<int> ISPService<HealDoneGeneralDto, int>.DeleteByProcedureAsync(int combatPlayerId)
        {
            var paramNames = new string[] { nameof(combatPlayerId) };
            var paramValues = new object[] { combatPlayerId };

            var response = await _repository.ExecuteStoredProcedureAsync(DbProcedureHelper.DeleteHealDoneGeneral, paramNames, paramValues);
            return response;
        }

        async Task<IEnumerable<HealDoneGeneralDto>> IService<HealDoneGeneralDto, int>.GetAllAsync()
        {
            var allData = await _repository.GetAllAsync();
            var result = _mapper.Map<List<HealDoneGeneralDto>>(allData);

            return result;
        }

        async Task<IEnumerable<HealDoneGeneralDto>> ISPService<HealDoneGeneralDto, int>.GetByProcedureAsync(int combatPlayerId)
        {
            var paramNames = new string[] { nameof(combatPlayerId) };
            var paramValues = new object[] { combatPlayerId };

            var data = await _repository.ExecuteStoredProcedureUseModelAsync(DbProcedureHelper.GetHealDoneGeneral, paramNames, paramValues);
            var result = _mapper.Map<IEnumerable<HealDoneGeneralDto>>(data);

            return result;
        }

        async Task<HealDoneGeneralDto> IService<HealDoneGeneralDto, int>.GetByIdAsync(int id)
        {
            var executeLoad = await _repository.GetByIdAsync(id);
            var result = _mapper.Map<HealDoneGeneralDto>(executeLoad);

            return result;
        }

        Task<int> IService<HealDoneGeneralDto, int>.UpdateAsync(HealDoneGeneralDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return UpdateInternalAsync(item);
        }

        private async Task<int> CreateInternalAsync(HealDoneGeneralDto item)
        {
            var map = _mapper.Map<HealDoneGeneral>(item);
            var createdCombatId = await _repository.CreateAsync(map);

            return createdCombatId;
        }

        private async Task<int> DeleteInternalAsync(HealDoneGeneralDto item)
        {
            var allData = await _repository.GetAllAsync();
            if (!allData.Any())
            {
                throw new NotFoundException($"Collection entity {nameof(HealDoneGeneralDto)} not found", nameof(allData));
            }

            var numberEntries = await _repository.DeleteAsync(_mapper.Map<HealDoneGeneral>(item));
            return numberEntries;
        }

        private async Task<int> UpdateInternalAsync(HealDoneGeneralDto item)
        {
            var allData = await _repository.GetAllAsync();
            if (!allData.Any())
            {
                throw new NotFoundException($"Collection entity {nameof(HealDoneGeneralDto)} not found", nameof(allData));
            }

            var numberEntries = await _repository.UpdateAsync(_mapper.Map<HealDoneGeneral>(item));
            return numberEntries;
        }
    }
}
