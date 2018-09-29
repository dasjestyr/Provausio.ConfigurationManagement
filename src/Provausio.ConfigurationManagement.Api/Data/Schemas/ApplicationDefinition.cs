using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Provausio.ConfigurationManagement.Api.Data.Schemas
{
    
    public class ApplicationDefinition
    {
        public string ApplicationId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public List<Environment> Environments { get; set; } = new List<Environment>();

        public Dictionary<string, string> Metadata { get; set; } = new Dictionary<string, string>();
    }

    public class Environment
    {
        public string EnvironmentId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public Configuration Configuration { get; set; }

        public string RequiredPermission { get; set; }

        public Dictionary<string, string> Metadata { get; set; } = new Dictionary<string, string>();
    }

    public class Configuration
    {
        public string ConfigurationId { get; set; }

        public string Content { get; set; }

        public Dictionary<string, string> Metadata { get; set; } = new Dictionary<string, string>();
    }
}
