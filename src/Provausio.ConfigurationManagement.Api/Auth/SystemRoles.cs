using System;
using System.Collections.Generic;
using System.Linq;

namespace Provausio.ConfigurationManagement.Api.Auth
{
    public class SystemRole
    {
        public const string GlobalAdminName      = "global_admin";
        public const string UserAdminName        = "user_admin";
        public const string ApplicationAdminName = "application_admin";
        public const string UserName             = "user";

        public static SystemRole GlobalAdmin      = new SystemRole(GlobalAdminName, "Full access user", 0);
        public static SystemRole UserAdmin        = new SystemRole(UserAdminName, "Full access to all environment configurations for all projects.", 1);
        public static SystemRole ApplicationAdmin = new SystemRole(ApplicationAdminName, "Full access to all environment configurations for a particular project.", 2);
        public static SystemRole User             = new SystemRole(UserName, "Has access to unrestricted environments for a particular project.", 3);

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

        public static IEnumerable<SystemRole> Roles
        {
            get
            {
                yield return GlobalAdmin;
                yield return UserAdmin;
                yield return ApplicationAdmin;
                yield return User;
            }
        }

        public string Name { get; }

        public string Description { get; }

        public int Priority { get; }

        private SystemRole(string name, string description, int priority)
        {
            Name = name;
            Description = description;
            Priority = priority;
        }

        public static SystemRole FromLevel(int level)
        {
            return Roles.SingleOrDefault(r => r.Priority == level);
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

        public override bool Equals(object obj)
        {
            return Equals(obj as SystemRole);
        }

        protected bool Equals(SystemRole other)
        {
            if (other == null) return false;
            return string.Equals(Name, other.Name) && 
                   string.Equals(Description, other.Description) && 
                   Priority == other.Priority;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Description != null ? Description.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Priority;
                return hashCode;
            }
        }
    }
}
