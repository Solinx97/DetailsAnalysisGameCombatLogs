using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CombatAnalysis.BL.Services
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

        Task<ResourceRecoveryGeneralDto> IService<ResourceRecoveryGeneralDto, int>.CreateAsync(ResourceRecoveryGeneralDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(ResourceRecoveryGeneralDto), $"The {nameof(ResourceRecoveryGeneralDto)} can't be null");
            }

            return CreateInternalAsync(item);
        }

        Task<int> IService<ResourceRecoveryGeneralDto, int>.DeleteAsync(ResourceRecoveryGeneralDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(ResourceRecoveryGeneralDto), $"The {nameof(ResourceRecoveryGeneralDto)} can't be null");
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
            var result = await _repository.GetByIdAsync(id);
            var resultMap = _mapper.Map<ResourceRecoveryGeneralDto>(result);

            return resultMap;
        }

        async Task<IEnumerable<ResourceRecoveryGeneralDto>> IService<ResourceRecoveryGeneralDto, int>.GetByParamAsync(string paramName, object value)
        {
            var result = await Task.Run(() => _repository.GetByParam(paramName, value));
            var resultMap = _mapper.Map<IEnumerable<ResourceRecoveryGeneralDto>>(result);

            return resultMap;
        }

        Task<int> IService<ResourceRecoveryGeneralDto, int>.UpdateAsync(ResourceRecoveryGeneralDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(ResourceRecoveryGeneralDto), $"The {nameof(ResourceRecoveryGeneralDto)} can't be null");
            }

            return UpdateInternalAsync(item);
        }

        private async Task<ResourceRecoveryGeneralDto> CreateInternalAsync(ResourceRecoveryGeneralDto item)
        {
            if (string.IsNullOrEmpty(item.SpellOrItem))
            {
                throw new ArgumentNullException(nameof(ResourceRecoveryGeneralDto), 
                    $"The property {nameof(ResourceRecoveryGeneralDto.SpellOrItem)} of the {nameof(ResourceRecoveryGeneralDto)} object can't be null or empty");
            }

            var map = _mapper.Map<ResourceRecoveryGeneral>(item);
            var createdItem = await _repository.CreateAsync(map);
            var resultMap = _mapper.Map<ResourceRecoveryGeneralDto>(createdItem);

            return resultMap;
        }

        private async Task<int> DeleteInternalAsync(ResourceRecoveryGeneralDto item)
        {
            if (string.IsNullOrEmpty(item.SpellOrItem))
            {
                throw new ArgumentNullException(nameof(ResourceRecoveryGeneralDto), 
                    $"The property {nameof(ResourceRecoveryGeneralDto.SpellOrItem)} of the {nameof(ResourceRecoveryGeneralDto)} object can't be null or empty");
            }

            var map = _mapper.Map<ResourceRecoveryGeneral>(item);
            var rowsAffected = await _repository.DeleteAsync(map);

            return rowsAffected;
        }

        private async Task<int> UpdateInternalAsync(ResourceRecoveryGeneralDto item)
        {
            if (string.IsNullOrEmpty(item.SpellOrItem))
            {
                throw new ArgumentNullException(nameof(ResourceRecoveryGeneralDto), 
                    $"The property {nameof(ResourceRecoveryGeneralDto.SpellOrItem)} of the {nameof(ResourceRecoveryGeneralDto)} object can't be null or empty");
            }

            var map = _mapper.Map<ResourceRecoveryGeneral>(item);
            var rowsAffected = await _repository.UpdateAsync(map);

            return rowsAffected;
        }
    }
}
