using Bobi.Api.Application.Contracts.DTO.RequestModel;
using Bobi.Api.Application.Contracts.Interfaces;
using Bobi.Api.Domain.Address;
using Bobi.Api.Domain.Build;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace Bobi.Api.Controller
{
    public class NumberController : ControllerBase
    {
        private readonly ILogger<NumberController> _logger;
        private readonly INumberAppService _numberAppService;

        public NumberController(ILogger<NumberController> logger, INumberAppService numberAppService)
        {
            _logger = logger;
            _numberAppService = numberAppService;
        }

        [HttpPost]
        [Route("api/number")]
        public async Task<IActionResult> CreateAsync(NumberRequestModel requestModel)
        {
            var result = await _numberAppService.CreateAsync(requestModel);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            _logger.LogError("Error creating number", requestModel);
            return StatusCode(result.Error[0].Code, result.Error);
        }

        [HttpDelete]
        [Route("api/number/{id}")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            var result = await _numberAppService.DeleteAsync(id);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            _logger.LogError($"Error deleting number. Id: {id}");
            return StatusCode(result.Error[0].Code, result.Error);
        }

        [HttpGet]
        [Route("api/number/{id}")]
        public async Task<IActionResult> GetByIdAsync(string id)
        {
            var result = await _numberAppService.GetByIdAsync(id);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            _logger.LogError($"Error getting number. Id: {id}");
            return StatusCode(result.Error[0].Code, result.Error);
        }

        [HttpPut]
        [Route("api/number/")]
        public async Task<IActionResult> UpdateAsync(NumberRequestModel requestModel)
        {
            var result = await _numberAppService.UpdateAsync(requestModel);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            _logger.LogError("Error updating number", requestModel);
            return StatusCode(result.Error[0].Code, result.Error);
        }

        [HttpGet]
        [Route("api/number/GetListAsync")]
        public async Task<IActionResult> GetListAsync()
        {
            var result = await _numberAppService.GetListAsync();
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            _logger.LogError("Number get fault!");
            return StatusCode(result.Error[0].Code, result.Error);
        }
    }
}
