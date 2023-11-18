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
    public interface IBuildAppService
    {
        Task<BaseReturnModel<BuildResponseModel>> CreateAsync(BuildRequestModel item);
        Task<BaseReturnModel<List<BuildResponseModel>>> GetListByFilterAsync(Expression<Func<Build, bool>> exp);
        Task<BaseReturnModel<BuildResponseModel>> GetByIdAsync(string id);
        Task<BaseReturnModel<BuildResponseModel>> UpdateAsync(BuildRequestModel item, string id);
        Task<BaseReturnModel<bool>> DeleteAsync(string id);
        Task<BaseReturnModel<List<BuildResponseModel>>> GetListAsync();

    }
}
