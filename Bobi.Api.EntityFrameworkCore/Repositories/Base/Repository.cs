using Bobi.Api.Application.Contracts.Interfaces;
using Bobi.Api.Application.Domain.Shared.Abstract;
using Bobi.Api.EntityFrameworkCore.Context;
using Bobi.Api.EntityFrameworkCore.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Bobi.Api.EntityFrameworkCore.Repositories.Base
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly BobiDbContext _context;
        public Repository(BobiDbContext context)
        {
            _context = context;
        }
        public async Task<BaseReturnModel<T>> CreateAsync(T item)
        {
            try
            {
                item.CreationTime = DateTime.UtcNow;
                item.IsDeleted = false;
                item.IsActive = true;
                item.DeletionTime = null;
                item.LastModificationTime = null;
                await _context.Set<T>().AddAsync(item);
                await _context.SaveChangesAsync();
                return new BaseReturnModel<T> { Data = item };
            }
            catch
            {
                return new BaseReturnModel<T>
                {
                    Error = new List<ErrorModel>
                    {
                        new ErrorModel(ErrorCodes.ProcessNotCompleted)
                    }
                };
            }
        }

        public async Task<BaseReturnModel<List<T>>> CreateManyAsync(List<T> item)
        {
            try
            {
                foreach (var i in item)
                {
                    i.CreationTime = DateTime.UtcNow;
                    i.IsDeleted = false;
                    i.IsActive = true;
                    await _context.Set<T>().AddAsync(i);
                }
                await _context.SaveChangesAsync();
                return new BaseReturnModel<List<T>> { Data = item };
            }
            catch
            {
                return new BaseReturnModel<List<T>>
                {
                    Error = new List<ErrorModel>
                    {
                        new ErrorModel(ErrorCodes.ProcessNotCompleted)
                    }
                };
            }
        }

        public async Task<BaseReturnModel<bool>> DeleteAsync(int id)
        {
        try
        {
            var item = await GetByIdAsync(id);
            if (!item.IsSuccess)
            {
                return new BaseReturnModel<bool>
                {
                    Error = new List<ErrorModel>
                        {
                        new ErrorModel(ErrorCodes.DataNotFound)
                        }
                };
            }
            item.Data.DeletionTime = DateTime.Now;
            item.Data.IsDeleted = true;
            item.Data.IsActive = false;

            _context.Set<T>().Update(item.Data);
            await _context.SaveChangesAsync();
            return new BaseReturnModel<bool> { Data = true };
        }

        catch (Exception ex)
        {
            return new BaseReturnModel<bool>
            {
                Error = new List<ErrorModel>
                        {
                        new ErrorModel(ErrorCodes.ProcessNotCompleted, ex.Message)
                        }
            };
        }
    }

        public async Task<BaseReturnModel<bool>> DeleteManyAsync(Expression<Func<T, bool>> exp)
        {
            try
            {
                var item = await GetListByFilterAsync(exp);
                if (item == null)
                {
                    return new BaseReturnModel<bool>
                    {
                        Error = new List<ErrorModel>
                        {
                            new ErrorModel(ErrorCodes.DataNotFound)
                        }
                    };
                }

                if (!item.IsSuccess)
                {
                    return new BaseReturnModel<bool>
                    {
                        Error = item.Error
                    };
                }

                foreach (var i in item.Data)
                {
                    i.IsDeleted = true;
                    i.DeletionTime = DateTime.UtcNow;
                }

                _context.Set<T>().UpdateRange(item.Data);
                await _context.SaveChangesAsync();
                return new BaseReturnModel<bool> { Data = true };
            }
            catch
            {
                return new BaseReturnModel<bool>
                {
                    Error = new List<ErrorModel>
                    {
                        new ErrorModel(ErrorCodes.ProcessNotCompleted)
                    }
                };
            }
        }

        public async Task<BaseReturnModel<T>> GetByFilterAsync(Expression<Func<T, bool>> exp)
        {
            try
            {
                var result = await _context.Set<T>().OrderByDescending(x => x.Id).FirstOrDefaultAsync(exp) ?? throw new Exception("Data Not Found");
                return new BaseReturnModel<T> { Data = result };
            }
            catch (Exception ex)
            {
                return new BaseReturnModel<T>
                {
                    Error = new List<ErrorModel>
                        {
                        new ErrorModel(ErrorCodes.ProcessNotCompleted, ex.Message)
                        }
                };
            }
        }

        public async Task<BaseReturnModel<T>> GetByIdAsync(int id)
        {
            try
            {
                var result = await _context.Set<T>().FirstOrDefaultAsync(x => !x.IsDeleted && x.IsActive && x.Id == id);
                if (result == null)
                {
                    return new BaseReturnModel<T>
                    {
                        Error = new List<ErrorModel>
                        {
                        new ErrorModel(ErrorCodes.DataNotFound)
                        }
                    };
                }
                return new BaseReturnModel<T> { Data = result };
            }
            catch (Exception ex)
            {
                return new BaseReturnModel<T>
                {
                    Error = new List<ErrorModel>
                        {
                        new ErrorModel(ErrorCodes.DataNotFound, ex.Message)
                        }
                };
            }
        }

        public async Task<BaseReturnModel<List<T>>> GetListAsync()
        {
            try
            {
                var result = await _context.Set<T>().ToListAsync();
                if (result == null)
                {
                    return new BaseReturnModel<List<T>>
                    {
                        Error = new List<ErrorModel>
                        {
                        new ErrorModel(ErrorCodes.DataNotFound)
                        }
                    };
                }
                return new BaseReturnModel<List<T>> { Data = result };
            }
            catch (Exception ex)
            {
                return new BaseReturnModel<List<T>>
                {
                    Error = new List<ErrorModel>
                        {
                        new ErrorModel(ErrorCodes.ProcessNotCompleted, ex.Message)
                        }
                };
            }
        }

        public async Task<BaseReturnModel<List<T>>> GetListByFilterAsync(Expression<Func<T, bool>> exp)
        {
            try
            {
                var result = await _context.Set<T>().Where(exp).ToListAsync();
                if (!result.Any())
                {
                    return new BaseReturnModel<List<T>> { Data = null };
                }
                return new BaseReturnModel<List<T>> { Data = result };
            }
            catch (Exception ex)
            {
                return new BaseReturnModel<List<T>>
                {
                    Error = new List<ErrorModel>
                        {
                        new ErrorModel(ErrorCodes.ProcessNotCompleted, ex.Message)
                        }
                };
            }
        }

        public async Task<BaseReturnModel<T>> UpdateAsync(T item)
        {
            try
            {
                var result = await GetByIdAsync(item.Id);
                if (!result.IsSuccess)
                {
                    return new BaseReturnModel<T>
                    {
                        Error = new List<ErrorModel>
                        {
                        new ErrorModel(ErrorCodes.DataNotFound)
                        }
                    };
                }
                item.LastModificationTime = DateTime.UtcNow;
                _context.Entry(result.Data).CurrentValues.SetValues(item);
                await _context.SaveChangesAsync();
                return new BaseReturnModel<T> { Data = item };
            }
            catch (Exception ex)
            {
                return new BaseReturnModel<T>
                {
                    Error = new List<ErrorModel>
                        {
                        new ErrorModel(ErrorCodes.ProcessNotCompleted, ex.Message)
                        }
                };
            }
        }

    }
}
