using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Provausio.ConfigurationManagement.Api.Data.Schemas;

namespace Provausio.ConfigurationManagement.Api.Auth
{
    public interface ITokenService
    {
        string GenerateToken(UserData user);
    }
    
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;

        public TokenService(IConfiguration config)
        {
            _config = config;
        }
        
        public string GenerateToken(UserData user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserId),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName),
                new Claim(JwtRegisteredClaimNames.FamilyName, user.LastName),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
                new Claim(ClaimTypes.Name, user.Username)
            };
            
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["CLIENT_SECRET"])); 
            var signingCreds = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            var expiration = DateTime.UtcNow.AddHours(Convert.ToDouble(_config["jwt_expire_hours"]));

            var token = new JwtSecurityToken(
                _config["jwt_issuer"],
                _config["jwt_audience"],
                claims,
                expires: expiration,
                signingCredentials: signingCreds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}