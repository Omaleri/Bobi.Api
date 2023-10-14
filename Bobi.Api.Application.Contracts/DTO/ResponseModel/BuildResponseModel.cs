using Bobi.Api.Application.Domain.Shared.Abstract;
using Bobi.Api.Domain.Build;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bobi.Api.Application.Contracts.DTO.ResponseModel
{
    public class BuildResponseModel : BaseDtoModel
    {
        public int AddressId { get; set; }
        public bool Situation { get; set; }
        public int NumberOfFloors { get; set; }
        public string TypeOfFeature { get; set; }
        public DateTime DateOfDestructive { get; set; }

        public List<DeviceResponseModel> Device { get; set; }
    }
}
