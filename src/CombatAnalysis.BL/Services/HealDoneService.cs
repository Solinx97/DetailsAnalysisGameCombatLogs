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
    internal class HealDoneService : IService<HealDoneDto, int>
    {
        private readonly IGenericRepository<HealDone, int> _repository;
        private readonly IMapper _mapper;

        public HealDoneService(IGenericRepository<HealDone, int> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        Task<HealDoneDto> IService<HealDoneDto, int>.CreateAsync(HealDoneDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return CreateInternalAsync(item);
        }

        Task<int> IService<HealDoneDto, int>.DeleteAsync(HealDoneDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return DeleteInternalAsync(item);
        }

        async Task<IEnumerable<HealDoneDto>> IService<HealDoneDto, int>.GetAllAsync()
        {
            var allData = await _repository.GetAllAsync();
            var result = _mapper.Map<List<HealDoneDto>>(allData);

            return result;
        }

        async Task<HealDoneDto> IService<HealDoneDto, int>.GetByIdAsync(int id)
        {
            var result = await _repository.GetByIdAsync(id);
            var resultMap = _mapper.Map<HealDoneDto>(result);

            return resultMap;
        }

        async Task<IEnumerable<HealDoneDto>> IService<HealDoneDto, int>.GetByParamAsync(string paramName, object value)
        {
            var result = await Task.Run(() => _repository.GetByParam(paramName, value));
            var resultMap = _mapper.Map<IEnumerable<HealDoneDto>>(result);

            return resultMap;
        }

        Task<int> IService<HealDoneDto, int>.UpdateAsync(HealDoneDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return UpdateInternalAsync(item);
        }

        private async Task<HealDoneDto> CreateInternalAsync(HealDoneDto item)
        {
            var map = _mapper.Map<HealDone>(item);
            var createdItem = await _repository.CreateAsync(map);
            var resultMap = _mapper.Map<HealDoneDto>(createdItem);

            return resultMap;
        }

        private async Task<int> DeleteInternalAsync(HealDoneDto item)
        {
            var allData = await _repository.GetAllAsync();
            if (!allData.Any())
            {
                throw new NotFoundException($"Collection entity {nameof(HealDoneDto)} not found", nameof(allData));
            }

            var numberEntriesAffected = await _repository.DeleteAsync(_mapper.Map<HealDone>(item));
            return numberEntriesAffected;
        }

        private async Task<int> UpdateInternalAsync(HealDoneDto item)
        {
            var allData = await _repository.GetAllAsync();
            if (!allData.Any())
            {
                throw new NotFoundException($"Collection entity {nameof(HealDoneDto)} not found", nameof(allData));
            }

            var numberEntriesAffected = await _repository.UpdateAsync(_mapper.Map<HealDone>(item));
            return numberEntriesAffected;
        }
    }
}
