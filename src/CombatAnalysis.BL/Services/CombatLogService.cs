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
    internal class CombatLogService : IService<CombatLogDto>
    {
        private readonly IGenericRepository<CombatLog> _repository;
        private readonly IMapper _mapper;

        public CombatLogService(IGenericRepository<CombatLog> userRepository, IMapper mapper)
        {
            _repository = userRepository;
            _mapper = mapper;
        }

        Task<int> IService<CombatLogDto>.CreateAsync(CombatLogDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return CreateInternalAsync(item);
        }

        Task<int> IService<CombatLogDto>.DeleteAsync(CombatLogDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return DeleteInternalAsync(item);
        }

        async Task<IEnumerable<CombatLogDto>> IService<CombatLogDto>.GetAllAsync()
        {
            var allData = await _repository.GetAllAsync();
            var result = _mapper.Map<List<CombatLogDto>>(allData);

            return result;
        }

        async Task<IEnumerable<CombatLogDto>> IService<CombatLogDto>.GetByProcedureAsync(int combatLogId)
        {
            var paramNames = new string[] { nameof(combatLogId) };
            var paramValues = new object[] { combatLogId };

            var data = await _repository.ExecuteStoredProcedureUseModelAsync(DbProcedureHelper.GetCombat, paramNames, paramValues);
            var result = _mapper.Map<IEnumerable<CombatLogDto>>(data);

            return result;
        }

        async Task<CombatLogDto> IService<CombatLogDto>.GetByIdAsync(int id)
        {
            var executeLoad = await _repository.GetByIdAsync(id);
            var result = _mapper.Map<CombatLogDto>(executeLoad);

            return result;
        }

        Task<int> IService<CombatLogDto>.UpdateAsync(CombatLogDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return UpdateInternalAsync(item);
        }

        private async Task<int> CreateInternalAsync(CombatLogDto item)
        {
            if (string.IsNullOrEmpty(item.Name))
            {
                throw new ArgumentNullException(nameof(item.Name));
            }

            var map = _mapper.Map<CombatLog>(item);
            var createdCombatId = await _repository.CreateAsync(map);

            return createdCombatId;
        }

        private async Task<int> DeleteInternalAsync(CombatLogDto item)
        {
            var allData = await _repository.GetAllAsync();
            if (!allData.Any())
            {
                throw new NotFoundException($"Collection entity {nameof(CombatLogDto)} not found", nameof(allData));
            }

            var numberEntries = await _repository.DeleteAsync(_mapper.Map<CombatLog>(item));
            return numberEntries;
        }

        private async Task<int> UpdateInternalAsync(CombatLogDto item)
        {
            var allData = await _repository.GetAllAsync();
            if (!allData.Any())
            {
                throw new NotFoundException($"Collection entity {nameof(CombatLogDto)} not found", nameof(allData));
            }

            if (string.IsNullOrEmpty(item.Name))
            {
                throw new ArgumentNullException(nameof(item.Name));
            }

            var numberEntries = await _repository.UpdateAsync(_mapper.Map<CombatLog>(item));
            return numberEntries;
        }

        Task<int> IService<CombatLogDto>.DeleteByProcedureAsync(int combatPlayerId)
        {
            throw new NotImplementedException();
        }

        Task<int> IService<CombatLogDto>.CreateByProcedureAsync(CombatLogDto item)
        {
            throw new NotImplementedException();
        }
    }
}
