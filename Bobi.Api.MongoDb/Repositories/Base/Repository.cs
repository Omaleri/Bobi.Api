using Bobi.Api.Application.Contracts.Interfaces;
using Bobi.Api.Application.Domain.Shared.Abstract;
using Bobi.Api.MongoDb.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Linq.Expressions;

namespace Bobi.Api.MongoDb.Repositories.Base
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly IMongoCollection<T> _collection;
        public Repository(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetSection("MongoDB:ConnectionString").Value);
            var database = client.GetDatabase(configuration.GetSection("MongoDB:DatabaseName").Value);
            _collection = database.GetCollection<T>(nameof(T));
        }
        public async Task<BaseReturnModel<T>> CreateAsync(T item)
        {
            try
            {
                item.CreationTime = DateTime.Now;

                await _collection.InsertOneAsync(item);
                return new BaseReturnModel<T>
                {
                    Data = item,
                };

            }
            catch (Exception ex)
            {
                return new BaseReturnModel<T>
                {
                    Data = item,
                    Error = new List<ErrorModel> 
                    { 
                        new ErrorModel(0, ex.Message)
                    }
                };

            }
        }

        public async Task<BaseReturnModel<List<T>>> CreateManyAsync(List<T> item)
        {
            throw new NotImplementedException();
            item.ForEach(x => 
            {
                x.CreationTime = DateTime.UtcNow;
                x.IsDeleted = false;
                x.IsActive = true;
            });
            await _collection.InsertManyAsync(item);
        }

        public Task<BaseReturnModel<bool>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<BaseReturnModel<bool>> DeleteManyAsync(Expression<Func<T, bool>> exp)
        {
            throw new NotImplementedException();
        }

        public Task<BaseReturnModel<T>> GetByFilterAsync(Expression<Func<T, bool>> exp)
        {
            throw new NotImplementedException();
        }

        public Task<BaseReturnModel<T>> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<BaseReturnModel<List<T>>> GetListAsync()
        {
            throw new NotImplementedException();
        }

        public Task<BaseReturnModel<List<T>>> GetListByFilterAsync(Expression<Func<T, bool>> exp)
        {
            throw new NotImplementedException();
        }

        public Task<BaseReturnModel<T>> UpdateAsync(T item)
        {
            throw new NotImplementedException();
        }
    }
}
