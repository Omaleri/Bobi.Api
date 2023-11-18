using Bobi.Api.Application.Domain.Shared.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bobi.Api.Application.Contracts.DTO.RequestModel
{
    public class ProvinceRequestModel : BaseDtoModel
    {
        public string Name { get; set; }
        public string Main { get; set; }
    }
}
