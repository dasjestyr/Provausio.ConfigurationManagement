using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Provausio.ConfigurationManagement.Api.Auth.Policies
{
    public class MinimumRoleHandler : AuthorizationHandler<MinimumRoleRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumRoleRequirement requirement)
        {
            // NOTE: "Highest" actually means lowest. In other words 1 is "higher" than 2.
            
            var roleClaim = context.User.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Role);
            if (roleClaim == null)
            {
                context.Fail();
                return Task.CompletedTask;
            }

            var systemRoles = roleClaim.Value
                .Split(',')
                .Select(SystemRole.FromValue)
                .OrderBy(r => r.Priority);

            var highestRole = systemRoles.Min(r => r.Priority);
            if (highestRole <= requirement.Role.Priority)
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }

            return Task.CompletedTask;
        }
    }
}