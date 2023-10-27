using Bobi.Api.Application.Domain.Shared.Abstract;
using Bobi.Api.Domain.Address;
using Bobi.Api.MongoDb.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Bobi.Api.MongoDb.Repositories.Base
{
    public class AddressRepository : IRepository<Address>
    {
        public Task<BaseReturnModel<Address>> CreateAsync(Address item)
        {
            throw new NotImplementedException();
        }

        public Task<BaseReturnModel<List<Address>>> CreateManyAsync(List<Address> item)
        {
            throw new NotImplementedException();
        }

        public Task<BaseReturnModel<bool>> DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<BaseReturnModel<bool>> DeleteManyAsync(Expression<Func<Address, bool>> exp)
        {
            throw new NotImplementedException();
        }

        public Task<BaseReturnModel<Address>> GetByFilterAsync(Expression<Func<Address, bool>> exp)
        {
            throw new NotImplementedException();
        }

        public Task<BaseReturnModel<Address>> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<BaseReturnModel<List<Address>>> GetListAsync()
        {
            throw new NotImplementedException();
        }

        public Task<BaseReturnModel<List<Address>>> GetListByFilterAsync(Expression<Func<Address, bool>> exp)
        {
            throw new NotImplementedException();
        }

        public Task<BaseReturnModel<Address>> UpdateAsync(Address item)
        {
            throw new NotImplementedException();
        }
    }
}
