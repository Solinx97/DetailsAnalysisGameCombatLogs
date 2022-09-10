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
    internal class ResourceRecoveryGeneralService : IService<ResourceRecoveryGeneralDto, int>
    {
        private readonly IGenericRepository<ResourceRecoveryGeneral> _repository;
        private readonly IMapper _mapper;

        public ResourceRecoveryGeneralService(IGenericRepository<ResourceRecoveryGeneral> repository, IMapper mapper)
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

        async Task<IEnumerable<ResourceRecoveryGeneralDto>> IService<ResourceRecoveryGeneralDto, int>.GetByParamAsync(string paramName, object value)
        {
            var executeLoad = await Task.Run(() => _repository.GetByParam(paramName, value));
            var result = _mapper.Map<IEnumerable<ResourceRecoveryGeneralDto>>(executeLoad);

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
