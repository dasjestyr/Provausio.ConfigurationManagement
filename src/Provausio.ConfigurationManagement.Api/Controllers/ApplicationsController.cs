using System;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// API for managing applications and environment definitions.
/// </summary>
[Route("[controller]")]
public class ApplicationsController : RestController
{
    /// <summary>
    /// Returns a list of applications that are visible to the user.
    /// </summary>
    /// <returns></returns>s
    [HttpGet, Route("/")]
    public IActionResult GetApplications()
    {
        return Ok();
    }

    /// <summary>
    /// Creates a new application on the user's account.
    /// </summary>
    /// <param name="id">A unique identifier for the application.</param>
    /// <returns></returns>
    [HttpPost, Route("/")]
    public IActionResult CreateApplication()
    {
        var id = "";
        var applicationDescription = new {};
        return CreatedWithLocation(id, applicationDescription);
    }

    /// <summary>
    /// Updates the application
    /// </summary>
    /// <param name="applicationId"></param>
    /// <returns></returns>
    [HttpPut, Route("/{applicationId}")]
    public IActionResult UpdateApplication(string applicationId)
    {
        return Ok();
    }

    /// <summary>
    /// Deletes the application.
    /// </summary>
    /// <param name="applicationId"></param>
    /// <returns></returns>
    [HttpDelete, Route("/{applicationId}")]
    public IActionResult DeleteApplication(string applicationId)
    {
        return Ok();
    }

    /// <summary>
    /// Retrieves all environments configured for an application.
    /// </summary>
    /// <param name="applicationId"></param>
    /// <returns></returns>
    [HttpGet, Route("/{applicationId}/environments")]
    public IActionResult GetEnvironments(string applicationId) 
    {
        return Ok();
    }

    /// <summary>
    /// Creates a new environment for the application
    /// </summary>
    /// <param name="applicationId"></param>
    /// <returns></returns>
    [HttpPost, Route("/{applicationId}/environments")]
    public IActionResult AddEnvironment(string applicationId)
    {
        var environmentId = "";
        var environmentDescription = new {};
        return CreatedWithLocation(environmentId, environmentDescription);
    }

    /// <summary>
    /// Updates the environment.
    /// </summary>
    /// <param name="applicationId"></param>
    /// <param name="environmentId"></param>
    /// <returns></returns>
    [HttpPut, Route("/{applicationId}/environments/{environmentId}")]
    public IActionResult UpdateEnvironment(string applicationId, string environmentId)
    {
        return Ok();
    }

    /// <summary>
    /// Deletes the environment from the application.
    /// </summary>
    /// <param name="environmentId"></param>
    /// <returns></returns>
    [HttpDelete, Route("/{applicationId}/environments/{environmentId}")]
    public IActionResult DeleteEnvironment(string environmentId)
    {
        return Ok();
    }
}