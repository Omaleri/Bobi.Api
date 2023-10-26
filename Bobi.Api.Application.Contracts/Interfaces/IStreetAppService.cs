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
    public interface IStreetAppService
    {
        Task<BaseReturnModel<StreetResponseModel>> CreateAsync(StreetRequestModel item);
        Task<BaseReturnModel<List<StreetResponseModel>>> GetListByFilterAsync(Expression<Func<Street, bool>> exp);
        Task<BaseReturnModel<StreetResponseModel>> GetByIdAsync(string id);
        Task<BaseReturnModel<StreetResponseModel>> UpdateAsync(StreetRequestModel item);
        Task<BaseReturnModel<bool>> DeleteAsync(string id);
        Task<BaseReturnModel<List<StreetResponseModel>>> GetListAsync();
    }
}
