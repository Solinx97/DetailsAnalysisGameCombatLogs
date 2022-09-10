using AutoMapper;
using CombatAnalysis.StoredProcedureBL.DTO;
using CombatAnalysis.StoredProcedureBL.Exceptions;
using CombatAnalysis.StoredProcedureBL.Interfaces;
using CombatAnalysis.StoredProcedureDAL.Entities;
using CombatAnalysis.StoredProcedureDAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CombatAnalysis.StoredProcedureBL.Services
{
    internal class ResourceRecoveryGeneralService : IService<ResourceRecoveryGeneralDto, int>
    {
        private readonly IGenericRepository<ResourceRecoveryGeneral, int> _repository;
        private readonly IMapper _mapper;

        public ResourceRecoveryGeneralService(IGenericRepository<ResourceRecoveryGeneral, int> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        Task<int> IService<ResourceRecoveryGeneralDto, int>.CreateAsync(ResourceRecoveryGeneralDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return CreateInternalAsync(item);
        }

        Task<int> IService<ResourceRecoveryGeneralDto, int>.DeleteAsync(ResourceRecoveryGeneralDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return DeleteInternalAsync(item);
        }
        async Task<IEnumerable<ResourceRecoveryGeneralDto>> IService<ResourceRecoveryGeneralDto, int>.GetAllAsync()
        {
            var allData = await _repository.GetAllAsync();
            var result = _mapper.Map<List<ResourceRecoveryGeneralDto>>(allData);

            return result;
        }

        async Task<ResourceRecoveryGeneralDto> IService<ResourceRecoveryGeneralDto, int>.GetByIdAsync(int id)
        {
            var executeLoad = await _repository.GetByIdAsync(id);
            var result = _mapper.Map<ResourceRecoveryGeneralDto>(executeLoad);

            return result;
        }

        Task<int> IService<ResourceRecoveryGeneralDto, int>.UpdateAsync(ResourceRecoveryGeneralDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return UpdateInternalAsync(item);
        }

        private async Task<int> CreateInternalAsync(ResourceRecoveryGeneralDto item)
        {
            var paramNames = new string[] { nameof(item.Value), nameof(item.ResourcePerSecond), nameof(item.SpellOrItem), nameof(item.CastNumber),
                nameof(item.MinValue), nameof(item.MaxValue), nameof(item.AverageValue), nameof(item.CombatPlayerId) };
            var paramValues = new object[] { item.Value, item.ResourcePerSecond, item.SpellOrItem, item.CastNumber,
                item.MinValue, item.MaxValue, item.AverageValue, item.CombatPlayerId };

            var createdCombatId = await _repository.CreateAsync(paramNames, paramValues);
            return createdCombatId;
        }

        private async Task<int> DeleteInternalAsync(ResourceRecoveryGeneralDto item)
        {
            var allData = await _repository.GetAllAsync();
            if (!allData.Any())
            {
                throw new NotFoundException($"Collection entity {nameof(ResourceRecoveryGeneralDto)} not found", nameof(allData));
            }

            var numberEntries = await _repository.DeleteAsync(item.Id);
            return numberEntries;
        }

        private async Task<int> UpdateInternalAsync(ResourceRecoveryGeneralDto item)
        {
            var allData = await _repository.GetAllAsync();
            if (!allData.Any())
            {
                throw new NotFoundException($"Collection entity {nameof(ResourceRecoveryGeneralDto)} not found", nameof(allData));
            }

            var paramNames = new string[] { nameof(item.Value), nameof(item.ResourcePerSecond), nameof(item.SpellOrItem), nameof(item.CastNumber),
                nameof(item.MinValue), nameof(item.MaxValue), nameof(item.AverageValue), nameof(item.CombatPlayerId) };
            var paramValues = new object[] { item.Value, item.ResourcePerSecond, item.SpellOrItem, item.CastNumber,
                item.MinValue, item.MaxValue, item.AverageValue, item.CombatPlayerId };

            var numberEntries = await _repository.UpdateAsync(paramNames, paramValues);
            return numberEntries;
        }
    }
}
