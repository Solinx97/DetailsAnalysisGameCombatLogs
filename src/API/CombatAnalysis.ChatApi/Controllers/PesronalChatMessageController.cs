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
    public class PesronalChatMessageController : ControllerBase
    {
        private readonly IService<PersonalChatMessageDto, int> _service;
        private readonly IMapper _mapper;

        public PesronalChatMessageController(IService<PersonalChatMessageDto, int> service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<PersonalChatMessageModel>> Get()
        {
            var result = await _service.GetAllAsync();
            var map = _mapper.Map<IEnumerable<PersonalChatMessageModel>>(result);

            return map;
        }

        [HttpGet("{id:int:min(1)}")]
        public async Task<PersonalChatMessageModel> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            var map = _mapper.Map<PersonalChatMessageModel>(result);

            return map;
        }

        [HttpPost]
        public async Task<PersonalChatMessageModel> Create(PersonalChatMessageModel model)
        {
            var map = _mapper.Map<PersonalChatMessageDto>(model);
            var result = await _service.CreateAsync(map);
            var resultMap = _mapper.Map<PersonalChatMessageModel>(result);

            return resultMap;
        }

        [HttpPut]
        public async Task<int> Update(PersonalChatMessageModel model)
        {
            var map = _mapper.Map<PersonalChatMessageDto>(model);
            var result = await _service.UpdateAsync(map);

            return result;
        }

        [HttpDelete]
        public async Task<int> Delete(PersonalChatMessageModel model)
        {
            var map = _mapper.Map<PersonalChatMessageDto>(model);
            var result = await _service.DeleteAsync(map);

            return result;
        }
    }
}
