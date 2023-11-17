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
        public string CityId { get; set; }
        public string ProvinceId { get; set; }
        public string TownId { get; set; }
        public string StreetId { get; set; }
        public string NumberId { get; set; }
        public bool Situation { get; set; }
        public int NumberOfFloors { get; set; }
        public string TypeOfFeature { get; set; }
        public DateTime DateOfDestructive { get; set; }
        public List<Device> Device { get; set; }
    }
}
