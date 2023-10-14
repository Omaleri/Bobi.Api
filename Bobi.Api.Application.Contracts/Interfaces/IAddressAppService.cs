using Bobi.Api.Application.Contracts.DTO.RequestModel;
using Bobi.Api.Application.Contracts.DTO.ResponseModel;
using Bobi.Api.Application.Domain.Shared.Abstract;
using Bobi.Api.Domain.Address;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Bobi.Api.Application.Contracts.Interfaces
{
    public interface IAddressAppService
    {
        Task<BaseReturnModel<AddressResponseModel>> CreateAsync(AddressRequestModel item);
        Task<BaseReturnModel<List<AddressResponseModel>>> GetListByFilterAsync(Expression<Func<Address, bool>> exp);
        Task<BaseReturnModel<AddressResponseModel>> GetByIdAsync(int id);
        Task<BaseReturnModel<AddressResponseModel>> UpdateAsync(AddressRequestModel item);
        Task<BaseReturnModel<bool>> DeleteAsync(int id);
        Task<BaseReturnModel<List<AddressResponseModel>>> GetListAsync();

    }
}
