using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CombatAnalysis.BL.Services
{
    internal class CombatService : IService<CombatDto>
    {
        private readonly IGenericRepository<Combat> _repository;
        private readonly IMapper _mapper;

        public CombatService(IGenericRepository<Combat> userRepository, IMapper mapper)
        {
            _repository = userRepository;
            _mapper = mapper;
        }

        Task<int> IService<CombatDto>.CreateAsync(CombatDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return CreateInternalAsync(item);
        }

        Task<int> IService<CombatDto>.DeleteAsync(CombatDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return DeleteInternalAsync(item);
        }

        async Task<IEnumerable<CombatDto>> IService<CombatDto>.GetAllAsync()
        {
            var allData = await _repository.GetAllAsync();
            var result = _mapper.Map<List<CombatDto>>(allData);

            return result;
        }

        async Task<CombatDto> IService<CombatDto>.GetByIdAsync(int id)
        {
            var executeLoad = await _repository.GetByIdAsync(id);
            var result = _mapper.Map<CombatDto>(executeLoad);

            return result;
        }

        Task<int> IService<CombatDto>.UpdateAsync(CombatDto item)
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

            var numberEntries = await _repository.CreateAsync(_mapper.Map<Combat>(item));

            return numberEntries;
        }

        private async Task<int> DeleteInternalAsync(CombatDto item)
        {
            var allData = await _repository.GetAllAsync();
            //if (!allData.Any())
            //{
            //    throw new NotFoundException($"Collection entity {nameof(CombatDto)} not found", nameof(allData));
            //}

            var numberEntries = await _repository.DeleteAsync(_mapper.Map<Combat>(item));
            return numberEntries;
        }

        private async Task<int> UpdateInternalAsync(CombatDto item)
        {
            var allData = await _repository.GetAllAsync();
            //if (!allData.Any())
            //{
            //    throw new NotFoundException($"Collection entity {nameof(CombatDto)} not found", nameof(allData));
            //}

            if (string.IsNullOrEmpty(item.Name))
            {
                throw new ArgumentNullException(nameof(item.Name));
            }

            var numberEntries = await _repository.UpdateAsync(_mapper.Map<Combat>(item));
            return numberEntries;
        }
    }
}
