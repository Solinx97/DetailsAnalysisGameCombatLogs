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
    internal class CombatService : IService<CombatDto, int>
    {
        private readonly IGenericRepository<Combat, int> _repository;
        private readonly IMapper _mapper;

        public CombatService(IGenericRepository<Combat, int> repository, IMapper mapper)
        {
            _repository = repository;
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

            var paramNames = new string[] { nameof(item.Name), nameof(item.DamageDone), nameof(item.HealDone),
                                            nameof(item.DamageTaken), nameof(item.EnergyRecovery), nameof(item.UsedBuffs), nameof(item.IsWin), nameof(item.CombatLogId) };
            var paramValues = new object[] { item.Name, item.DamageDone, item.HealDone, item.DamageTaken, item.EnergyRecovery, item.UsedBuffs, item.IsWin, item.CombatLogId };

            var createdCombatId = await _repository.CreateAsync(paramNames, paramValues);
            return createdCombatId;
        }

        private async Task<int> DeleteInternalAsync(CombatDto item)
        {
            var allData = await _repository.GetAllAsync();
            if (!allData.Any())
            {
                throw new NotFoundException($"Collection entity {nameof(CombatDto)} not found", nameof(allData));
            }

            var numberEntries = await _repository.DeleteAsync(item.Id);
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

            var paramNames = new string[] { nameof(item.Name), nameof(item.DamageDone), nameof(item.HealDone),
                                            nameof(item.DamageTaken), nameof(item.EnergyRecovery), nameof(item.UsedBuffs), nameof(item.IsWin), nameof(item.CombatLogId) };
            var paramValues = new object[] { item.Name, item.DamageDone, item.HealDone, item.DamageTaken, item.EnergyRecovery, item.UsedBuffs, item.IsWin, item.CombatLogId };

            var numberEntries = await _repository.UpdateAsync(paramNames, paramValues);
            return numberEntries;
        }
    }
}
