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
    internal class HealDoneService : IService<HealDoneDto>
    {
        private readonly IGenericRepository<HealDone> _repository;
        private readonly IMapper _mapper;

        public HealDoneService(IGenericRepository<HealDone> userRepository, IMapper mapper)
        {
            _repository = userRepository;
            _mapper = mapper;
        }

        Task<int> IService<HealDoneDto>.CreateAsync(HealDoneDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return CreateInternalAsync(item);
        }

        async Task<int> IService<HealDoneDto>.CreateByProcedureAsync(HealDoneDto item)
        {
            var paramNames = new string[] { nameof(item.ValueWithOverheal), nameof(item.Time), nameof(item.Overheal), nameof(item.Value),
                nameof(item.FromPlayer), nameof(item.ToPlayer), nameof(item.SpellOrItem),  nameof(item.CurrentHealth),
                nameof(item.MaxHealth), nameof(item.IsCrit), nameof(item.IsFullOverheal), nameof(item.CombatPlayerDataId) };
            var paramValues = new object[] { item.ValueWithOverheal, item.Time, item.Overheal, item.Value,
                item.FromPlayer, item.ToPlayer, item.SpellOrItem, item.CurrentHealth,
                item.MaxHealth, item.IsCrit, item.IsFullOverheal, item.CombatPlayerDataId };

            var response = await _repository.ExecuteStoredProcedureAsync(DbProcedureHelper.InsertIntoHealDone, paramNames, paramValues);
            return response;
        }

        Task<int> IService<HealDoneDto>.DeleteAsync(HealDoneDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return DeleteInternalAsync(item);
        }

        async Task<int> IService<HealDoneDto>.DeleteByProcedureAsync(int combatPlayerId)
        {
            var paramNames = new string[] { nameof(combatPlayerId) };
            var paramValues = new object[] { combatPlayerId };

            var response = await _repository.ExecuteStoredProcedureAsync(DbProcedureHelper.DeleteHealDone, paramNames, paramValues);
            return response;
        }

        async Task<IEnumerable<HealDoneDto>> IService<HealDoneDto>.GetAllAsync()
        {
            var allData = await _repository.GetAllAsync();
            var result = _mapper.Map<List<HealDoneDto>>(allData);

            return result;
        }

        async Task<IEnumerable<HealDoneDto>> IService<HealDoneDto>.GetByProcedureAsync(int combatPlayerId)
        {
            var paramNames = new string[] { nameof(combatPlayerId) };
            var paramValues = new object[] { combatPlayerId };

            var data = await _repository.ExecuteStoredProcedureUseModelAsync(DbProcedureHelper.GetHealDone, paramNames, paramValues);
            var result = _mapper.Map<IEnumerable<HealDoneDto>>(data);

            return result;
        }

        async Task<HealDoneDto> IService<HealDoneDto>.GetByIdAsync(int id)
        {
            var executeLoad = await _repository.GetByIdAsync(id);
            var result = _mapper.Map<HealDoneDto>(executeLoad);

            return result;
        }

        Task<int> IService<HealDoneDto>.UpdateAsync(HealDoneDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return UpdateInternalAsync(item);
        }

        private async Task<int> CreateInternalAsync(HealDoneDto item)
        {
            var map = _mapper.Map<HealDone>(item);
            var createdCombatId = await _repository.CreateAsync(map);

            return createdCombatId;
        }

        private async Task<int> DeleteInternalAsync(HealDoneDto item)
        {
            var allData = await _repository.GetAllAsync();
            if (!allData.Any())
            {
                throw new NotFoundException($"Collection entity {nameof(HealDoneDto)} not found", nameof(allData));
            }

            var numberEntries = await _repository.DeleteAsync(_mapper.Map<HealDone>(item));
            return numberEntries;
        }

        private async Task<int> UpdateInternalAsync(HealDoneDto item)
        {
            var allData = await _repository.GetAllAsync();
            if (!allData.Any())
            {
                throw new NotFoundException($"Collection entity {nameof(HealDoneDto)} not found", nameof(allData));
            }

            var numberEntries = await _repository.UpdateAsync(_mapper.Map<HealDone>(item));
            return numberEntries;
        }
    }
}
