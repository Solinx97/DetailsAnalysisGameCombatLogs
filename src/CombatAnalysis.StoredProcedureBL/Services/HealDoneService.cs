using AutoMapper;
using CombatAnalysis.StoredProcedureBL.DTO;
using CombatAnalysis.StoredProcedureBL.Exceptions;
using CombatAnalysis.StoredProcedureBL.Interfaces;
using CombatAnalysis.StoredProcedureDAL.Entities;
using CombatAnalysis.StoredProcedureDAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CombatAnalysis.StoredProcedureBL.Services
{
    internal class HealDoneService : IService<HealDoneDto, int>
    {
        private readonly IGenericRepository<HealDone, int> _repository;
        private readonly IMapper _mapper;

        public HealDoneService(IGenericRepository<HealDone, int> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        Task<int> IService<HealDoneDto, int>.CreateAsync(HealDoneDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return CreateInternalAsync(item);
        }

        Task<int> IService<HealDoneDto, int>.DeleteAsync(HealDoneDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return DeleteInternalAsync(item);
        }

        async Task<IEnumerable<HealDoneDto>> IService<HealDoneDto, int>.GetAllAsync()
        {
            var allData = await _repository.GetAllAsync();
            var result = _mapper.Map<List<HealDoneDto>>(allData);

            return result;
        }

        async Task<HealDoneDto> IService<HealDoneDto, int>.GetByIdAsync(int id)
        {
            var executeLoad = await _repository.GetByIdAsync(id);
            var result = _mapper.Map<HealDoneDto>(executeLoad);

            return result;
        }

        Task<int> IService<HealDoneDto, int>.UpdateAsync(HealDoneDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return UpdateInternalAsync(item);
        }

        private async Task<int> CreateInternalAsync(HealDoneDto item)
        {
            var paramNames = new string[] { nameof(item.ValueWithOverheal), nameof(item.Time), nameof(item.Overheal), nameof(item.Value),
                nameof(item.FromPlayer), nameof(item.ToPlayer), nameof(item.SpellOrItem),  nameof(item.CurrentHealth),
                nameof(item.MaxHealth), nameof(item.IsCrit), nameof(item.IsFullOverheal), nameof(item.CombatPlayerId) };
            var paramValues = new object[] { item.ValueWithOverheal, item.Time, item.Overheal, item.Value,
                item.FromPlayer, item.ToPlayer, item.SpellOrItem, item.CurrentHealth,
                item.MaxHealth, item.IsCrit, item.IsFullOverheal, item.CombatPlayerId };

            var createdCombatId = await _repository.CreateAsync(paramNames, paramValues);

            return createdCombatId;
        }

        private async Task<int> DeleteInternalAsync(HealDoneDto item)
        {
            var allData = await _repository.GetAllAsync();
            if (!allData.Any())
            {
                throw new NotFoundException($"Collection entity {nameof(HealDoneDto)} not found", nameof(allData));
            }

            var numberEntries = await _repository.DeleteAsync(item.Id);
            return numberEntries;
        }

        private async Task<int> UpdateInternalAsync(HealDoneDto item)
        {
            var allData = await _repository.GetAllAsync();
            if (!allData.Any())
            {
                throw new NotFoundException($"Collection entity {nameof(HealDoneDto)} not found", nameof(allData));
            }

            var paramNames = new string[] { nameof(item.ValueWithOverheal), nameof(item.Time), nameof(item.Overheal), nameof(item.Value),
                nameof(item.FromPlayer), nameof(item.ToPlayer), nameof(item.SpellOrItem),  nameof(item.CurrentHealth),
                nameof(item.MaxHealth), nameof(item.IsCrit), nameof(item.IsFullOverheal), nameof(item.CombatPlayerId) };
            var paramValues = new object[] { item.ValueWithOverheal, item.Time, item.Overheal, item.Value,
                item.FromPlayer, item.ToPlayer, item.SpellOrItem, item.CurrentHealth,
                item.MaxHealth, item.IsCrit, item.IsFullOverheal, item.CombatPlayerId };

            var numberEntries = await _repository.UpdateAsync(paramNames, paramValues);
            return numberEntries;
        }
    }
}
