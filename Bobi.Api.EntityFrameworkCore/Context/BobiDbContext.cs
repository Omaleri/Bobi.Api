
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;
using Bobi.Api.Domain.Address;

namespace Bobi.Api.EntityFrameworkCore.Context
{
    public class BobiDbContext : DbContext
    {

        private readonly IMongoDatabase _database;

        public BobiDbContext(IConfiguration configuration)
        {
          var connectionString = configuration.GetSection("MongoDB:ConnectionString").Value;
            var databaseName = configuration.GetSection("MongoDB:DatabaseName").Value;

            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);
        }

        public IMongoCollection<Address> Addresses => _database.GetCollection<Address>("addresses");
        // Diğer koleksiyonlar da buraya eklenebilir

    }
}

