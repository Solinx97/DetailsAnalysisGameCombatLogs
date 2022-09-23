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
    internal class CombatService : IService<CombatDto, int>
    {
        private readonly IGenericRepository<Combat, int> _repository;
        private readonly IMapper _mapper;

        public CombatService(IGenericRepository<Combat, int> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        Task<CombatDto> IService<CombatDto, int>.CreateAsync(CombatDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(CombatDto), $"The {nameof(CombatDto)} can't be null");
            }

            return CreateInternalAsync(item);
        }

        Task<int> IService<CombatDto, int>.DeleteAsync(CombatDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(CombatDto), $"The {nameof(CombatDto)} can't be null");
            }

            return DeleteInternalAsync(item);
        }

        async Task<IEnumerable<CombatDto>> IService<CombatDto, int>.GetAllAsync()
        {
            var allData = await _repository.GetAllAsync();
            var result = _mapper.Map<List<CombatDto>>(allData);

            return result;
        }

        async Task<CombatDto> IService<CombatDto, int>.GetByIdAsync(int id)
        {
            var result = await _repository.GetByIdAsync(id);
            var resultMap = _mapper.Map<CombatDto>(result);

            return resultMap;
        }

        async Task<IEnumerable<CombatDto>> IService<CombatDto, int>.GetByParamAsync(string paramName, object value)
        {
            var result = await Task.Run(() =>_repository.GetByParam(paramName, value));
            var resultMap = _mapper.Map<IEnumerable<CombatDto>>(result);

            return resultMap;
        }

        Task<int> IService<CombatDto, int>.UpdateAsync(CombatDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(CombatDto), $"The {nameof(CombatDto)} can't be null");
            }

            return UpdateInternalAsync(item);
        }

        private async Task<CombatDto> CreateInternalAsync(CombatDto item)
        {
            if (string.IsNullOrEmpty(item.Name))
            {
                throw new ArgumentNullException(nameof(CombatDto), 
                    $"The property {nameof(CombatDto.Name)} of the {nameof(CombatDto)} object can't be null or empty");
            }
            if (string.IsNullOrEmpty(item.DungeonName))
            {
                throw new ArgumentNullException(nameof(CombatDto), 
                    $"The property {nameof(CombatDto.DungeonName)} of the {nameof(CombatDto)} object can't be null or empty");
            }

            var map = _mapper.Map<Combat>(item);
            var createdItem = await _repository.CreateAsync(map);
            var resultMap = _mapper.Map<CombatDto>(createdItem);

            return resultMap;
        }

        private async Task<int> DeleteInternalAsync(CombatDto item)
        {
            if (string.IsNullOrEmpty(item.Name))
            {
                throw new ArgumentNullException(nameof(CombatDto), 
                    $"The property {nameof(CombatDto.Name)} of the {nameof(CombatDto)} object can't be null or empty");
            }
            if (string.IsNullOrEmpty(item.DungeonName))
            {
                throw new ArgumentNullException(nameof(CombatDto), 
                    $"The property {nameof(CombatDto.DungeonName)} of the {nameof(CombatDto)} object can't be null or empty");
            }

            var map = _mapper.Map<Combat>(item);
            var rowsAffected = await _repository.DeleteAsync(map);

            return rowsAffected;
        }

        private async Task<int> UpdateInternalAsync(CombatDto item)
        {
            if (string.IsNullOrEmpty(item.Name))
            {
                throw new ArgumentNullException(nameof(CombatDto), 
                    $"The property {nameof(CombatDto.Name)} of the {nameof(CombatDto)} object can't be null or empty");
            }
            if (string.IsNullOrEmpty(item.DungeonName))
            {
                throw new ArgumentNullException(nameof(CombatDto), 
                    $"The property {nameof(CombatDto.DungeonName)} of the {nameof(CombatDto)} object can't be null or empty");
            }

            var map = _mapper.Map<Combat>(item);
            var rowsAffected = await _repository.UpdateAsync(map);

            return rowsAffected;
        }
    }
}
