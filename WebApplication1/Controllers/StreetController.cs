﻿using Bobi.Api.Application.Contracts.DTO.RequestModel;
using Bobi.Api.Application.Contracts.Interfaces;
using Bobi.Api.Application.Services;
using Bobi.Api.Domain.Address;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace Bobi.Api.Controller
{
    public class StreetController : ControllerBase
    {
        private readonly ILogger<StreetController> _logger;
        private readonly IStreetAppService _streetAppService;

        public StreetController(ILogger<StreetController> logger, IStreetAppService streetAppService)
        {
            _logger = logger;
            _streetAppService = streetAppService;
        }

        [HttpPost]
        [Route("api/street")]
        public async Task<IActionResult> CreateAsync(StreetRequestModel requestModel)
        {
            var result = await _streetAppService.CreateAsync(requestModel);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            _logger.LogError("Error creating street", requestModel);
            return StatusCode(result.Error[0].Code, result.Error);
        }

        [HttpDelete]
        [Route("api/street/{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await _streetAppService.DeleteAsync(id);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            _logger.LogError($"Error deleting street. Id: {id}");
            return StatusCode(result.Error[0].Code, result.Error);
        }

        [HttpGet]
        [Route("api/street/{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var result = await _streetAppService.GetByIdAsync(id);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            _logger.LogError($"Error getting street. Id: {id}");
            return StatusCode(result.Error[0].Code, result.Error);
        }

        [HttpPut]
        [Route("api/streetUpdate")]
        public async Task<IActionResult> UpdateAsync(StreetRequestModel requestModel)
        {
            var result = await _streetAppService.UpdateAsync(requestModel);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            _logger.LogError("Error updating street", requestModel);
            return StatusCode(result.Error[0].Code, result.Error);
        }

        [HttpGet]
        [Route("api/filteredlist")]
        public async Task<IActionResult> GetListByFilterAsync(Expression<Func<Street, bool>> exp)
        {
            var result = await _streetAppService.GetListByFilterAsync(exp);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            _logger.LogError("Street get fault!");
            return StatusCode(result.Error[0].Code, result.Error);
        }

        [HttpGet]
        [Route("api/list")]
        public async Task<IActionResult> GetListAsync()
        {
            var result = await _streetAppService.GetListAsync();
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            _logger.LogError("Street get fault!");
            return StatusCode(result.Error[0].Code, result.Error);
        }
    }
}
