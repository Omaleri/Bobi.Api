using Bobi.Api.Application.Contracts.DTO.RequestModel;
using Bobi.Api.Application.Contracts.DTO.ResponseModel;
using Bobi.Api.Application.Contracts.Interfaces;
using Bobi.Api.Application.Domain.Shared.Abstract;
using Bobi.Api.Domain.Address;
using Bobi.Api.MongoDb.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Bobi.Api.Application.Services
{
    public class ProvinceAppService : IProvinceAppService
    {
        private readonly ILogger<ProvinceAppService> _logger;
        private readonly IRepository<Province> _provinceRepository;

        public ProvinceAppService(ILogger<ProvinceAppService> logger, IRepository<Province> provinceRepository)
        {
            _logger = logger;
            _provinceRepository = provinceRepository;
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

        public async Task<BaseReturnModel<ProvinceResponseModel>> CreateAsync(ProvinceRequestModel item)
        {
            try
            {
                var province = new Province
                {
                    Name = item.Name,
                    Main = item.Main
                };
                var result = await _provinceRepository.CreateAsync(province);
                if (!result.IsSuccess)
                {
                    return HandleError<ProvinceResponseModel>("province create fault!");
                }
                return new BaseReturnModel<ProvinceResponseModel>
                {
                    Data = new ProvinceResponseModel
                    {
                        Name = result.Data.Name,
                        Main = result.Data.Main

                    }
                };
            }
            catch (Exception ex)
            {
                return HandleError<ProvinceResponseModel>(ex.Message);
            }
        }

        public async Task<BaseReturnModel<bool>> DeleteAsync(string id)
        {
            var result = await _provinceRepository.DeleteAsync(id);
            if (!result.IsSuccess)
            {
                return HandleError<bool>("Province delete fault!");
            }
            _logger.LogInformation($"Province deleted. Id:{id}");
            return new BaseReturnModel<bool> { Data = true };
        }

        public async Task<BaseReturnModel<ProvinceResponseModel>> GetByIdAsync(string id)
        {
            var result = await _provinceRepository.GetByIdAsync(id);
            if (!result.IsSuccess)
            {
                return HandleError<ProvinceResponseModel>("Province get fault!");
            }
            return new BaseReturnModel<ProvinceResponseModel>
            {
                Data = new ProvinceResponseModel
                {
                    Id = result.Data.Id.ToString(),
                    Name = result.Data.Name,
                    Main = result.Data.Main
                }
            };
        }

        public async Task<BaseReturnModel<List<ProvinceResponseModel>>> GetListAsync()
        {
            var result = await _provinceRepository.GetListAsync();
            if (!result.IsSuccess)
            {
                return HandleError<List<ProvinceResponseModel>>("Province list get fault!");

            }
            var filteredData = result.Data.Where(x => !x.IsDeleted).Select(x => new ProvinceResponseModel
            {
                Id = x.Id.ToString(),
                Name = x.Name,
                Main = x.Main
            }).ToList();

            return new BaseReturnModel<List<ProvinceResponseModel>>
            {
                Data = filteredData
            };
        }

        public async Task<BaseReturnModel<List<ProvinceResponseModel>>> GetListByFilterAsync(Expression<Func<Province, bool>> exp)
        {
            var result = await _provinceRepository.GetListByFilterAsync(exp);
            if (!result.IsSuccess)
            {
                return HandleError<List<ProvinceResponseModel>>("Province get fault!");
            }
            return new BaseReturnModel<List<ProvinceResponseModel>>
            {
                Data = result.Data.Select(x => new ProvinceResponseModel
                {
                    Id = x.Id.ToString(),
                    Name = x.Name,
                    Main = x.Main
                }).ToList(),
            };
        }

        public async Task<BaseReturnModel<ProvinceResponseModel>> UpdateAsync(ProvinceRequestModel item, string id)
        {
            try
            {
                var province = await _provinceRepository.GetByIdAsync(id);
                if (!province.IsSuccess)
                {
                    return HandleError<ProvinceResponseModel>("Province update fault!");
                }
                province.Data.Name = item.Name;
                province.Data.Main = item.Main;
                var result = await _provinceRepository.UpdateAsync(province.Data, id);
                if (!result.IsSuccess)
                {
                    return HandleError<ProvinceResponseModel>("Province update fault!");
                }
                return new BaseReturnModel<ProvinceResponseModel>
                {
                    Data = new ProvinceResponseModel
                    {
                        Id = result.Data.Id.ToString(),
                        Name = result.Data.Name,
                        Main = result.Data.Main
                    }
                };
            }
            catch (Exception)
            {
                return HandleError<ProvinceResponseModel>("Province update fault!");
            }
        }
    }
 }

