using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Bobi.Api.Application.Domain.Shared.Abstract
{
    public abstract class BaseDtoModel
    {
        [JsonIgnore]
        public ObjectId Id { get; set; }
    }
}
