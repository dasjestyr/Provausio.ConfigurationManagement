using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace Provausio.ConfigurationManagement.Api.Data.Schemas
{
    [BsonIgnoreExtraElements]
    public class UserData
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string NormalizedUserName { get; set; }
        public string Email { get; set; }
        public string NormalizedEmail { get; set; }
        public bool EmailConfirmed { get; set; }
        
        public List<string> Roles { get; set; } = new List<string>();
    }

    [BsonIgnoreExtraElements]
    public class RoleData
    {
        public string RoleId { get; set; }
        public string Name { get; set; }
        public string NormalizedName { get; set; }
    }
}