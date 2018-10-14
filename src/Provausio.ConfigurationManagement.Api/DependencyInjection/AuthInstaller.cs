using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Provausio.ConfigurationManagement.Api.Auth;
using Provausio.ConfigurationManagement.Api.Data.Schemas;

namespace Provausio.ConfigurationManagement.Api.DependencyInjection
{
    public static class AuthInstaller
    {
        public static void AddAuth(this IServiceCollection services, IConfiguration config)
        {
            services.AddTransient<ITokenService, TokenService>();
            
            services
                .AddIdentity<UserData, RoleData>(options =>
                {
                    options.SignIn.RequireConfirmedEmail = false;
                    options.SignIn.RequireConfirmedPhoneNumber = false;
                })
                .AddUserManager<UserManager<UserData>>()
                .AddSignInManager<SignInManager<UserData>>()
                .AddUserStore<UserStore>()
                .AddRoleStore<RoleStore>()
                .AddDefaultTokenProviders();


            var clientSecret = Encoding.UTF8.GetBytes(config["CLIENT_SECRET"]);
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Audience = config["jwt_audience"];
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = config["jwt_issuer"],
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(clientSecret)
                };
            });
        }
    }
}