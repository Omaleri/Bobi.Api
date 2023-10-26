using Bobi.Api.Application.Contracts.Interfaces;
using Bobi.Api.Application.Domain.Shared.Abstract;
using Bobi.Api.Domain.User;
using System.Linq.Expressions;

namespace Bobi.Api.MongoDb.Repositories.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<BaseReturnModel<T>> CreateAsync(T item);
        Task<BaseReturnModel<List<T>>> GetListByFilterAsync(Expression<Func<T, bool>> exp);
        Task<BaseReturnModel<T>> GetByFilterAsync(Expression<Func<T, bool>> exp);
        Task<BaseReturnModel<T>> GetByIdAsync(string id);
        Task<BaseReturnModel<T>> UpdateAsync(T item);
        Task<BaseReturnModel<bool>> DeleteAsync(string id);
        Task<BaseReturnModel<List<T>>> GetListAsync();
        Task<BaseReturnModel<List<T>>> CreateManyAsync(List<T> item);
        Task<BaseReturnModel<bool>> DeleteManyAsync(Expression<Func<T, bool>> exp);

    }
}
