using Bobi.Api.Application.Contracts.DTO.RequestModel;
using Bobi.Api.Application.Contracts.DTO.ResponseModel;
using Bobi.Api.Application.Contracts.Interfaces;
using Bobi.Api.Application.Domain.Shared.Abstract;
using Bobi.Api.Domain.Address;
using Bobi.Api.Domain.Build;
using Bobi.Api.MongoDb.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using System;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;

namespace Bobi.Api.Application.Services
{
    public class BuildAppService : IBuildAppService
    {
        private readonly ILogger<BuildAppService> _logger;
        private readonly IRepository<Build> _buildRepository;
        private readonly IRepository<Device> _deviceRepository;
        private readonly IRepository<City> _cityRepository;
        private readonly IRepository<Province> _provinceRepository;
        private readonly IRepository<Number> _numberRepository;
        private readonly IRepository<Street> _streetRepository;
        private readonly IRepository<Town> _townRepository;


        public BuildAppService(ILogger<BuildAppService> logger, IRepository<Build> buildRepository, IRepository<Device> deviceRepository, IRepository<City> cityRepository, IRepository<Province> provinceRepository, IRepository<Number> numberRepository, IRepository<Street> streetRepository, IRepository<Town> townRepository)
        {
            _logger = logger;
            _buildRepository = buildRepository;
            _deviceRepository = deviceRepository;
            _cityRepository = cityRepository;
            _provinceRepository = provinceRepository;
            _numberRepository = numberRepository;
            _streetRepository = streetRepository;
            _townRepository = townRepository;
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
            #region Create Device
            var deviceList = item.Device
                .Where(deviceItem => deviceItem != null)
                .Select(deviceItem => new Device
                {
                    DeviceName = deviceItem.DeviceName
                })
                .ToList();

            var deviceResult = await _deviceRepository.CreateManyAsync(deviceList);
            if (!deviceResult.IsSuccess)
            {
                return HandleError<BuildResponseModel>("Create device fault!");
            }
            #endregion

            #region Create Build
            var build = new Build
            {
                CityId = item.CityId,
                ProvinceId = item.ProvinceId,
                TownId = item.TownId,
                StreetId = item.StreetId,
                NumberId = item.NumberId,
                DateOfDestructive = item.DateOfDestructive,
                NumberOfFloors = item.NumberOfFloors,
                Situation = item.Situation,
                TypeOfFeature = item.TypeOfFeature,
                Device = deviceList
            };
            var result = await _buildRepository.CreateAsync(build);
            if (!result.IsSuccess)
            {
                return HandleError<BuildResponseModel>("City create fault!");
            }
            #endregion

           

            return new BaseReturnModel<BuildResponseModel>
            {
                Data = new BuildResponseModel
                {
                    CityId = item.CityId,
                    ProvinceId = item.ProvinceId,
                    TownId = item.TownId,
                    StreetId = item.StreetId,
                    NumberId = item.NumberId,
                    DateOfDestructive = item.DateOfDestructive,
                    NumberOfFloors = item.NumberOfFloors,
                    Situation = item.Situation,
                    TypeOfFeature = item.TypeOfFeature,
                    Device = deviceList.Select(x => new DeviceResponseModel
                    {
                        Id = x.Id.ToString(),
                        DeviceName = x.DeviceName
                    }).ToList()
                }
            };
        }

        public async Task<BaseReturnModel<bool>> DeleteAsync(string id)
        {
            var buildResult = await _buildRepository.GetByIdAsync(id);
            if (buildResult.IsSuccess)
            {
                //var deviceResult = await _deviceRepository.GetByFilterAsync(x => x.BuildId == id.ToString());
                try
                {
                    //var device = await _deviceRepository.DeleteManyAsync(x => x.BuildId == id.ToString());
                    var build = await _buildRepository.DeleteAsync(id);
                }
                catch (Exception)
                {
                    _logger.LogError("Error while deleting Build", buildResult);
                }
            }

            else
            {
                return HandleError<bool>("Delete build fault!");
            }
            return new BaseReturnModel<bool> { Data = true };
        }

