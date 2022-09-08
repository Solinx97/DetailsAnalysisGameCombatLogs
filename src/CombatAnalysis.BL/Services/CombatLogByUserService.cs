using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Exceptions;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Helpers;
using CombatAnalysis.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CombatAnalysis.BL.Services
{
    internal class CombatLogByUserService : IService<CombatLogByUserDto>
    {
        private readonly IGenericRepository<CombatLogByUser> _repository;
        private readonly IMapper _mapper;

        public CombatLogByUserService(IGenericRepository<CombatLogByUser> userRepository, IMapper mapper)
        {
            _repository = userRepository;
            _mapper = mapper;
        }

        Task<int> IService<CombatLogByUserDto>.CreateAsync(CombatLogByUserDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return CreateInternalAsync(item);
        }

        Task<int> IService<CombatLogByUserDto>.DeleteAsync(CombatLogByUserDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return DeleteInternalAsync(item);
        }

        async Task<IEnumerable<CombatLogByUserDto>> IService<CombatLogByUserDto>.GetAllAsync()
        {
            var allData = await _repository.GetAllAsync();
            var result = _mapper.Map<List<CombatLogByUserDto>>(allData);

            return result;
        }

        async Task<IEnumerable<CombatLogByUserDto>> IService<CombatLogByUserDto>.GetByProcedureAsync(int combatLogId)
        {
            var paramNames = new string[] { nameof(combatLogId) };
            var paramValues = new object[] { combatLogId };

            var data = await _repository.ExecuteStoredProcedureUseModelAsync(DbProcedureHelper.GetCombat, paramNames, paramValues);
            var result = _mapper.Map<IEnumerable<CombatLogByUserDto>>(data);

            return result;
        }

        async Task<CombatLogByUserDto> IService<CombatLogByUserDto>.GetByIdAsync(int id)
        {
            var executeLoad = await _repository.GetByIdAsync(id);
            var result = _mapper.Map<CombatLogByUserDto>(executeLoad);

            return result;
        }

        Task<int> IService<CombatLogByUserDto>.UpdateAsync(CombatLogByUserDto item)
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

        Task<int> IService<CombatLogByUserDto>.DeleteByProcedureAsync(int combatPlayerId)
        {
            throw new NotImplementedException();
        }

        Task<int> IService<CombatLogByUserDto>.CreateByProcedureAsync(CombatLogByUserDto item)
        {
            throw new NotImplementedException();
        }
    }
}
