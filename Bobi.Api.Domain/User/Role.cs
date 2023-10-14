using Bobi.Api.Application.Contracts.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bobi.Api.Domain.User
{
    public class Role : BaseEntity
    {
        public bool isAuthorized { get; set; }
    }
}
