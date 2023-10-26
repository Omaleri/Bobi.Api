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
    public interface IProvinceAppService
    {
        Task<BaseReturnModel<ProvinceResponseModel>> CreateAsync(ProvinceRequestModel item);
        Task<BaseReturnModel<List<ProvinceResponseModel>>> GetListByFilterAsync(Expression<Func<Province, bool>> exp);
        Task<BaseReturnModel<ProvinceResponseModel>> GetByIdAsync(string id);
        Task<BaseReturnModel<ProvinceResponseModel>> UpdateAsync(ProvinceRequestModel item);
        Task<BaseReturnModel<bool>> DeleteAsync(string id);
        Task<BaseReturnModel<List<ProvinceResponseModel>>> GetListAsync();
    }
}
