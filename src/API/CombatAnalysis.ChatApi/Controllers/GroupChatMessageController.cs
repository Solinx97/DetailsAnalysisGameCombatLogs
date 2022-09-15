using AutoMapper;
using CombatAnalysis.BL.DTO.Chat;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.ChatApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CombatAnalysis.ChatApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class GroupChatMessageController : ControllerBase
    {
        private readonly IService<GroupChatMessageDto, int> _service;
        private readonly IMapper _mapper;

        public GroupChatMessageController(IService<GroupChatMessageDto, int> service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<GroupChatMessageModel>> Get()
        {
            var result = await _service.GetAllAsync();
            var map = _mapper.Map<IEnumerable<GroupChatMessageModel>>(result);

            return map;
        }

        [HttpGet("{id:int:min(1)}")]
        public async Task<GroupChatMessageModel> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            var map = _mapper.Map<GroupChatMessageModel>(result);

            return map;
        }

        [HttpPost]
        public async Task<GroupChatMessageModel> Create(GroupChatMessageModel model)
        {
            var map = _mapper.Map<GroupChatMessageDto>(model);
            var result = await _service.CreateAsync(map);
            var resultMap = _mapper.Map<GroupChatMessageModel>(result);

            return resultMap;
        }

        [HttpPut]
        public async Task<int> Update(GroupChatMessageModel model)
        {
            var map = _mapper.Map<GroupChatMessageDto>(model);
            var result = await _service.UpdateAsync(map);

            return result;
        }

        [HttpDelete]
        public async Task<int> Delete(GroupChatMessageModel model)
        {
            var map = _mapper.Map<GroupChatMessageDto>(model);
            var result = await _service.DeleteAsync(map);

            return result;
        }
    }
}
