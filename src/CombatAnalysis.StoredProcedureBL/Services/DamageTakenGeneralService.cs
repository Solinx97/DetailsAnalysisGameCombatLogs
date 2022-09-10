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
    internal class DamageTakenGeneralService : IService<DamageTakenGeneralDto, int>
    {
        private readonly IGenericRepository<DamageTakenGeneral, int> _repository;
        private readonly IMapper _mapper;

        public DamageTakenGeneralService(IGenericRepository<DamageTakenGeneral, int> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        Task<int> IService<DamageTakenGeneralDto, int>.CreateAsync(DamageTakenGeneralDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return CreateInternalAsync(item);
        }

        Task<int> IService<DamageTakenGeneralDto, int>.DeleteAsync(DamageTakenGeneralDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return DeleteInternalAsync(item);
        }

        async Task<IEnumerable<DamageTakenGeneralDto>> IService<DamageTakenGeneralDto, int>.GetAllAsync()
        {
            var allData = await _repository.GetAllAsync();
            var result = _mapper.Map<List<DamageTakenGeneralDto>>(allData);

            return result;
        }

        async Task<DamageTakenGeneralDto> IService<DamageTakenGeneralDto, int>.GetByIdAsync(int id)
        {
            var executeLoad = await _repository.GetByIdAsync(id);
            var result = _mapper.Map<DamageTakenGeneralDto>(executeLoad);

            return result;
        }

        Task<int> IService<DamageTakenGeneralDto, int>.UpdateAsync(DamageTakenGeneralDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return UpdateInternalAsync(item);
        }

        private async Task<int> CreateInternalAsync(DamageTakenGeneralDto item)
        {
            var paramNames = new string[] { nameof(item.Value), nameof(item.DamageTakenPerSecond), nameof(item.SpellOrItem),
                nameof(item.CritNumber), nameof(item.MissNumber), nameof(item.CastNumber), nameof(item.MinValue),
                nameof(item.MaxValue), nameof(item.AverageValue), nameof(item.CombatPlayerId) };
            var paramValues = new object[] { item.Value, item.DamageTakenPerSecond, item.SpellOrItem,
                item.CritNumber, item.MissNumber, item.CastNumber, item.MinValue,
                item.MaxValue, item.AverageValue, item.CombatPlayerId };

            var createdCombatId = await _repository.CreateAsync(paramNames, paramValues);

            return createdCombatId;
        }

        private async Task<int> DeleteInternalAsync(DamageTakenGeneralDto item)
        {
            var allData = await _repository.GetAllAsync();
            if (!allData.Any())
            {
                throw new NotFoundException($"Collection entity {nameof(DamageTakenGeneralDto)} not found", nameof(allData));
            }

            var numberEntries = await _repository.DeleteAsync(item.Id);
            return numberEntries;
        }

        private async Task<int> UpdateInternalAsync(DamageTakenGeneralDto item)
        {
            var allData = await _repository.GetAllAsync();
            if (!allData.Any())
            {
                throw new NotFoundException($"Collection entity {nameof(DamageTakenGeneralDto)} not found", nameof(allData));
            }

            var paramNames = new string[] { nameof(item.Value), nameof(item.DamageTakenPerSecond), nameof(item.SpellOrItem),
                nameof(item.CritNumber), nameof(item.MissNumber), nameof(item.CastNumber), nameof(item.MinValue),
                nameof(item.MaxValue), nameof(item.AverageValue), nameof(item.CombatPlayerId) };
            var paramValues = new object[] { item.Value, item.DamageTakenPerSecond, item.SpellOrItem,
                item.CritNumber, item.MissNumber, item.CastNumber, item.MinValue,
                item.MaxValue, item.AverageValue, item.CombatPlayerId };

            var numberEntries = await _repository.UpdateAsync(paramNames, paramValues);
            return numberEntries;
        }
    }
}
