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
    internal class HealDoneGeneralService : IService<HealDoneGeneralDto, int>
    {
        private readonly IGenericRepository<HealDoneGeneral, int> _repository;
        private readonly IMapper _mapper;

        public HealDoneGeneralService(IGenericRepository<HealDoneGeneral, int> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        Task<int> IService<HealDoneGeneralDto, int>.CreateAsync(HealDoneGeneralDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return CreateInternalAsync(item);
        }

        Task<int> IService<HealDoneGeneralDto, int>.DeleteAsync(HealDoneGeneralDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
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
            var executeLoad = await _repository.GetByIdAsync(id);
            var result = _mapper.Map<HealDoneGeneralDto>(executeLoad);

            return result;
        }

        async Task<IEnumerable<HealDoneGeneralDto>> IService<HealDoneGeneralDto, int>.GetByParamAsync(string paramName, object value)
        {
            var executeLoad = await Task.Run(() => _repository.GetByParam(paramName, value));
            var result = _mapper.Map<IEnumerable<HealDoneGeneralDto>>(executeLoad);

            return result;
        }

        Task<int> IService<HealDoneGeneralDto, int>.UpdateAsync(HealDoneGeneralDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return UpdateInternalAsync(item);
        }

        private async Task<int> CreateInternalAsync(HealDoneGeneralDto item)
        {
            var map = _mapper.Map<HealDoneGeneral>(item);
            var createdCombatId = await _repository.CreateAsync(map);

            return createdCombatId;
        }

        private async Task<int> DeleteInternalAsync(HealDoneGeneralDto item)
        {
            var allData = await _repository.GetAllAsync();
            if (!allData.Any())
            {
                throw new NotFoundException($"Collection entity {nameof(HealDoneGeneralDto)} not found", nameof(allData));
            }

            var numberEntries = await _repository.DeleteAsync(_mapper.Map<HealDoneGeneral>(item));
            return numberEntries;
        }

        private async Task<int> UpdateInternalAsync(HealDoneGeneralDto item)
        {
            var allData = await _repository.GetAllAsync();
            if (!allData.Any())
            {
                throw new NotFoundException($"Collection entity {nameof(HealDoneGeneralDto)} not found", nameof(allData));
            }

            var numberEntries = await _repository.UpdateAsync(_mapper.Map<HealDoneGeneral>(item));
            return numberEntries;
        }
    }
}
