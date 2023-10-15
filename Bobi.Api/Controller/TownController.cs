//using Bobi.Api.Application.Contracts.DTO.RequestModel;
//using Bobi.Api.Application.Contracts.Interfaces;
//using Bobi.Api.Domain.Address;
//using Microsoft.AspNetCore.Mvc;
//using System.Linq.Expressions;

//namespace Bobi.Api.Controller
//{
//    public class TownController : ControllerBase
//    {
//        private readonly ILogger<TownController> _logger;
//        private readonly ITownAppService _townAppService;

//        public TownController(ILogger<TownController> logger, ITownAppService townAppService)
//        {
//            _logger = logger;
//            _townAppService = townAppService;
//        }

//        [HttpPost]
//        [Route("api/town")]
//        public async Task<IActionResult> CreateAsync(TownRequestModel requestModel)
//        {
//            var result = await _townAppService.CreateAsync(requestModel);
//            if (result.IsSuccess)
//            {
//                return Ok(result.Data);
//            }
//            _logger.LogError("Error creating town", requestModel);
//            return StatusCode(result.Error[0].Code, result.Error);
//        }

//        [HttpDelete]
//        [Route("api/town/{id}")]
//        public async Task<IActionResult> DeleteAsync(int id)
//        {
//            var result = await _townAppService.DeleteAsync(id);
//            if (result.IsSuccess)
//            {
//                return Ok(result.Data);
//            }
//            _logger.LogError($"Error deleting town. Id: {id}");
//            return StatusCode(result.Error[0].Code, result.Error);
//        }

//        [HttpGet]
//        [Route("api/town/{id}")]
//        public async Task<IActionResult> GetByIdAsync(int id)
//        {
//            var result = await _townAppService.GetByIdAsync(id);
//            if (result.IsSuccess)
//            {
//                return Ok(result.Data);
//            }
//            _logger.LogError($"Error getting town. Id: {id}");
//            return StatusCode(result.Error[0].Code, result.Error);
//        }

//        [HttpPut]
//        [Route("api/townUpdate")]
//        public async Task<IActionResult> UpdateAsync(TownRequestModel requestModel)
//        {
//            var result = await _townAppService.UpdateAsync(requestModel);
//            if (result.IsSuccess)
//            {
//                return Ok(result.Data);
//            }
//            _logger.LogError("Error updating town", requestModel);
//            return StatusCode(result.Error[0].Code, result.Error);
//        }

//        [HttpGet]
//        [Route("api/list")]
//        public async Task<IActionResult> GetListAsync()
//        {
//            var result = await _townAppService.GetListAsync();
//            if (result.IsSuccess)
//            {
//                return Ok(result.Data);
//            }
//            _logger.LogError("Town get fault!");
//            return StatusCode(result.Error[0].Code, result.Error);
//        }
//    }
//}
