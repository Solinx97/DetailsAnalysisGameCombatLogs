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

        Task<int> IService<HealDoneDto, int>.CreateAsync(HealDoneDto item)
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
            var executeLoad = await _repository.GetByIdAsync(id);
            var result = _mapper.Map<HealDoneDto>(executeLoad);

            return result;
        }

        async Task<IEnumerable<HealDoneDto>> IService<HealDoneDto, int>.GetByParamAsync(string paramName, object value)
        {
            var executeLoad = await Task.Run(() => _repository.GetByParam(paramName, value));
            var result = _mapper.Map<IEnumerable<HealDoneDto>>(executeLoad);

            return result;
        }

        Task<int> IService<HealDoneDto, int>.UpdateAsync(HealDoneDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return UpdateInternalAsync(item);
        }

        private async Task<int> CreateInternalAsync(HealDoneDto item)
        {
            var map = _mapper.Map<HealDone>(item);
            var createdCombatId = await _repository.CreateAsync(map);

            return createdCombatId;
        }

        private async Task<int> DeleteInternalAsync(HealDoneDto item)
        {
            var allData = await _repository.GetAllAsync();
            if (!allData.Any())
            {
                throw new NotFoundException($"Collection entity {nameof(HealDoneDto)} not found", nameof(allData));
            }

            var numberEntries = await _repository.DeleteAsync(_mapper.Map<HealDone>(item));
            return numberEntries;
        }

        private async Task<int> UpdateInternalAsync(HealDoneDto item)
        {
            var allData = await _repository.GetAllAsync();
            if (!allData.Any())
            {
                throw new NotFoundException($"Collection entity {nameof(HealDoneDto)} not found", nameof(allData));
            }

            var numberEntries = await _repository.UpdateAsync(_mapper.Map<HealDone>(item));
            return numberEntries;
        }
    }
}
