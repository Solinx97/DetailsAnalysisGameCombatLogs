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

        Task<ResourceRecoveryDto> IService<ResourceRecoveryDto, int>.CreateAsync(ResourceRecoveryDto item)
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
            var result = await _repository.GetByIdAsync(id);
            var resultMap = _mapper.Map<ResourceRecoveryDto>(result);

            return resultMap;
        }

        async Task<IEnumerable<ResourceRecoveryDto>> IService<ResourceRecoveryDto, int>.GetByParamAsync(string paramName, object value)
        {
            var result = await Task.Run(() => _repository.GetByParam(paramName, value));
            var resultMap = _mapper.Map<IEnumerable<ResourceRecoveryDto>>(result);

            return resultMap;
        }

        Task<int> IService<ResourceRecoveryDto, int>.UpdateAsync(ResourceRecoveryDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return UpdateInternalAsync(item);
        }

        private async Task<ResourceRecoveryDto> CreateInternalAsync(ResourceRecoveryDto item)
        {
            var map = _mapper.Map<ResourceRecovery>(item);
            var createdItem = await _repository.CreateAsync(map);
            var resultMap = _mapper.Map<ResourceRecoveryDto>(item);

            return resultMap;
        }

        private async Task<int> DeleteInternalAsync(ResourceRecoveryDto item)
        {
            var allData = await _repository.GetAllAsync();
            if (!allData.Any())
            {
                throw new NotFoundException($"Collection entity {nameof(ResourceRecoveryDto)} not found", nameof(allData));
            }

            var numberEntriesAffected = await _repository.DeleteAsync(_mapper.Map<ResourceRecovery>(item));
            return numberEntriesAffected;
        }

        private async Task<int> UpdateInternalAsync(ResourceRecoveryDto item)
        {
            var allData = await _repository.GetAllAsync();
            if (!allData.Any())
            {
                throw new NotFoundException($"Collection entity {nameof(ResourceRecoveryDto)} not found", nameof(allData));
            }

            var numberEntriesAffected = await _repository.UpdateAsync(_mapper.Map<ResourceRecovery>(item));
            return numberEntriesAffected;
        }
    }
}
