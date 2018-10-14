using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Provausio.ConfigurationManagement.Api.Data.Schemas;

namespace Provausio.ConfigurationManagement.Api.Auth
{
    public class RoleStore : IRoleStore<RoleData>
    {
        private readonly ILogger<RoleStore> _logger;
        private readonly IMongoCollection<RoleData> _roles;

        public RoleStore(IMongoDatabase database, ILogger<RoleStore> logger)
        {
            _logger = logger;
            _roles = database.GetCollection<RoleData>("userRoles");
        }
        
        public async Task<IdentityResult> CreateAsync(RoleData role, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                if(role == null) throw new ArgumentNullException(nameof(role));
                
                await _roles.InsertOneAsync(role, cancellationToken: cancellationToken).ConfigureAwait(false);
                
                return IdentityResult.Success;
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, $"Failed to create role {role?.Name ?? "undefined"}!");
                return IdentityResult.Failed(new IdentityError
                {
                    Code = Constants.GenericIdentityErrorCode,
                    Description = e.Message
                });
            }
        }

        public async Task<IdentityResult> UpdateAsync(RoleData role, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                if(role == null) throw new ArgumentNullException(nameof(role));

                await _roles.ReplaceOneAsync(
                        Builders<RoleData>.Filter.Eq(r => r.RoleId, role.RoleId), role,
                        cancellationToken: cancellationToken)
                    .ConfigureAwait(false);
                
                return IdentityResult.Success;
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, $"Failed to update role {role?.Name ?? "undefined"}!");
                return IdentityResult.Failed(new IdentityError
                {
                    Code = Constants.GenericIdentityErrorCode,
                    Description = e.Message
                });
            }
        }

        public async Task<IdentityResult> DeleteAsync(RoleData role, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                if(role == null) throw new ArgumentNullException(nameof(role));

                await _roles.DeleteOneAsync(
                        Builders<RoleData>.Filter.Eq(r => r.RoleId, role.RoleId),
                        cancellationToken: cancellationToken)
                    .ConfigureAwait(false);
                
                return IdentityResult.Success;
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, $"Failed to create role {role?.Name ?? "undefined"}!");
                return IdentityResult.Failed(new IdentityError
                {
                    Code = Constants.GenericIdentityErrorCode,
                    Description = e.Message
                });
            }
        }

        public Task<string> GetRoleIdAsync(RoleData role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if(role == null) throw new ArgumentNullException(nameof(role));
            return Task.FromResult(role.RoleId);
        }

        public Task<string> GetRoleNameAsync(RoleData role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if(role == null) throw new ArgumentNullException(nameof(role));
            return Task.FromResult(role.Name);
        }

        public Task SetRoleNameAsync(RoleData role, string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if(role == null) throw new ArgumentNullException(nameof(role));
            role.Name = roleName;
            return Task.CompletedTask;
        }

        public Task<string> GetNormalizedRoleNameAsync(RoleData role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if(role == null) throw new ArgumentNullException(nameof(role));
            return Task.FromResult(role.NormalizedName);
        }

        public Task SetNormalizedRoleNameAsync(RoleData role, string normalizedName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if(role == null) throw new ArgumentNullException(nameof(role));
            role.NormalizedName = normalizedName;
            return Task.CompletedTask;
        }

        public async Task<RoleData> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                if(roleId == null) throw new ArgumentNullException(nameof(roleId));

                var roles = await _roles.FindAsync(
                        Builders<RoleData>.Filter.Eq(r => roleId, roleId), 
                        cancellationToken: cancellationToken)
                    .ConfigureAwait(false);

                return await roles.SingleOrDefaultAsync(cancellationToken).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, $"Failed to find role {roleId ?? "undefined"}!");
                return null;
            }
        }

        public async Task<RoleData> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                if(normalizedRoleName == null) throw new ArgumentNullException(nameof(normalizedRoleName));

                var roles = await _roles.FindAsync(
                        Builders<RoleData>.Filter.Eq(r => r.NormalizedName, normalizedRoleName),
                        cancellationToken: cancellationToken)
                    .ConfigureAwait(false);

                return await roles.SingleOrDefaultAsync(cancellationToken).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, $"Failed to create role {normalizedRoleName ?? "undefined"}!");
                return null;
            }
        }
        
        public void Dispose() { }
    }
}