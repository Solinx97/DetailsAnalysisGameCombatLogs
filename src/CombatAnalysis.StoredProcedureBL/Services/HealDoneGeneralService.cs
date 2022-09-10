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
    internal class HealDoneGeneralService : IService<HealDoneGeneralDto, int>
    {
        private readonly IGenericRepository<HealDoneGeneral, int> _repository;
        private readonly IMapper _mapper;

        public HealDoneGeneralService(IGenericRepository<HealDoneGeneral, int> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        Task<int> IService<HealDoneGeneralDto, int>.CreateAsync(HealDoneGeneralDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return CreateInternalAsync(item);
        }

        Task<int> IService<HealDoneGeneralDto, int>.DeleteAsync(HealDoneGeneralDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return DeleteInternalAsync(item);
        }

        async Task<IEnumerable<HealDoneGeneralDto>> IService<HealDoneGeneralDto, int>.GetAllAsync()
        {
            var allData = await _repository.GetAllAsync();
            var result = _mapper.Map<List<HealDoneGeneralDto>>(allData);

            return result;
        }

        async Task<HealDoneGeneralDto> IService<HealDoneGeneralDto, int>.GetByIdAsync(int id)
        {
            var executeLoad = await _repository.GetByIdAsync(id);
            var result = _mapper.Map<HealDoneGeneralDto>(executeLoad);

            return result;
        }

        Task<int> IService<HealDoneGeneralDto, int>.UpdateAsync(HealDoneGeneralDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return UpdateInternalAsync(item);
        }

        private async Task<int> CreateInternalAsync(HealDoneGeneralDto item)
        {
            var paramNames = new string[] { nameof(item.Value), nameof(item.HealPerSecond), nameof(item.SpellOrItem),
                nameof(item.CritNumber), nameof(item.CastNumber), nameof(item.MinValue), nameof(item.MaxValue),
                nameof(item.AverageValue), nameof(item.CombatPlayerId) };
            var paramValues = new object[] { item.Value, item.HealPerSecond, item.SpellOrItem,
                item.CritNumber, item.CastNumber, item.MinValue, item.MaxValue,
                item.AverageValue, item.CombatPlayerId };

            var createdCombatId = await _repository.CreateAsync(paramNames, paramValues);
            return createdCombatId;
        }

        private async Task<int> DeleteInternalAsync(HealDoneGeneralDto item)
        {
            var allData = await _repository.GetAllAsync();
            if (!allData.Any())
            {
                throw new NotFoundException($"Collection entity {nameof(HealDoneGeneralDto)} not found", nameof(allData));
            }

            var numberEntries = await _repository.DeleteAsync(item.Id);
            return numberEntries;
        }

        private async Task<int> UpdateInternalAsync(HealDoneGeneralDto item)
        {
            var allData = await _repository.GetAllAsync();
            if (!allData.Any())
            {
                throw new NotFoundException($"Collection entity {nameof(HealDoneGeneralDto)} not found", nameof(allData));
            }

            var paramNames = new string[] { nameof(item.Value), nameof(item.HealPerSecond), nameof(item.SpellOrItem),
                nameof(item.CritNumber), nameof(item.CastNumber), nameof(item.MinValue), nameof(item.MaxValue),
                nameof(item.AverageValue), nameof(item.CombatPlayerId) };
            var paramValues = new object[] { item.Value, item.HealPerSecond, item.SpellOrItem,
                item.CritNumber, item.CastNumber, item.MinValue, item.MaxValue,
                item.AverageValue, item.CombatPlayerId };

            var numberEntries = await _repository.UpdateAsync(paramNames, paramValues);
            return numberEntries;
        }
    }
}
