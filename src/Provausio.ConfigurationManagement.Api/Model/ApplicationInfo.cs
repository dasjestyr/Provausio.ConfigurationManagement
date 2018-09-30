namespace Provausio.ConfigurationManagement.Api.Model
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class ApplicationInfo 
    {
        /// <summary>
        /// The application id
        /// </summary>
        public string ApplicationId { get; set; }

        /// <summary>
        /// Name of the application or component name.
        /// </summary>
        /// <value></value>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Description of the application or component.
        /// </summary>
        [Required]
        public string Description { get; set; }

        /// <summary>
        /// Includes metadata information about the application. For example, tech stack, repo location, etc.
        /// </summary>
        /// <value></value>
        public Dictionary<string, string> Metadata { get; set; } = new Dictionary<string, string>();
    }
}