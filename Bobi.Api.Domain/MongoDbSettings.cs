using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bobi.Api.Domain
{
    public class MongoDbSettings
    {
        public string ConnectionURI { get; set; } = null!;
        public string DataBaseName { get; set; } = null!;
        public string CollectionName { get; set; } = null!;
    }
}
