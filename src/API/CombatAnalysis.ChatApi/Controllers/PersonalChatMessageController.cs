﻿using AutoMapper;
using CombatAnalysis.BL.DTO.Chat;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.ChatApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CombatAnalysis.ChatApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PersonalChatMessageController : ControllerBase
    {
        private readonly IService<PersonalChatMessageDto, int> _service;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public PersonalChatMessageController(IService<PersonalChatMessageDto, int> service, IMapper mapper, ILogger logger)
        {
            _service = service;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            var map = _mapper.Map<IEnumerable<PersonalChatMessageModel>>(result);

            return Ok(map);
        }

        [HttpGet("{id:int:min(1)}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            var map = _mapper.Map<PersonalChatMessageModel>(result);

            return Ok(map);
        }

        [HttpPost]
        public async Task<IActionResult> Create(PersonalChatMessageModel model)
        {
            try
            {
                var map = _mapper.Map<PersonalChatMessageDto>(model);
                var result = await _service.CreateAsync(map);
                var resultMap = _mapper.Map<PersonalChatMessageModel>(result);

                return Ok(resultMap);
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, ex.Message);

                return BadRequest();
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update(PersonalChatMessageModel model)
        {
            try
            {
                var map = _mapper.Map<PersonalChatMessageDto>(model);
                var result = await _service.UpdateAsync(map);

                return Ok(result);
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, ex.Message);

                return BadRequest();
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(PersonalChatMessageModel model)
        {
            try
            {
                var map = _mapper.Map<PersonalChatMessageDto>(model);
                var result = await _service.DeleteAsync(map);

                return Ok(result);
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, ex.Message);

                return BadRequest();
            }
        }
    }
}