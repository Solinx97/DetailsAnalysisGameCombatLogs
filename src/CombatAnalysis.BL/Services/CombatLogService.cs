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
    internal class CombatLogService : IService<CombatLogDto, int>
    {
        private readonly IGenericRepository<CombatLog, int> _repository;
        private readonly IMapper _mapper;

        public CombatLogService(IGenericRepository<CombatLog, int> userRepository, IMapper mapper)
        {
            _repository = userRepository;
            _mapper = mapper;
        }

        Task<CombatLogDto> IService<CombatLogDto, int>.CreateAsync(CombatLogDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(CombatLogDto), $"The {nameof(CombatLogDto)} can't be null");
            }

            return CreateInternalAsync(item);
        }

        Task<int> IService<CombatLogDto, int>.DeleteAsync(CombatLogDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(CombatLogDto), $"The {nameof(CombatLogDto)} can't be null");
            }

            return DeleteInternalAsync(item);
        }

        async Task<IEnumerable<CombatLogDto>> IService<CombatLogDto, int>.GetAllAsync()
        {
            var allData = await _repository.GetAllAsync();
            var result = _mapper.Map<List<CombatLogDto>>(allData);

            return result;
        }

        async Task<CombatLogDto> IService<CombatLogDto, int>.GetByIdAsync(int id)
        {
            var result = await _repository.GetByIdAsync(id);
            var resultMap = _mapper.Map<CombatLogDto>(result);

            return resultMap;
        }

        async Task<IEnumerable<CombatLogDto>> IService<CombatLogDto, int>.GetByParamAsync(string paramName, object value)
        {
            var result = await Task.Run(() => _repository.GetByParam(paramName, value));
            var resultMap = _mapper.Map<IEnumerable<CombatLogDto>>(result);

            return resultMap;
        }

        Task<int> IService<CombatLogDto, int>.UpdateAsync(CombatLogDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(CombatLogDto), $"The {nameof(CombatLogDto)} can't be null");
            }

            return UpdateInternalAsync(item);
        }

        private async Task<CombatLogDto> CreateInternalAsync(CombatLogDto item)
        {
            if (string.IsNullOrEmpty(item.Name))
            {
                throw new ArgumentNullException(nameof(CombatLogDto),
                    $"The property {nameof(CombatLogDto.Name)} of the {nameof(CombatLogDto)} object can't be null or empty");
            }

            var map = _mapper.Map<CombatLog>(item);
            var createdItem = await _repository.CreateAsync(map);
            var resultMap = _mapper.Map<CombatLogDto>(createdItem);

            return resultMap;
        }

        private async Task<int> DeleteInternalAsync(CombatLogDto item)
        {
            if (string.IsNullOrEmpty(item.Name))
            {
                throw new ArgumentNullException(nameof(CombatLogDto),
                    $"The property {nameof(CombatLogDto.Name)} of the {nameof(CombatLogDto)} object can't be null or empty");
            }

            var map = _mapper.Map<CombatLog>(item);
            var rowsAffected = await _repository.DeleteAsync(map);

            return rowsAffected;
        }

        private async Task<int> UpdateInternalAsync(CombatLogDto item)
        {
            if (string.IsNullOrEmpty(item.Name))
            {
                throw new ArgumentNullException(nameof(CombatLogDto),
                    $"The property {nameof(CombatLogDto.Name)} of the {nameof(CombatLogDto)} object can't be null or empty");
            }

            var map = _mapper.Map<CombatLog>(item);
            var rowsAffected = await _repository.UpdateAsync(map);

            return rowsAffected;
        }
    }
}
