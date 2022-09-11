using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Exceptions;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CombatAnalysis.BL.Services
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

        async Task<IEnumerable<ResourceRecoveryDto>> IService<ResourceRecoveryDto, int>.GetByParamAsync(string paramName, object value)
        {
            var executeLoad = await Task.Run(() => _repository.GetByParam(paramName, value));
            var result = _mapper.Map<IEnumerable<ResourceRecoveryDto>>(executeLoad);

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
            var map = _mapper.Map<ResourceRecovery>(item);
            var createdCombatId = await _repository.CreateAsync(map);

            return createdCombatId;
        }

        private async Task<int> DeleteInternalAsync(ResourceRecoveryDto item)
        {
            var allData = await _repository.GetAllAsync();
            if (!allData.Any())
            {
                throw new NotFoundException($"Collection entity {nameof(ResourceRecoveryDto)} not found", nameof(allData));
            }

            var numberEntries = await _repository.DeleteAsync(_mapper.Map<ResourceRecovery>(item));
            return numberEntries;
        }

        private async Task<int> UpdateInternalAsync(ResourceRecoveryDto item)
        {
            var allData = await _repository.GetAllAsync();
            if (!allData.Any())
            {
                throw new NotFoundException($"Collection entity {nameof(ResourceRecoveryDto)} not found", nameof(allData));
            }

            var numberEntries = await _repository.UpdateAsync(_mapper.Map<ResourceRecovery>(item));
            return numberEntries;
        }
    }
}
