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
    internal class CombatPlayerService : ISPService<CombatPlayerDataDto, int>
    {
        private readonly ISPGenericRepository<CombatPlayerData> _repository;
        private readonly IMapper _mapper;

        public CombatPlayerService(ISPGenericRepository<CombatPlayerData> userRepository, IMapper mapper)
        {
            _repository = userRepository;
            _mapper = mapper;
        }

        Task<int> IService<CombatPlayerDataDto, int>.CreateAsync(CombatPlayerDataDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return CreateInternalAsync(item);
        }

        Task<int> IService<CombatPlayerDataDto, int>.DeleteAsync(CombatPlayerDataDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return DeleteInternalAsync(item);
        }

        async Task<IEnumerable<CombatPlayerDataDto>> IService<CombatPlayerDataDto, int>.GetAllAsync()
        {
            var allData = await _repository.GetAllAsync();
            var result = _mapper.Map<List<CombatPlayerDataDto>>(allData);

            return result;
        }

        async Task<IEnumerable<CombatPlayerDataDto>> ISPService<CombatPlayerDataDto, int>.GetByProcedureAsync(int combatId)
        {
            var paramNames = new string[] { nameof(combatId) };
            var paramValues = new object[] { combatId };

            var data = await _repository.ExecuteStoredProcedureUseModelAsync(DbProcedureHelper.GetCombatPlayer, paramNames, paramValues);
            var result = _mapper.Map<IEnumerable<CombatPlayerDataDto>>(data);

            return result;
        }

        async Task<CombatPlayerDataDto> IService<CombatPlayerDataDto, int>.GetByIdAsync(int id)
        {
            var executeLoad = await _repository.GetByIdAsync(id);
            var result = _mapper.Map<CombatPlayerDataDto>(executeLoad);

            return result;
        }

        Task<int> IService<CombatPlayerDataDto, int>.UpdateAsync(CombatPlayerDataDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return UpdateInternalAsync(item);
        }

        private async Task<int> CreateInternalAsync(CombatPlayerDataDto item)
        {
            if (string.IsNullOrEmpty(item.UserName))
            {
                throw new ArgumentNullException(nameof(item.UserName));
            }

            var map = _mapper.Map<CombatPlayerData>(item);
            var createdCombatId = await _repository.CreateAsync(map);

            return createdCombatId;
        }

        private async Task<int> DeleteInternalAsync(CombatPlayerDataDto item)
        {
            var allData = await _repository.GetAllAsync();
            if (!allData.Any())
            {
                throw new NotFoundException($"Collection entity {nameof(CombatPlayerDataDto)} not found", nameof(allData));
            }

            var numberEntries = await _repository.DeleteAsync(_mapper.Map<CombatPlayerData>(item));
            return numberEntries;
        }

        private async Task<int> UpdateInternalAsync(CombatPlayerDataDto item)
        {
            var allData = await _repository.GetAllAsync();
            if (!allData.Any())
            {
                throw new NotFoundException($"Collection entity {nameof(CombatPlayerDataDto)} not found", nameof(allData));
            }

            if (string.IsNullOrEmpty(item.UserName))
            {
                throw new ArgumentNullException(nameof(item.UserName));
            }

            var numberEntries = await _repository.UpdateAsync(_mapper.Map<CombatPlayerData>(item));
            return numberEntries;
        }

        public Task<int> DeleteByProcedureAsync(int combatPlayerId)
        {
            throw new NotImplementedException();
        }

        public Task<int> CreateByProcedureAsync(CombatPlayerDataDto item)
        {
            throw new NotImplementedException();
        }
    }
}
