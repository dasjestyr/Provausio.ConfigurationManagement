using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Newtonsoft.Json;
using Provausio.ConfigurationManagement.Api.Data.Schemas;

namespace Provausio.ConfigurationManagement.Api.Auth
{
    public class UserStore : IUserEmailStore<UserData>, IUserPasswordStore<UserData>, IUserRoleStore<UserData>
    {
        private readonly ILogger<UserStore> _logger;
        private readonly IMongoCollection<UserData> _users;

        public UserStore(IMongoDatabase database, ILogger<UserStore> logger)
        {
            _logger = logger;
            _users = database.GetCollection<UserData>("users");
        }

        public Task<string> GetUserIdAsync(UserData user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if(user == null) throw new ArgumentNullException(nameof(user));
            return Task.FromResult(user.UserId);
        }

        public Task<string> GetUserNameAsync(UserData user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if(user == null) throw new ArgumentNullException(nameof(user));
            return Task.FromResult(user.Username);
        }

        public Task SetUserNameAsync(UserData user, string userName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if(user == null) throw new ArgumentNullException(nameof(user));
            user.Username = userName;
            return Task.CompletedTask;
        }

        public Task<string> GetNormalizedUserNameAsync(UserData user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if(user == null) throw new ArgumentNullException(nameof(user));
            return Task.FromResult(user.NormalizedUserName);
        }

        public Task SetNormalizedUserNameAsync(UserData user, string normalizedName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if(user == null) throw new ArgumentNullException(nameof(user));
            user.NormalizedUserName = normalizedName;
            return Task.CompletedTask;
        }

