﻿using System;
using Bobi.Api.Domain.Address;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Bobi.Api.MongoDb.Repositories.Base
{
    public class AddressRepository : Repository<Address>
    {
        private readonly IConfiguration _configuration;

        public AddressRepository(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }
    }
}
