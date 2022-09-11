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
    internal class DamageDoneGeneralService : IService<DamageDoneGeneralDto, int>
    {
        private readonly IGenericRepository<DamageDoneGeneral, int> _repository;
        private readonly IMapper _mapper;

        public DamageDoneGeneralService(IGenericRepository<DamageDoneGeneral, int> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        Task<int> IService<DamageDoneGeneralDto, int>.CreateAsync(DamageDoneGeneralDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return CreateInternalAsync(item);
        }

        Task<int> IService<DamageDoneGeneralDto, int>.DeleteAsync(DamageDoneGeneralDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return DeleteInternalAsync(item);
        }

        async Task<IEnumerable<DamageDoneGeneralDto>> IService<DamageDoneGeneralDto, int>.GetAllAsync()
        {
            var allData = await _repository.GetAllAsync();
            var result = _mapper.Map<List<DamageDoneGeneralDto>>(allData);

            return result;
        }

        async Task<DamageDoneGeneralDto> IService<DamageDoneGeneralDto, int>.GetByIdAsync(int id)
        {
            var executeLoad = await _repository.GetByIdAsync(id);
            var result = _mapper.Map<DamageDoneGeneralDto>(executeLoad);

            return result;
        }

        async Task<IEnumerable<DamageDoneGeneralDto>> IService<DamageDoneGeneralDto, int>.GetByParamAsync(string paramName, object value)
        {
            var executeLoad = await Task.Run(() => _repository.GetByParam(paramName, value));
            var result = _mapper.Map<IEnumerable<DamageDoneGeneralDto>>(executeLoad);

            return result;
        }

        Task<int> IService<DamageDoneGeneralDto, int>.UpdateAsync(DamageDoneGeneralDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return UpdateInternalAsync(item);
        }

        private async Task<int> CreateInternalAsync(DamageDoneGeneralDto item)
        {
            var map = _mapper.Map<DamageDoneGeneral>(item);
            var createdCombatId = await _repository.CreateAsync(map);

            return createdCombatId;
        }

        private async Task<int> DeleteInternalAsync(DamageDoneGeneralDto item)
        {
            var allData = await _repository.GetAllAsync();
            if (!allData.Any())
            {
                throw new NotFoundException($"Collection entity {nameof(DamageDoneGeneralDto)} not found", nameof(allData));
            }

            var numberEntries = await _repository.DeleteAsync(_mapper.Map<DamageDoneGeneral>(item));
            return numberEntries;
        }

        private async Task<int> UpdateInternalAsync(DamageDoneGeneralDto item)
        {
            var allData = await _repository.GetAllAsync();
            if (!allData.Any())
            {
                throw new NotFoundException($"Collection entity {nameof(DamageDoneGeneralDto)} not found", nameof(allData));
            }

            var numberEntries = await _repository.UpdateAsync(_mapper.Map<DamageDoneGeneral>(item));
            return numberEntries;
        }
    }
}