        public async Task<IdentityResult> CreateAsync(UserData user, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                if(user == null) throw new ArgumentNullException(nameof(user));
                await _users.InsertOneAsync(user, cancellationToken: cancellationToken).ConfigureAwait(false);
                return IdentityResult.Success;
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, $"Failed to create {user?.Username ?? "user."}");
                return IdentityResult.Failed(new IdentityError
                {
                    Code = Constants.GenericIdentityErrorCode,
                    Description = e.Message
                });
            }
        }

        public async Task<IdentityResult> UpdateAsync(UserData user, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                if(user == null) throw new ArgumentNullException(nameof(user));

                await _users.ReplaceOneAsync(
                        Builders<UserData>.Filter.Eq(u => u.UserId, user.UserId), 
                        user, 
                        cancellationToken: cancellationToken)
                    .ConfigureAwait(false);
                
                return IdentityResult.Success;
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, $"Failed to update user {user?.Username ?? "user"}");
                return IdentityResult.Failed(new IdentityError
                {
                    Code = Constants.GenericIdentityErrorCode, 
                    Description = e.Message
                });
            }
        }

        public async Task<IdentityResult> DeleteAsync(UserData user, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                if(user == null) throw new ArgumentNullException(nameof(user));

                await _users.DeleteOneAsync(
                        Builders<UserData>.Filter.Eq(u => u.UserId, user.UserId),
                        cancellationToken: cancellationToken)
                    .ConfigureAwait(false);
                
                return IdentityResult.Success;
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, $"Failed to delete user {user?.Username ?? "user"}");
                return IdentityResult.Failed();
            }
        }

        public async Task<UserData> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                if(string.IsNullOrEmpty(userId)) throw new ArgumentNullException(nameof(userId));

                var user = await _users.FindAsync(
                        Builders<UserData>.Filter.Eq(u => u.UserId, userId), 
                        cancellationToken: cancellationToken)
                    .ConfigureAwait(false);

                return await user.SingleOrDefaultAsync(cancellationToken).ConfigureAwait(false);

            }
            catch (Exception e)
            {
                _logger.LogCritical(e, $"Failed to find user {userId ?? "undefined"}");
                return null;
            }
        }

        public async Task<UserData> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                if(string.IsNullOrEmpty(normalizedUserName)) throw new ArgumentNullException(nameof(normalizedUserName));

                var user = await _users.FindAsync(
                        Builders<UserData>.Filter.Eq(u => u.NormalizedUserName, normalizedUserName),
                        cancellationToken: cancellationToken)
                    .ConfigureAwait(false);

                return await user.SingleOrDefaultAsync(cancellationToken).ConfigureAwait(false);

            }
            catch (Exception e)
            {
                _logger.LogCritical(e, $"Failed to find user {normalizedUserName ?? "undefined"}");
                return null;
            }
        }
        
        public void Dispose() { }
        
        public Task SetEmailAsync(UserData user, string email, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if(string.IsNullOrEmpty(email)) throw new ArgumentNullException(nameof(email));
            if(user == null) throw new ArgumentNullException(nameof(user));
            user.Email = email;
            return Task.CompletedTask;
        }

        public Task<string> GetEmailAsync(UserData user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if(user == null) throw new ArgumentNullException(nameof(user));
            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(UserData user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if(user == null) throw new ArgumentNullException(nameof(user));
            return Task.FromResult(user.EmailConfirmed);
        }

        public Task SetEmailConfirmedAsync(UserData user, bool confirmed, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            user.EmailConfirmed = confirmed;
            return Task.CompletedTask;
        }

        public async Task<UserData> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                if(string.IsNullOrEmpty(normalizedEmail)) throw new ArgumentNullException(nameof(normalizedEmail));

                var users = await _users.FindAsync(
                        Builders<UserData>.Filter.Eq(u => u.NormalizedEmail, normalizedEmail),
                        cancellationToken: cancellationToken)
                    .ConfigureAwait(false);

                return await users.SingleOrDefaultAsync(cancellationToken).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, $"Failed to find user {normalizedEmail ?? "undefined"}");
                return null;
            }
        }

        public Task<string> GetNormalizedEmailAsync(UserData user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if(user == null) throw new ArgumentNullException(nameof(user));
            return Task.FromResult(user.NormalizedEmail);
        }

        public Task SetNormalizedEmailAsync(UserData user, string normalizedEmail, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if(user == null) throw new ArgumentNullException(nameof(user));
            if(string.IsNullOrEmpty(normalizedEmail)) throw new ArgumentNullException(nameof(normalizedEmail));
            user.NormalizedEmail = normalizedEmail;
            return Task.CompletedTask;
        }

        public Task SetPasswordHashAsync(UserData user, string passwordHash, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if(user == null) throw new ArgumentNullException(nameof(user));
            if(string.IsNullOrEmpty(passwordHash)) throw new ArgumentNullException(nameof(passwordHash));
            user.Password = passwordHash;
            return Task.CompletedTask;
        }

        public Task<string> GetPasswordHashAsync(UserData user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if(user == null) throw new ArgumentNullException(nameof(user));
            return Task.FromResult(user.Password);
        }

        public Task<bool> HasPasswordAsync(UserData user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentException(nameof(user));
            return Task.FromResult(!string.IsNullOrEmpty(user.Password));
        }

        public async Task AddToRoleAsync(UserData user, string roleName, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                if(user == null) throw new ArgumentNullException(nameof(user));
                if(string.IsNullOrEmpty(roleName)) throw new ArgumentNullException(nameof(roleName));

                // BUG: What the fuck? The query is correct. It works if you run it by hand
                // in CLI but it doesn't work here???
                
                _logger.LogInformation($"Updating user {user.UserId}");                
                var x = await _users.UpdateOneAsync(
                        Builders<UserData>.Filter.Eq(u => u.UserId, user.UserId), 
                        Builders<UserData>.Update.AddToSet(u => u.Roles, roleName),
                        cancellationToken: cancellationToken)
                    .ConfigureAwait(false);

                // BUG: this even says that it was modified!
                var json = JsonConvert.SerializeObject(x, Formatting.Indented);
                
                _logger.LogInformation(json);
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, $"Failed to add user {user?.Username ?? "undefined"} to role {roleName ?? "undefined"}");
            }
        }

        public async Task RemoveFromRoleAsync(UserData user, string roleName, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                if(user == null) throw new ArgumentNullException(nameof(user));
                if(string.IsNullOrEmpty(roleName)) throw new ArgumentNullException(nameof(roleName));

                var pullFilter = Builders<UserData>.Update.PullFilter(
                    u => u.Roles, r => r == roleName);

                await _users
                    .FindOneAndUpdateAsync(
                        u => u.UserId == user.UserId, 
                        pullFilter,
                        cancellationToken: cancellationToken)
                    .ConfigureAwait(false);

            }
            catch (Exception e)
            {
                _logger.LogCritical(e, $"Failed to remove user {user?.Username ?? "undefined"} from role {roleName ?? "undefined"}");
            }
        }

        public async Task<IList<string>> GetRolesAsync(UserData user, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                if(user == null) throw new ArgumentNullException(nameof(user));

                var users = await _users.FindAsync(
                        u => u.UserId == user.UserId, 
                        cancellationToken: cancellationToken)
                    .ConfigureAwait(false);

                var found = await users.SingleOrDefaultAsync(cancellationToken).ConfigureAwait(false);
                return found?.Roles;
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, $"Failed to retrieve roles for {user?.Username ?? "undefined"}");
                return null;
            }
        }

        public async Task<bool> IsInRoleAsync(UserData user, string roleName, CancellationToken cancellationToken)
        {
            var found = await FindByIdAsync(user.UserId, cancellationToken).ConfigureAwait(false);
            return found?.Roles != null && found.Roles.Contains(roleName);
        }

        public async Task<IList<UserData>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if(string.IsNullOrEmpty(roleName)) throw new ArgumentNullException(nameof(roleName));

            var users = await _users.FindAsync(
                    Builders<UserData>.Filter.ElemMatch(
                        u => u.Roles, 
                        Builders<string>.Filter.Eq(r => r, roleName)),
                    cancellationToken: cancellationToken)
                .ConfigureAwait(false);

            return await users.ToListAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}