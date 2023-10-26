using Bobi.Api.Application.Contracts.DTO.RequestModel;
using Bobi.Api.Application.Contracts.DTO.ResponseModel;
using Bobi.Api.Application.Contracts.Interfaces;
using Bobi.Api.Application.Domain.Shared.Abstract;
using Bobi.Api.Domain.Address;
using Bobi.Api.Domain.User;
using Bobi.Api.MongoDb.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using System.Linq.Expressions;

namespace Bobi.Api.Application.Services
{

    public class AddressAppService : IAddressAppService
    {
        private readonly ILogger<AddressAppService> _logger;
        private readonly IRepository<Address> _addressRepository;
        private readonly IRepository<City> _cityRepository;
        private readonly IRepository<Number> _numberRepository;
        private readonly IRepository<Province> _provinceRepository;
        private readonly IRepository<Street> _streetRepository;
        private readonly IRepository<Town> _townRepository;

        public AddressAppService(ILogger<AddressAppService> logger, IRepository<Address> addressRepository, IRepository<Street> streetRepository, IRepository<City> cityRepository, IRepository<Number> numberRepository, IRepository<Province> provinceRepository, IRepository<Town> townRepository)
        {
            _logger = logger;
            _addressRepository = addressRepository;
            _streetRepository = streetRepository;
            _cityRepository = cityRepository;
            _numberRepository = numberRepository;
            _provinceRepository = provinceRepository;
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

        public async Task<BaseReturnModel<AddressResponseModel>> CreateAsync(AddressRequestModel item)
        {
            #region Get Street
            ObjectId streetObjectId;
            if (!ObjectId.TryParse(item.StreetId.ToString(), out streetObjectId))
            {
                return HandleError<AddressResponseModel>("Invalid StreetId format!");
            }
            var street = await _streetRepository.GetByFilterAsync(x => x.Id == streetObjectId);
            if (!street.IsSuccess)
            {
                return HandleError<AddressResponseModel>("Get street fault!");
            }
            #endregion

            #region Get City
            ObjectId cityObjectId;
            if (!ObjectId.TryParse(item.CityId.ToString(), out cityObjectId))
            {
                return HandleError<AddressResponseModel>("Invalid StreetId format!");
            }
            var city = await _cityRepository.GetByFilterAsync(x => x.Id == cityObjectId);
            if (!city.IsSuccess)
            {
                return HandleError<AddressResponseModel>("Get city fault!");
            }
            #endregion

            #region Get Number
            ObjectId numberObjectId;
            if (!ObjectId.TryParse(item.NumberId.ToString(), out numberObjectId))
            {
                return HandleError<AddressResponseModel>("Invalid StreetId format!");
            }
            var number = await _numberRepository.GetByFilterAsync(x => x.Id == numberObjectId);
            if (!number.IsSuccess)
            {
                return HandleError<AddressResponseModel>("Get number fault!");
            }
            #endregion

            #region Get Province
            ObjectId provinceObjectId;
            if (!ObjectId.TryParse(item.ProvinceId.ToString(), out provinceObjectId))
            {
                return HandleError<AddressResponseModel>("Invalid StreetId format!");
            }
            var province = await _provinceRepository.GetByFilterAsync(x => x.Id == provinceObjectId);
            if (!province.IsSuccess)
            {
                return HandleError<AddressResponseModel>("Get Province fault!");
            }
            #endregion

            #region Get Town
            ObjectId townObjectId;
            if (!ObjectId.TryParse(item.TownId.ToString(), out townObjectId))
            {
                return HandleError<AddressResponseModel>("Invalid StreetId format!");
            }
            var town = await _townRepository.GetByFilterAsync(x => x.Id == townObjectId);
            if (!town.IsSuccess)
            {
                return HandleError<AddressResponseModel>("Get number fault!");
            }
            #endregion

            try
            {
                var address = new Address
                {
                    CityId = item.CityId,
                    NumberId = item.NumberId,
                    OpenAddress = item.OpenAddress,
                    ProvinceId = item.ProvinceId,
                    StreetId = item.StreetId,
                    TownId = item.TownId
                };
                var result = await _addressRepository.CreateAsync(address);
                if (!result.IsSuccess)
                {
                    return HandleError<AddressResponseModel>("City create fault!");
                }
                return new BaseReturnModel<AddressResponseModel>
                {
                    Data = new AddressResponseModel
                    {
                        CityId = city.Data.Name,
                        NumberId = number.Data.Name,
                        ProvinceId = province.Data.Name,
                        StreetId = street.Data.Name,
                        TownId = town.Data.Name,
                        OpenAddress = item.OpenAddress,
                        Id = item.Id,
                    }
                };
            }
            catch (Exception ex)
            {
                return HandleError<AddressResponseModel>(ex.Message);
            }
        }

        public async Task<BaseReturnModel<bool>> DeleteAsync(string id)
        {
            var addressResult = await _addressRepository.DeleteAsync(id);
            if (!addressResult.IsSuccess)
            {
                return HandleError<bool>($"Address Id: {id} Not Found!");
            }
            _logger.LogInformation($"Address deleted. Id:{id}");
            return new BaseReturnModel<bool> { Data = true };
        }

        public async Task<BaseReturnModel<AddressResponseModel>> GetByIdAsync(string id)
        {
            var address = await _addressRepository.GetByIdAsync(id);
            if (!address.IsSuccess)
            {
                return HandleError<AddressResponseModel>($"Coin Id: {id} Not Found");
            }
           /* var city = await _cityRepository.GetByFilterAsync(x => x.Id == address.Data.CityId);
            var number = await _numberRepository.GetByFilterAsync(x => x.Id == address.Data.NumberId);
            var province = await _provinceRepository.GetByFilterAsync(x => x.Id == address.Data.ProvinceId);
            var street = await _streetRepository.GetByFilterAsync(x => x.Id == address.Data.StreetId);
            var town = await _townRepository.GetByFilterAsync(x => x.Id == address.Data.TownId);*/

            return new BaseReturnModel<AddressResponseModel>
            {
                Data = new AddressResponseModel
                {
                    OpenAddress = address.Data.OpenAddress,
                    CityId = address.Data.CityId,
                    NumberId = address.Data.NumberId,
                    ProvinceId = address.Data.ProvinceId,
                    StreetId = address.Data.StreetId,
                    TownId = address.Data.TownId
                }
            };
        }

        public async Task<BaseReturnModel<List<AddressResponseModel>>> GetListAsync()
        {
            var result = await _addressRepository.GetListAsync();
            if (!result.IsSuccess)
            {
                return HandleError<List<AddressResponseModel>>("Address list get fault!");
            }
            var filteredData = result.Data.Where(x => !x.IsDeleted).Select(x => new AddressResponseModel
            {
                OpenAddress = x.OpenAddress,
                CityId = x.CityId,
                NumberId = x.NumberId,
                ProvinceId = x.ProvinceId,
                StreetId = x.StreetId,
                TownId = x.TownId
            }).ToList();

            return new BaseReturnModel<List<AddressResponseModel>>
            {
                Data = filteredData
            };
        }

        public async Task<BaseReturnModel<List<AddressResponseModel>>> GetListByFilterAsync(Expression<Func<Address, bool>> exp)
        {
            var result = await _addressRepository.GetListByFilterAsync(exp);
            if (!result.IsSuccess)
            {
                return HandleError<List<AddressResponseModel>>("Address get fault!");
            }
            return new BaseReturnModel<List<AddressResponseModel>>
            {
                Data = result.Data.Select(x => new AddressResponseModel
                {
                    OpenAddress = x.OpenAddress,
                    CityId = x.CityId,
                    NumberId = x.NumberId,
                    ProvinceId = x.ProvinceId,
                    StreetId = x.StreetId,
                    TownId = x.TownId
                }).ToList(),
            };
        }

        public async Task<BaseReturnModel<AddressResponseModel>> UpdateAsync(AddressRequestModel item)
        {
            try
            {
                
                var address = await _addressRepository.GetByIdAsync(item.Id);
                if (!address.IsSuccess)
                {
                    return HandleError<AddressResponseModel>("Adrress update fault!");
                }
                address.Data.CityId = item.CityId;
                address.Data.OpenAddress = item.OpenAddress;
                address.Data.ProvinceId = item.ProvinceId;
                address.Data.NumberId = item.NumberId;
                address.Data.StreetId = item.StreetId;
                address.Data.TownId = item.TownId;
                var result = await _addressRepository.UpdateAsync(address.Data);
                if (!result.IsSuccess)
                {
                    return HandleError<AddressResponseModel>("Address update fault!");
                }
                return new BaseReturnModel<AddressResponseModel>
                {
                    Data = new AddressResponseModel
                    {
                        CityId = result.Data.CityId,
                        TownId= result.Data.TownId,
                        StreetId = result.Data.StreetId,
                        NumberId = result.Data.NumberId,
                        OpenAddress = result.Data.OpenAddress,
                        ProvinceId = result.Data.ProvinceId
                    }
                };
            }
            catch (Exception)
            {
                return HandleError<AddressResponseModel>("Address update fault!");
            }
        }
    }
}
