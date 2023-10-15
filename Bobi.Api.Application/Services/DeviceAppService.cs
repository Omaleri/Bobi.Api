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
    public class DeviceAppService : IDeviceAppService
    {
        private readonly ILogger<DeviceAppService> _logger;
        private readonly IRepository<Device> _deviceRepository;

        public DeviceAppService(ILogger<DeviceAppService> logger, IRepository<Device> deviceRepository)
        {
            _logger = logger;
            _deviceRepository = deviceRepository;
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

        public async Task<BaseReturnModel<DeviceResponseModel>> CreateAsync(DeviceRequestModel item)
        {
            try
            {
                var device = new Device
                {
                    BuildId = item.BuildId,
                    DeviceName = item.DeviceName,
                };
                var result = await _deviceRepository.CreateAsync(device);
                if (!result.IsSuccess)
                {
                    return HandleError<DeviceResponseModel>("Device create fault!");
                }
                return new BaseReturnModel<DeviceResponseModel>
                {
                    Data = new DeviceResponseModel
                    {
                        BuildId = result.Data.BuildId,
                        DeviceName = result.Data.DeviceName
                    }
                };
            }
            catch (Exception ex)
            {
                return HandleError<DeviceResponseModel>(ex.Message);
            }
        }

        public async Task<BaseReturnModel<bool>> DeleteAsync(int id)
        {
            var result = await _deviceRepository.DeleteAsync(id);
            if (!result.IsSuccess)
            {
                return HandleError<bool>("Device delete fault!");
            }
            _logger.LogInformation($"Device deleted. Id:{id}");
            return new BaseReturnModel<bool> { Data = true };
        }

        public async Task<BaseReturnModel<DeviceResponseModel>> GetByIdAsync(int id)
        {
            var result = await _deviceRepository.GetByIdAsync(id);
            if (!result.IsSuccess)
            {
                return HandleError<DeviceResponseModel>("Device get fault!");
            }
            return new BaseReturnModel<DeviceResponseModel>
            {
                Data = new DeviceResponseModel
                {
                    Id = result.Data.Id,
                    BuildId = result.Data.BuildId,
                    DeviceName = result.Data.DeviceName
                }
            };
        }

        public async Task<BaseReturnModel<List<DeviceResponseModel>>> GetListAsync()
        {
            var result = await _deviceRepository.GetListAsync();
            if (!result.IsSuccess)
            {
                return HandleError<List<DeviceResponseModel>>("Device list get fault!");

            }
            var filteredData = result.Data.Where(x => !x.IsDeleted).Select(x => new DeviceResponseModel
            {
                Id = x.Id,
                BuildId = x.BuildId,
                DeviceName = x.DeviceName
            }).ToList();

            return new BaseReturnModel<List<DeviceResponseModel>>
            {
                Data = filteredData
            };
        }

        public async Task<BaseReturnModel<List<DeviceResponseModel>>> GetListByFilterAsync(Expression<Func<Device, bool>> exp)
        {
            var result = await _deviceRepository.GetListByFilterAsync(exp);
            if (!result.IsSuccess)
            {
                return HandleError<List<DeviceResponseModel>>("Device get fault!");
            }
            return new BaseReturnModel<List<DeviceResponseModel>>
            {
                Data = result.Data.Select(x => new DeviceResponseModel
                {
                    Id = x.Id,
                    BuildId = x.BuildId,
                    DeviceName = x.DeviceName
                }).ToList(),
            };
        }

        public async Task<BaseReturnModel<DeviceResponseModel>> UpdateAsync(DeviceRequestModel item)
        {
            try
            {
                var device = await _deviceRepository.GetByIdAsync(item.Id);
                if (!device.IsSuccess)
                {
                    return HandleError<DeviceResponseModel>("Device update fault!");
                }
                device.Data.DeviceName = item.DeviceName;
                device.Data.BuildId = item.BuildId;
                var result = await _deviceRepository.UpdateAsync(device.Data);
                if (!result.IsSuccess)
                {
                    return HandleError<DeviceResponseModel>("Device update fault!");
                }
                return new BaseReturnModel<DeviceResponseModel>
                {
                    Data = new DeviceResponseModel
                    {
                        Id = result.Data.Id,
                        BuildId = result.Data.BuildId,
                        DeviceName = result.Data.DeviceName
                    }
                };
            }
            catch (Exception)
            {
                return HandleError<DeviceResponseModel>("Device update fault!");
            }
        }
    }
}
