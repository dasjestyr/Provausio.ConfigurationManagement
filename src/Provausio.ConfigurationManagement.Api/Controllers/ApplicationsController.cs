namespace Provausio.ConfigurationManagement.Api.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;
    using Data;
    using Microsoft.AspNetCore.Mvc;
    using Model;
    using XidNet;

    /// <summary>
    /// API for managing applications and environment definitions.
    /// </summary>
    [Route("[controller]")]
    public class ApplicationsController : RestController
    {
        private readonly IApplicationDefinitionStore _definitionStore;

        public ApplicationsController(IApplicationDefinitionStore definitionStore)
        {
            _definitionStore = definitionStore;
        }

        /// <summary>
        /// Returns a list of applications.
        /// </summary>
        /// <returns></returns>s
        [HttpGet, Route("/")]
        public async Task<IActionResult> GetApplications()
        {
            var environments = await _definitionStore.GetApplications().ConfigureAwait(false);
            if (environments == null || !environments.Any())
                return NotFound();
            return Ok(environments);
        }

        /// <summary>
        /// Defines a new application.
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("/")]
        public async Task<IActionResult> CreateApplication(ApplicationInfo payload)
        {
            var appId = Xid.NewXid().ToString();
            payload.ApplicationId = appId;
            await _definitionStore.CreateApplication(appId, payload).ConfigureAwait(false);

            return CreatedWithLocation(appId, payload);
        }

        /// <summary>
        /// Updates the application
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="payload"></param>
        /// <returns></returns>
        [HttpPut, Route("/{applicationId}")]
        public async Task<IActionResult> UpdateApplication(string applicationId, ApplicationInfo payload)
        {
            await _definitionStore.UpdateApplication(applicationId, payload).ConfigureAwait(false);
            return Ok(payload);
        }

        /// <summary>
        /// Deletes the application.
        /// </summary>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        [HttpDelete, Route("/{applicationId}")]
        public async Task<IActionResult> DeleteApplication(string applicationId)
        {
            await _definitionStore.DeleteApplication(applicationId).ConfigureAwait(false);
            return NoContent();
        }

        /// <summary>
        /// Retrieves all environments configured for an application.
        /// </summary>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        [HttpGet, Route("/{applicationId}/environments")]
        public async Task<IActionResult> GetEnvironments(string applicationId)
        {
            var environments = await _definitionStore.GetEnvironments(applicationId).ConfigureAwait(false);
            if (environments == null || !environments.Any())
                return NotFound();
            return Ok(environments);
        }

        /// <summary>
        /// Creates a new environment for the application
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="payload"></param>
        /// <returns></returns>
        [HttpPost, Route("/{applicationId}/environments")]
        public async Task<IActionResult> AddEnvironment(string applicationId, EnvironmentInfo payload)
        {
            await _definitionStore.CreateEnvironment(applicationId, payload.Name, payload).ConfigureAwait(false);
            return CreatedWithLocation(payload.Name, payload);
        }

        /// <summary>
        /// Updates the environment.
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="name"></param>
        /// <param name="payload"></param>
        /// <returns></returns>
        [HttpPut, Route("/{applicationId}/environments/{name}")]
        public async Task<IActionResult> UpdateEnvironment(string applicationId, string name, EnvironmentInfo payload)
        {
            if (payload.Name != name)
                return BadRequest("names do not match");

            await _definitionStore.UpdateEnvironment(applicationId, name, payload).ConfigureAwait(false);
            return Ok(payload);
        }

        /// <summary>
        /// Deletes the environment from the application.
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="environmentId"></param>
        /// <returns></returns>
        [HttpDelete, Route("/{applicationId}/environments/{environmentId}")]
        public async Task<IActionResult> DeleteEnvironment(string applicationId, string environmentId)
        {
            await _definitionStore.DeleteEnvironment(applicationId, environmentId).ConfigureAwait(false);
            return NoContent();
        }
    }
}