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
    public interface IVoiceAppService
    {
        Task<BaseReturnModel<VoiceResponseModel>> CreateAsync(VoiceRequestModel item);
        Task<BaseReturnModel<List<VoiceResponseModel>>> GetListByFilterAsync(Expression<Func<Voice, bool>> exp);
        Task<BaseReturnModel<VoiceResponseModel>> GetByIdAsync(int id);
        Task<BaseReturnModel<VoiceResponseModel>> UpdateAsync(VoiceRequestModel item);
        Task<BaseReturnModel<bool>> DeleteAsync(int id);
        Task<BaseReturnModel<List<VoiceResponseModel>>> GetListAsync();
    }
}
