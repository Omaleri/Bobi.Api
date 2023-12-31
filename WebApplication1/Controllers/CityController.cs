﻿using Bobi.Api.Application.Contracts.DTO.RequestModel;
using Bobi.Api.Application.Contracts.Interfaces;
using Bobi.Api.Application.Services;
using Bobi.Api.Domain.Address;
using Bobi.Api.Domain.Build;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace Bobi.Api.Controller
{
    public class CityController : ControllerBase
    {
        private readonly ILogger<CityController> _logger;
        private readonly ICityAppService _cityAppService;

        public CityController(ILogger<CityController> logger, ICityAppService cityAppService)
        {
            _logger = logger;
            _cityAppService = cityAppService;
        }

        [HttpPost]
        [Route("api/city")]
        public async Task<IActionResult> CreateAsync(CityRequestModel requestModel)
        {
            var result = await _cityAppService.CreateAsync(requestModel);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            _logger.LogError("Error creating city", requestModel);
            return StatusCode(result.Error[0].Code, result.Error);
        }

        [HttpDelete]
        [Route("api/city/{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await _cityAppService.DeleteAsync(id);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            _logger.LogError($"Error deleting city. Id: {id}");
            return StatusCode(result.Error[0].Code, result.Error);
        }

        [HttpGet]
        [Route("api/city/{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var result = await _cityAppService.GetByIdAsync(id);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            _logger.LogError($"Error getting city. Id: {id}");
            return StatusCode(result.Error[0].Code, result.Error);
        }

        [HttpPut]
        [Route("api/cityUpdate")]
        public async Task<IActionResult> UpdateAsync(CityRequestModel requestModel)
        {
            var result = await _cityAppService.UpdateAsync(requestModel);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            _logger.LogError("Error updating city", requestModel);
            return StatusCode(result.Error[0].Code, result.Error);
        }

        [HttpGet]
        [Route("api/filteredlist")]
        public async Task<IActionResult> GetListByFilterAsync(Expression<Func<City, bool>> exp)
        {
            var result = await _cityAppService.GetListByFilterAsync(exp);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            _logger.LogError("City get fault!");
            return StatusCode(result.Error[0].Code, result.Error);
        }

        [HttpGet]
        [Route("api/list")]
        public async Task<IActionResult> GetListAsync()
        {
            var result = await _cityAppService.GetListAsync();
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            _logger.LogError("City get fault!");
            return StatusCode(result.Error[0].Code, result.Error);
        }
    }
}
