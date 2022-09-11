using AutoMapper;
using CombatAnalysis.BL.DTO.User;
using CombatAnalysis.BL.Exceptions;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.DAL.Entities.User;
using CombatAnalysis.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CombatAnalysis.BL.Services
{
    internal class UserService : IUserService<AppUserDto>
    {
        private readonly IUserRepository _repository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        Task<string> IUserService<AppUserDto>.CreateAsync(AppUserDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return CreateInternalAsync(item);
        }

        Task<int> IUserService<AppUserDto>.DeleteAsync(AppUserDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return DeleteInternalAsync(item);
        }

        async Task<IEnumerable<AppUserDto>> IUserService<AppUserDto>.GetAllAsync()
        {
            var allData = await _repository.GetAllAsync();
            var result = _mapper.Map<List<AppUserDto>>(allData);

            return result;
        }

        async Task<AppUserDto> IUserService<AppUserDto>.GetByIdAsync(string id)
        {
            var user = await _repository.GetByIdAsync(id);
            var result = _mapper.Map<AppUserDto>(user);

            return result;
        }

        async Task<AppUserDto> IUserService<AppUserDto>.GetAsync(string emil, string password)
        {
            var user = await _repository.GetAsync(emil, password);
            var result = _mapper.Map<AppUserDto>(user);

            return result;
        }

        async Task<AppUserDto> IUserService<AppUserDto>.GetAsync(string emil)
        {
            var user = await _repository.GetAsync(emil);
            var result = _mapper.Map<AppUserDto>(user);

            return result;
        }

        Task<int> IUserService<AppUserDto>.UpdateAsync(AppUserDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return UpdateInternalAsync(item);
        }


        private async Task<string> CreateInternalAsync(AppUserDto item)
        {
            var map = _mapper.Map<AppUser>(item);
            var createdUserId = await _repository.CreateAsync(map);

            return createdUserId;
        }

        private async Task<int> DeleteInternalAsync(AppUserDto item)
        {
            var allData = await _repository.GetAllAsync();
            if (!allData.Any())
            {
                throw new NotFoundException($"Collection entity {nameof(AppUserDto)} not found", nameof(allData));
            }

            var numberEntries = await _repository.DeleteAsync(_mapper.Map<AppUser>(item));
            return numberEntries;
        }

        private async Task<int> UpdateInternalAsync(AppUserDto item)
        {
            var allData = await _repository.GetAllAsync();
            if (!allData.Any())
            {
                throw new NotFoundException($"Collection entity {nameof(AppUserDto)} not found", nameof(allData));
            }

            var numberEntries = await _repository.UpdateAsync(_mapper.Map<AppUser>(item));
            return numberEntries;
        }
    }
}
