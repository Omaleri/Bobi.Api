using Bobi.Api.Application.Contracts.Interfaces;
using Bobi.Api.Application.Domain.Shared.Abstract;
using Bobi.Api.Domain.User;
using Bobi.Api.MongoDb.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Linq.Expressions;

namespace Bobi.Api.MongoDb.Repositories.Base
{
    public abstract class IRepository<T> : Interfaces.IRepository<T> where T : BaseEntity
    {
        private readonly IMongoCollection<T> _collection;
        public IRepository(IConfiguration configuration)
        {
            var collectionName = (nameof(T));
            var connString = configuration.GetSection("MongoDB:ConnectionString").Value;
            var dbName = configuration.GetSection("MongoDB:DatabaseName").Value;
            var client = new MongoClient(connString);
            var database = client.GetDatabase(dbName);
            _collection = database.GetCollection<T>(collectionName);
        }
        private readonly ILogger<T> _logger;

        private BaseReturnModel<T> HandleError<T>(string errorMessage)
        {
            _logger.LogError(errorMessage);
            return new BaseReturnModel<T>
            {
                Error = new List<ErrorModel>
                {
                    new ErrorModel(ErrorCodes.ProcessNotCompleted, errorMessage)
                }
            };
        }

        public async Task<BaseReturnModel<T>> CreateAsync(T item)
        {
            try
            {
                item.CreationTime = DateTime.UtcNow;
                item.IsDeleted = false;
                item.IsActive = true;
                item.CreatedUserId = 0;
                item.IsActive = true;
                item.LastUpdatedUserId = 0;
                item.LastModificationTime = DateTime.UtcNow;
                await _collection.InsertOneAsync(item);
                if (_collection == null)
                {
                    return HandleError<T>("Create fault!");
                }
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
            try
            {
                await _collection.InsertManyAsync(item);
                if (_collection == null)
                {
                    return HandleError<List<T>>("Create many fault!");
                }
                item.ForEach(x =>
                {
                    x.CreationTime = DateTime.UtcNow;
                    x.IsDeleted = false;
                    x.IsActive = true;
                    x.CreatedUserId = 0;
                    x.IsActive = true;
                    x.LastUpdatedUserId = 0;
                    x.LastModificationTime = DateTime.UtcNow;

                });

                return new BaseReturnModel<List<T>> { Data = item };
            }
            catch (Exception ex)
            {
                return HandleError<List<T>>("Create many fault!");
            }
        }

        public async Task<BaseReturnModel<bool>> DeleteAsync(string id)
        {
            try
            {
                var objectId = ObjectId.Parse(id);
                var filter = Builders<T>.Filter.Eq("_id", objectId);
                var result = await _collection.DeleteOneAsync(filter);

                if (result.DeletedCount == 1)
                {
                    return new BaseReturnModel<bool>
                    {
                        Data = true,
                    };
                }
                else
                {
                    return new BaseReturnModel<bool>
                    {
                        Data = false,
                        Error = new List<ErrorModel>
                        {
                            new ErrorModel(0, "Id bulunamadı.")
                        }
                    };
                }
            }
            catch (Exception ex)
            {
                return new BaseReturnModel<bool>
                {
                    Data = false,
                    Error = new List<ErrorModel>
                    {
                        new ErrorModel(0, ex.Message)
                    }
                };
            }
        }

        public async Task<BaseReturnModel<bool>> DeleteManyAsync(Expression<Func<T, bool>> exp)
        {
            try
            {
                var result = await _collection.DeleteManyAsync(exp);
                if (result.DeletedCount > 0)
                {
                    return new BaseReturnModel<bool>
                    {
                        Data = true,
                    };
                }
                else
                {
                    return new BaseReturnModel<bool>
                    {
                        Data = false,
                        Error = new List<ErrorModel>
                    {
                        new ErrorModel(0, "Belgeler bulunamadı.")
                    }
                    };
                }
            }
            catch (Exception ex)
            {
                return new BaseReturnModel<bool>
                {
                    Data = false,
                    Error = new List<ErrorModel>
                    {
                        new ErrorModel(0, ex.Message)
                    }
                };
            }

        }

        public async Task<BaseReturnModel<T>> GetByFilterAsync(Expression<Func<T, bool>> exp)
        {
            try
            {
                var filter = Builders<T>.Filter.Where(exp);
                var result = await _collection.Find(filter).FirstOrDefaultAsync();
                result.CreationTime = DateTime.UtcNow;
                result.IsDeleted = false;
                result.IsActive = true;
                result.CreatedUserId = 0;
                result.IsActive = true;
                result.LastUpdatedUserId = 0;
                result.LastModificationTime = DateTime.UtcNow;
                if (result != null)
                {
                    return new BaseReturnModel<T>
                    {
                        Data = result,
                    };
                }
                else
                {
                    return new BaseReturnModel<T>
                    {
                        Data = null,
                        Error = new List<ErrorModel>
                {
                    new ErrorModel(0, "Filtreye göre dosya bulunamadı.")
                }
                    };
                }
            }
            catch (Exception ex)
            {
                return new BaseReturnModel<T>
                {
                    Data = null,
                    Error = new List<ErrorModel>
            {
                new ErrorModel(0, ex.Message)
            }
                };
            }
        }

        public async Task<BaseReturnModel<T>> GetByIdAsync(string id)
        {
            try
            {
                var objectId = ObjectId.Parse(id);
                var filter = Builders<T>.Filter.Eq("_id", objectId);
                var result = await _collection.Find(filter).FirstOrDefaultAsync();

                if (result != null)
                {
                    result.CreationTime = DateTime.UtcNow;
                    result.IsDeleted = false;
                    result.IsActive = true;
                    result.CreatedUserId = 0;
                    result.LastUpdatedUserId = 0;
                    result.LastModificationTime = DateTime.UtcNow;

                    return new BaseReturnModel<T>
                    {
                        Data = result
                    };
                }
                else
                {
                    return new BaseReturnModel<T>
                    {
                        Data = null,
                        Error = new List<ErrorModel>
                        {
                            new ErrorModel(0, $"Bu id: {id}'de bir dosya bulunamadı.")
                        }
                    };
                }
            }
            catch (Exception ex)
            {
                return new BaseReturnModel<T>
                {
                    Data = null,
                    Error = new List<ErrorModel>
                    {
                        new ErrorModel(0, ex.Message)
                    }
                };
            }
        }

        public async Task<BaseReturnModel<List<T>>> GetListAsync()
        {
            try
            {
                var result = await _collection.Find(Builders<T>.Filter.Empty).ToListAsync();

                if (result != null && result.Any())
                {
                    result.ForEach(item =>
                    {
                        item.CreationTime = DateTime.UtcNow;
                        item.IsDeleted = false;
                        item.IsActive = true;
                        item.CreatedUserId = 0;
                        item.LastUpdatedUserId = 0;
                        item.LastModificationTime = DateTime.UtcNow;
                    });

                    return new BaseReturnModel<List<T>>
                    {
                        Data = result,
                    };
                }
                else
                {
                    return new BaseReturnModel<List<T>>
                    {
                        Data = null,
                        Error = new List<ErrorModel>
                        {
                            new ErrorModel(0, "Hiç belge bulunamadı.")
                        }
                    };
                }
            }
            catch (Exception ex)
            {
                return new BaseReturnModel<List<T>>
                {
                    Data = null,
                    Error = new List<ErrorModel>
                    {
                        new ErrorModel(0, ex.Message)
                    }
                };
            }
        }

        public async Task<BaseReturnModel<List<T>>> GetListByFilterAsync(Expression<Func<T, bool>> exp)
        {
            try
            {
                var filter = Builders<T>.Filter.Where(exp);
                var result = await _collection.Find(filter).ToListAsync();

                if (result != null && result.Any())
                {
                    result.ForEach(item =>
                    {
                        item.CreationTime = DateTime.UtcNow;
                        item.IsDeleted = false;
                        item.IsActive = true;
                        item.CreatedUserId = 0;
                        item.LastUpdatedUserId = 0;
                        item.LastModificationTime = DateTime.UtcNow;
                    });

                    return new BaseReturnModel<List<T>>
                    {
                        Data = result,
                    };
                }
                else
                {
                    return new BaseReturnModel<List<T>>
                    {
                        Data = null,
                        Error = new List<ErrorModel>
                {
                    new ErrorModel(0, "Bu filtrelemeye göre belge bulunamadı.")
                }
                    };
                }
            }
            catch (Exception ex)
            {
                return new BaseReturnModel<List<T>>
                {
                    Data = null,
                    Error = new List<ErrorModel>
            {
                new ErrorModel(0, ex.Message)
            }
                };
            }
        }

        public async Task<BaseReturnModel<T>> UpdateAsync(T item)
        {

            try
            {
                var filter = Builders<T>.Filter.Eq("_id", item.Id);
                var result = await _collection.ReplaceOneAsync(filter, item);

                if (result.IsAcknowledged && result.ModifiedCount > 0)
                {
                    item.LastModificationTime = DateTime.UtcNow;
                    return new BaseReturnModel<T>
                    {
                        Data = item,
                    };
                }
                else
                {
                    return new BaseReturnModel<T>
                    {
                        Data = null,
                        Error = new List<ErrorModel>
                        {
                            new ErrorModel(0, $"Belge güncellenemedi id: {item.Id}.")
                        }
                    };
                }
            }
            catch (Exception ex)
            {
                return new BaseReturnModel<T>
                {
                    Data = null,
                    Error = new List<ErrorModel>
                    {
                        new ErrorModel(0, ex.Message)
                    }
                };
            }
        }
    }
}
