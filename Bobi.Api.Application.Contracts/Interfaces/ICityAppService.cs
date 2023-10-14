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
    public interface ICityAppService
    {
        Task<BaseReturnModel<CityResponseModel>> CreateAsync(CityRequestModel item);
        Task<BaseReturnModel<List<CityResponseModel>>> GetListByFilterAsync(Expression<Func<City, bool>> exp);
        Task<BaseReturnModel<CityResponseModel>> GetByIdAsync(int id);
        Task<BaseReturnModel<CityResponseModel>> UpdateAsync(CityRequestModel item);
        Task<BaseReturnModel<bool>> DeleteAsync(int id);
        Task<BaseReturnModel<List<CityResponseModel>>> GetListAsync();
    }
}
