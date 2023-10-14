using Bobi.Api.Application.Contracts.DTO.RequestModel;
using Bobi.Api.Application.Contracts.Interfaces;
using Bobi.Api.Domain.Address;
using Bobi.Api.Domain.Build;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace Bobi.Api.Controller
{
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
        public async Task<IActionResult> DeleteAsync(int id)
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
        public async Task<IActionResult> GetByIdAsync(int id)
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
        [Route("api/voiceUpdate")]
        public async Task<IActionResult> UpdateAsync(VoiceRequestModel requestModel)
        {
            var result = await _voiceAppService.UpdateAsync(requestModel);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            _logger.LogError("Error updating voice", requestModel);
            return StatusCode(result.Error[0].Code, result.Error);
        }

        [HttpGet]
        [Route("api/filteredlist")]
        public async Task<IActionResult> GetListByFilterAsync(Expression<Func<Voice, bool>> exp)
        {
            var result = await _voiceAppService.GetListByFilterAsync(exp);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            _logger.LogError("Voice get fault!");
            return StatusCode(result.Error[0].Code, result.Error);
        }

        [HttpGet]
        [Route("api/list")]
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
