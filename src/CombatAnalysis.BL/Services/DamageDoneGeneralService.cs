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
    internal class DamageDoneGeneralService : ISPService<DamageDoneGeneralDto, int>
    {
        private readonly ISPGenericRepository<DamageDoneGeneral> _repository;
        private readonly IMapper _mapper;

        public DamageDoneGeneralService(ISPGenericRepository<DamageDoneGeneral> userRepository, IMapper mapper)
        {
            _repository = userRepository;
            _mapper = mapper;
        }

        Task<int> IService<DamageDoneGeneralDto, int>.CreateAsync(DamageDoneGeneralDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return CreateInternalAsync(item);
        }

        async Task<int> ISPService<DamageDoneGeneralDto, int>.CreateByProcedureAsync(DamageDoneGeneralDto item)
        {
            var paramNames = new string[] { nameof(item.Value), nameof(item.DamagePerSecond), nameof(item.SpellOrItem),
                nameof(item.CritNumber), nameof(item.MissNumber), nameof(item.CastNumber), nameof(item.MinValue),  nameof(item.MaxValue),
                nameof(item.AverageValue), nameof(item.CombatPlayerDataId) };
            var paramValues = new object[] { item.Value, item.DamagePerSecond, item.SpellOrItem,
                item.CritNumber, item.MissNumber, item.CastNumber, item.MinValue, item.MaxValue,
                item.AverageValue, item.CombatPlayerDataId };

            var response = await _repository.ExecuteStoredProcedureAsync(DbProcedureHelper.InsertIntoDamageDoneGeneral, paramNames, paramValues);
            return response;
        }

        Task<int> IService<DamageDoneGeneralDto, int>.DeleteAsync(DamageDoneGeneralDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return DeleteInternalAsync(item);
        }

        async Task<int> ISPService<DamageDoneGeneralDto, int>.DeleteByProcedureAsync(int combatPlayerId)
        {
            var paramNames = new string[] { nameof(combatPlayerId) };
            var paramValues = new object[] { combatPlayerId };

            var response = await _repository.ExecuteStoredProcedureAsync(DbProcedureHelper.DeleteDamageDoneGeneral, paramNames, paramValues);
            return response;
        }

        async Task<IEnumerable<DamageDoneGeneralDto>> IService<DamageDoneGeneralDto, int>.GetAllAsync()
        {
            var allData = await _repository.GetAllAsync();
            var result = _mapper.Map<List<DamageDoneGeneralDto>>(allData);

            return result;
        }

        async Task<IEnumerable<DamageDoneGeneralDto>> ISPService<DamageDoneGeneralDto, int>.GetByProcedureAsync(int combatPlayerId)
        {
            var paramNames = new string[] { nameof(combatPlayerId) };
            var paramValues = new object[] { combatPlayerId };

            var data = await _repository.ExecuteStoredProcedureUseModelAsync(DbProcedureHelper.GetDamageDoneGeneral, paramNames, paramValues);
            var result = _mapper.Map<IEnumerable<DamageDoneGeneralDto>>(data);

            return result;
        }

        async Task<DamageDoneGeneralDto> IService<DamageDoneGeneralDto, int>.GetByIdAsync(int id)
        {
            var executeLoad = await _repository.GetByIdAsync(id);
            var result = _mapper.Map<DamageDoneGeneralDto>(executeLoad);

            return result;
        }

        Task<int> IService<DamageDoneGeneralDto, int>.UpdateAsync(DamageDoneGeneralDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return UpdateInternalAsync(item);
        }

        private async Task<int> CreateInternalAsync(DamageDoneGeneralDto item)
        {
            var map = _mapper.Map<DamageDoneGeneral>(item);
            var createdCombatId = await _repository.CreateAsync(map);

            return createdCombatId;
        }

        private async Task<int> DeleteInternalAsync(DamageDoneGeneralDto item)
        {
            var allData = await _repository.GetAllAsync();
            if (!allData.Any())
            {
                throw new NotFoundException($"Collection entity {nameof(DamageDoneGeneralDto)} not found", nameof(allData));
            }

            var numberEntries = await _repository.DeleteAsync(_mapper.Map<DamageDoneGeneral>(item));
            return numberEntries;
        }

        private async Task<int> UpdateInternalAsync(DamageDoneGeneralDto item)
        {
            var allData = await _repository.GetAllAsync();
            if (!allData.Any())
            {
                throw new NotFoundException($"Collection entity {nameof(DamageDoneGeneralDto)} not found", nameof(allData));
            }

            var numberEntries = await _repository.UpdateAsync(_mapper.Map<DamageDoneGeneral>(item));
            return numberEntries;
        }
    }
}
