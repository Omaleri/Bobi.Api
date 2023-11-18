using Bobi.Api.Application.Contracts.DTO.RequestModel;
using Bobi.Api.Application.Contracts.DTO.ResponseModel;
using Bobi.Api.Application.Domain.Shared.Abstract;
using Bobi.Api.Domain.Build;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Bobi.Api.Application.Contracts.Interfaces
{
    public interface IDeviceAppService
    {
        Task<BaseReturnModel<DeviceResponseModel>> CreateAsync(DeviceRequestModel item);
        Task<BaseReturnModel<List<DeviceResponseModel>>> GetListByFilterAsync(Expression<Func<Device, bool>> exp);
        Task<BaseReturnModel<DeviceResponseModel>> GetByIdAsync(string id);
        Task<BaseReturnModel<DeviceResponseModel>> UpdateAsync(DeviceRequestModel item, string id);
        Task<BaseReturnModel<bool>> DeleteAsync(string id);
        Task<BaseReturnModel<List<DeviceResponseModel>>> GetListAsync();
    }
}
