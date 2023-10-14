using Bobi.Api.Application.Contracts.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bobi.Api.Domain.Build
{
    public class Device : BaseEntity
    {
        public int BuildId { get; set; }
        public string DeviceName { get; set; }
    }
}
