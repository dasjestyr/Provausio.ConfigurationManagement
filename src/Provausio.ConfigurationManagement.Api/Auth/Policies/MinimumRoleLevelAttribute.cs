using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Provausio.ConfigurationManagement.Api.Auth.Policies
{
    public class MinimumRoleLevelAttribute : AuthorizeAttribute
    {
        public const string PolicyPrefix = "MinimumLevel";

        public MinimumRoleLevelAttribute(string role)
        {
            Role = SystemRole.FromValue(role);
            MinimumLevel = Role.Priority;
        }

        public int MinimumLevel
        {
            get => int.TryParse(Policy.Substring(PolicyPrefix.Length), out var level) ? level : default(int);
            set => Policy = $"{PolicyPrefix}{value}";
        }

        public SystemRole Role { get; set; }
    }
}
