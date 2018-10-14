using System.ComponentModel.DataAnnotations;

namespace Provausio.ConfigurationManagement.Api.Model.UserManagement
{
    public class UserInfo
    {
        [Required, MaxLength(100)]
        public string FirstName { get; set; }
        
        [Required, MaxLength(100)]
        public string LastName { get; set; }
        
        [Required, MaxLength(100)]
        public string Username { get; set; }

        [Required, MaxLength(100)]
        public string Password { get; set; }
        
        [Required, EmailAddress, MaxLength(100)]
        public string Email { get; set; }
    }
}