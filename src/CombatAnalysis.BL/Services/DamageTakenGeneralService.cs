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
    internal class DamageTakenGeneralService : IService<DamageTakenGeneralDto, int>
    {
        private readonly IGenericRepository<DamageTakenGeneral, int> _repository;
        private readonly IMapper _mapper;

        public DamageTakenGeneralService(IGenericRepository<DamageTakenGeneral, int> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        Task<DamageTakenGeneralDto> IService<DamageTakenGeneralDto, int>.CreateAsync(DamageTakenGeneralDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(DamageTakenGeneralDto), $"The {nameof(DamageTakenGeneralDto)} can't be null");
            }

            return CreateInternalAsync(item);
        }

        Task<int> IService<DamageTakenGeneralDto, int>.DeleteAsync(DamageTakenGeneralDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(DamageTakenGeneralDto), $"The {nameof(DamageTakenGeneralDto)} can't be null");
            }

            return DeleteInternalAsync(item);
        }

        async Task<IEnumerable<DamageTakenGeneralDto>> IService<DamageTakenGeneralDto, int>.GetAllAsync()
        {
            var allData = await _repository.GetAllAsync();
            var result = _mapper.Map<List<DamageTakenGeneralDto>>(allData);

            return result;
        }

        async Task<DamageTakenGeneralDto> IService<DamageTakenGeneralDto, int>.GetByIdAsync(int id)
        {
            var result = await _repository.GetByIdAsync(id);
            var resultMap = _mapper.Map<DamageTakenGeneralDto>(result);

            return resultMap;
        }

        async Task<IEnumerable<DamageTakenGeneralDto>> IService<DamageTakenGeneralDto, int>.GetByParamAsync(string paramName, object value)
        {
            var result = await Task.Run(() => _repository.GetByParam(paramName, value));
            var resultMap = _mapper.Map<IEnumerable<DamageTakenGeneralDto>>(result);

            return resultMap;
        }

        Task<int> IService<DamageTakenGeneralDto, int>.UpdateAsync(DamageTakenGeneralDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(DamageTakenGeneralDto), $"The {nameof(DamageTakenGeneralDto)} can't be null");
            }

            return UpdateInternalAsync(item);
        }

        private async Task<DamageTakenGeneralDto> CreateInternalAsync(DamageTakenGeneralDto item)
        {
            if (string.IsNullOrEmpty(item.SpellOrItem))
            {
                throw new ArgumentNullException(nameof(DamageTakenGeneralDto), 
                    $"The property {nameof(DamageTakenGeneralDto.SpellOrItem)} of the {nameof(DamageTakenGeneralDto)} object can't be null or empty");
            }

            var map = _mapper.Map<DamageTakenGeneral>(item);
            var createdItem = await _repository.CreateAsync(map);
            var resultMap = _mapper.Map<DamageTakenGeneralDto>(createdItem);

            return resultMap;
        }

        private async Task<int> DeleteInternalAsync(DamageTakenGeneralDto item)
        {
            if (string.IsNullOrEmpty(item.SpellOrItem))
            {
                throw new ArgumentNullException(nameof(DamageTakenGeneralDto), 
                    $"The property {nameof(DamageTakenGeneralDto.SpellOrItem)} of the {nameof(DamageTakenGeneralDto)} object can't be null or empty");
            }

            var map = _mapper.Map<DamageTakenGeneral>(item);
            var rowsAffected = await _repository.DeleteAsync(map);

            return rowsAffected;
        }

        private async Task<int> UpdateInternalAsync(DamageTakenGeneralDto item)
        {
            if (string.IsNullOrEmpty(item.SpellOrItem))
            {
                throw new ArgumentNullException(nameof(DamageTakenGeneralDto),
                    $"The property {nameof(DamageTakenGeneralDto.SpellOrItem)} of the {nameof(DamageTakenGeneralDto)} object can't be null or empty");
            }

            var map = _mapper.Map<DamageTakenGeneral>(item);
            var rowsAffected = await _repository.UpdateAsync(map);

            return rowsAffected;
        }
    }
}
