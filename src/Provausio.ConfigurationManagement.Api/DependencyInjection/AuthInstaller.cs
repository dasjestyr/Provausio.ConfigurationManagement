using System;
using System.Data;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Provausio.ConfigurationManagement.Api.Auth;
using Provausio.ConfigurationManagement.Api.Data.Schemas;
using XidNet;

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
                .AddRoleManager<RoleManager<RoleData>>()
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

            var provider = services.BuildServiceProvider();
            SetDefaultUser(provider, config);
        }

        private static void SetDefaultUser(IServiceProvider provider, IConfiguration config)
        {
            var userManager = provider.GetRequiredService<UserManager<UserData>>();
            var roleManager = provider.GetRequiredService<RoleManager<RoleData>>();
            var logger = provider.GetRequiredService<ILogger<Startup>>();

            // make sure roles exist
            logger.LogInformation("Ensuring base roles...");
            foreach (var role in SystemRole.RoleValues)
            {
                if (roleManager.RoleExistsAsync(role.ToUpper()).Result) continue;
                var roleData = new RoleData
                {
                    Name = role, 
                    NormalizedName = role.ToLower(), 
                    RoleId = Xid.NewXid().ToString()
                };
                
                var roleResult = roleManager.CreateAsync(roleData).Result;
                if(!roleResult.Succeeded) throw new ApplicationException("Failed to add base roles.");
            }
            
            // make sure default user exists with the correct role
            logger.LogInformation("Ensuring default user...");
            var defaultUserInfo = config.GetSection("default_user").Get<UserData>();
            var defaultUser = userManager.FindByNameAsync(defaultUserInfo.Username).Result;

            var adminRole = SystemRole.ApplicationAdmin.Name;

            if (defaultUser != null)
            {
                if (defaultUser.Roles.Contains(adminRole)) return;
                defaultUser.Roles.Add(adminRole);
                userManager.UpdateAsync(defaultUser).Wait();
            }
            else
            {
                defaultUserInfo.UserId = Xid.NewXid().ToString();
                defaultUserInfo.Roles.Add(adminRole);
                var result = userManager.CreateAsync(defaultUserInfo, defaultUserInfo.Password).Result;
                if (!result.Succeeded)
                    throw new ApplicationException("Failed to set default user!!");

                logger.LogInformation("Ensuring default user roles...");
                var addRoleResult = userManager.AddToRoleAsync(defaultUserInfo, "SystemAdmin").Result;
                if (!addRoleResult.Succeeded) throw new ApplicationException("Failed to add admin user to admin role");
            }
        }
    }
}