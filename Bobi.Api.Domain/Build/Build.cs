using Bobi.Api.Application.Contracts.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bobi.Api.Domain.Build
{
    public class Build : BaseEntity
    {
        public int AddressId { get; set; }
        public bool Situation { get; set; }
        public int NumberOfFloors { get; set; }
        public string TypeOfFeature { get; set; }
        public DateTime DateOfDestructive { get; set; }
    }
}
