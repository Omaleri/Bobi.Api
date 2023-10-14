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
        public int CityId { get; set; }
        public int ProvinceId { get; set; }
        public int TownId { get; set; }
        public int StreetId { get; set; }
        public int NumberId { get; set; }
        public string OpenAddress { get; set; }
    }
}
