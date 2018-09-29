namespace Provausio.ConfigurationManagement.Api.Data.Schemas
{
    using System.Collections.Generic;

    public class Configuration
    {
        public string ConfigurationId { get; set; }

        public string Content { get; set; }

        public Dictionary<string, string> Metadata { get; set; } = new Dictionary<string, string>();
    }
}