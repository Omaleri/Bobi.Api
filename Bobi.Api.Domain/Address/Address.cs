using Bobi.Api.Application.Contracts.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bobi.Api.Domain.Address
{
    public class Address : BaseEntity
    {
        public string CityId { get; set; }
        public string ProvinceId { get; set; }
        public string TownId { get; set; }
        public string StreetId { get; set; }
        public string NumberId { get; set; }
        public string OpenAddress { get; set; }
    }
}
