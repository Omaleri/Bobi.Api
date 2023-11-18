using Bobi.Api.Application.Contracts.DTO.RequestModel;
using Bobi.Api.Application.Contracts.DTO.ResponseModel;
using Bobi.Api.Application.Contracts.Interfaces;
using Bobi.Api.Application.Domain.Shared.Abstract;
using Bobi.Api.Domain.Build;
using Bobi.Api.MongoDb.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Bobi.Api.Application.Services
{
    public class VoiceAppService : IVoiceAppService
    {

        private readonly ILogger<VoiceAppService> _logger;
        private readonly IRepository<Voice> _voiceRepository;

        public VoiceAppService(ILogger<VoiceAppService> logger, IRepository<Voice> voiceRepository)
        {
            _logger = logger;
            _voiceRepository = voiceRepository;
        }

        private BaseReturnModel<T> HandleError<T>(string errorMessage)
        {
            _logger.LogError(errorMessage);
            return new BaseReturnModel<T>
            {
                Error = new List<ErrorModel>
                {
                    new ErrorModel(ErrorCodes.ProcessNotCompleted, errorMessage)
                }
            };
        }


        public async Task<BaseReturnModel<VoiceResponseModel>> CreateAsync(VoiceRequestModel item)
        {
            try
            {
                var voice = new Voice
                {
                    BuildId = item.BuildId,
                    Link = item.Link,
                    VoiceDate = item.VoiceDate,
                    VoiceTime = item.VoiceTime
                };
                var result = await _voiceRepository.CreateAsync(voice);
                if (!result.IsSuccess)
                {
                    return HandleError<VoiceResponseModel>("Voice create fault!");
                }
                return new BaseReturnModel<VoiceResponseModel>
                {
                    Data = new VoiceResponseModel
                    {
                        Id = item.Id,
                        BuildId = result.Data.BuildId,
                        Link = result.Data.Link,
                        VoiceTime = result.Data.VoiceTime,
                        VoiceDate = result.Data.VoiceDate
                    }
                };
            }
            catch (Exception ex)
            {
                return HandleError<VoiceResponseModel>(ex.Message);
            }
        }

        public async Task<BaseReturnModel<bool>> DeleteAsync(string id)
        {
            var result = await _voiceRepository.DeleteAsync(id);
            if (!result.IsSuccess)
            {
                return HandleError<bool>("Voice delete fault!");
            }
            _logger.LogInformation($"Voice deleted. Id:{id}");
            return new BaseReturnModel<bool> { Data = true };
        }

        public async Task<BaseReturnModel<VoiceResponseModel>> GetByIdAsync(string id)
        {
            var result = await _voiceRepository.GetByIdAsync(id);
            if (!result.IsSuccess)
            {
                return HandleError<VoiceResponseModel>("Voice get fault!");
            }
            return new BaseReturnModel<VoiceResponseModel>
            {
                Data = new VoiceResponseModel
                {
                    BuildId = result.Data.BuildId,
                    Link = result.Data.Link,
                    VoiceDate = result.Data.VoiceDate,
                    VoiceTime = result.Data.VoiceTime
                }
            };
        }

        public async Task<BaseReturnModel<List<VoiceResponseModel>>> GetListAsync()
        {
            var result = await _voiceRepository.GetListAsync();
            if (!result.IsSuccess)
            {
                return HandleError<List<VoiceResponseModel>>("Voice list get fault!");

            }
            var filteredData = result.Data.Where(x => !x.IsDeleted).Select(x => new VoiceResponseModel
            {
                Id = x.Id.ToString(),
                BuildId = x.BuildId,
                Link = x.Link,
                VoiceDate = x.VoiceDate,
                VoiceTime = x.VoiceTime
            }).ToList();

            return new BaseReturnModel<List<VoiceResponseModel>>
            {
                Data = filteredData
            };
        }

        public async Task<BaseReturnModel<List<VoiceResponseModel>>> GetListByFilterAsync(Expression<Func<Voice, bool>> exp)
        {
            var result = await _voiceRepository.GetListByFilterAsync(exp);
            if (!result.IsSuccess)
            {
                return HandleError<List<VoiceResponseModel>>("Voice get fault!");
            }
            return new BaseReturnModel<List<VoiceResponseModel>>
            {
                Data = result.Data.Select(x => new VoiceResponseModel
                {
                    BuildId = x.BuildId,
                    Link = x.Link,
                    VoiceDate = x.VoiceDate,
                    VoiceTime = x.VoiceTime
                }).ToList(),
            };
        }

        public async Task<BaseReturnModel<VoiceResponseModel>> UpdateAsync(VoiceRequestModel item, string id)
        {
            try
            {
                var voice = await _voiceRepository.GetByIdAsync(id);
                if (!voice.IsSuccess)
                {
                    return HandleError<VoiceResponseModel>("Voice update fault!");
                }
                voice.Data.BuildId = item.BuildId;
                voice.Data.Link = item.Link;
                voice.Data.VoiceDate = item.VoiceDate;
                voice.Data.VoiceTime = item.VoiceTime;
                var result = await _voiceRepository.UpdateAsync(voice.Data, id);
                if (!result.IsSuccess)
                {
                    return HandleError<VoiceResponseModel>("Voice update fault!");
                }
                return new BaseReturnModel<VoiceResponseModel>
                {
                    Data = new VoiceResponseModel
                    {
                        BuildId = result.Data.BuildId,
                        Link = result.Data.Link,
                        VoiceDate = result.Data.VoiceDate,
                        VoiceTime = result.Data.VoiceTime
                    }
                };
            }
            catch (Exception)
            {
                return HandleError<VoiceResponseModel>("Voice update fault!");
            }
        }
    }
}
