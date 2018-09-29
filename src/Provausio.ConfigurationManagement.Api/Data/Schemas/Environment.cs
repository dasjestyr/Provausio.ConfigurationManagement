namespace Provausio.ConfigurationManagement.Api.Data.Schemas
{
    using System.Collections.Generic;

    public class Environment
    {
        public string EnvironmentId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public Configuration Configuration { get; set; }

        public string RequiredPermission { get; set; }

        public Dictionary<string, string> Metadata { get; set; } = new Dictionary<string, string>();
    }
}