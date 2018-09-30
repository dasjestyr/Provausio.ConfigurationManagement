using System.Collections.Generic;
using MongoDB.Bson;

namespace Provausio.ConfigurationManagement.Api.Data.Schemas
{
    
    public class ApplicationDefinition
    {
        public ObjectId Id { get; set; }

        public string ApplicationId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public List<Environment> Environments { get; set; } = new List<Environment>();

        public Dictionary<string, string> Metadata { get; set; } = new Dictionary<string, string>();
    }
}
