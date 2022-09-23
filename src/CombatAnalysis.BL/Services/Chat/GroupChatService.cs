using AutoMapper;
using CombatAnalysis.BL.DTO.Chat;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.DAL.Entities.Chat;
using CombatAnalysis.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CombatAnalysis.BL.Services.Chat
{
    internal class GroupChatService : IService<GroupChatDto, int>
    {
        private readonly IGenericRepository<GroupChat, int> _repository;
        private readonly IMapper _mapper;

        public GroupChatService(IGenericRepository<GroupChat, int> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        Task<GroupChatDto> IService<GroupChatDto, int>.CreateAsync(GroupChatDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(GroupChatDto), $"The {nameof(GroupChatDto)} can't be null");
            }

            return CreateInternalAsync(item);
        }

        Task<int> IService<GroupChatDto, int>.DeleteAsync(GroupChatDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(GroupChatDto), $"The {nameof(GroupChatDto)} can't be null");
            }

            return DeleteInternalAsync(item);
        }

        async Task<IEnumerable<GroupChatDto>> IService<GroupChatDto, int>.GetAllAsync()
        {
            var allData = await _repository.GetAllAsync();
            var result = _mapper.Map<IEnumerable<GroupChatDto>>(allData);

            return result;
        }

        async Task<GroupChatDto> IService<GroupChatDto, int>.GetByIdAsync(int id)
        {
            var result = await _repository.GetByIdAsync(id);
            var resultMap = _mapper.Map<GroupChatDto>(result);

            return resultMap;
        }

        async Task<IEnumerable<GroupChatDto>> IService<GroupChatDto, int>.GetByParamAsync(string paramName, object value)
        {
            var result = await Task.Run(() => _repository.GetByParam(paramName, value));
            var resultMap = _mapper.Map<IEnumerable<GroupChatDto>>(result);

            return resultMap;
        }

        Task<int> IService<GroupChatDto, int>.UpdateAsync(GroupChatDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(GroupChatDto), $"The {nameof(GroupChatDto)} can't be null");
            }

            return UpdateInternalAsync(item);
        }

        private async Task<GroupChatDto> CreateInternalAsync(GroupChatDto item)
        {
            if (string.IsNullOrEmpty(item.ShortName))
            {
                throw new ArgumentNullException(nameof(GroupChatDto), 
                    $"The property {nameof(GroupChatDto.ShortName)} of the {nameof(GroupChatDto)} object can't be null or empty");
            }
            if (string.IsNullOrEmpty(item.Name))
            {
                throw new ArgumentNullException(nameof(GroupChatDto), 
                    $"The property {nameof(GroupChatDto.Name)} of the {nameof(GroupChatDto)} object can't be null or empty");
            }
            if (string.IsNullOrEmpty(item.LastMessage))
            {
                throw new ArgumentNullException(nameof(GroupChatDto), 
                    $"The property {nameof(GroupChatDto.LastMessage)} of the {nameof(GroupChatDto)} object can't be null or empty");
            }

            var map = _mapper.Map<GroupChat>(item);
            var createdItem = await _repository.CreateAsync(map);
            var resultMap = _mapper.Map<GroupChatDto>(createdItem);

            return resultMap;
        }

        private async Task<int> DeleteInternalAsync(GroupChatDto item)
        {
            if (string.IsNullOrEmpty(item.ShortName))
            {
                throw new ArgumentNullException(nameof(GroupChatDto), 
                    $"The property {nameof(GroupChatDto.ShortName)} of the {nameof(GroupChatDto)} object can't be null or empty");
            }
            if (string.IsNullOrEmpty(item.Name))
            {
                throw new ArgumentNullException(nameof(GroupChatDto), 
                    $"The property {nameof(GroupChatDto.Name)} of the {nameof(GroupChatDto)} object can't be null or empty");
            }
            if (string.IsNullOrEmpty(item.LastMessage))
            {
                throw new ArgumentNullException(nameof(GroupChatDto), 
                    $"The property {nameof(GroupChatDto.LastMessage)} of the {nameof(GroupChatDto)} object can't be null or empty");
            }

            var map = _mapper.Map<GroupChat>(item);
            var rowsAffected = await _repository.DeleteAsync(map);

            return rowsAffected;
        }

        private async Task<int> UpdateInternalAsync(GroupChatDto item)
        {
            if (string.IsNullOrEmpty(item.ShortName))
            {
                throw new ArgumentNullException(nameof(GroupChatDto), 
                    $"The property {nameof(GroupChatDto.ShortName)} of the {nameof(GroupChatDto)} object can't be null or empty");
            }
            if (string.IsNullOrEmpty(item.Name))
            {
                throw new ArgumentNullException(nameof(GroupChatDto), 
                    $"The property {nameof(GroupChatDto.Name)} of the {nameof(GroupChatDto)} object can't be null or empty");
            }
            if (string.IsNullOrEmpty(item.LastMessage))
            {
                throw new ArgumentNullException(nameof(GroupChatDto), 
                    $"The property {nameof(GroupChatDto.LastMessage)} of the {nameof(GroupChatDto)} object can't be null or empty");
            }

            var map = _mapper.Map<GroupChat>(item);
            var rowsAffected = await _repository.UpdateAsync(map);

            return rowsAffected;
        }
    }
}
