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
    internal class CombatService : ISPService<CombatDto, int>, IService<CombatDto, int>
    {
        private readonly ISPGenericRepository<Combat> _spRepository;
        private readonly IGenericRepository<Combat> _repository;
        private readonly IMapper _mapper;

        public CombatService(ISPGenericRepository<Combat> spRepository, IGenericRepository<Combat> repository, IMapper mapper)
        {
            _spRepository = spRepository;
            _repository = repository;
            _mapper = mapper;
        }

        Task<int> IService<CombatDto, int>.CreateAsync(CombatDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return CreateInternalAsync(item);
        }

        async Task<int> ISPService<CombatDto, int>.CreateByProcedureAsync(CombatDto item)
        {
            var paramNames = new string[] { nameof(item.DungeonName), nameof(item.Name), nameof(item.DamageDone),
                nameof(item.HealDone), nameof(item.DamageTaken), nameof(item.EnergyRecovery), nameof(item.DeathNumber),  nameof(item.UsedBuffs),
                nameof(item.IsWin), nameof(item.StartDate), nameof(item.FinishDate), nameof(item.CombatLogId) };
            var paramValues = new object[] { item.DungeonName, item.Name, item.DamageDone,
                item.HealDone, item.DamageTaken, item.EnergyRecovery, item.DeathNumber, item.UsedBuffs,
                item.IsWin, item.StartDate, item.FinishDate, item.CombatLogId };

            var response = await _spRepository.ExecuteStoredProcedureAsync(DbProcedureHelper.InsertIntoCombat, paramNames, paramValues);
            return response;
        }

        Task<int> IService<CombatDto, int>.DeleteAsync(CombatDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return DeleteInternalAsync(item);
        }

        async Task<int> ISPService<CombatDto, int>.DeleteByProcedureAsync(int combatLogId)
        {
            var paramNames = new string[] { nameof(combatLogId) };
            var paramValues = new object[] { combatLogId };

            var response = await _spRepository.ExecuteStoredProcedureAsync(DbProcedureHelper.DeleteCombat, paramNames, paramValues);
            return response;
        }

        async Task<IEnumerable<CombatDto>> IService<CombatDto, int>.GetAllAsync()
        {
            var allData = await _repository.GetAllAsync();
            var result = _mapper.Map<List<CombatDto>>(allData);

            return result;
        }

        async Task<CombatDto> IService<CombatDto, int>.GetByIdAsync(int id)
        {
            var executeLoad = await _repository.GetByIdAsync(id);
            var result = _mapper.Map<CombatDto>(executeLoad);

            return result;
        }

        Task<int> IService<CombatDto, int>.UpdateAsync(CombatDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return UpdateInternalAsync(item);
        }

        private async Task<int> CreateInternalAsync(CombatDto item)
        {
            if (string.IsNullOrEmpty(item.Name))
            {
                throw new ArgumentNullException(nameof(item.Name));
            }

            var map = _mapper.Map<Combat>(item);
            var createdCombatId = await _repository.CreateAsync(map);

            return createdCombatId;
        }

        private async Task<int> DeleteInternalAsync(CombatDto item)
        {
            var allData = await _repository.GetAllAsync();
            if (!allData.Any())
            {
                throw new NotFoundException($"Collection entity {nameof(CombatDto)} not found", nameof(allData));
            }

            var numberEntries = await _repository.DeleteAsync(_mapper.Map<Combat>(item));
            return numberEntries;
        }

        private async Task<int> UpdateInternalAsync(CombatDto item)
        {
            var allData = await _repository.GetAllAsync();
            if (!allData.Any())
            {
                throw new NotFoundException($"Collection entity {nameof(CombatDto)} not found", nameof(allData));
            }

            if (string.IsNullOrEmpty(item.Name))
            {
                throw new ArgumentNullException(nameof(item.Name));
            }

            var numberEntries = await _repository.UpdateAsync(_mapper.Map<Combat>(item));
            return numberEntries;
        }

        async Task<IEnumerable<CombatDto>> ISPService<CombatDto, int>.GetByProcedureAsync(int combatLogId)
        {
            var paramNames = new string[] { nameof(combatLogId) };
            var paramValues = new object[] { combatLogId };

            var data = await _spRepository.ExecuteStoredProcedureUseModelAsync("GetCombatByCombatLogId", paramNames, paramValues);
            var result = _mapper.Map<IEnumerable<CombatDto>>(data);

            return result;
        }
    }
}
