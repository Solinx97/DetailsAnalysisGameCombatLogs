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

        Task<int> IService<DamageTakenGeneralDto, int>.CreateAsync(DamageTakenGeneralDto item)
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
            var executeLoad = await _repository.GetByIdAsync(id);
            var result = _mapper.Map<DamageTakenGeneralDto>(executeLoad);

            return result;
        }

        async Task<IEnumerable<DamageTakenGeneralDto>> IService<DamageTakenGeneralDto, int>.GetByParamAsync(string paramName, object value)
        {
            var executeLoad = await Task.Run(() => _repository.GetByParam(paramName, value));
            var result = _mapper.Map<IEnumerable<DamageTakenGeneralDto>>(executeLoad);

            return result;
        }

        Task<int> IService<DamageTakenGeneralDto, int>.UpdateAsync(DamageTakenGeneralDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return UpdateInternalAsync(item);
        }

        private async Task<int> CreateInternalAsync(DamageTakenGeneralDto item)
        {
            var map = _mapper.Map<DamageTakenGeneral>(item);
            var createdCombatId = await _repository.CreateAsync(map);

            return createdCombatId;
        }

        private async Task<int> DeleteInternalAsync(DamageTakenGeneralDto item)
        {
            var allData = await _repository.GetAllAsync();
            if (!allData.Any())
            {
                throw new NotFoundException($"Collection entity {nameof(DamageTakenGeneralDto)} not found", nameof(allData));
            }

            var numberEntries = await _repository.DeleteAsync(_mapper.Map<DamageTakenGeneral>(item));
            return numberEntries;
        }

        private async Task<int> UpdateInternalAsync(DamageTakenGeneralDto item)
        {
            var allData = await _repository.GetAllAsync();
            if (!allData.Any())
            {
                throw new NotFoundException($"Collection entity {nameof(DamageTakenGeneralDto)} not found", nameof(allData));
            }

            var numberEntries = await _repository.UpdateAsync(_mapper.Map<DamageTakenGeneral>(item));
            return numberEntries;
        }
    }
}