        public async Task<BaseReturnModel<BuildResponseModel>> GetByIdAsync(string id)
        {
            var build = await _buildRepository.GetByIdAsync(id);

            if (!build.IsSuccess)
            {
                return HandleError<BuildResponseModel>($"Build Id: {id} Not Found");
            }

            var device = await _deviceRepository.GetListByFilterAsync(x => x.Id.ToString() == id);

            return new BaseReturnModel<BuildResponseModel>
            {
                Data = new BuildResponseModel
                {

                    Id = build.Data.Id.ToString(),
                    CityId = build.Data.CityId,
                    ProvinceId = build.Data.ProvinceId,
                    TownId = build.Data.TownId,
                    StreetId = build.Data.StreetId,
                    NumberId = build.Data.NumberId,
                    DateOfDestructive = build.Data.DateOfDestructive,
                    Device = device.Data.Select(x => new DeviceResponseModel
                    {
                        DeviceName = x.DeviceName,
                        Id = x.Id.ToString()
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
            if (!device.IsSuccess)
            {
                return HandleError<List<BuildResponseModel>>("Device get list fault!");
            }

            var buildDevice = build.Data.Any();

            var filteredData = build.Data.Where(x => !x.IsDeleted).Select(x => new BuildResponseModel
            {
                Id = x.Id.ToString(),
                CityId = x.CityId,
                ProvinceId = x.ProvinceId,
                TownId = x.TownId,
                StreetId = x.StreetId,
                NumberId = x.NumberId,
                DateOfDestructive = x.DateOfDestructive,
                NumberOfFloors = x.NumberOfFloors,
                Situation = x.Situation,
                TypeOfFeature = x.TypeOfFeature,
                Device = device.Data
            .Join(x.Device, deviceFromList => deviceFromList.Id, buildDevice => buildDevice.Id, (deviceFromList, buildDevice) => new DeviceResponseModel
            {
                DeviceName = deviceFromList.DeviceName,
                Id = deviceFromList.Id.ToString()
            })
            .ToList(),
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
            var device = await _deviceRepository.GetListByFilterAsync(x => selectedBuild.Id == x.Id);
            return new BaseReturnModel<List<BuildResponseModel>>
            {
                Data = buildList.Data.Select(x => new BuildResponseModel
                {
                    Id = x.Id.ToString(),
                    CityId = x.CityId,
                    ProvinceId = x.ProvinceId,
                    TownId = x.TownId,
                    StreetId = x.StreetId,
                    NumberId = x.NumberId,
                    DateOfDestructive = x.DateOfDestructive,
                    NumberOfFloors = x.NumberOfFloors,
                    Situation = x.Situation,
                    TypeOfFeature = x.TypeOfFeature,
                    Device = device.Data.Select(x => new DeviceResponseModel
                    {
                        DeviceName = x.DeviceName,
                        Id = x.Id.ToString(),
                    }).ToList(),
                }).ToList(),
            };
        }

        public async Task<BaseReturnModel<BuildResponseModel>> UpdateAsync(BuildRequestModel item,string id)
        {
            try
            {
                var device = await _deviceRepository.GetListAsync();
                if (!device.IsSuccess)
                {
                    return HandleError<BuildResponseModel>("Device get list fault!");
                }

                var build = await _buildRepository.GetByIdAsync(id);
                if (!build.IsSuccess)
                {
                    return HandleError<BuildResponseModel>("Build update fault!");
                }

                Console.Write("Lütfen cihaz adını girin: ");
                string desiredDeviceName = Console.ReadLine();

                // Alınan cihaz adına göre cihazı bul
                var selectedDevice = device.Data.FirstOrDefault(x => x.DeviceName == desiredDeviceName);

                if (selectedDevice != null)
                {
                    // Cihazı build içerisine yaz
                    build.Data.Device = new List<Device> { selectedDevice };
                }
                else
                {
                    return HandleError<BuildResponseModel>("Belirtilen cihaz adına sahip bir cihaz bulunamadı!");
                }


                /*build.Data.CityId = item.CityId;
                build.Data.ProvinceId = item.ProvinceId;
                build.Data.TownId = item.TownId;
                build.Data.StreetId = item.StreetId;
                build.Data.NumberId = item.NumberId;
                build.Data.NumberOfFloors = item.NumberOfFloors;
                build.Data.TypeOfFeature = item.TypeOfFeature;
                build.Data.DateOfDestructive = item.DateOfDestructive;
                build.Data.Situation = item.Situation;*/
                
                var result = await _buildRepository.UpdateAsync(build.Data, id);
                if (!result.IsSuccess)
                {
                    return HandleError<BuildResponseModel>("Build update fault!");
                }
                return new BaseReturnModel<BuildResponseModel>
                {
                    Data = new BuildResponseModel
                    {
                        Id = result.Data.Id.ToString(),
                        CityId = result.Data.CityId,
                        ProvinceId = result.Data.ProvinceId,
                        TownId = result.Data.TownId,
                        StreetId = result.Data.StreetId,
                        NumberId = result.Data.NumberId,
                        DateOfDestructive = result.Data.DateOfDestructive,
                        Situation = result.Data.Situation,
                        TypeOfFeature = result.Data.TypeOfFeature,
                        NumberOfFloors = result.Data.NumberOfFloors,
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
