namespace Provausio.ConfigurationManagement.Api.Model
{
    using System.Collections.Generic;

    public class ConfigurationInfo 
    {
        public string Content { get; set; }

        public string Format { get; set; }

        public Dictionary<string, string> Metadata { get; set; }
    }
}