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
    internal class ResourceRecoveryService : IService<ResourceRecoveryDto, int>
    {
        private readonly IGenericRepository<ResourceRecovery, int> _repository;
        private readonly IMapper _mapper;

        public ResourceRecoveryService(IGenericRepository<ResourceRecovery, int> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        Task<int> IService<ResourceRecoveryDto, int>.CreateAsync(ResourceRecoveryDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return CreateInternalAsync(item);
        }

        Task<int> IService<ResourceRecoveryDto, int>.DeleteAsync(ResourceRecoveryDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return DeleteInternalAsync(item);
        }

        async Task<IEnumerable<ResourceRecoveryDto>> IService<ResourceRecoveryDto, int>.GetAllAsync()
        {
            var allData = await _repository.GetAllAsync();
            var result = _mapper.Map<List<ResourceRecoveryDto>>(allData);

            return result;
        }

        async Task<ResourceRecoveryDto> IService<ResourceRecoveryDto, int>.GetByIdAsync(int id)
        {
            var executeLoad = await _repository.GetByIdAsync(id);
            var result = _mapper.Map<ResourceRecoveryDto>(executeLoad);

            return result;
        }

        Task<int> IService<ResourceRecoveryDto, int>.UpdateAsync(ResourceRecoveryDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return UpdateInternalAsync(item);
        }

        private async Task<int> CreateInternalAsync(ResourceRecoveryDto item)
        {
            var paramNames = new string[] { nameof(item.Value), nameof(item.Time), nameof(item.SpellOrItem), nameof(item.CombatPlayerId) };
            var paramValues = new object[] { item.Value, item.Time, item.SpellOrItem, item.CombatPlayerId };

            var createdCombatId = await _repository.CreateAsync(paramNames, paramValues);
            return createdCombatId;
        }

        private async Task<int> DeleteInternalAsync(ResourceRecoveryDto item)
        {
            var allData = await _repository.GetAllAsync();
            if (!allData.Any())
            {
                throw new NotFoundException($"Collection entity {nameof(ResourceRecoveryDto)} not found", nameof(allData));
            }

            var numberEntries = await _repository.DeleteAsync(item.Id);
            return numberEntries;
        }

        private async Task<int> UpdateInternalAsync(ResourceRecoveryDto item)
        {
            var allData = await _repository.GetAllAsync();
            if (!allData.Any())
            {
                throw new NotFoundException($"Collection entity {nameof(ResourceRecoveryDto)} not found", nameof(allData));
            }

            var paramNames = new string[] { nameof(item.Value), nameof(item.Time), nameof(item.SpellOrItem), nameof(item.CombatPlayerId) };
            var paramValues = new object[] { item.Value, item.Time, item.SpellOrItem, item.CombatPlayerId };

            var numberEntries = await _repository.UpdateAsync(paramNames, paramValues);
            return numberEntries;
        }
    }
}
