namespace Provausio.ConfigurationManagement.Api.Controllers
{
    using System.Threading.Tasks;
    using Data;
    using Microsoft.AspNetCore.Mvc;
    using Model;

    [Route("applications/{applicationId}/environment/{environmentName}")]
    public class ConfigurationsController : RestController
    {
        private readonly IApplicationDefinitionStore _definitionStore;

        public ConfigurationsController(IApplicationDefinitionStore definitionStore)
        {
            _definitionStore = definitionStore;
        }

        /// <summary>
        /// Create a configuration.
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="environmentName"></param>
        /// <param name="environmentId"></param>
        /// <param name="payload"></param>
        /// <returns></returns>
        [HttpPost, Route("/")]
        public async Task<IActionResult> CreateConfiguration(
            string applicationId, 
            string environmentName, 
            string environmentId, 
            ConfigurationInfo payload)
        {
            await _definitionStore.SaveConfiguration(applicationId, environmentName, payload).ConfigureAwait(false);
            return CreatedWithLocation(environmentName, payload);
        }

        /// <summary>
        /// Retrieve configuration by id.
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="environmentName"></param>
        /// <returns></returns>
        [HttpGet, Route("/configuration")]
        public async Task<IActionResult> GetConfiguration(string applicationId, string environmentName)
        {
            var configuration = await _definitionStore
                .GetConfiguration(applicationId, environmentName)
                .ConfigureAwait(false);

            if (configuration == null)
                return NotFound();
            return Ok(configuration);
        }

        /// <summary>
        /// Updates the configuration.
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="environmentName"></param>
        /// <param name="payload"></param>
        /// <returns></returns>
        [HttpPut, Route("/configuration")]
        public async Task<IActionResult> UpdateConfiguration(string applicationId, string environmentName, ConfigurationInfo payload)
        {
            await _definitionStore
                .SaveConfiguration(applicationId, environmentName, payload)
                .ConfigureAwait(false);

            return Ok(payload);
        }

        /// <summary>
        /// Deletes the configuration.
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="environmentName"></param>
        /// <returns></returns>
        [HttpDelete, Route("/{configurationId}")]
        public async Task<IActionResult> DeleteConfiguration(string applicationId, string environmentName)
        {
            await _definitionStore.SaveConfiguration(applicationId, environmentName, new ConfigurationInfo());
            return NoContent();
        }
    }
}