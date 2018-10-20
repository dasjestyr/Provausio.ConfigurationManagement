using System;
using System.Collections.Generic;

namespace Provausio.ConfigurationManagement.Api.Auth
{
    public class SystemRole
    {
        private const string GlobalAdminName      = "global_admin";
        private const string UserAdminName        = "user_admin";
        private const string ApplicationAdminName = "application_admin";
        private const string UserName             = "user";

        public static SystemRole GlobalAdmin      = new SystemRole(GlobalAdminName, "Full access user");
        public static SystemRole UserAdmin        = new SystemRole(UserAdminName, "Full access to all environment configurations for all projects.");
        public static SystemRole ApplicationAdmin = new SystemRole(ApplicationAdminName, "Full access to all environment configurations for a particular project.");
        public static SystemRole User             = new SystemRole(UserName, "Has access to unrestricted environments for a particular project.");

        public static IEnumerable<string> RoleValues
        {
            get
            {
                yield return GlobalAdminName;
                yield return UserAdminName;
                yield return ApplicationAdminName;
                yield return UserName;
            }
        }

        public string Name { get; }

        public string Description { get; }

        private SystemRole(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public static SystemRole FromValue(string value)
        {
            SystemRole role;
            switch (value)
            {
                case GlobalAdminName:
                    role = GlobalAdmin;
                    break;
                case UserAdminName:
                    role = UserAdmin;
                    break;
                case ApplicationAdminName:
                    role = ApplicationAdmin;
                    break;
                case UserName:
                    role = User;
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"Unknown role \"{value}\"");
            }

            return role;
        }
    }
}
