using Bobi.Api.Application.Contracts.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bobi.Api.Domain.Address
{
    public class Town : BaseEntity
    {
        public string Name { get; set; }
        public string Main { get; set; }
    }
}
