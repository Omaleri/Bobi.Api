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
    public class NumberAppService : INumberAppService
    {
        private readonly ILogger<NumberAppService> _logger;
        private readonly IRepository<Number> _numberRepository;

        public NumberAppService(ILogger<NumberAppService> logger, IRepository<Number> numberRepository)
        {
            _logger = logger;
            _numberRepository = numberRepository;
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


        public async Task<BaseReturnModel<NumberResponseModel>> CreateAsync(NumberRequestModel item)
        {
            try
            {
                var number = new Number
                {
                    Name = item.Name,
                    Main = item.Main
                };
                var result = await _numberRepository.CreateAsync(number);
                if (!result.IsSuccess)
                {
                    return HandleError<NumberResponseModel>("Number create fault!");
                }
                return new BaseReturnModel<NumberResponseModel>
                {
                    Data = new NumberResponseModel
                    {
                        Name = result.Data.Name,
                        Main = result.Data.Main

                    }
                };
            }
            catch (Exception ex)
            {
                return HandleError<NumberResponseModel>(ex.Message);
            }
        }

        public async Task<BaseReturnModel<bool>> DeleteAsync(string id)
        {
            var result = await _numberRepository.DeleteAsync(id);
            if (!result.IsSuccess)
            {
                return HandleError<bool>("Number delete fault!");
            }
            _logger.LogInformation($"Number deleted. Id:{id}");
            return new BaseReturnModel<bool> { Data = true };
        }

        public async Task<BaseReturnModel<NumberResponseModel>> GetByIdAsync(string id)
        {
            var result = await _numberRepository.GetByIdAsync(id);
            if (!result.IsSuccess)
            {
                return HandleError<NumberResponseModel>("Number get fault!");
            }
            return new BaseReturnModel<NumberResponseModel>
            {
                Data = new NumberResponseModel
                {
                    Id = result.Data.Id.ToString(),
                    Name = result.Data.Name,
                    Main = result.Data.Main
                }
            };
        }

        public async Task<BaseReturnModel<List<NumberResponseModel>>> GetListAsync()
        {
            var result = await _numberRepository.GetListAsync();
            if (!result.IsSuccess)
            {
                return HandleError<List<NumberResponseModel>>("Number list get fault!");

            }
            var filteredData = result.Data.Where(x => !x.IsDeleted).Select(x => new NumberResponseModel
            {
                Id = x.Id.ToString(),
                Name = x.Name,
                Main = x.Main
            }).ToList();

            return new BaseReturnModel<List<NumberResponseModel>>
            {
                Data = filteredData
            };
        }

        public async Task<BaseReturnModel<List<NumberResponseModel>>> GetListByFilterAsync(Expression<Func<Number, bool>> exp)
        {
            var result = await _numberRepository.GetListByFilterAsync(exp);
            if (!result.IsSuccess)
            {
                return HandleError<List<NumberResponseModel>>("Number get fault!");
            }
            return new BaseReturnModel<List<NumberResponseModel>>
            {
                Data = result.Data.Select(x => new NumberResponseModel
                {
                    Id = x.Id.ToString(),
                    Name = x.Name,
                    Main = x.Main
                }).ToList(),
            };
        }

        public async Task<BaseReturnModel<NumberResponseModel>> UpdateAsync(NumberRequestModel item, string id)
        {
            try
            {
                var number = await _numberRepository.GetByIdAsync(id);
                if (!number.IsSuccess)
                {
                    return HandleError<NumberResponseModel>("Number update fault!");
                }
                number.Data.Name = item.Name;
                number.Data.Main = item.Main;
                var result = await _numberRepository.UpdateAsync(number.Data, id);
                if (!result.IsSuccess)
                {
                    return HandleError<NumberResponseModel>("Number update fault!");
                }
                return new BaseReturnModel<NumberResponseModel>
                {
                    Data = new NumberResponseModel
                    {
                        Id = result.Data.Id.ToString(),
                        Name = result.Data.Name,
                        Main = result.Data.Main
                    }
                };
            }
            catch (Exception)
            {
                return HandleError<NumberResponseModel>("Number update fault!");
            }
        }
    }
}
