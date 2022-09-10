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
    internal class DamageTakenService : IService<DamageTakenDto, int>
    {
        private readonly IGenericRepository<DamageTaken, int> _repository;
        private readonly IMapper _mapper;

        public DamageTakenService(IGenericRepository<DamageTaken, int> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        Task<int> IService<DamageTakenDto, int>.CreateAsync(DamageTakenDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return CreateInternalAsync(item);
        }

        Task<int> IService<DamageTakenDto, int>.DeleteAsync(DamageTakenDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return DeleteInternalAsync(item);
        }

        async Task<IEnumerable<DamageTakenDto>> IService<DamageTakenDto, int>.GetAllAsync()
        {
            var allData = await _repository.GetAllAsync();
            var result = _mapper.Map<List<DamageTakenDto>>(allData);

            return result;
        }

        async Task<DamageTakenDto> IService<DamageTakenDto, int>.GetByIdAsync(int id)
        {
            var executeLoad = await _repository.GetByIdAsync(id);
            var result = _mapper.Map<DamageTakenDto>(executeLoad);

            return result;
        }

        Task<int> IService<DamageTakenDto, int>.UpdateAsync(DamageTakenDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return UpdateInternalAsync(item);
        }

        private async Task<int> CreateInternalAsync(DamageTakenDto item)
        {
            var paramNames = new string[] { nameof(item.Value), nameof(item.Time), nameof(item.From),
                nameof(item.To), nameof(item.SpellOrItem), nameof(item.IsDodge), nameof(item.IsParry), nameof(item.IsMiss),
                nameof(item.IsResist), nameof(item.IsImmune), nameof(item.IsCrushing), nameof(item.CombatPlayerId) };
            var paramValues = new object[] { item.Value, item.Time, item.From,
                item.To, item.SpellOrItem, item.IsDodge, item.IsParry, item.IsMiss,
                item.IsResist, item.IsImmune, item.IsCrushing, item.CombatPlayerId };

            var createdCombatId = await _repository.CreateAsync(paramNames, paramValues);
            return createdCombatId;
        }

        private async Task<int> DeleteInternalAsync(DamageTakenDto item)
        {
            var allData = await _repository.GetAllAsync();
            if (!allData.Any())
            {
                throw new NotFoundException($"Collection entity {nameof(DamageTakenDto)} not found", nameof(allData));
            }

            var numberEntries = await _repository.DeleteAsync(item.Id);
            return numberEntries;
        }

        private async Task<int> UpdateInternalAsync(DamageTakenDto item)
        {
            var allData = await _repository.GetAllAsync();
            if (!allData.Any())
            {
                throw new NotFoundException($"Collection entity {nameof(DamageTakenDto)} not found", nameof(allData));
            }

            var paramNames = new string[] { nameof(item.Value), nameof(item.Time), nameof(item.From),
                nameof(item.To), nameof(item.SpellOrItem), nameof(item.IsDodge), nameof(item.IsParry), nameof(item.IsMiss),
                nameof(item.IsResist), nameof(item.IsImmune), nameof(item.IsCrushing), nameof(item.CombatPlayerId) };
            var paramValues = new object[] { item.Value, item.Time, item.From,
                item.To, item.SpellOrItem, item.IsDodge, item.IsParry, item.IsMiss,
                item.IsResist, item.IsImmune, item.IsCrushing, item.CombatPlayerId };

            var numberEntries = await _repository.UpdateAsync(paramNames, paramValues);
            return numberEntries;
        }
    }
}
