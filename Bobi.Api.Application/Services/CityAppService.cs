using Bobi.Api.Application.Contracts.DTO.RequestModel;
using Bobi.Api.Application.Contracts.DTO.ResponseModel;
using Bobi.Api.Application.Contracts.Interfaces;
using Bobi.Api.Application.Domain.Shared.Abstract;
using Bobi.Api.Domain.Address;
using Bobi.Api.EntityFrameworkCore.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Bobi.Api.Application.Services
{
    public class CityAppService : ICityAppService
    {
        private readonly ILogger<CityAppService> _logger;
        private readonly IRepository<City> _cityRepository;

        public CityAppService(ILogger<CityAppService> logger, IRepository<City> cityRepository)
        {
            _logger = logger;
            _cityRepository = cityRepository;
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

        public async Task<BaseReturnModel<CityResponseModel>> CreateAsync(CityRequestModel item)
        {
            try
            {
                var city = new City
                {
                    Name = item.Name
                };
                var result = await _cityRepository.CreateAsync(city);
                if (!result.IsSuccess)
                {
                    return HandleError<CityResponseModel>("City create fault!");
                }
                return new BaseReturnModel<CityResponseModel>
                {
                    Data = new CityResponseModel
                    {
                        Name = result.Data.Name
                    }
                };
            }
            catch (Exception ex)
            {
                return HandleError<CityResponseModel>(ex.Message);
            }
        }

        public async Task<BaseReturnModel<bool>> DeleteAsync(int id)
        {
            var result = await _cityRepository.DeleteAsync(id);
            if (!result.IsSuccess)
            {
                return HandleError<bool>("City delete fault!");
            }
            _logger.LogInformation($"City deleted. Id:{id}");
            return new BaseReturnModel<bool> { Data = true };
        }

        public async Task<BaseReturnModel<CityResponseModel>> GetByIdAsync(int id)
        {
            var result = await _cityRepository.GetByIdAsync(id);
            if (!result.IsSuccess)
            {
                return HandleError<CityResponseModel>("City get fault!");
            }
            return new BaseReturnModel<CityResponseModel>
            {
                Data = new CityResponseModel
                {
                    Id = result.Data.Id,
                    Name = result.Data.Name
                }
            };
        }

        public async Task<BaseReturnModel<List<CityResponseModel>>> GetListAsync()
        {
            var result = await _cityRepository.GetListAsync();
            if (!result.IsSuccess)
            {
                return HandleError<List<CityResponseModel>>("City list get fault!");

            }
            var filteredData = result.Data.Where(x => !x.IsDeleted).Select(x => new CityResponseModel
            {
                Id = x.Id,
                Name = x.Name
            }).ToList();

            return new BaseReturnModel<List<CityResponseModel>>
            {
                Data = filteredData
            };
        }

        public async Task<BaseReturnModel<List<CityResponseModel>>> GetListByFilterAsync(Expression<Func<City, bool>> exp)
        {
            var result = await _cityRepository.GetListByFilterAsync(exp);
            if (!result.IsSuccess)
            {
                return HandleError<List<CityResponseModel>>("City get fault!");
            }
            return new BaseReturnModel<List<CityResponseModel>>
            {
                Data = result.Data.Select(x => new CityResponseModel
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToList(),
            };
        }

        public async Task<BaseReturnModel<CityResponseModel>> UpdateAsync(CityRequestModel item)
        {
            try
            {
                var build = await _cityRepository.GetByIdAsync(item.Id);
                if (!build.IsSuccess)
                {
                    return HandleError<CityResponseModel>("City update fault!");
                }
                build.Data.Name = item.Name;
                var result = await _cityRepository.UpdateAsync(build.Data);
                if (!result.IsSuccess)
                {
                    return HandleError<CityResponseModel>("City update fault!");
                }
                return new BaseReturnModel<CityResponseModel>
                {
                    Data = new CityResponseModel
                    {
                        Id = result.Data.Id,
                        Name = result.Data.Name
                    }
                };
            }
            catch (Exception)
            {
                return HandleError<CityResponseModel>("City update fault!");
            }
        }
    }
}
