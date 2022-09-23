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
    internal class DamageTakenService : IService<DamageTakenDto, int>
    {
        private readonly IGenericRepository<DamageTaken, int> _repository;
        private readonly IMapper _mapper;

        public DamageTakenService(IGenericRepository<DamageTaken, int> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        Task<DamageTakenDto> IService<DamageTakenDto, int>.CreateAsync(DamageTakenDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(DamageTakenDto), $"The {nameof(DamageTakenDto)} can't be null");
            }

            return CreateInternalAsync(item);
        }

        Task<int> IService<DamageTakenDto, int>.DeleteAsync(DamageTakenDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(DamageTakenDto), $"The {nameof(DamageTakenDto)} can't be null");
            }

            return DeleteInternalAsync(item);
        }

        async Task<IEnumerable<DamageTakenDto>> IService<DamageTakenDto, int>.GetAllAsync()
        {
            var allData = await _repository.GetAllAsync();
            var result = _mapper.Map<List<DamageTakenDto>>(allData);

            return result;
        }

        async Task<DamageTakenDto> IService<DamageTakenDto, int>.GetByIdAsync(int id)
        {
            var result = await _repository.GetByIdAsync(id);
            var resultMap = _mapper.Map<DamageTakenDto>(result);

            return resultMap;
        }

        async Task<IEnumerable<DamageTakenDto>> IService<DamageTakenDto, int>.GetByParamAsync(string paramName, object value)
        {
            var result = await Task.Run(() => _repository.GetByParam(paramName, value));
            var resultMap = _mapper.Map<IEnumerable<DamageTakenDto>>(result);

            return resultMap;
        }

        Task<int> IService<DamageTakenDto, int>.UpdateAsync(DamageTakenDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(DamageTakenDto), $"The {nameof(DamageTakenDto)} can't be null");
            }

            return UpdateInternalAsync(item);
        }

        private async Task<DamageTakenDto> CreateInternalAsync(DamageTakenDto item)
        {
            if (string.IsNullOrEmpty(item.FromEnemy))
            {
                throw new ArgumentNullException(nameof(DamageTakenDto),
                    $"The property {nameof(DamageTakenDto.FromEnemy)} of the {nameof(DamageTakenDto)} object can't be null or empty");
            }
            if (string.IsNullOrEmpty(item.ToPlayer))
            {
                throw new ArgumentNullException(nameof(DamageTakenDto),
                    $"The property {nameof(DamageTakenDto.ToPlayer)} of the {nameof(DamageTakenDto)} object can't be null or empty");
            }
            if (string.IsNullOrEmpty(item.SpellOrItem))
            {
                throw new ArgumentNullException(nameof(DamageTakenDto),
                    $"The property {nameof(DamageTakenDto.SpellOrItem)} of the {nameof(DamageTakenDto)} object can't be null or empty");
            }

            var map = _mapper.Map<DamageTaken>(item);
            var createdItem = await _repository.CreateAsync(map);
            var resultMap = _mapper.Map<DamageTakenDto>(createdItem);

            return resultMap;
        }

        private async Task<int> DeleteInternalAsync(DamageTakenDto item)
        {
            if (string.IsNullOrEmpty(item.FromEnemy))
            {
                throw new ArgumentNullException(nameof(DamageTakenDto),
                    $"The property {nameof(DamageTakenDto.FromEnemy)} of the {nameof(DamageTakenDto)} object can't be null or empty");
            }
            if (string.IsNullOrEmpty(item.ToPlayer))
            {
                throw new ArgumentNullException(nameof(DamageTakenDto),
                    $"The property {nameof(DamageTakenDto.ToPlayer)} of the {nameof(DamageTakenDto)} object can't be null or empty");
            }
            if (string.IsNullOrEmpty(item.SpellOrItem))
            {
                throw new ArgumentNullException(nameof(DamageTakenDto), 
                    $"The property {nameof(DamageTakenDto.SpellOrItem)} of the {nameof(DamageTakenDto)} object can't be null or empty");
            }

            var map = _mapper.Map<DamageTaken>(item);
            var rowsAffected = await _repository.DeleteAsync(map);

            return rowsAffected;
        }

        private async Task<int> UpdateInternalAsync(DamageTakenDto item)
        {
            if (string.IsNullOrEmpty(item.FromEnemy))
            {
                throw new ArgumentNullException(nameof(DamageTakenDto), 
                    $"The property {nameof(DamageTakenDto.FromEnemy)} of the {nameof(DamageTakenDto)} object can't be null or empty");
            }
            if (string.IsNullOrEmpty(item.ToPlayer))
            {
                throw new ArgumentNullException(nameof(DamageTakenDto), 
                    $"The property {nameof(DamageTakenDto.ToPlayer)} of the {nameof(DamageTakenDto)} object can't be null or empty");
            }
            if (string.IsNullOrEmpty(item.SpellOrItem))
            {
                throw new ArgumentNullException(nameof(DamageTakenDto),
                    $"The property {nameof(DamageTakenDto.SpellOrItem)} of the {nameof(DamageTakenDto)} object can't be null or empty");
            }

            var map = _mapper.Map<DamageTaken>(item);
            var rowsAffected = await _repository.UpdateAsync(map);

            return rowsAffected;
        }
    }
}
