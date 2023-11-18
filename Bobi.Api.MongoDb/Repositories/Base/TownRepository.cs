using Bobi.Api.Application.Domain.Shared.Abstract;
using Bobi.Api.Domain.Address;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Bobi.Api.MongoDb.Repositories.Base
{
    public class TownRepository : Interfaces.IRepository<Town>
    {
        private readonly IMongoCollection<Town> _collection;

        public TownRepository(IConfiguration configuration)
        {
            var collectionName = (nameof(Town));
            var connString = configuration.GetSection("MongoDB:ConnectionString").Value;
            var dbName = configuration.GetSection("MongoDB:DatabaseName").Value;
            var client = new MongoClient(connString);
            var database = client.GetDatabase(dbName);
            _collection = database.GetCollection<Town>(collectionName);
        }
        private readonly ILogger<Town> _logger;

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

        public async Task<BaseReturnModel<Town>> CreateAsync(Town item)
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
                    return HandleError<Town>("Create fault!");
                }
                return new BaseReturnModel<Town>
                {
                    Data = item,
                };

            }
            catch (Exception ex)
            {
                return new BaseReturnModel<Town>
                {
                    Data = item,
                    Error = new List<ErrorModel>
                    {
                        new ErrorModel(0, ex.Message)
                    }
                };

            }
        }

        public async Task<BaseReturnModel<List<Town>>> CreateManyAsync(List<Town> item)
        {
            try
            {
                await _collection.InsertManyAsync(item);
                if (_collection == null)
                {
                    return HandleError<List<Town>>("Create many fault!");
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

                return new BaseReturnModel<List<Town>> { Data = item };
            }
            catch (Exception ex)
            {
                return HandleError<List<Town>>("Create many fault!");

            }
        }

        public async Task<BaseReturnModel<bool>> DeleteAsync(string id)
        {
            try
            {
                var objectId = ObjectId.Parse(id);
                var filter = Builders<Town>.Filter.Eq("_id", objectId);
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

        public async Task<BaseReturnModel<bool>> DeleteManyAsync(Expression<Func<Town, bool>> exp)
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

        public async Task<BaseReturnModel<Town>> GetByFilterAsync(Expression<Func<Town, bool>> exp)
        {
            try
            {
                var filter = Builders<Town>.Filter.Where(exp);
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
                    return new BaseReturnModel<Town>
                    {
                        Data = result,
                    };
                }
                else
                {
                    return new BaseReturnModel<Town>
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
                return new BaseReturnModel<Town>
                {
                    Data = null,
                    Error = new List<ErrorModel>
            {
                new ErrorModel(0, ex.Message)
            }
                };
            }
        }

        public async Task<BaseReturnModel<Town>> GetByIdAsync(string id)
        {
            try
            {
                var objectId = ObjectId.Parse(id);
                var filter = Builders<Town>.Filter.Eq("_id", objectId);
                var result = await _collection.Find(filter).FirstOrDefaultAsync();

                if (result != null)
                {
                    result.CreationTime = DateTime.UtcNow;
                    result.IsDeleted = false;
                    result.IsActive = true;
                    result.CreatedUserId = 0;
                    result.LastUpdatedUserId = 0;
                    result.LastModificationTime = DateTime.UtcNow;

                    return new BaseReturnModel<Town>
                    {
                        Data = result
                    };
                }
                else
                {
                    return new BaseReturnModel<Town>
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
                return new BaseReturnModel<Town>
                {
                    Data = null,
                    Error = new List<ErrorModel>
                    {
                        new ErrorModel(0, ex.Message)
                    }
                };
            }
        }

        public async Task<BaseReturnModel<List<Town>>> GetListAsync()
        {
            try
            {
                var result = await _collection.Find(Builders<Town>.Filter.Empty).ToListAsync();

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

                    return new BaseReturnModel<List<Town>>
                    {
                        Data = result,
                    };
                }
                else
                {
                    return new BaseReturnModel<List<Town>>
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
                return new BaseReturnModel<List<Town>>
                {
                    Data = null,
                    Error = new List<ErrorModel>
                    {
                        new ErrorModel(0, ex.Message)
                    }
                };
            }
        }

        public async Task<BaseReturnModel<List<Town>>> GetListByFilterAsync(Expression<Func<Town, bool>> exp)
        {
            try
            {
                var filter = Builders<Town>.Filter.Where(exp);
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

                    return new BaseReturnModel<List<Town>>
                    {
                        Data = result,
                    };
                }
                else
                {
                    return new BaseReturnModel<List<Town>>
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
                return new BaseReturnModel<List<Town>>
                {
                    Data = null,
                    Error = new List<ErrorModel>
            {
                new ErrorModel(0, ex.Message)
            }
                };
            }
        }

        public async Task<BaseReturnModel<Town>> UpdateAsync(Town item, string id)
        {

            try
            {
                var filter = Builders<Town>.Filter.Eq("_id", item.Id);
                var result = await _collection.ReplaceOneAsync(filter, item);

                if (result.IsAcknowledged && result.ModifiedCount > 0)
                {
                    item.LastModificationTime = DateTime.UtcNow;
                    return new BaseReturnModel<Town>
                    {
                        Data = item,
                    };
                }
                else
                {
                    return new BaseReturnModel<Town>
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
                return new BaseReturnModel<Town>
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
