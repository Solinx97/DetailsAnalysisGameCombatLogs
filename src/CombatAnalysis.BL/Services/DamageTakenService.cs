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
    internal class DamageTakenService : ISPService<DamageTakenDto, int>, IService<DamageTakenDto, int>
    {
        private readonly ISPGenericRepository<DamageTaken> _spRepository;
        private readonly IGenericRepository<DamageTaken> _repository;
        private readonly IMapper _mapper;

        public DamageTakenService(ISPGenericRepository<DamageTaken> spRepository, IGenericRepository<DamageTaken> repository, IMapper mapper)
        {
            _spRepository = spRepository;
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

        async Task<int> ISPService<DamageTakenDto, int>.CreateByProcedureAsync(DamageTakenDto item)
        {
            var paramNames = new string[] { nameof(item.Value), nameof(item.Time), nameof(item.From),
                nameof(item.To), nameof(item.SpellOrItem), nameof(item.IsDodge), nameof(item.IsParry), nameof(item.IsMiss),
                nameof(item.IsResist), nameof(item.IsImmune), nameof(item.IsCrushing), nameof(item.CombatPlayerDataId) };
            var paramValues = new object[] { item.Value, item.Time, item.From,
                item.To, item.SpellOrItem, item.IsDodge, item.IsParry, item.IsMiss,
                item.IsResist, item.IsImmune, item.IsCrushing, item.CombatPlayerDataId };

            var response = await _spRepository.ExecuteStoredProcedureAsync(DbProcedureHelper.InsertIntoDamageTaken, paramNames, paramValues);
            return response;
        }

        Task<int> IService<DamageTakenDto, int>.DeleteAsync(DamageTakenDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return DeleteInternalAsync(item);
        }

        async Task<int> ISPService<DamageTakenDto, int>.DeleteByProcedureAsync(int combatPlayerId)
        {
            var paramNames = new string[] { nameof(combatPlayerId) };
            var paramValues = new object[] { combatPlayerId };

            var response = await _spRepository.ExecuteStoredProcedureAsync(DbProcedureHelper.DeleteDamageTaken, paramNames, paramValues);
            return response;
        }

        async Task<IEnumerable<DamageTakenDto>> IService<DamageTakenDto, int>.GetAllAsync()
        {
            var allData = await _repository.GetAllAsync();
            var result = _mapper.Map<List<DamageTakenDto>>(allData);

            return result;
        }

        async Task<IEnumerable<DamageTakenDto>> ISPService<DamageTakenDto, int>.GetByProcedureAsync(int combatPlayerId)
        {
            var paramNames = new string[] { nameof(combatPlayerId) };
            var paramValues = new object[] { combatPlayerId };

            var data = await _spRepository.ExecuteStoredProcedureUseModelAsync(DbProcedureHelper.GetDamageTaken, paramNames, paramValues);
            var result = _mapper.Map<IEnumerable<DamageTakenDto>>(data);

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
            var map = _mapper.Map<DamageTaken>(item);
            var createdCombatId = await _repository.CreateAsync(map);

            return createdCombatId;
        }

        private async Task<int> DeleteInternalAsync(DamageTakenDto item)
        {
            var allData = await _repository.GetAllAsync();
            if (!allData.Any())
            {
                throw new NotFoundException($"Collection entity {nameof(DamageTakenDto)} not found", nameof(allData));
            }

            var numberEntries = await _repository.DeleteAsync(_mapper.Map<DamageTaken>(item));
            return numberEntries;
        }

        private async Task<int> UpdateInternalAsync(DamageTakenDto item)
        {
            var allData = await _repository.GetAllAsync();
            if (!allData.Any())
            {
                throw new NotFoundException($"Collection entity {nameof(DamageTakenDto)} not found", nameof(allData));
            }

            var numberEntries = await _repository.UpdateAsync(_mapper.Map<DamageTaken>(item));
            return numberEntries;
        }
    }
}
