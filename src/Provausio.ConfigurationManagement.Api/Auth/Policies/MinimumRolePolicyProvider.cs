using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Provausio.ConfigurationManagement.Api.Auth.Policies
{
    public class MinimumRolePolicyProvider : IAuthorizationPolicyProvider
    {
        public Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            if (!IsMinimumRoleAuth(policyName, out var level))
                return Task.FromResult<AuthorizationPolicy>(null);

            var policyBuilder = new AuthorizationPolicyBuilder();
            policyBuilder.AddRequirements(new MinimumRoleRequirement(SystemRole.FromLevel(level)));
            return Task.FromResult(policyBuilder.Build());
        }

        public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
        {
            return Task.FromResult(new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build());
        }

        private static bool IsMinimumRoleAuth(string policyName, out int level)
        {
            level = -1;
            if (!policyName.StartsWith(MinimumRoleLevelAttribute.PolicyPrefix, StringComparison.OrdinalIgnoreCase))
                return false;

            if (!int.TryParse(policyName.Substring(MinimumRoleLevelAttribute.PolicyPrefix.Length), out var minLevel))
                return false;

            level = minLevel;
            return true;
        }
    }
}