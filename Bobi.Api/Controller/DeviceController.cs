using Bobi.Api.Application.Contracts.DTO.RequestModel;
using Bobi.Api.Application.Contracts.Interfaces;
using Bobi.Api.Domain.Build;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace Bobi.Api.Controller
{
    public class DeviceController : ControllerBase
    {
        private readonly ILogger<DeviceController> _logger;
        private readonly IDeviceAppService _deviceAppService;

        public DeviceController(ILogger<DeviceController> logger, IDeviceAppService deviceAppService)
        {
            _logger = logger;
            _deviceAppService = deviceAppService;
        }

        [HttpPost]
        [Route("api/device")]
        public async Task<IActionResult> CreateAsync(DeviceRequestModel requestModel)
        {
            var result = await _deviceAppService.CreateAsync(requestModel);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            _logger.LogError("Error creating device", requestModel);
            return StatusCode(result.Error[0].Code, result.Error);
        }

        [HttpDelete]
        [Route("api/device/{id}")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            var result = await _deviceAppService.DeleteAsync(id);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            _logger.LogError($"Error deleting device. Id: {id}");
            return StatusCode(result.Error[0].Code, result.Error);
        }

        [HttpGet]
        [Route("api/device/{id}")]
        public async Task<IActionResult> GetByIdAsync(string id)
        {
            var result = await _deviceAppService.GetByIdAsync(id);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            _logger.LogError($"Error getting device. Id: {id}");
            return StatusCode(result.Error[0].Code, result.Error);
        }

        [HttpPut]
        [Route("api/device/")]
        public async Task<IActionResult> UpdateAsync(DeviceRequestModel requestModel)
        {
            var result = await _deviceAppService.UpdateAsync(requestModel);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            _logger.LogError("Error updating device", requestModel);
            return StatusCode(result.Error[0].Code, result.Error);
        }

        [HttpGet]
        [Route("api/device/")]
        public async Task<IActionResult> GetListAsync()
        {
            var result = await _deviceAppService.GetListAsync();
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            _logger.LogError("Device get fault!");
            return StatusCode(result.Error[0].Code, result.Error);
        }
    }
}
