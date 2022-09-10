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
    internal class CombatLogByUserService : IService<CombatLogByUserDto, int>
    {
        private readonly IGenericRepository<CombatLogByUser> _repository;
        private readonly IMapper _mapper;

        public CombatLogByUserService(IGenericRepository<CombatLogByUser> userRepository, IMapper mapper)
        {
            _repository = userRepository;
            _mapper = mapper;
        }

        Task<int> IService<CombatLogByUserDto, int>.CreateAsync(CombatLogByUserDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return CreateInternalAsync(item);
        }

        Task<int> IService<CombatLogByUserDto, int>.DeleteAsync(CombatLogByUserDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return DeleteInternalAsync(item);
        }

        async Task<IEnumerable<CombatLogByUserDto>> IService<CombatLogByUserDto, int>.GetAllAsync()
        {
            var allData = await _repository.GetAllAsync();
            var result = _mapper.Map<List<CombatLogByUserDto>>(allData);

            return result;
        }

        async Task<CombatLogByUserDto> IService<CombatLogByUserDto, int>.GetByIdAsync(int id)
        {
            var executeLoad = await _repository.GetByIdAsync(id);
            var result = _mapper.Map<CombatLogByUserDto>(executeLoad);

            return result;
        }

        async Task<IEnumerable<CombatLogByUserDto>> IService<CombatLogByUserDto, int>.GetByParamAsync(string paramName, object value)
        {
            var executeLoad = await Task.Run(() => _repository.GetByParam(paramName, value));
            var result = _mapper.Map<IEnumerable<CombatLogByUserDto>>(executeLoad);

            return result;
        }

        Task<int> IService<CombatLogByUserDto, int>.UpdateAsync(CombatLogByUserDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return UpdateInternalAsync(item);
        }

        private async Task<int> CreateInternalAsync(CombatLogByUserDto item)
        {
            var map = _mapper.Map<CombatLogByUser>(item);
            var createdCombatId = await _repository.CreateAsync(map);

            return createdCombatId;
        }

        private async Task<int> DeleteInternalAsync(CombatLogByUserDto item)
        {
            var allData = await _repository.GetAllAsync();
            if (!allData.Any())
            {
                throw new NotFoundException($"Collection entity {nameof(CombatLogByUserDto)} not found", nameof(allData));
            }

            var numberEntries = await _repository.DeleteAsync(_mapper.Map<CombatLogByUser>(item));
            return numberEntries;
        }

        private async Task<int> UpdateInternalAsync(CombatLogByUserDto item)
        {
            var allData = await _repository.GetAllAsync();
            if (!allData.Any())
            {
                throw new NotFoundException($"Collection entity {nameof(CombatLogByUserDto)} not found", nameof(allData));
            }

            var numberEntries = await _repository.UpdateAsync(_mapper.Map<CombatLogByUser>(item));
            return numberEntries;
        }
    }
}
