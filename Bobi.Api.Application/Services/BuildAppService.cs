using Bobi.Api.Application.Contracts.DTO.RequestModel;
using Bobi.Api.Application.Contracts.DTO.ResponseModel;
using Bobi.Api.Application.Contracts.Interfaces;
using Bobi.Api.Application.Domain.Shared.Abstract;
using Bobi.Api.Domain.Build;
using Bobi.Api.MongoDb.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using System;
using System.Linq.Expressions;

namespace Bobi.Api.Application.Services
{
    public class BuildAppService : IBuildAppService
    {
        private readonly ILogger<BuildAppService> _logger;
        private readonly IRepository<Build> _buildRepository;
        private readonly IRepository<Device> _deviceRepository;

        public BuildAppService(ILogger<BuildAppService> logger, IRepository<Build> buildRepository, IRepository<Device> deviceRepository)
        {
            _logger = logger;
            _buildRepository = buildRepository;
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

        public async Task<BaseReturnModel<BuildResponseModel>> CreateAsync(BuildRequestModel item)
        {
            #region Create Build
            var build = new Build
            {
                AddressId = item.AddressId,
                DateOfDestructive = item.DateOfDestructive,
                NumberOfFloors = item.NumberOfFloors,
                Situation = item.Situation,
                TypeOfFeature = item.TypeOfFeature
            };
            var result = await _buildRepository.CreateAsync(build);
            if (!result.IsSuccess)
            {
                return HandleError<BuildResponseModel>("City create fault!");
            }

            #endregion

            #region Create Device
            var deviceList = new List<Device>();
            foreach (var deviceItem in item.Device)
            {
                if (deviceItem != null)
                {
                    var device = new Device
                    {
                        BuildId = deviceItem.BuildId.ToString(),
                        DeviceName = deviceItem.DeviceName
                    };
                    deviceList.Add(device);
                }
                var deviceResult = await _deviceRepository.CreateManyAsync(deviceList);
                if (!deviceResult.IsSuccess)
                {
                    return HandleError<BuildResponseModel>("Create device fault!");
                }
            }
            #endregion        
            
            return new BaseReturnModel<BuildResponseModel>
            {
                Data = new BuildResponseModel
                {
                    AddressId = item.AddressId,
                    DateOfDestructive = item.DateOfDestructive,
                    NumberOfFloors = item.NumberOfFloors,
                    Situation = item.Situation,
                    TypeOfFeature = item.TypeOfFeature,
                    Device = deviceList.Select(x => new DeviceResponseModel
                    {
                        BuildId = build.Id.ToString(),
                        DeviceName = x.DeviceName
                    }).ToList(),
                }
            };
        }

        public async Task<BaseReturnModel<bool>> DeleteAsync(int id)
        {
            var buildResult = await _buildRepository.GetByIdAsync(id);
            if (buildResult.IsSuccess)
            {
                var deviceResult = await _deviceRepository.GetByFilterAsync(x => x.BuildId == id.ToString());
                try
                {
                    var device = await _deviceRepository.DeleteManyAsync(x => x.BuildId == id.ToString());
                    var build = await _buildRepository.DeleteAsync(id);
                }
                catch (Exception)
                {
                    _logger.LogError("Error while deleting coin", buildResult);
                }
            }

            else
            {
                return HandleError<bool>("Delete device fault!");
            }
            return new BaseReturnModel<bool> { Data = true };
        }

        public async Task<BaseReturnModel<BuildResponseModel>> GetByIdAsync(int id)
        {
            var build = await _buildRepository.GetByIdAsync(id);

            if (!build.IsSuccess)
            {
                return HandleError<BuildResponseModel>($"Build Id: {id} Not Found");
            }

            var device = await _deviceRepository.GetListByFilterAsync(x => x.BuildId == id.ToString());

            return new BaseReturnModel<BuildResponseModel>
            {
                Data = new BuildResponseModel
                {

                    Id = build.Data.Id,
                    AddressId = build.Data.AddressId,
                    DateOfDestructive = build.Data.DateOfDestructive,
                    Device = device.Data.Select(x => new DeviceResponseModel
                    {
                        Id = x.Id,
                        BuildId = build.Data.Id.ToString(),
                        DeviceName = x.DeviceName
                    }).ToList(),
                    NumberOfFloors = build.Data.NumberOfFloors,
                    Situation = build.Data.Situation,
                    TypeOfFeature = build.Data.TypeOfFeature
                }
            };
        }
        public async Task<BaseReturnModel<List<BuildResponseModel>>> GetListAsync()
        {
            var build = await _buildRepository.GetListAsync();
            if (!build.IsSuccess)
            {
                return HandleError<List<BuildResponseModel>>("Build get list fault!");
            }

            var device = await _deviceRepository.GetListAsync();
            if (!build.IsSuccess)
            {
                return HandleError<List<BuildResponseModel>>("Device get list fault!");
            }

            var filteredData = build.Data.Where(x => !x.IsDeleted).Select(x => new BuildResponseModel
            {
                Id = x.Id,
                AddressId = x.AddressId,
                DateOfDestructive = x.DateOfDestructive,
                NumberOfFloors = x.NumberOfFloors,
                Situation = x.Situation,
                TypeOfFeature = x.TypeOfFeature,
                Device = device.Data.Select(x => new DeviceResponseModel
                {
                    BuildId = x.BuildId,
                    DeviceName = x.DeviceName,
                    Id = x.Id,
                }).ToList(),
            }).ToList();

            return new BaseReturnModel<List<BuildResponseModel>>
            {
                Data = filteredData
            };
        }

        public async Task<BaseReturnModel<List<BuildResponseModel>>> GetListByFilterAsync(Expression<Func<Build, bool>> exp)
        {
            var buildList = await _buildRepository.GetListByFilterAsync(exp);
            if (!buildList.IsSuccess)
            {
                return HandleError<List<BuildResponseModel>>("Build get filtered list fault!");
            }

            var selectedBuild = buildList.Data.First();
            var device = await _deviceRepository.GetListByFilterAsync(x => x.BuildId == selectedBuild.Id.ToString());
            return new BaseReturnModel<List<BuildResponseModel>>
            {
                Data = buildList.Data.Select(x => new BuildResponseModel
                {
                    Id = x.Id,
                    AddressId = x.AddressId,
                    DateOfDestructive = x.DateOfDestructive,
                    NumberOfFloors = x.NumberOfFloors,
                    Situation = x.Situation,
                    TypeOfFeature = x.TypeOfFeature,
                    Device = device.Data.Select(x => new DeviceResponseModel
                    {
                        BuildId = x.BuildId,
                        DeviceName = x.DeviceName,
                        Id = x.Id,
                    }).ToList(),
                }).ToList(),
            };
        }

        public async Task<BaseReturnModel<BuildResponseModel>> UpdateAsync(BuildRequestModel item)
        {
            try
            {
                // ObjectId'i int'e çevirelim, eğer çevrilemezse hata verelim
                if (!int.TryParse(item.Id.ToString(), out int idAsInt))
                {
                    return HandleError<BuildResponseModel>("Invalid int format!");
                }

                var build = await _buildRepository.GetByIdAsync(idAsInt);
                if (!build.IsSuccess)
                {
                    return HandleError<BuildResponseModel>("Build update fault!");
                }

                build.Data.AddressId = item.AddressId;
                build.Data.NumberOfFloors = item.NumberOfFloors;
                build.Data.TypeOfFeature = item.TypeOfFeature;
                build.Data.DateOfDestructive = item.DateOfDestructive;
                build.Data.Situation = item.Situation;
                var result = await _buildRepository.UpdateAsync(build.Data);
                if (!result.IsSuccess)
                {
                    return HandleError<BuildResponseModel>("Build update fault!");
                }
                return new BaseReturnModel<BuildResponseModel>
                {
                    Data = new BuildResponseModel
                    {
                        Id = result.Data.Id,
                        AddressId = result.Data.AddressId,
                        DateOfDestructive = result.Data.DateOfDestructive,
                        Situation = result.Data.Situation,
                        TypeOfFeature = result.Data.TypeOfFeature,
                        NumberOfFloors = result.Data.NumberOfFloors
                    }
                };
            }
            catch (Exception)
            {
                return HandleError<BuildResponseModel>("Build update fault!");
            }
        }
    }
}
