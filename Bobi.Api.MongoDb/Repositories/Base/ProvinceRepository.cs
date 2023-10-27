using Bobi.Api.Domain.Address;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bobi.Api.MongoDb.Repositories.Base
{
    public class ProvinceRepository : Repository<Province>
    {
        private readonly IConfiguration _configuration;

        public ProvinceRepository(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }
    }
}
