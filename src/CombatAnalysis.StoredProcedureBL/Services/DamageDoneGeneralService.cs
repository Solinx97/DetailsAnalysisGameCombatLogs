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
            var paramNames = new string[] { nameof(item.Value), nameof(item.DamagePerSecond), nameof(item.SpellOrItem),
                nameof(item.CritNumber), nameof(item.MissNumber), nameof(item.CastNumber), nameof(item.MinValue),  nameof(item.MaxValue),
                nameof(item.AverageValue), nameof(item.CombatPlayerId) };
            var paramValues = new object[] { item.Value, item.DamagePerSecond, item.SpellOrItem,
                item.CritNumber, item.MissNumber, item.CastNumber, item.MinValue, item.MaxValue,
                item.AverageValue, item.CombatPlayerId };

            var createdCombatId = await _repository.CreateAsync(paramNames, paramValues);
            return createdCombatId;
        }

        private async Task<int> DeleteInternalAsync(DamageDoneGeneralDto item)
        {
            var allData = await _repository.GetAllAsync();
            if (!allData.Any())
            {
                throw new NotFoundException($"Collection entity {nameof(DamageDoneGeneralDto)} not found", nameof(allData));
            }

            var numberEntries = await _repository.DeleteAsync(item.Id);
            return numberEntries;
        }

        private async Task<int> UpdateInternalAsync(DamageDoneGeneralDto item)
        {
            var allData = await _repository.GetAllAsync();
            if (!allData.Any())
            {
                throw new NotFoundException($"Collection entity {nameof(DamageDoneGeneralDto)} not found", nameof(allData));
            }

            var paramNames = new string[] { nameof(item.Value), nameof(item.DamagePerSecond), nameof(item.SpellOrItem),
                nameof(item.CritNumber), nameof(item.MissNumber), nameof(item.CastNumber), nameof(item.MinValue),  nameof(item.MaxValue),
                nameof(item.AverageValue), nameof(item.CombatPlayerId) };
            var paramValues = new object[] { item.Value, item.DamagePerSecond, item.SpellOrItem,
                item.CritNumber, item.MissNumber, item.CastNumber, item.MinValue, item.MaxValue,
                item.AverageValue, item.CombatPlayerId };

            var numberEntries = await _repository.UpdateAsync(paramNames, paramValues);
            return numberEntries;
        }
    }
}
