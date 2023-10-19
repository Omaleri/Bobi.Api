using Bobi.Api.Application.Contracts.Interfaces;
using Bobi.Api.Application.Domain.Shared.Abstract;
using Bobi.Api.Domain.Address;
using Bobi.Api.Domain.User;
using Bobi.Api.MongoDb.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bobi.Api.Application.Services
{
    public class RoleAppService : IRoleAppService
    {
        private readonly ILogger<RoleAppService> _logger;
        private readonly IRepository<Role> _roleRepository;

        public RoleAppService(ILogger<RoleAppService> logger, IRepository<Role> roleRepository)
        {
            _roleRepository = roleRepository;
            _logger = logger;
        }

        public async Task<BaseReturnModel<Role>> AddNewRoleAsync(bool isAuthorized)
        {
            var newRole = new Role { isAuthorized = isAuthorized };
            return await _roleRepository.CreateAsync(newRole);
        }
    }
}
