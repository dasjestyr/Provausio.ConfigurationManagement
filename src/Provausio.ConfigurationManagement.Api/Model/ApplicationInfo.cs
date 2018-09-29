namespace Provausio.ConfigurationManagement.Api.Model
{
    using System.Collections.Generic;

    public class ApplicationInfo 
    {
        /// <summary>
        /// Name of the application or component name.
        /// </summary>
        /// <value></value>
        public string Name { get; set; }

        /// <summary>
        /// Includes metadata information about the application. For example, tech stack, repo location, etc.
        /// </summary>
        /// <value></value>
        public Dictionary<string, string> Metadata { get; set; } = new Dictionary<string, string>();
    }
}