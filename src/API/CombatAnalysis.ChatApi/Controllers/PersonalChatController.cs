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
    public class PersonalChatController : ControllerBase
    {
        private readonly IService<PersonalChatDto, int> _service;
        private readonly IMapper _mapper;

        public PersonalChatController(IService<PersonalChatDto, int> service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<PersonalChatModel>> Get()
        {
            var result = await _service.GetAllAsync();
            var map = _mapper.Map<IEnumerable<PersonalChatModel>>(result);

            return map;
        }

        [HttpGet("{id:int:min(1)}")]
        public async Task<PersonalChatModel> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            var map = _mapper.Map<PersonalChatModel>(result);

            return map;
        }

        [HttpPost]
        public async Task<PersonalChatModel> Create(PersonalChatModel model)
        {
            var map = _mapper.Map<PersonalChatDto>(model);
            var result = await _service.CreateAsync(map);
            var resultMap = _mapper.Map<PersonalChatModel>(result);

            return resultMap;
        }

        [HttpPut]
        public async Task<int> Update(PersonalChatModel model)
        {
            var map = _mapper.Map<PersonalChatDto>(model);
            var result = await _service.UpdateAsync(map);

            return result;
        }

        [HttpDelete]
        public async Task<int> Delete(PersonalChatModel model)
        {
            var map = _mapper.Map<PersonalChatDto>(model);
            var result = await _service.DeleteAsync(map);

            return result;
        }
    }
}
