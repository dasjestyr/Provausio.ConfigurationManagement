namespace Provausio.ConfigurationManagement.Api.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using MongoDB.Driver;
    using Schemas;
    using System.Threading.Tasks;
    using Model;


    public class ApplicationDefinitionStore : IApplicationDefinitionStore
    {
        private readonly IMongoCollection<ApplicationDefinition> _collection;

        public ApplicationDefinitionStore(IMongoDatabase mongoDatabase)
        {
            _collection = mongoDatabase.GetCollection<ApplicationDefinition>("applications");
        }

        public Task CreateApplication(string appId, ApplicationInfo application)
        {
            var data = MapApplication(application);
            data.ApplicationId = appId;
            return _collection.InsertOneAsync(data);
        }

        public async Task<IEnumerable<ApplicationInfo>> GetApplications()
        {
            var results = await _collection.Find(_ => true).ToListAsync();
            return results.Select(MapApplicationInfo);
        }

        public async Task<ApplicationInfo> GetApplication(string id)
        {
            var result = await _collection.FindAsync(
                Builders<ApplicationDefinition>.Filter.Eq(doc => doc.ApplicationId, id));
            var def = await result.FirstOrDefaultAsync();
            return def == null ? null : MapApplicationInfo(def);
        }

        public Task UpdateApplication(string appId, ApplicationInfo application)
        {
            var data = MapApplication(application);
            return _collection.ReplaceOneAsync(
                Builders<ApplicationDefinition>.Filter.Eq(doc => doc.ApplicationId, appId),
                data);
        }

        public Task DeleteApplication(string appId)
        {
            return _collection.DeleteOneAsync(
                Builders<ApplicationDefinition>.Filter.Eq(doc => doc.ApplicationId, appId));
        }

        public Task CreateEnvironment(string appId, string environmentId, EnvironmentInfo environment)
        {
            var env = MapEnvironment(environment);
            env.EnvironmentId = environmentId;
            return _collection.UpdateOneAsync(
                Builders<ApplicationDefinition>.Filter.Eq(doc => doc.ApplicationId, appId),
                Builders<ApplicationDefinition>.Update.AddToSet(doc => doc.Environments, env),
                new UpdateOptions {IsUpsert = true});
        }

        public async Task<IEnumerable<EnvironmentInfo>> GetEnvironments(string appId)
        {
            
            var query = _collection
                .Find(Builders<ApplicationDefinition>.Filter.Eq(doc => doc.ApplicationId, appId))
                .Project(definition => definition.Environments);

            var results = await query.ToListAsync();

            return results.FirstOrDefault()?.Select(MapEnvironmentInfo);
        }

        public Task UpdateEnvironment(string appId, string environmentId, EnvironmentInfo environment)
        {
            var replacement = MapEnvironment(environment);
            return _collection.FindOneAndUpdateAsync(
                c => c.ApplicationId == appId && c.Environments.Any(e => e.EnvironmentId == environmentId),
                Builders<ApplicationDefinition>.Update.Set(doc => doc.Environments[-1], replacement));
        }

        public Task DeleteEnvironment(string appId, string environmentId)
        {
            return _collection.FindOneAndUpdateAsync(
                c => c.ApplicationId == appId && c.Environments.Any(e => e.EnvironmentId == environmentId),
                Builders<ApplicationDefinition>.Update.PullFilter(doc => doc.Environments,
                    el => el.EnvironmentId == environmentId));
        }

        public Task SaveConfiguration(string appId, string environmentId, ConfigurationInfo info)
        {
            var configData = MapConfiguration(info);
            return _collection.FindOneAndUpdateAsync(
                c => c.ApplicationId == appId && c.Environments.Any(e => e.EnvironmentId == environmentId),
                Builders<ApplicationDefinition>.Update.Set(doc => doc.Environments[-1].Configuration, configData));
        }

        public async Task<ConfigurationInfo> GetConfiguration(string appId, string environmentId)
        {
            var result = await _collection
                .Find(doc => doc.ApplicationId == appId && doc.Environments.Any(e => e.EnvironmentId == environmentId))
                .Project(doc => doc.Environments[-1].Configuration)
                .FirstOrDefaultAsync()
                .ConfigureAwait(false);

            return result == null ? null : MapConfigurationInfo(result);
        }

        private static ApplicationDefinition MapApplication(ApplicationInfo info)
        {
            var data = new ApplicationDefinition
            {
                Name = info.Name,
                Description = info.Description,
                Metadata = info.Metadata
            };
            return data;
        }

        private static ApplicationInfo MapApplicationInfo(ApplicationDefinition definition)
        {
            return new ApplicationInfo
            {
                ApplicationId = definition.ApplicationId,
                Name = definition.Name,
                Description = definition.Description,
                Metadata = definition.Metadata
            };
        }

        private static Environment MapEnvironment(EnvironmentInfo info)
        {
            var environment = new Environment
            {
                EnvironmentId = info.Id,
                Name = info.Name,
                Description = info.Description,
                Configuration = info.Configuration != null
                    ? MapConfiguration(info.Configuration)
                    : null,
                RequiredPermission = info.RequiredPermission,
                Metadata = info.Metadata ?? new Dictionary<string, string>()
            };
            return environment;
        }

        private static EnvironmentInfo MapEnvironmentInfo(Environment definition)
        {
            return new EnvironmentInfo
            {
                Id = definition.EnvironmentId,
                Name = definition.Name,
                Description = definition.Description,
                Configuration = definition.Configuration != null 
                    ? MapConfigurationInfo(definition.Configuration)
                    : null,
                RequiredPermission = definition.RequiredPermission,
                Metadata = definition.Metadata ?? new Dictionary<string, string>()
            };
        }

        private static Configuration MapConfiguration(ConfigurationInfo info)
        {
            return new Configuration
            {
                Content = info.Content,
                Format = info.Format,
                Metadata = info.Metadata ?? new Dictionary<string, string>()
            };
        }

        private static ConfigurationInfo MapConfigurationInfo(Configuration definition)
        {
            return new ConfigurationInfo
            {
                Format = definition.Format,
                Content = definition.Content,
                Metadata = definition.Metadata ?? new Dictionary<string, string>()
            };
        }
    }
}
