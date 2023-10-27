using Bobi.Api.Domain.Build;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bobi.Api.MongoDb.Repositories.Base
{
    public class BuildRepository : Repository<Build>
    {
        private readonly IConfiguration _configuration;

        public BuildRepository(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }
    }
}
