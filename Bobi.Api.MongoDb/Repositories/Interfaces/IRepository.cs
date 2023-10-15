using Bobi.Api.Application.Contracts.Interfaces;
using Bobi.Api.Application.Domain.Shared.Abstract;
using System.Linq.Expressions;

namespace Bobi.Api.MongoDb.Repositories.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<BaseReturnModel<T>> CreateAsync(T item);
        Task<BaseReturnModel<List<T>>> GetListByFilterAsync(Expression<Func<T, bool>> exp);
        Task<BaseReturnModel<T>> GetByFilterAsync(Expression<Func<T, bool>> exp);
        Task<BaseReturnModel<T>> GetByIdAsync(int id);
        Task<BaseReturnModel<T>> UpdateAsync(T item);
        Task<BaseReturnModel<bool>> DeleteAsync(int id);
        Task<BaseReturnModel<List<T>>> GetListAsync();
        Task<BaseReturnModel<List<T>>> CreateManyAsync(List<T> item);
        Task<BaseReturnModel<bool>> DeleteManyAsync(Expression<Func<T, bool>> exp);

    }
}
