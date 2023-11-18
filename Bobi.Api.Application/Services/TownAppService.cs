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
    public class TownAppService : ITownAppService
    {

        private readonly ILogger<TownAppService> _logger;
        private readonly IRepository<Town> _townRepository;

        public TownAppService(ILogger<TownAppService> logger, IRepository<Town> townRepository)
        {
            _logger = logger;
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

        public async Task<BaseReturnModel<TownResponseModel>> CreateAsync(TownRequestModel item)
        {
            try
            {
                var town = new Town
                {
                    Name = item.Name,
                    Main = item.Main
                };
                var result = await _townRepository.CreateAsync(town);
                if (!result.IsSuccess)
                {
                    return HandleError<TownResponseModel>("Town create fault!");
                }
                return new BaseReturnModel<TownResponseModel>
                {
                    Data = new TownResponseModel
                    {
                        Name = result.Data.Name,
                         Main = result.Data.Main
                    }
                };
            }
            catch (Exception ex)
            {
                return HandleError<TownResponseModel>(ex.Message);
            }
        }

        public async Task<BaseReturnModel<bool>> DeleteAsync(string id)
        {
            var result = await _townRepository.DeleteAsync(id);
            if (!result.IsSuccess)
            {
                return HandleError<bool>("Town delete fault!");
            }
            _logger.LogInformation($"Town deleted. Id:{id}");
            return new BaseReturnModel<bool> { Data = true };
        }

        public async Task<BaseReturnModel<TownResponseModel>> GetByIdAsync(string id)
        {
            var result = await _townRepository.GetByIdAsync(id);
            if (!result.IsSuccess)
            {
                return HandleError<TownResponseModel>("Town get fault!");
            }
            return new BaseReturnModel<TownResponseModel>
            {
                Data = new TownResponseModel
                {
                    Name = result.Data.Name,
                    Main = result.Data.Main
                }
            };
        }

        public async Task<BaseReturnModel<List<TownResponseModel>>> GetListAsync()
        {
            var result = await _townRepository.GetListAsync();
            if (!result.IsSuccess)
            {
                return HandleError<List<TownResponseModel>>("Town list get fault!");
            }
            var filteredData = result.Data.Where(x => !x.IsDeleted).Select(x => new TownResponseModel
            {
                Id = x.Id.ToString(),
                Name = x.Name,
                Main = x.Main
            }).ToList();

            return new BaseReturnModel<List<TownResponseModel>>
            {
                Data = filteredData
            };
        }

        public async Task<BaseReturnModel<List<TownResponseModel>>> GetListByFilterAsync(Expression<Func<Town, bool>> exp)
        {
            var result = await _townRepository.GetListByFilterAsync(exp);
            if (!result.IsSuccess)
            {
                return HandleError<List<TownResponseModel>>("Town get fault!");
            }
            return new BaseReturnModel<List<TownResponseModel>>
            {
                Data = result.Data.Select(x => new TownResponseModel
                {
                    Name = x.Name,
                    Main = x.Main
                }).ToList(),
            };
        }

        public async Task<BaseReturnModel<TownResponseModel>> UpdateAsync(TownRequestModel item, string id)
        {
            try
            {
                var town = await _townRepository.GetByIdAsync(id);
                if (!town.IsSuccess)
                {
                    return HandleError<TownResponseModel>("Town update fault!");
                }
                town.Data.Name = item.Name;
                town.Data.Main = item.Main;

                var result = await _townRepository.UpdateAsync(town.Data, id);
                if (!result.IsSuccess)
                {
                    return HandleError<TownResponseModel>("Town update fault!");
                }
                return new BaseReturnModel<TownResponseModel>
                {
                    Data = new TownResponseModel
                    {
                        Name = result.Data.Name,
                        Main = result.Data.Main
                    }
                };
            }
            catch (Exception)
            {
                return HandleError<TownResponseModel>("Town update fault!");
            }
        }
    }
}
