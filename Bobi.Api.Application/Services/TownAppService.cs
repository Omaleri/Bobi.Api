﻿using Bobi.Api.Application.Contracts.DTO.RequestModel;
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
                    Name = item.Name
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
                        Name = result.Data.Name
                    }
                };
            }
            catch (Exception ex)
            {
                return HandleError<TownResponseModel>(ex.Message);
            }
        }

        public async Task<BaseReturnModel<bool>> DeleteAsync(int id)
        {
            var result = await _townRepository.DeleteAsync(id);
            if (!result.IsSuccess)
            {
                return HandleError<bool>("Town delete fault!");
            }
            _logger.LogInformation($"Town deleted. Id:{id}");
            return new BaseReturnModel<bool> { Data = true };
        }

        public async Task<BaseReturnModel<TownResponseModel>> GetByIdAsync(int id)
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
                    Id = result.Data.Id,
                    Name = result.Data.Name
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
                Id = x.Id,
                Name = x.Name
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
                    Id = x.Id,
                    Name = x.Name
                }).ToList(),
            };
        }

        public async Task<BaseReturnModel<TownResponseModel>> UpdateAsync(TownRequestModel item)
        {
            try
            {
                var report = await _townRepository.GetByIdAsync(item.Id);
                if (!report.IsSuccess)
                {
                    return HandleError<TownResponseModel>("Town update fault!");
                }
                report.Data.Name = item.Name;
                var result = await _townRepository.UpdateAsync(report.Data);
                if (!result.IsSuccess)
                {
                    return HandleError<TownResponseModel>("Town update fault!");
                }
                return new BaseReturnModel<TownResponseModel>
                {
                    Data = new TownResponseModel
                    {
                        Id = result.Data.Id,
                        Name = result.Data.Name
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