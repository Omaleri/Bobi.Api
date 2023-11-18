using Bobi.Api.Application.Contracts.DTO.RequestModel;
using Bobi.Api.Application.Contracts.Interfaces;
using Bobi.Api.Domain.Address;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace Bobi.Api.Controller
{
    public class ProvinceController : ControllerBase
    {
        private readonly ILogger<ProvinceController> _logger;
        private readonly IProvinceAppService _provinceAppService;

        public ProvinceController(ILogger<ProvinceController> logger, IProvinceAppService provinceAppService)
        {
            _logger = logger;
            _provinceAppService = provinceAppService;
        }

        [HttpPost]
        [Route("api/province")]
        public async Task<IActionResult> CreateAsync(ProvinceRequestModel requestModel)
        {
            var result = await _provinceAppService.CreateAsync(requestModel);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            _logger.LogError("Error creating province", requestModel);
            return StatusCode(result.Error[0].Code, result.Error);
        }

        [HttpDelete]
        [Route("api/province/{id}")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            var result = await _provinceAppService.DeleteAsync(id);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            _logger.LogError($"Error deleting province. Id: {id}");
            return StatusCode(result.Error[0].Code, result.Error);
        }

        [HttpGet]
        [Route("api/province/{id}")]
        public async Task<IActionResult> GetByIdAsync(string id)
        {
            var result = await _provinceAppService.GetByIdAsync(id);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            _logger.LogError($"Error getting province. Id: {id}");
            return StatusCode(result.Error[0].Code, result.Error);
        }

        [HttpPut]
        [Route("api/province/")]
        public async Task<IActionResult> UpdateAsync(ProvinceRequestModel requestModel, string id)
        {
            var result = await _provinceAppService.UpdateAsync(requestModel, id);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            _logger.LogError("Error updating province", requestModel);
            return StatusCode(result.Error[0].Code, result.Error);
        }

        [HttpGet]
        [Route("api/province/GetListAsync")]
        public async Task<IActionResult> GetListAsync()
        {
            var result = await _provinceAppService.GetListAsync();
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            _logger.LogError("Province get fault!");
            return StatusCode(result.Error[0].Code, result.Error);
        }
    }
}
