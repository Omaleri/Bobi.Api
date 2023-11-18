using Bobi.Api.Application.Contracts.DTO.RequestModel;
using Bobi.Api.Application.Contracts.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Bobi.Api.Controller
{
    [Route("api/voice")]
    [ApiController]
    public class VoiceController : ControllerBase
    {
        private readonly ILogger<VoiceController> _logger;
        private readonly IVoiceAppService _voiceAppService;

        public VoiceController(ILogger<VoiceController> logger, IVoiceAppService voiceAppService)
        {
            _logger = logger;
            _voiceAppService = voiceAppService;
        }

        [HttpPost]
        [Route("api/voice")]
        public async Task<IActionResult> CreateAsync(VoiceRequestModel requestModel)
        {
            var result = await _voiceAppService.CreateAsync(requestModel);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            _logger.LogError("Error creating voice", requestModel);
            return StatusCode(result.Error[0].Code, result.Error);
        }

        [HttpDelete]
        [Route("api/voice/{id}")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            var result = await _voiceAppService.DeleteAsync(id);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            _logger.LogError($"Error deleting voice. Id: {id}");
            return StatusCode(result.Error[0].Code, result.Error);
        }

        [HttpGet]
        [Route("api/voice/{id}")]
        public async Task<IActionResult> GetByIdAsync(string id)
        {
            var result = await _voiceAppService.GetByIdAsync(id);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            _logger.LogError($"Error getting voice. Id: {id}");
            return StatusCode(result.Error[0].Code, result.Error);
        }

        [HttpPut]
        [Route("api/voice/")]
        public async Task<IActionResult> UpdateAsync(VoiceRequestModel requestModel, string id)
        {
            var result = await _voiceAppService.UpdateAsync(requestModel, id);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            _logger.LogError("Error updating voice", requestModel);
            return StatusCode(result.Error[0].Code, result.Error);
        }

        [HttpGet("GetListAsync")]
        public async Task<IActionResult> GetListAsync()
        {
            var result = await _voiceAppService.GetListAsync();
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            _logger.LogError("Voice get fault!");
            return StatusCode(result.Error[0].Code, result.Error);
        }
    }
}
