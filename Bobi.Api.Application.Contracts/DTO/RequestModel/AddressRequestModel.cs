using Bobi.Api.Application.Domain.Shared.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bobi.Api.Application.Contracts.DTO.RequestModel
{
    public class AddressRequestModel : BaseDtoModel
    {
        public int CityId { get; set; }
        public int ProvinceId { get; set; }
        public int TownId { get; set; }
        public int NumberId { get; set; }
        public int StreetId { get; set; }
        public string OpenAddress { get; set; }
    }
}
