﻿using Bobi.Api.Application.Contracts.DTO.RequestModel;
using Bobi.Api.Application.Contracts.Interfaces;
using Bobi.Api.Domain.Address;
using Bobi.Api.Domain.Build;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace Bobi.Api.Controller
{
    public class BuildController : ControllerBase
    {
        private readonly ILogger<BuildController> _logger;
        private readonly IBuildAppService _buildAppService;

        public BuildController(ILogger<BuildController> logger, IBuildAppService buildAppService)
        {
            _logger = logger;
            _buildAppService = buildAppService;
        }

        [HttpPost]
        [Route("api/build")]
        public async Task<IActionResult> CreateAsync(BuildRequestModel requestModel)
        {
            var result = await _buildAppService.CreateAsync(requestModel);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            _logger.LogError("Error creating build", requestModel);
            return StatusCode(result.Error[0].Code, result.Error);
        }

        [HttpDelete]
        [Route("api/build/{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await _buildAppService.DeleteAsync(id);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            _logger.LogError($"Error deleting build. Id: {id}");
            return StatusCode(result.Error[0].Code, result.Error);
        }

        [HttpGet]
        [Route("api/build/{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var result = await _buildAppService.GetByIdAsync(id);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            _logger.LogError($"Error getting build. Id: {id}");
            return StatusCode(result.Error[0].Code, result.Error);
        }

        [HttpPut]
        [Route("api/buildUpdate")]
        public async Task<IActionResult> UpdateAsync(BuildRequestModel requestModel)
        {
            var result = await _buildAppService.UpdateAsync(requestModel);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            _logger.LogError("Error updating build", requestModel);
            return StatusCode(result.Error[0].Code, result.Error);
        }

        [HttpGet]
        [Route("api/filteredlist")]
        public async Task<IActionResult> GetListByFilterAsync(Expression<Func<Build, bool>> exp)
        {
            var result = await _buildAppService.GetListByFilterAsync(exp);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            _logger.LogError("Build get fault!");
            return StatusCode(result.Error[0].Code, result.Error);
        }

        [HttpGet]
        [Route("api/list")]
        public async Task<IActionResult> GetListAsync()
        {
            var result = await _buildAppService.GetListAsync();
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            _logger.LogError("Build get fault!");
            return StatusCode(result.Error[0].Code, result.Error);
        }
    }
}
