using System.Security;
using MongoDB.Bson.Serialization.Attributes;

namespace Provausio.ConfigurationManagement.Api.Data.Schemas
{
    [BsonIgnoreExtraElements]
    public class UserData
    {
        private string _normalizedUserName;
        private string _normalizedEmail;

        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public string NormalizedUserName
        {
            get => _normalizedUserName;
            set => _normalizedUserName = value.ToLower();
        }

        public string Email { get; set; }

        public string NormalizedEmail
        {
            get => _normalizedEmail;
            set => _normalizedEmail = value.ToLower();
        }

        public bool EmailConfirmed { get; set; }
    }

    public class RoleData
    {
        private string _normalizedName;
        
        public string RoleId { get; set; }
        public string Name { get; set; }

        public string NormalizedName
        {
            get => _normalizedName;
            set => _normalizedName = value.ToLower();
        }
    }
}