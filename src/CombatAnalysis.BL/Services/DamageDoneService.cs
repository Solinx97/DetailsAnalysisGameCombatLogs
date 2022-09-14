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
    internal class DamageDoneService : IService<DamageDoneDto, int>
    {
        private readonly IGenericRepository<DamageDone, int> _repository;
        private readonly IMapper _mapper;

        public DamageDoneService(IGenericRepository<DamageDone, int> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        Task<DamageDoneDto> IService<DamageDoneDto, int>.CreateAsync(DamageDoneDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return CreateInternalAsync(item);
        }

        Task<int> IService<DamageDoneDto, int>.DeleteAsync(DamageDoneDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return DeleteInternalAsync(item);
        }

        async Task<IEnumerable<DamageDoneDto>> IService<DamageDoneDto, int>.GetAllAsync()
        {
            var allData = await _repository.GetAllAsync();
            var result = _mapper.Map<List<DamageDoneDto>>(allData);

            return result;
        }

        async Task<DamageDoneDto> IService<DamageDoneDto, int>.GetByIdAsync(int id)
        {
            var result = await _repository.GetByIdAsync(id);
            var resultMap = _mapper.Map<DamageDoneDto>(result);

            return resultMap;
        }

        async Task<IEnumerable<DamageDoneDto>> IService<DamageDoneDto, int>.GetByParamAsync(string paramName, object value)
        {
            var result = await Task.Run(() => _repository.GetByParam(paramName, value));
            var resultMap = _mapper.Map<IEnumerable<DamageDoneDto>>(result);

            return resultMap;
        }

        Task<int> IService<DamageDoneDto, int>.UpdateAsync(DamageDoneDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return UpdateInternalAsync(item);
        }

        private async Task<DamageDoneDto> CreateInternalAsync(DamageDoneDto item)
        {
            var map = _mapper.Map<DamageDone>(item);
            var createdItem = await _repository.CreateAsync(map);
            var resultMap = _mapper.Map<DamageDoneDto>(createdItem);

            return resultMap;
        }

        private async Task<int> DeleteInternalAsync(DamageDoneDto item)
        {
            var allData = await _repository.GetAllAsync();
            if (!allData.Any())
            {
                throw new NotFoundException($"Collection entity {nameof(DamageDoneDto)} not found", nameof(allData));
            }

            var numberEntriesAffected = await _repository.DeleteAsync(_mapper.Map<DamageDone>(item));
            return numberEntriesAffected;
        }

        private async Task<int> UpdateInternalAsync(DamageDoneDto item)
        {
            var allData = await _repository.GetAllAsync();
            if (!allData.Any())
            {
                throw new NotFoundException($"Collection entity {nameof(DamageDoneDto)} not found", nameof(allData));
            }

            var numberEntriesAffected = await _repository.UpdateAsync(_mapper.Map<DamageDone>(item));
            return numberEntriesAffected;
        }
    }
}
