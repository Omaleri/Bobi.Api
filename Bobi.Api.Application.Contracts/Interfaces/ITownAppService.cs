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
    public interface ITownAppService
    {
        Task<BaseReturnModel<TownResponseModel>> CreateAsync(TownRequestModel item);
        Task<BaseReturnModel<List<TownResponseModel>>> GetListByFilterAsync(Expression<Func<Town, bool>> exp);
        Task<BaseReturnModel<TownResponseModel>> GetByIdAsync(string id);
        Task<BaseReturnModel<TownResponseModel>> UpdateAsync(TownRequestModel item, string id);
        Task<BaseReturnModel<bool>> DeleteAsync(string id);
        Task<BaseReturnModel<List<TownResponseModel>>> GetListAsync();
    }
}
