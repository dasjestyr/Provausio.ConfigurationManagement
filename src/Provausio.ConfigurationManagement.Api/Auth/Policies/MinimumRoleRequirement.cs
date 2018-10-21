using Microsoft.AspNetCore.Authorization;

namespace Provausio.ConfigurationManagement.Api.Auth.Policies
{
    public class MinimumRoleRequirement : IAuthorizationRequirement
    {
        public SystemRole Role { get; }

        public MinimumRoleRequirement(SystemRole role)
        {
            Role = role;
        }
    }
}