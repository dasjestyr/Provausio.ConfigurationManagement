using Provausio.ConfigurationManagement.Api.Data.Schemas;

namespace Provausio.ConfigurationManagement.Api.Auth
{
    public interface ITokenHandler
    {
        string GenerateToken(UserData user);
    }
}