using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Provausio.ConfigurationManagement.Api.Data.Schemas;

namespace Provausio.ConfigurationManagement.Api.Auth
{
    public class UserProvider : IUserStore<UserData>
    {
        private readonly ILogger<UserProvider> _logger;
        private readonly IMongoCollection<UserData> _users;

        public UserProvider(IMongoDatabase database, ILogger<UserProvider> logger)
        {
            _logger = logger;
            _users = database.GetCollection<UserData>("users");
        }
        
        public void Dispose() { }

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
                _logger.LogCritical(e, $"Failed to update user {normalizedUserName ?? "undefined"}");
                return null;
            }
        }
    }
}