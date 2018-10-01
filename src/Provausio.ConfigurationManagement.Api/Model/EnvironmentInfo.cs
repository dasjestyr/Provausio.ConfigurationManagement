namespace Provausio.ConfigurationManagement.Api.Model
{
    using System.Collections.Generic;

    public class EnvironmentInfo
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string RequiredPermission { get; set; }

        public Dictionary<string, string> Metadata { get; set; }
    }
}