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
    internal class CombatPlayerService : IService<CombatPlayerDto, int>
    {
        private readonly IGenericRepository<CombatPlayer, int> _repository;
        private readonly IMapper _mapper;

        public CombatPlayerService(IGenericRepository<CombatPlayer, int> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        Task<int> IService<CombatPlayerDto, int>.CreateAsync(CombatPlayerDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return CreateInternalAsync(item);
        }

        Task<int> IService<CombatPlayerDto, int>.DeleteAsync(CombatPlayerDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return DeleteInternalAsync(item);
        }

        async Task<IEnumerable<CombatPlayerDto>> IService<CombatPlayerDto, int>.GetAllAsync()
        {
            var allData = await _repository.GetAllAsync();
            var result = _mapper.Map<List<CombatPlayerDto>>(allData);

            return result;
        }

        async Task<CombatPlayerDto> IService<CombatPlayerDto, int>.GetByIdAsync(int id)
        {
            var executeLoad = await _repository.GetByIdAsync(id);
            var result = _mapper.Map<CombatPlayerDto>(executeLoad);

            return result;
        }

        Task<int> IService<CombatPlayerDto, int>.UpdateAsync(CombatPlayerDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return UpdateInternalAsync(item);
        }

        private async Task<int> CreateInternalAsync(CombatPlayerDto item)
        {
            if (string.IsNullOrEmpty(item.UserName))
            {
                throw new ArgumentNullException(nameof(item.UserName));
            }

            var paramNames = new string[] { nameof(item.UserName), nameof(item.DamageDone), nameof(item.HealDone),
                                            nameof(item.DamageTaken), nameof(item.EnergyRecovery), nameof(item.UsedBuffs), nameof(item.CombatId)};
            var paramValues = new object[] { item.UserName, item.DamageDone, item.HealDone, item.DamageTaken, item.EnergyRecovery, item.UsedBuffs, item.CombatId };

            var createdCombatId = await _repository.CreateAsync(paramNames, paramValues);
            return createdCombatId;
        }

        private async Task<int> DeleteInternalAsync(CombatPlayerDto item)
        {
            var allData = await _repository.GetAllAsync();
            if (!allData.Any())
            {
                throw new NotFoundException($"Collection entity {nameof(CombatPlayerDto)} not found", nameof(allData));
            }

            var numberEntries = await _repository.DeleteAsync(item.Id);
            return numberEntries;
        }

        private async Task<int> UpdateInternalAsync(CombatPlayerDto item)
        {
            var allData = await _repository.GetAllAsync();
            if (!allData.Any())
            {
                throw new NotFoundException($"Collection entity {nameof(CombatPlayerDto)} not found", nameof(allData));
            }

            if (string.IsNullOrEmpty(item.UserName))
            {
                throw new ArgumentNullException(nameof(item.UserName));
            }

            var paramNames = new string[] { nameof(item.UserName), nameof(item.DamageDone), nameof(item.HealDone),
                                            nameof(item.DamageTaken), nameof(item.EnergyRecovery), nameof(item.UsedBuffs), nameof(item.CombatId)};
            var paramValues = new object[] { item.UserName, item.DamageDone, item.HealDone, item.DamageTaken, item.EnergyRecovery, item.UsedBuffs, item.CombatId };

            var numberEntries = await _repository.UpdateAsync(paramNames, paramValues);
            return numberEntries;
        }
    }
}
