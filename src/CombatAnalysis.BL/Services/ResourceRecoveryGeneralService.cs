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
    internal class ResourceRecoveryGeneralService : IService<ResourceRecoveryGeneralDto>
    {
        private readonly IGenericRepository<ResourceRecoveryGeneral> _repository;
        private readonly IMapper _mapper;

        public ResourceRecoveryGeneralService(IGenericRepository<ResourceRecoveryGeneral> userRepository, IMapper mapper)
        {
            _repository = userRepository;
            _mapper = mapper;
        }

        Task<int> IService<ResourceRecoveryGeneralDto>.CreateAsync(ResourceRecoveryGeneralDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return CreateInternalAsync(item);
        }

        async Task<int> IService<ResourceRecoveryGeneralDto>.CreateByProcedureAsync(ResourceRecoveryGeneralDto item)
        {
            var paramNames = new string[] { nameof(item.Value), nameof(item.ResourcePerSecond), nameof(item.SpellOrItem), nameof(item.CastNumber),
                nameof(item.MinValue), nameof(item.MaxValue), nameof(item.AverageValue), nameof(item.CombatPlayerDataId) };
            var paramValues = new object[] { item.Value, item.ResourcePerSecond, item.SpellOrItem, item.CastNumber,
                item.MinValue, item.MaxValue, item.AverageValue, item.CombatPlayerDataId };

            var response = await _repository.ExecuteStoredProcedureAsync(DbProcedureHelper.InsertIntoResourceRecoveryGeneral, paramNames, paramValues);
            return response;
        }

        Task<int> IService<ResourceRecoveryGeneralDto>.DeleteAsync(ResourceRecoveryGeneralDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return DeleteInternalAsync(item);
        }

        async Task<int> IService<ResourceRecoveryGeneralDto>.DeleteByProcedureAsync(int combatPlayerId)
        {
            var paramNames = new string[] { nameof(combatPlayerId) };
            var paramValues = new object[] { combatPlayerId };

            var response = await _repository.ExecuteStoredProcedureAsync(DbProcedureHelper.DeleteResourceRecoveryGeneral, paramNames, paramValues);
            return response;
        }

        async Task<IEnumerable<ResourceRecoveryGeneralDto>> IService<ResourceRecoveryGeneralDto>.GetAllAsync()
        {
            var allData = await _repository.GetAllAsync();
            var result = _mapper.Map<List<ResourceRecoveryGeneralDto>>(allData);

            return result;
        }

        async Task<IEnumerable<ResourceRecoveryGeneralDto>> IService<ResourceRecoveryGeneralDto>.GetByProcedureAsync(int combatPlayerId)
        {
            var paramNames = new string[] { nameof(combatPlayerId) };
            var paramValues = new object[] { combatPlayerId };

            var data = await _repository.ExecuteStoredProcedureUseModelAsync(DbProcedureHelper.GetResourceRecoveryGeneral, paramNames, paramValues);
            var result = _mapper.Map<IEnumerable<ResourceRecoveryGeneralDto>>(data);

            return result;
        }

        async Task<ResourceRecoveryGeneralDto> IService<ResourceRecoveryGeneralDto>.GetByIdAsync(int id)
        {
            var executeLoad = await _repository.GetByIdAsync(id);
            var result = _mapper.Map<ResourceRecoveryGeneralDto>(executeLoad);

            return result;
        }

        Task<int> IService<ResourceRecoveryGeneralDto>.UpdateAsync(ResourceRecoveryGeneralDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return UpdateInternalAsync(item);
        }

        private async Task<int> CreateInternalAsync(ResourceRecoveryGeneralDto item)
        {
            var map = _mapper.Map<ResourceRecoveryGeneral>(item);
            var createdCombatId = await _repository.CreateAsync(map);

            return createdCombatId;
        }

        private async Task<int> DeleteInternalAsync(ResourceRecoveryGeneralDto item)
        {
            var allData = await _repository.GetAllAsync();
            if (!allData.Any())
            {
                throw new NotFoundException($"Collection entity {nameof(ResourceRecoveryGeneralDto)} not found", nameof(allData));
            }

            var numberEntries = await _repository.DeleteAsync(_mapper.Map<ResourceRecoveryGeneral>(item));
            return numberEntries;
        }

        private async Task<int> UpdateInternalAsync(ResourceRecoveryGeneralDto item)
        {
            var allData = await _repository.GetAllAsync();
            if (!allData.Any())
            {
                throw new NotFoundException($"Collection entity {nameof(ResourceRecoveryGeneralDto)} not found", nameof(allData));
            }

            var numberEntries = await _repository.UpdateAsync(_mapper.Map<ResourceRecoveryGeneral>(item));
            return numberEntries;
        }
    }
}
