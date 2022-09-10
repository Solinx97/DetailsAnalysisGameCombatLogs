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
    internal class DamageDoneService : IService<DamageDoneDto, int>
    {
        private readonly IGenericRepository<DamageDone, int> _repository;
        private readonly IMapper _mapper;

        public DamageDoneService(IGenericRepository<DamageDone, int> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        Task<int> IService<DamageDoneDto, int>.CreateAsync(DamageDoneDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return CreateInternalAsync(item);
        }

        Task<int> IService<DamageDoneDto, int>.DeleteAsync(DamageDoneDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return DeleteInternalAsync(item);
        }

        async Task<IEnumerable<DamageDoneDto>> IService<DamageDoneDto, int>.GetAllAsync()
        {
            var allData = await _repository.GetAllAsync();
            var result = _mapper.Map<List<DamageDoneDto>>(allData);

            return result;
        }

        async Task<DamageDoneDto> IService<DamageDoneDto, int>.GetByIdAsync(int id)
        {
            var executeLoad = await _repository.GetByIdAsync(id);
            var result = _mapper.Map<DamageDoneDto>(executeLoad);

            return result;
        }

        Task<int> IService<DamageDoneDto, int>.UpdateAsync(DamageDoneDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return UpdateInternalAsync(item);
        }

        private async Task<int> CreateInternalAsync(DamageDoneDto item)
        {
            var paramNames = new string[] { nameof(item.Value), nameof(item.Time), nameof(item.FromPlayer),
                nameof(item.ToEnemy), nameof(item.SpellOrItem), nameof(item.IsDodge), nameof(item.IsParry),  nameof(item.IsMiss),
                nameof(item.IsResist), nameof(item.IsImmune), nameof(item.IsCrit), nameof(item.CombatPlayerId) };
            var paramValues = new object[] { item.Value, item.Time.ToString(), item.FromPlayer,
                item.ToEnemy, item.SpellOrItem, item.IsDodge, item.IsParry, item.IsMiss,
                item.IsResist, item.IsImmune, item.IsCrit, item.CombatPlayerId };

            var createdCombatId = await _repository.CreateAsync(paramNames, paramValues);
            return createdCombatId;
        }

        private async Task<int> DeleteInternalAsync(DamageDoneDto item)
        {
            var allData = await _repository.GetAllAsync();
            if (!allData.Any())
            {
                throw new NotFoundException($"Collection entity {nameof(DamageDoneDto)} not found", nameof(allData));
            }

            var numberEntries = await _repository.DeleteAsync(item.Id);
            return numberEntries;
        }

        private async Task<int> UpdateInternalAsync(DamageDoneDto item)
        {
            var allData = await _repository.GetAllAsync();
            if (!allData.Any())
            {
                throw new NotFoundException($"Collection entity {nameof(DamageDoneDto)} not found", nameof(allData));
            }

            var paramNames = new string[] { nameof(item.Value), nameof(item.Time), nameof(item.FromPlayer),
                nameof(item.ToEnemy), nameof(item.SpellOrItem), nameof(item.IsDodge), nameof(item.IsParry),  nameof(item.IsMiss),
                nameof(item.IsResist), nameof(item.IsImmune), nameof(item.IsCrit), nameof(item.CombatPlayerId) };
            var paramValues = new object[] { item.Value, item.Time.ToString(), item.FromPlayer,
                item.ToEnemy, item.SpellOrItem, item.IsDodge, item.IsParry, item.IsMiss,
                item.IsResist, item.IsImmune, item.IsCrit, item.CombatPlayerId };

            var numberEntries = await _repository.UpdateAsync(paramNames, paramValues);
            return numberEntries;
        }
    }
}
