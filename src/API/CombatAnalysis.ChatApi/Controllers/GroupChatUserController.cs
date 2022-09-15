using AutoMapper;
using CombatAnalysis.BL.DTO.Chat;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.ChatApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CombatAnalysis.ChatApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class GroupChatUserController : ControllerBase
    {
        private readonly IService<GroupChatUserDto, int> _service;
        private readonly IMapper _mapper;

        public GroupChatUserController(IService<GroupChatUserDto, int> service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<GroupChatUserModel>> Get()
        {
            var result = await _service.GetAllAsync();
            var map = _mapper.Map<IEnumerable<GroupChatUserModel>>(result);

            return map;
        }

        [HttpGet("{id:int:min(1)}")]
        public async Task<GroupChatUserModel> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            var map = _mapper.Map<GroupChatUserModel>(result);

            return map;
        }

        [HttpPost]
        public async Task<GroupChatUserModel> Create(GroupChatUserModel model)
        {
            var map = _mapper.Map<GroupChatUserDto>(model);
            var result = await _service.CreateAsync(map);
            var resultMap = _mapper.Map<GroupChatUserModel>(result);

            return resultMap;
        }

        [HttpPut]
        public async Task<int> Update(GroupChatUserModel model)
        {
            var map = _mapper.Map<GroupChatUserDto>(model);
            var result = await _service.UpdateAsync(map);

            return result;
        }

        [HttpDelete]
        public async Task<int> Delete(GroupChatUserModel model)
        {
            var map = _mapper.Map<GroupChatUserDto>(model);
            var result = await _service.DeleteAsync(map);

            return result;
        }
    }
}
