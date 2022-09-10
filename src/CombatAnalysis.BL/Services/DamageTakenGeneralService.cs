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
    internal class DamageTakenGeneralService : ISPService<DamageTakenGeneralDto, int>, IService<DamageTakenGeneralDto, int>
    {
        private readonly ISPGenericRepository<DamageTakenGeneral> _spRepository;
        private readonly IGenericRepository<DamageTakenGeneral> _repository;
        private readonly IMapper _mapper;

        public DamageTakenGeneralService(ISPGenericRepository<DamageTakenGeneral> spRepository, IGenericRepository<DamageTakenGeneral> repository, IMapper mapper)
        {
            _spRepository = spRepository;
            _repository = repository;
            _mapper = mapper;
        }

        Task<int> IService<DamageTakenGeneralDto, int>.CreateAsync(DamageTakenGeneralDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return CreateInternalAsync(item);
        }

        async Task<int> ISPService<DamageTakenGeneralDto, int>.CreateByProcedureAsync(DamageTakenGeneralDto item)
        {
            var paramNames = new string[] { nameof(item.Value), nameof(item.DamageTakenPerSecond), nameof(item.SpellOrItem),
                nameof(item.CritNumber), nameof(item.MissNumber), nameof(item.CastNumber), nameof(item.MinValue),
                nameof(item.MaxValue), nameof(item.AverageValue), nameof(item.CombatPlayerDataId) };
            var paramValues = new object[] { item.Value, item.DamageTakenPerSecond, item.SpellOrItem,
                item.CritNumber, item.MissNumber, item.CastNumber, item.MinValue,
                item.MaxValue, item.AverageValue, item.CombatPlayerDataId };

            var response = await _spRepository.ExecuteStoredProcedureAsync(DbProcedureHelper.InsertIntoDamageTakenGeneral, paramNames, paramValues);
            return response;
        }

        Task<int> IService<DamageTakenGeneralDto, int>.DeleteAsync(DamageTakenGeneralDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return DeleteInternalAsync(item);
        }

        async Task<int> ISPService<DamageTakenGeneralDto, int>.DeleteByProcedureAsync(int combatPlayerId)
        {
            var paramNames = new string[] { nameof(combatPlayerId) };
            var paramValues = new object[] { combatPlayerId };

            var response = await _spRepository.ExecuteStoredProcedureAsync(DbProcedureHelper.DeleteDamageTakenGeneral, paramNames, paramValues);
            return response;
        }

        async Task<IEnumerable<DamageTakenGeneralDto>> IService<DamageTakenGeneralDto, int>.GetAllAsync()
        {
            var allData = await _repository.GetAllAsync();
            var result = _mapper.Map<List<DamageTakenGeneralDto>>(allData);

            return result;
        }

        async Task<IEnumerable<DamageTakenGeneralDto>> ISPService<DamageTakenGeneralDto, int>.GetByProcedureAsync(int combatPlayerId)
        {
            var paramNames = new string[] { nameof(combatPlayerId) };
            var paramValues = new object[] { combatPlayerId };

            var data = await _spRepository.ExecuteStoredProcedureUseModelAsync(DbProcedureHelper.GetDamageTakenGeneral, paramNames, paramValues);
            var result = _mapper.Map<IEnumerable<DamageTakenGeneralDto>>(data);

            return result;
        }

        async Task<DamageTakenGeneralDto> IService<DamageTakenGeneralDto, int>.GetByIdAsync(int id)
        {
            var executeLoad = await _repository.GetByIdAsync(id);
            var result = _mapper.Map<DamageTakenGeneralDto>(executeLoad);

            return result;
        }

        Task<int> IService<DamageTakenGeneralDto, int>.UpdateAsync(DamageTakenGeneralDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return UpdateInternalAsync(item);
        }

        private async Task<int> CreateInternalAsync(DamageTakenGeneralDto item)
        {
            var map = _mapper.Map<DamageTakenGeneral>(item);
            var createdCombatId = await _repository.CreateAsync(map);

            return createdCombatId;
        }

        private async Task<int> DeleteInternalAsync(DamageTakenGeneralDto item)
        {
            var allData = await _repository.GetAllAsync();
            if (!allData.Any())
            {
                throw new NotFoundException($"Collection entity {nameof(DamageTakenGeneralDto)} not found", nameof(allData));
            }

            var numberEntries = await _repository.DeleteAsync(_mapper.Map<DamageTakenGeneral>(item));
            return numberEntries;
        }

        private async Task<int> UpdateInternalAsync(DamageTakenGeneralDto item)
        {
            var allData = await _repository.GetAllAsync();
            if (!allData.Any())
            {
                throw new NotFoundException($"Collection entity {nameof(DamageTakenGeneralDto)} not found", nameof(allData));
            }

            var numberEntries = await _repository.UpdateAsync(_mapper.Map<DamageTakenGeneral>(item));
            return numberEntries;
        }
    }
}
