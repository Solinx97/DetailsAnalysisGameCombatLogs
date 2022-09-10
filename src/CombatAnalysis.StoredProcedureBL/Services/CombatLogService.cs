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
    internal class CombatLogService : IService<CombatLogDto, int>
    {
        private readonly IGenericRepository<CombatLog ,int> _repository;
        private readonly IMapper _mapper;

        public CombatLogService(IGenericRepository<CombatLog, int> userRepository, IMapper mapper)
        {
            _repository = userRepository;
            _mapper = mapper;
        }

        Task<int> IService<CombatLogDto, int>.CreateAsync(CombatLogDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return CreateInternalAsync(item);
        }

        Task<int> IService<CombatLogDto, int>.DeleteAsync(CombatLogDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return DeleteInternalAsync(item);
        }

        async Task<IEnumerable<CombatLogDto>> IService<CombatLogDto, int>.GetAllAsync()
        {
            var allData = await _repository.GetAllAsync();
            var result = _mapper.Map<List<CombatLogDto>>(allData);

            return result;
        }

        async Task<CombatLogDto> IService<CombatLogDto, int>.GetByIdAsync(int id)
        {
            var executeLoad = await _repository.GetByIdAsync(id);
            var result = _mapper.Map<CombatLogDto>(executeLoad);

            return result;
        }

        Task<int> IService<CombatLogDto, int>.UpdateAsync(CombatLogDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return UpdateInternalAsync(item);
        }

        private async Task<int> CreateInternalAsync(CombatLogDto item)
        {
            if (string.IsNullOrEmpty(item.Name))
            {
                throw new ArgumentNullException(nameof(item.Name));
            }

            var paramNames = new string[] { nameof(item.Name), nameof(item.Date), nameof(item.IsReady) };
            var paramValues = new object[] { item.Name, item.Date, item.IsReady };

            var createdCombatId = await _repository.CreateAsync(paramNames, paramValues);

            return createdCombatId;
        }

        private async Task<int> DeleteInternalAsync(CombatLogDto item)
        {
            var allData = await _repository.GetAllAsync();
            if (!allData.Any())
            {
                throw new NotFoundException($"Collection entity {nameof(CombatLogDto)} not found", nameof(allData));
            }

            var numberEntries = await _repository.DeleteAsync(item.Id);
            return numberEntries;
        }

        private async Task<int> UpdateInternalAsync(CombatLogDto item)
        {
            var allData = await _repository.GetAllAsync();
            if (!allData.Any())
            {
                throw new NotFoundException($"Collection entity {nameof(CombatLogDto)} not found", nameof(allData));
            }

            if (string.IsNullOrEmpty(item.Name))
            {
                throw new ArgumentNullException(nameof(item.Name));
            }

            var paramNames = new string[] { nameof(item.Name), nameof(item.Date), nameof(item.IsReady) };
            var paramValues = new object[] { item.Name, item.Date, item.IsReady };

            var numberEntries = await _repository.UpdateAsync(paramNames, paramValues);
            return numberEntries;
        }
    }
}
