using Bobi.Api.Application.Contracts.DTO.RequestModel;
using Bobi.Api.Application.Contracts.Interfaces;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

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
        public async Task<IActionResult> DeleteAsync(string id)
        {
            var result = await _cityAppService.DeleteAsync(id);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            _logger.LogError($"Error deleting city. Id: {id}");
            return StatusCode(result.Error[0].Code, result.Error);
        }

        [HttpGet("api/city/GetByIdAsync/{id}")]
        public async Task<IActionResult> GetByIdAsync(string id)
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
        [Route("api/city/")]
        public async Task<IActionResult> UpdateAsync(CityRequestModel requestModel, string id)
        {
            var result = await _cityAppService.UpdateAsync(requestModel, id);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            _logger.LogError("Error updating city", requestModel);
            return StatusCode(result.Error[0].Code, result.Error);
        }

        [HttpGet]
        [Route("api/city/GetListAsync")]
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
