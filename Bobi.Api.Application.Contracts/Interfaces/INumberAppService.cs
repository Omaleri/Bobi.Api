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
    public interface INumberAppService
    {
        Task<BaseReturnModel<NumberResponseModel>> CreateAsync(NumberRequestModel item);
        Task<BaseReturnModel<List<NumberResponseModel>>> GetListByFilterAsync(Expression<Func<Number, bool>> exp);
        Task<BaseReturnModel<NumberResponseModel>> GetByIdAsync(string id);
        Task<BaseReturnModel<NumberResponseModel>> UpdateAsync(NumberRequestModel item, string id);
        Task<BaseReturnModel<bool>> DeleteAsync(string id);
        Task<BaseReturnModel<List<NumberResponseModel>>> GetListAsync();
    }
}
