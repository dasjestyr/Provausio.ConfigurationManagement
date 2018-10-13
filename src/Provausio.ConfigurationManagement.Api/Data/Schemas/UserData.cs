namespace Provausio.ConfigurationManagement.Api.Data.Schemas
{
    public class UserData
    {
        private string _normalizedUserName;
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }

        public string NormalizedUserName
        {
            get => _normalizedUserName;
            set => _normalizedUserName = value.ToLower();
        }

        public string Email { get; set; }
    }
}