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
                throw new ArgumentNullException(nameof(item));
            }

            return CreateInternalAsync(item);
        }

        Task<int> IService<DamageTakenGeneralDto, int>.DeleteAsync(DamageTakenGeneralDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
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
                throw new ArgumentNullException(nameof(item));
            }

            return UpdateInternalAsync(item);
        }

        private async Task<DamageTakenGeneralDto> CreateInternalAsync(DamageTakenGeneralDto item)
        {
            var map = _mapper.Map<DamageTakenGeneral>(item);
            var createdItem = await _repository.CreateAsync(map);
            var resultMap = _mapper.Map<DamageTakenGeneralDto>(createdItem);

            return resultMap;
        }

        private async Task<int> DeleteInternalAsync(DamageTakenGeneralDto item)
        {
            var allData = await _repository.GetAllAsync();
            if (!allData.Any())
            {
                throw new NotFoundException($"Collection entity {nameof(DamageTakenGeneralDto)} not found", nameof(allData));
            }

            var numberEntriesAffected = await _repository.DeleteAsync(_mapper.Map<DamageTakenGeneral>(item));
            return numberEntriesAffected;
        }

        private async Task<int> UpdateInternalAsync(DamageTakenGeneralDto item)
        {
            var allData = await _repository.GetAllAsync();
            if (!allData.Any())
            {
                throw new NotFoundException($"Collection entity {nameof(DamageTakenGeneralDto)} not found", nameof(allData));
            }

            var numberEntriesAffected = await _repository.UpdateAsync(_mapper.Map<DamageTakenGeneral>(item));
            return numberEntriesAffected;
        }
    }
}
