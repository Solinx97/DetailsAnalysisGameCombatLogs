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
    internal class DamageTakenService : IService<DamageTakenDto>
    {
        private readonly IGenericRepository<DamageTaken> _repository;
        private readonly IMapper _mapper;

        public DamageTakenService(IGenericRepository<DamageTaken> userRepository, IMapper mapper)
        {
            _repository = userRepository;
            _mapper = mapper;
        }

        Task<int> IService<DamageTakenDto>.CreateAsync(DamageTakenDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return CreateInternalAsync(item);
        }

        Task<int> IService<DamageTakenDto>.DeleteAsync(DamageTakenDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return DeleteInternalAsync(item);
        }

        async Task<IEnumerable<DamageTakenDto>> IService<DamageTakenDto>.GetAllAsync()
        {
            var allData = await _repository.GetAllAsync();
            var result = _mapper.Map<List<DamageTakenDto>>(allData);

            return result;
        }

        async Task<IEnumerable<DamageTakenDto>> IService<DamageTakenDto>.GetByProcedureAsync(int combatPlayerId)
        {
            var paramNames = new string[] { nameof(combatPlayerId) };
            var paramValues = new object[] { combatPlayerId };

            var data = await _repository.GetByProcedureAsync(DbProcedureHelper.GetDamageTaken, paramNames, paramValues);
            var result = _mapper.Map<IEnumerable<DamageTakenDto>>(data);

            return result;
        }

        async Task<DamageTakenDto> IService<DamageTakenDto>.GetByIdAsync(int id)
        {
            var executeLoad = await _repository.GetByIdAsync(id);
            var result = _mapper.Map<DamageTakenDto>(executeLoad);

            return result;
        }

        Task<int> IService<DamageTakenDto>.UpdateAsync(DamageTakenDto item)
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

        public Task<int> DeleteByProcedureAsync(int combatPlayerId)
        {
            throw new NotImplementedException();
        }
    }
}
