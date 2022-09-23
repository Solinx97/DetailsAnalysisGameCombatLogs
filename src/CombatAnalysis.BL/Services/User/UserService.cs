using AutoMapper;
using CombatAnalysis.BL.DTO.User;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.DAL.Entities.User;
using CombatAnalysis.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CombatAnalysis.BL.Services.User
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

        Task<AppUserDto> IUserService<AppUserDto>.CreateAsync(AppUserDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(AppUserDto), $"The {nameof(AppUserDto)} can't be null");
            }

            return CreateInternalAsync(item);
        }

        Task<int> IUserService<AppUserDto>.DeleteAsync(AppUserDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(AppUserDto), $"The {nameof(AppUserDto)} can't be null");
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
            var result = await _repository.GetByIdAsync(id);
            var resultMap = _mapper.Map<AppUserDto>(result);

            return resultMap;
        }

        async Task<AppUserDto> IUserService<AppUserDto>.GetAsync(string emil, string password)
        {
            var result = await _repository.GetAsync(emil, password);
            var resultMap = _mapper.Map<AppUserDto>(result);

            return resultMap;
        }

        async Task<AppUserDto> IUserService<AppUserDto>.GetAsync(string emil)
        {
            var result = await _repository.GetAsync(emil);
            var resultMap = _mapper.Map<AppUserDto>(result);

            return resultMap;
        }

        Task<int> IUserService<AppUserDto>.UpdateAsync(AppUserDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(AppUserDto), $"The {nameof(AppUserDto)} can't be null");
            }

            return UpdateInternalAsync(item);
        }


        private async Task<AppUserDto> CreateInternalAsync(AppUserDto item)
        {
            if (string.IsNullOrEmpty(item.Email))
            {
                throw new ArgumentNullException(nameof(AppUserDto), 
                    $"The property {nameof(AppUserDto.Email)} of the {nameof(AppUserDto)} object can't be null or empty");
            }
            if (string.IsNullOrEmpty(item.Password))
            {
                throw new ArgumentNullException(nameof(AppUserDto), 
                    $"The property {nameof(AppUserDto.Password)} of the {nameof(AppUserDto)} object can't be null or empty");
            }

            var map = _mapper.Map<AppUser>(item);
            var createdItem = await _repository.CreateAsync(map);
            var resultMap = _mapper.Map<AppUserDto>(createdItem);

            return resultMap;
        }

        private async Task<int> DeleteInternalAsync(AppUserDto item)
        {
            if (string.IsNullOrEmpty(item.Email))
            {
                throw new ArgumentNullException(nameof(AppUserDto), 
                    $"The property {nameof(AppUserDto.Email)} of the {nameof(AppUserDto)} object can't be null or empty");
            }
            if (string.IsNullOrEmpty(item.Password))
            {
                throw new ArgumentNullException(nameof(AppUserDto),
                    $"The property {nameof(AppUserDto.Password)} of the {nameof(AppUserDto)} object can't be null or empty");
            }

            var map = _mapper.Map<AppUser>(item);
            var rowsAffected = await _repository.DeleteAsync(map);

            return rowsAffected;
        }

        private async Task<int> UpdateInternalAsync(AppUserDto item)
        {
            if (string.IsNullOrEmpty(item.Email))
            {
                throw new ArgumentNullException(nameof(AppUserDto), 
                    $"The property {nameof(AppUserDto.Email)} of the {nameof(AppUserDto)} object can't be null or empty");
            }
            if (string.IsNullOrEmpty(item.Password))
            {
                throw new ArgumentNullException(nameof(AppUserDto), 
                    $"The property {nameof(AppUserDto.Password)} of the {nameof(AppUserDto)} object can't be null or empty");
            }

            var map = _mapper.Map<AppUser>(item);
            var rowsAffected = await _repository.UpdateAsync(map);

            return rowsAffected;
        }
    }
}
