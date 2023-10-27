using Bobi.Api.Application.Contracts.Interfaces;
using MongoDB.Bson;

namespace Bobi.Api.Domain.Address
{
    [BsonCollection("productCollection")]

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
