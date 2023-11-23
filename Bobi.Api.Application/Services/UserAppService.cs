using Bobi.Api.Application.Contracts.DTO.RequestModel;
using Bobi.Api.Application.Contracts.DTO.ResponseModel;
using Bobi.Api.Application.Contracts.Interfaces;
using Bobi.Api.Application.Domain.Shared.Abstract;
using Bobi.Api.Domain.User;
using Bobi.Api.MongoDb.Repositories.Base;
using Bobi.Api.MongoDb.Repositories.Interfaces;
using Microsoft.Extensions.Logging;

namespace Bobi.Api.Application.Services
{
    public class UserAppService : IUserAppService
    {

        private readonly ILogger<UserAppService> _logger;
        private readonly MongoDb.Repositories.Interfaces.IRepository<User> _userRepository;
        public UserAppService(ILogger<UserAppService> logger, MongoDb.Repositories.Interfaces.IRepository<User> userRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
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

        public async Task<BaseReturnModel<UserResponseModel>> CreateAsync(UserRequestModel item)
        {
            try
            {
                var user = new User
                {
                    Email = item.Email,
                    Password = item.Password,
                    isAdmin = item.isAdmin
                };
                var result = await _userRepository.CreateAsync(user);
                if (!result.IsSuccess)
                {
                    return HandleError<UserResponseModel>("User create fault!");
                }
                return new BaseReturnModel<UserResponseModel>
                {
                    Data = new UserResponseModel
                    {
                        Email = item.Email,
                        Password = item.Password,
                        isAdmin = item.isAdmin
                    }
                };
            }
            catch (Exception ex)
            {
                return HandleError<UserResponseModel>(ex.Message);
            }
        }

        public async Task<BaseReturnModel<bool>> DeleteAsync(string id)
        {
            var result = await _userRepository.DeleteAsync(id);
            if (!result.IsSuccess)
            {
                return HandleError<bool>("User delete fault!");
            }
            _logger.LogInformation($"User deleted. Id:{id}");
            return new BaseReturnModel<bool> { Data = true };
        }

        public async Task<BaseReturnModel<UserResponseModel>> GetByIdAsync(string id)
        {
            var result = await _userRepository.GetByIdAsync(id);
            if (!result.IsSuccess)
            {
                return HandleError<UserResponseModel>("User get fault!");
            }
            return new BaseReturnModel<UserResponseModel>
            {
                Data = new UserResponseModel
                {
                    Email = result.Data.Email,
                    Password = result.Data.Password,
                    isAdmin = result.Data.isAdmin
                }
            };
        }

        public async Task<BaseReturnModel<UserResponseModel>> UpdateAsync(UserRequestModel item, string id)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(id);
                if (!user.IsSuccess)
                {
                    return HandleError<UserResponseModel>("User update fault!");
                }
                user.Data.isAdmin = item.isAdmin;
                user.Data.Email = item.Email;
                user.Data.Password = item.Password;
                var result = await _userRepository.UpdateAsync(user.Data, id);
                if (!result.IsSuccess)
                {
                    return HandleError<UserResponseModel>("User update fault!");
                }
                return new BaseReturnModel<UserResponseModel>
                {
                    Data = new UserResponseModel
                    {
                        Email = result.Data.Email,
                        Password = result.Data.Password,
                        isAdmin = result.Data.isAdmin
                    }
                };
            }
            catch (Exception)
            {
                return HandleError<UserResponseModel>("User update fault!");
            }
        }

        public async Task<BaseReturnModel<List<UserResponseModel>>> GetListAsync()
        {
            var result = await _userRepository.GetListAsync();
            if (!result.IsSuccess)
            {
                return HandleError<List<UserResponseModel>>("User list get fault!");

            }
            var filteredData = result.Data.Where(x => !x.IsDeleted).Select(x => new UserResponseModel
            {
                Id = x.Id.ToString(),
                Email = x.Email,
                isAdmin=x.isAdmin,
                Password = x.Password
                
            }).ToList();

            return new BaseReturnModel<List<UserResponseModel>>
            {
                Data = filteredData
            };
        }
    }
}
