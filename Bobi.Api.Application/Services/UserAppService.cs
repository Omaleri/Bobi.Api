using Bobi.Api.Application.Contracts.DTO.RequestModel;
using Bobi.Api.Application.Contracts.DTO.ResponseModel;
using Bobi.Api.Application.Contracts.Interfaces;
using Bobi.Api.Application.Domain.Shared.Abstract;
using Bobi.Api.Domain.User;
using Bobi.Api.MongoDb.Repositories.Interfaces;
using Microsoft.Extensions.Logging;

namespace Bobi.Api.Application.Services
{
    public class UserAppService : IUserAppService
    {

        private readonly ILogger<UserAppService> _logger;
        private readonly IRepository<User> _userRepository;
        public UserAppService(ILogger<UserAppService> logger, IRepository<User> userRepository)
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
                    RoleId = item.RoleId
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
                        RoleId = item.RoleId
                    }
                };
            }
            catch (Exception ex)
            {
                return HandleError<UserResponseModel>(ex.Message);
            }
        }

        public async Task<BaseReturnModel<bool>> DeleteAsync(int id)
        {
            var result = await _userRepository.DeleteAsync(id);
            if (!result.IsSuccess)
            {
                return HandleError<bool>("User delete fault!");
            }
            _logger.LogInformation($"User deleted. Id:{id}");
            return new BaseReturnModel<bool> { Data = true };
        }

        public async Task<BaseReturnModel<UserResponseModel>> GetByIdAsync(int id)
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
                    Id = result.Data.Id,
                    Email = result.Data.Email,
                    Password = result.Data.Password,
                    RoleId = result.Data.RoleId
                }
            };
        }

        public async Task<BaseReturnModel<UserResponseModel>> UpdateAsync(UserRequestModel item)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(item.Id);
                if (!user.IsSuccess)
                {
                    return HandleError<UserResponseModel>("User update fault!");
                }
                user.Data.RoleId = item.RoleId;
                user.Data.Email = item.Email;
                user.Data.Password = item.Password;
                user.Data.Id = item.Id;
                var result = await _userRepository.UpdateAsync(user.Data);
                if (!result.IsSuccess)
                {
                    return HandleError<UserResponseModel>("User update fault!");
                }
                return new BaseReturnModel<UserResponseModel>
                {
                    Data = new UserResponseModel
                    {
                        Id = result.Data.Id,
                        Email = result.Data.Email,
                        Password = result.Data.Password,
                        RoleId = result.Data.RoleId
                    }
                };
            }
            catch (Exception)
            {
                return HandleError<UserResponseModel>("User update fault!");
            }
        }
    }
}
