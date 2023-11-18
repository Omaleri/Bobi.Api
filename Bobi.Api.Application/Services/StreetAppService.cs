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
    public class StreetAppService : IStreetAppService
    {

        private readonly ILogger<StreetAppService> _logger;
        private readonly IRepository<Street> _streetRepository;

        public StreetAppService(ILogger<StreetAppService> logger, IRepository<Street> streetRepository)
        {
            _logger = logger;
            _streetRepository = streetRepository;
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

        public async Task<BaseReturnModel<StreetResponseModel>> CreateAsync(StreetRequestModel item)
        {
            try
            {
                var street = new Street
                {
                    Name = item.Name,
                    Main = item.Main
                };
                var result = await _streetRepository.CreateAsync(street);
                if (!result.IsSuccess)
                {
                    return HandleError<StreetResponseModel>("Street create fault!");
                }
                return new BaseReturnModel<StreetResponseModel>
                {
                    Data = new StreetResponseModel
                    {
                        Name = result.Data.Name,
                        Main = result.Data.Main

                    }
                };
            }
            catch (Exception ex)
            {
                return HandleError<StreetResponseModel>(ex.Message);
            }
        }

        public async Task<BaseReturnModel<bool>> DeleteAsync(string id)
        {
            var result = await _streetRepository.DeleteAsync(id);
            if (!result.IsSuccess)
            {
                return HandleError<bool>("Street delete fault!");
            }
            _logger.LogInformation($"Street deleted. Id:{id}");
            return new BaseReturnModel<bool> { Data = true };
        }

        public async Task<BaseReturnModel<StreetResponseModel>> GetByIdAsync(string id)
        {
            var result = await _streetRepository.GetByIdAsync(id);
            if (!result.IsSuccess)
            {
                return HandleError<StreetResponseModel>("Street get fault!");
            }
            return new BaseReturnModel<StreetResponseModel>
            {
                Data = new StreetResponseModel
                {
                    Id = result.Data.Id.ToString(),
                    Name = result.Data.Name,
                    Main = result.Data.Main
                }
            };
        }

        public async Task<BaseReturnModel<List<StreetResponseModel>>> GetListAsync()
        {
            var result = await _streetRepository.GetListAsync();
            if (!result.IsSuccess)
            {
                return HandleError<List<StreetResponseModel>>("Street list get fault!");

            }
            var filteredData = result.Data.Where(x => !x.IsDeleted).Select(x => new StreetResponseModel
            {
                Id = x.Id.ToString(),
                Name = x.Name,
                Main = x.Main
            }).ToList();

            return new BaseReturnModel<List<StreetResponseModel>>
            {
                Data = filteredData
            };
        }

        public async Task<BaseReturnModel<List<StreetResponseModel>>> GetListByFilterAsync(Expression<Func<Street, bool>> exp)
        {
            var result = await _streetRepository.GetListByFilterAsync(exp);
            if (!result.IsSuccess)
            {
                return HandleError<List<StreetResponseModel>>("Street get fault!");
            }
            return new BaseReturnModel<List<StreetResponseModel>>
            {
                Data = result.Data.Select(x => new StreetResponseModel
                {
                    Id = x.Id.ToString(),
                    Name = x.Name,
                    Main = x.Main
                }).ToList(),
            };
        }

        public async Task<BaseReturnModel<StreetResponseModel>> UpdateAsync(StreetRequestModel item, string id)
        {
            try
            {
                var street = await _streetRepository.GetByIdAsync(id);
                if (!street.IsSuccess)
                {
                    return HandleError<StreetResponseModel>("Street update fault!");
                }
                street.Data.Name = item.Name;
                street.Data.Main = item.Main;
                var result = await _streetRepository.UpdateAsync(street.Data, id);
                if (!result.IsSuccess)
                {
                    return HandleError<StreetResponseModel>("Street update fault!");
                }
                return new BaseReturnModel<StreetResponseModel>
                {
                    Data = new StreetResponseModel
                    {
                        Id = result.Data.Id.ToString(),
                        Name = result.Data.Name,
                        Main = result.Data.Main
            }
                };
            }
            catch (Exception)
            {
                return HandleError<StreetResponseModel>("Street update fault!");
            }
        }
    }
}
