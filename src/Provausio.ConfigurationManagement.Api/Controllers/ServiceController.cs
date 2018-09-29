namespace Provausio.ConfigurationManagement.Api.Controllers
{
    using System;
    using Microsoft.AspNetCore.Mvc;

    public abstract class RestController : Controller
    {
        protected IActionResult CreatedWithLocation(string id, object responseObject)
        {
            return Created(new Uri($"{Request.Path}/{id}", UriKind.Relative), responseObject);
        }
    }
}