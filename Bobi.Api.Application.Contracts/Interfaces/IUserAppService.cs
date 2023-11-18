using Bobi.Api.Application.Contracts.DTO.RequestModel;
using Bobi.Api.Application.Contracts.DTO.ResponseModel;
using Bobi.Api.Application.Domain.Shared.Abstract;
using Bobi.Api.Domain.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Bobi.Api.Application.Contracts.Interfaces
{
    public interface IUserAppService
    {
        Task<BaseReturnModel<UserResponseModel>> CreateAsync(UserRequestModel item);
        Task<BaseReturnModel<UserResponseModel>> GetByIdAsync(string id);
        Task<BaseReturnModel<UserResponseModel>> UpdateAsync(UserRequestModel item, string id);
        Task<BaseReturnModel<bool>> DeleteAsync(string id);
    }
}
