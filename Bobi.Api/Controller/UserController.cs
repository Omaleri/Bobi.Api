using Bobi.Api.Application.Contracts.DTO.RequestModel;
using Bobi.Api.Application.Contracts.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Bobi.Api.Controller
{
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserAppService _userAppService;

        public UserController(ILogger<UserController> logger, IUserAppService userAppService)
        {
            _logger = logger;
            _userAppService = userAppService;
        }

        [HttpPost]
        [Route("api/user")]
        public async Task<IActionResult> CreateAsync(UserRequestModel requestModel)
        {
            var result = await _userAppService.CreateAsync(requestModel);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            _logger.LogError("Error creating user", requestModel);
            return StatusCode(result.Error[0].Code, result.Error);
        }

        [HttpDelete]
        [Route("api/user/{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await _userAppService.DeleteAsync(id);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            _logger.LogError($"Error deleting user. Id: {id}");
            return StatusCode(result.Error[0].Code, result.Error);
        }

        [HttpGet]
        [Route("api/user/{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var result = await _userAppService.GetByIdAsync(id);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            _logger.LogError($"Error getting user. Id: {id}");
            return StatusCode(result.Error[0].Code, result.Error);
        }

        [HttpPut]
        [Route("api/user/")]
        public async Task<IActionResult> UpdateAsync(UserRequestModel requestModel)
        {
            var result = await _userAppService.UpdateAsync(requestModel);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            _logger.LogError("Error updating user", requestModel);
            return StatusCode(result.Error[0].Code, result.Error);
        }
    }
}
