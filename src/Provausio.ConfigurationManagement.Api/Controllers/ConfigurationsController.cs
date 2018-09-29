using Microsoft.AspNetCore.Mvc;

[Route("[controller]")]
public class ConfigurationsController : RestController
{
    /// <summary>
    /// Create a configuration.
    /// </summary>
    /// <param name="environmentId"></param>
    /// <returns></returns>
    [HttpPost, Route("/")]
    public IActionResult CreateConfiguration(string environmentId)
    {
        // do we wanna validate that the environment matches the intended application or simply
        // validate that the environmentId belongs to an application to which the user has access?

        var configurationDescriptor = new {};
        var configurationId = "";
        return CreatedWithLocation(configurationId, configurationDescriptor);
    }

    /// <summary>
    /// Retrieve configuration by id.
    /// </summary>
    /// <param name="configurationId"></param>
    /// <returns></returns>
    [HttpGet, Route("/{configurationId}")]
    public IActionResult GetConfiguration(string configurationId)
    {
        return Ok();
    }

    /// <summary>
    /// Updates the configuration.
    /// </summary>
    /// <param name="configurationId"></param>
    /// <returns></returns>
    [HttpPut, Route("/{configurationId}")]
    public IActionResult UpdateConfiguration(string configurationId)
    {
        return Ok();
    }

    /// <summary>
    /// Deletes the configuration.
    /// </summary>
    /// <param name="configurationId"></param>
    /// <returns></returns>
    [HttpDelete, Route("/{configurationId}")]
    public IActionResult DeleteConfiguration(string configurationId)
    {
        return Ok();
    }
}