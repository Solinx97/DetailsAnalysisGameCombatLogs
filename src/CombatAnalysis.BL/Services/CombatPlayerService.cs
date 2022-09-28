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
    internal class CombatPlayerService : IService<CombatPlayerDto, int>
    {
        private readonly IGenericRepository<CombatPlayer, int> _repository;
        private readonly IMapper _mapper;

        public CombatPlayerService(IGenericRepository<CombatPlayer, int> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        Task<CombatPlayerDto> IService<CombatPlayerDto, int>.CreateAsync(CombatPlayerDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(CombatPlayerDto), $"The {nameof(CombatPlayerDto)} can't be null");
            }

            return CreateInternalAsync(item);
        }

        Task<int> IService<CombatPlayerDto, int>.DeleteAsync(CombatPlayerDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(CombatPlayerDto), $"The {nameof(CombatPlayerDto)} can't be null");
            }

            return DeleteInternalAsync(item);
        }

        async Task<IEnumerable<CombatPlayerDto>> IService<CombatPlayerDto, int>.GetAllAsync()
        {
            var allData = await _repository.GetAllAsync();
            var result = _mapper.Map<List<CombatPlayerDto>>(allData);

            return result;
        }

        async Task<CombatPlayerDto> IService<CombatPlayerDto, int>.GetByIdAsync(int id)
        {
            var result = await _repository.GetByIdAsync(id);
            var resultMap = _mapper.Map<CombatPlayerDto>(result);

            return resultMap;
        }

        async Task<IEnumerable<CombatPlayerDto>> IService<CombatPlayerDto, int>.GetByParamAsync(string paramName, object value)
        {
            var result = await Task.Run(() => _repository.GetByParam(paramName, value));
            var resultMap = _mapper.Map<IEnumerable<CombatPlayerDto>>(result);

            return resultMap;
        }

        Task<int> IService<CombatPlayerDto, int>.UpdateAsync(CombatPlayerDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(CombatPlayerDto), $"The {nameof(CombatPlayerDto)} can't be null");
            }

            return UpdateInternalAsync(item);
        }

        private async Task<CombatPlayerDto> CreateInternalAsync(CombatPlayerDto item)
        {
            if (string.IsNullOrEmpty(item.UserName))
            {
                throw new ArgumentNullException(nameof(CombatPlayerDto),
                    $"The property {nameof(CombatPlayerDto.UserName)} of the {nameof(CombatPlayerDto)} object can't be null or empty");
            }

            var map = _mapper.Map<CombatPlayer>(item);
            var createdItem = await _repository.CreateAsync(map);
            var resultMap = _mapper.Map<CombatPlayerDto>(createdItem);

            return resultMap;
        }

        private async Task<int> DeleteInternalAsync(CombatPlayerDto item)
        {
            if (string.IsNullOrEmpty(item.UserName))
            {
                throw new ArgumentNullException(nameof(CombatPlayerDto),
                    $"The property {nameof(CombatPlayerDto.UserName)} of the {nameof(CombatPlayerDto)} object can't be null or empty");
            }

            var map = _mapper.Map<CombatPlayer>(item);
            var rowsAffected = await _repository.DeleteAsync(map);

            return rowsAffected;
        }

        private async Task<int> UpdateInternalAsync(CombatPlayerDto item)
        {
            if (string.IsNullOrEmpty(item.UserName))
            {
                throw new ArgumentNullException(nameof(CombatPlayerDto),
                    $"The property {nameof(CombatPlayerDto.UserName)} of the {nameof(CombatPlayerDto)} object can't be null or empty");
            }

            var map = _mapper.Map<CombatPlayer>(item);
            var rowsAffected = await _repository.UpdateAsync(map);

            return rowsAffected;
        }
    }
}
