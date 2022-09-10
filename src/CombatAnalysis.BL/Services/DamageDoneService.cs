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
    internal class DamageDoneService : ISPService<DamageDoneDto, int>, IService<DamageDoneDto, int>
    {
        private readonly ISPGenericRepository<DamageDone> _spRepository;
        private readonly IGenericRepository<DamageDone> _repository;
        private readonly IMapper _mapper;

        public DamageDoneService(ISPGenericRepository<DamageDone> spRepository, IGenericRepository<DamageDone> repository, IMapper mapper)
        {
            _spRepository = spRepository;
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

        async Task<int> ISPService<DamageDoneDto, int>.CreateByProcedureAsync(DamageDoneDto item)
        {
            var paramNames = new string[] { nameof(item.Value), nameof(item.Time), nameof(item.FromPlayer),
                nameof(item.ToEnemy), nameof(item.SpellOrItem), nameof(item.IsDodge), nameof(item.IsParry),  nameof(item.IsMiss),
                nameof(item.IsResist), nameof(item.IsImmune), nameof(item.IsCrit), nameof(item.CombatPlayerDataId) };
            var paramValues = new object[] { item.Value, item.Time.ToString(), item.FromPlayer,
                item.ToEnemy, item.SpellOrItem, item.IsDodge, item.IsParry, item.IsMiss,
                item.IsResist, item.IsImmune, item.IsCrit, item.CombatPlayerDataId };

            var response = await _spRepository.ExecuteStoredProcedureAsync(DbProcedureHelper.InsertIntoDamageDone, paramNames, paramValues);
            return response;
        }

        Task<int> IService<DamageDoneDto, int>.DeleteAsync(DamageDoneDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return DeleteInternalAsync(item);
        }

        async Task<int> ISPService<DamageDoneDto, int>.DeleteByProcedureAsync(int combatPlayerId)
        {
            var paramNames = new string[] { nameof(combatPlayerId) };
            var paramValues = new object[] { combatPlayerId };

            var response = await _spRepository.ExecuteStoredProcedureAsync(DbProcedureHelper.DeleteDamageDone, paramNames, paramValues);
            return response;
        }

        async Task<IEnumerable<DamageDoneDto>> IService<DamageDoneDto, int>.GetAllAsync()
        {
            var allData = await _repository.GetAllAsync();
            var result = _mapper.Map<List<DamageDoneDto>>(allData);

            return result;
        }

        async Task<IEnumerable<DamageDoneDto>> ISPService<DamageDoneDto, int>.GetByProcedureAsync(int combatPlayerId)
        {
            var paramNames = new string[] { nameof(combatPlayerId) };
            var paramValues = new object[] { combatPlayerId };

            var data = await _spRepository.ExecuteStoredProcedureUseModelAsync(DbProcedureHelper.GetDamageDone, paramNames, paramValues);
            var result = _mapper.Map<IEnumerable<DamageDoneDto>>(data);

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
            var map = _mapper.Map<DamageDone>(item);
            var createdCombatId = await _repository.CreateAsync(map);

            return createdCombatId;
        }

        private async Task<int> DeleteInternalAsync(DamageDoneDto item)
        {
            var allData = await _repository.GetAllAsync();
            if (!allData.Any())
            {
                throw new NotFoundException($"Collection entity {nameof(DamageDoneDto)} not found", nameof(allData));
            }

            var numberEntries = await _repository.DeleteAsync(_mapper.Map<DamageDone>(item));
            return numberEntries;
        }

        private async Task<int> UpdateInternalAsync(DamageDoneDto item)
        {
            var allData = await _repository.GetAllAsync();
            if (!allData.Any())
            {
                throw new NotFoundException($"Collection entity {nameof(DamageDoneDto)} not found", nameof(allData));
            }

            var numberEntries = await _repository.UpdateAsync(_mapper.Map<DamageDone>(item));
            return numberEntries;
        }
    }
}
