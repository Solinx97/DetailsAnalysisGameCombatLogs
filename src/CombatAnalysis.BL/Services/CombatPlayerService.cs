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
    internal class CombatPlayerService : ISPService<CombatPlayerDto, int>, IService<CombatPlayerDto, int>
    {
        private readonly ISPGenericRepository<CombatPlayer> _spRepository;
        private readonly IGenericRepository<CombatPlayer> _repository;
        private readonly IMapper _mapper;

        public CombatPlayerService(ISPGenericRepository<CombatPlayer> spRepository, IGenericRepository<CombatPlayer> repository, IMapper mapper)
        {
            _spRepository = spRepository;
            _repository = repository;
            _mapper = mapper;
        }

        Task<int> IService<CombatPlayerDto, int>.CreateAsync(CombatPlayerDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return CreateInternalAsync(item);
        }

        async Task<int> ISPService<CombatPlayerDto, int>.CreateByProcedureAsync(CombatPlayerDto item)
        {
            var paramNames = new string[] { nameof(item.UserName), nameof(item.DamageDone), nameof(item.HealDone),
                nameof(item.DamageTaken), nameof(item.EnergyRecovery), nameof(item.UsedBuffs), nameof(item.CombatId) };
            var paramValues = new object[] { item.UserName, item.DamageDone, item.HealDone,
                item.DamageTaken, item.EnergyRecovery, item.UsedBuffs, item.CombatId };

            var response = await _spRepository.ExecuteStoredProcedureAsync(DbProcedureHelper.InsertIntoCombatPlayer, paramNames, paramValues);
            return response;
        }

        Task<int> IService<CombatPlayerDto, int>.DeleteAsync(CombatPlayerDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return DeleteInternalAsync(item);
        }

        async Task<int> ISPService<CombatPlayerDto, int>.DeleteByProcedureAsync(int combatId)
        {
            var paramNames = new string[] { nameof(combatId) };
            var paramValues = new object[] { combatId };

            var response = await _spRepository.ExecuteStoredProcedureAsync(DbProcedureHelper.DeleteCombatPlayer, paramNames, paramValues);
            return response;
        }

        async Task<IEnumerable<CombatPlayerDto>> IService<CombatPlayerDto, int>.GetAllAsync()
        {
            var allData = await _repository.GetAllAsync();
            var result = _mapper.Map<List<CombatPlayerDto>>(allData);

            return result;
        }

        async Task<IEnumerable<CombatPlayerDto>> ISPService<CombatPlayerDto, int>.GetByProcedureAsync(int combatId)
        {
            var paramNames = new string[] { nameof(combatId) };
            var paramValues = new object[] { combatId };

            var data = await _spRepository.ExecuteStoredProcedureUseModelAsync(DbProcedureHelper.GetCombatPlayer, paramNames, paramValues);
            var result = _mapper.Map<IEnumerable<CombatPlayerDto>>(data);

            return result;
        }

        async Task<CombatPlayerDto> IService<CombatPlayerDto, int>.GetByIdAsync(int id)
        {
            var executeLoad = await _repository.GetByIdAsync(id);
            var result = _mapper.Map<CombatPlayerDto>(executeLoad);

            return result;
        }

        Task<int> IService<CombatPlayerDto, int>.UpdateAsync(CombatPlayerDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return UpdateInternalAsync(item);
        }

        private async Task<int> CreateInternalAsync(CombatPlayerDto item)
        {
            if (string.IsNullOrEmpty(item.UserName))
            {
                throw new ArgumentNullException(nameof(item.UserName));
            }

            var map = _mapper.Map<CombatPlayer>(item);
            var createdCombatId = await _repository.CreateAsync(map);

            return createdCombatId;
        }

        private async Task<int> DeleteInternalAsync(CombatPlayerDto item)
        {
            var allData = await _repository.GetAllAsync();
            if (!allData.Any())
            {
                throw new NotFoundException($"Collection entity {nameof(CombatPlayerDto)} not found", nameof(allData));
            }

            var numberEntries = await _repository.DeleteAsync(_mapper.Map<CombatPlayer>(item));
            return numberEntries;
        }

        private async Task<int> UpdateInternalAsync(CombatPlayerDto item)
        {
            var allData = await _repository.GetAllAsync();
            if (!allData.Any())
            {
                throw new NotFoundException($"Collection entity {nameof(CombatPlayerDto)} not found", nameof(allData));
            }

            if (string.IsNullOrEmpty(item.UserName))
            {
                throw new ArgumentNullException(nameof(item.UserName));
            }

            var numberEntries = await _repository.UpdateAsync(_mapper.Map<CombatPlayer>(item));
            return numberEntries;
        }
    }
}
