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
    internal class CombatLogByUserService : IService<CombatLogByUserDto, int>
    {
        private readonly IGenericRepository<CombatLogByUser, int> _repository;
        private readonly IMapper _mapper;

        public CombatLogByUserService(IGenericRepository<CombatLogByUser, int> userRepository, IMapper mapper)
        {
            _repository = userRepository;
            _mapper = mapper;
        }

        Task<int> IService<CombatLogByUserDto, int>.CreateAsync(CombatLogByUserDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return CreateInternalAsync(item);
        }

        Task<int> IService<CombatLogByUserDto, int>.DeleteAsync(CombatLogByUserDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return DeleteInternalAsync(item);
        }

        async Task<IEnumerable<CombatLogByUserDto>> IService<CombatLogByUserDto, int>.GetAllAsync()
        {
            var allData = await _repository.GetAllAsync();
            var result = _mapper.Map<List<CombatLogByUserDto>>(allData);

            return result;
        }

        async Task<CombatLogByUserDto> IService<CombatLogByUserDto, int>.GetByIdAsync(int id)
        {
            var executeLoad = await _repository.GetByIdAsync(id);
            var result = _mapper.Map<CombatLogByUserDto>(executeLoad);

            return result;
        }

        Task<int> IService<CombatLogByUserDto, int>.UpdateAsync(CombatLogByUserDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return UpdateInternalAsync(item);
        }

        private async Task<int> CreateInternalAsync(CombatLogByUserDto item)
        {
            var paramNames = new string[] { nameof(item.UserId), nameof(item.PersonalLogType), nameof(item.CombatLogId) };
            var paramValues = new object[] { item.UserId, item.PersonalLogType, item.CombatLogId };

            var createdCombatId = await _repository.CreateAsync(paramNames, paramValues);

            return createdCombatId;
        }

        private async Task<int> DeleteInternalAsync(CombatLogByUserDto item)
        {
            var allData = await _repository.GetAllAsync();
            if (!allData.Any())
            {
                throw new NotFoundException($"Collection entity {nameof(CombatLogByUserDto)} not found", nameof(allData));
            }

            var numberEntries = await _repository.DeleteAsync(item.Id);
            return numberEntries;
        }

        private async Task<int> UpdateInternalAsync(CombatLogByUserDto item)
        {
            var allData = await _repository.GetAllAsync();
            if (!allData.Any())
            {
                throw new NotFoundException($"Collection entity {nameof(CombatLogByUserDto)} not found", nameof(allData));
            }

            var paramNames = new string[] { nameof(item.UserId), nameof(item.PersonalLogType), nameof(item.CombatLogId) };
            var paramValues = new object[] { item.UserId, item.PersonalLogType, item.CombatLogId };

            var numberEntries = await _repository.UpdateAsync(paramNames, paramValues);
            return numberEntries;
        }
    }
}
