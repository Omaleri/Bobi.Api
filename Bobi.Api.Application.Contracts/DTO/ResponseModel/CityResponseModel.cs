using Bobi.Api.Application.Domain.Shared.Abstract;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Bobi.Api.Application.Contracts.DTO.ResponseModel
{
    public class CityResponseModel : BaseDtoModel
    {
        public string Name { get; set; }
    }
}
