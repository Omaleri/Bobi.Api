using Bobi.Api.Application.Contracts.DTO.RequestModel;
using Bobi.Api.Application.Contracts.Interfaces;
using Bobi.Api.Domain.Address;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace Bobi.Api.Controller
{
    public class AddressController : ControllerBase
    {


        private readonly ILogger<AddressController> _logger;
        private readonly IAddressAppService _addressAppService;

        public AddressController(ILogger<AddressController> logger, IAddressAppService addressAppService)
        {
            _logger = logger;
            _addressAppService = addressAppService;
        }

        [HttpPost]
        [Route("api/address")]
        public async Task<IActionResult> CreateAsync(AddressRequestModel requestModel)
        {
            var result = await _addressAppService.CreateAsync(requestModel);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            _logger.LogError("Error creating Address", requestModel);
            return StatusCode(result.Error[0].Code, result.Error);
        }

        [HttpDelete]
        [Route("api/address/{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await _addressAppService.DeleteAsync(id);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            _logger.LogError($"Error deleting address. Id: {id}");
            return StatusCode(result.Error[0].Code, result.Error);
        }

        [HttpGet]
        [Route("api/address/{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var result = await _addressAppService.GetByIdAsync(id);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            _logger.LogError($"Error getting address. Id: {id}");
            return StatusCode(result.Error[0].Code, result.Error);
        }

        [HttpPut]
        [Route("api/addressUpdate")]
        public async Task<IActionResult> UpdateAsync(AddressRequestModel requestModel)
        {
            var result = await _addressAppService.UpdateAsync(requestModel);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            _logger.LogError("Error updating address", requestModel);
            return StatusCode(result.Error[0].Code, result.Error);
        }

        [HttpGet]
        [Route("api/filteredlist")]
        public async Task<IActionResult> GetListByFilterAsync(Expression<Func<Address, bool>> exp)
        {
            var result = await _addressAppService.GetListByFilterAsync(exp);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            _logger.LogError("Address get fault!");
            return StatusCode(result.Error[0].Code, result.Error);
        }

        [HttpGet]
        [Route("api/list")]
        public async Task<IActionResult> GetListAsync()
        {
            var result = await _addressAppService.GetListAsync();
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            _logger.LogError("Address get fault!");
            return StatusCode(result.Error[0].Code, result.Error);
        }
    }
}
