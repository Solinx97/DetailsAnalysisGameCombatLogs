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
    internal class DamageDoneService : IService<DamageDoneDto>
    {
        private readonly IGenericRepository<DamageDone> _repository;
        private readonly IMapper _mapper;

        public DamageDoneService(IGenericRepository<DamageDone> userRepository, IMapper mapper)
        {
            _repository = userRepository;
            _mapper = mapper;
        }

        Task<int> IService<DamageDoneDto>.CreateAsync(DamageDoneDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return CreateInternalAsync(item);
        }

        Task<int> IService<DamageDoneDto>.DeleteAsync(DamageDoneDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return DeleteInternalAsync(item);
        }

        async Task<IEnumerable<DamageDoneDto>> IService<DamageDoneDto>.GetAllAsync()
        {
            var allData = await _repository.GetAllAsync();
            var result = _mapper.Map<List<DamageDoneDto>>(allData);

            return result;
        }

        async Task<DamageDoneDto> IService<DamageDoneDto>.GetByIdAsync(int id)
        {
            var executeLoad = await _repository.GetByIdAsync(id);
            var result = _mapper.Map<DamageDoneDto>(executeLoad);

            return result;
        }

        Task<int> IService<DamageDoneDto>.UpdateAsync(DamageDoneDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return UpdateInternalAsync(item);
        }

        private async Task<int> CreateInternalAsync(DamageDoneDto item)
        {
            //if (string.IsNullOrEmpty(item.Name))
            //{
            //    throw new ArgumentNullException(nameof(item.Name));
            //}

            var map = _mapper.Map<DamageDone>(item);
            var createdCombatId = await _repository.CreateAsync(map);

            return createdCombatId;
        }

        private async Task<int> DeleteInternalAsync(DamageDoneDto item)
        {
            var allData = await _repository.GetAllAsync();
            //if (!allData.Any())
            //{
            //    throw new NotFoundException($"Collection entity {nameof(CombatDto)} not found", nameof(allData));
            //}

            var numberEntries = await _repository.DeleteAsync(_mapper.Map<DamageDone>(item));
            return numberEntries;
        }

        private async Task<int> UpdateInternalAsync(DamageDoneDto item)
        {
            var allData = await _repository.GetAllAsync();
            //if (!allData.Any())
            //{
            //    throw new NotFoundException($"Collection entity {nameof(CombatDto)} not found", nameof(allData));
            //}

            //if (string.IsNullOrEmpty(item.Name))
            //{
            //    throw new ArgumentNullException(nameof(item.Name));
            //}

            var numberEntries = await _repository.UpdateAsync(_mapper.Map<DamageDone>(item));
            return numberEntries;
        }
    }
}
