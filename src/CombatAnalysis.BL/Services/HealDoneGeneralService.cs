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
    internal class HealDoneGeneralService : IService<HealDoneGeneralDto, int>
    {
        private readonly IGenericRepository<HealDoneGeneral, int> _repository;
        private readonly IMapper _mapper;

        public HealDoneGeneralService(IGenericRepository<HealDoneGeneral, int> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        Task<HealDoneGeneralDto> IService<HealDoneGeneralDto, int>.CreateAsync(HealDoneGeneralDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(HealDoneGeneralDto), $"The {nameof(HealDoneGeneralDto)} can't be null");
            }

            return CreateInternalAsync(item);
        }

        Task<int> IService<HealDoneGeneralDto, int>.DeleteAsync(HealDoneGeneralDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(HealDoneGeneralDto), $"The {nameof(HealDoneGeneralDto)} can't be null");
            }

            return DeleteInternalAsync(item);
        }

        async Task<IEnumerable<HealDoneGeneralDto>> IService<HealDoneGeneralDto, int>.GetAllAsync()
        {
            var allData = await _repository.GetAllAsync();
            var result = _mapper.Map<List<HealDoneGeneralDto>>(allData);

            return result;
        }

        async Task<HealDoneGeneralDto> IService<HealDoneGeneralDto, int>.GetByIdAsync(int id)
        {
            var result = await _repository.GetByIdAsync(id);
            var resultMap = _mapper.Map<HealDoneGeneralDto>(result);

            return resultMap;
        }

        async Task<IEnumerable<HealDoneGeneralDto>> IService<HealDoneGeneralDto, int>.GetByParamAsync(string paramName, object value)
        {
            var result = await Task.Run(() => _repository.GetByParam(paramName, value));
            var resultMap = _mapper.Map<IEnumerable<HealDoneGeneralDto>>(result);

            return resultMap;
        }

        Task<int> IService<HealDoneGeneralDto, int>.UpdateAsync(HealDoneGeneralDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(HealDoneGeneralDto), $"The {nameof(HealDoneGeneralDto)} can't be null");
            }

            return UpdateInternalAsync(item);
        }

        private async Task<HealDoneGeneralDto> CreateInternalAsync(HealDoneGeneralDto item)
        {
            var map = _mapper.Map<HealDoneGeneral>(item);
            var createdItem = await _repository.CreateAsync(map);
            var resultMap = _mapper.Map<HealDoneGeneralDto>(createdItem);

            return resultMap;
        }

        private async Task<int> DeleteInternalAsync(HealDoneGeneralDto item)
        {
            var map = _mapper.Map<HealDoneGeneral>(item);
            var rowsAffected = await _repository.DeleteAsync(map);

            return rowsAffected;
        }

        private async Task<int> UpdateInternalAsync(HealDoneGeneralDto item)
        {
            var map = _mapper.Map<HealDoneGeneral>(item);
            var rowsAffected = await _repository.UpdateAsync(map);

            return rowsAffected;
        }
    }
}
