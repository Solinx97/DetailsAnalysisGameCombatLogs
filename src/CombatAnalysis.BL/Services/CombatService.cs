using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Exceptions;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CombatAnalysis.BL.Services
{
    internal class CombatService : ISPService<CombatDto, int>
    {
        private readonly ISPGenericRepository<Combat> _repository;
        private readonly IMapper _mapper;

        public CombatService(ISPGenericRepository<Combat> userRepository, IMapper mapper)
        {
            _repository = userRepository;
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

        Task<int> IService<CombatDto, int>.DeleteAsync(CombatDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return DeleteInternalAsync(item);
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

            var data = await _repository.ExecuteStoredProcedureUseModelAsync("GetCombatByCombatLogId", paramNames, paramValues);
            var result = _mapper.Map<IEnumerable<CombatDto>>(data);

            return result;
        }

        Task<int> ISPService<CombatDto, int>.DeleteByProcedureAsync(int combatPlayerId)
        {
            throw new NotImplementedException();
        }

        Task<int> ISPService<CombatDto, int>.CreateByProcedureAsync(CombatDto item)
        {
            throw new NotImplementedException();
        }
    }
}
