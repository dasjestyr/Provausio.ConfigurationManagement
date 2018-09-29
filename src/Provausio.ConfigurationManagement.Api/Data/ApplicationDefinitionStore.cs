namespace Provausio.ConfigurationManagement.Api.Data
{
    using System.Linq;
    using MongoDB.Driver;
    using Schemas;
    using System.Threading.Tasks;
    using Model;


    public class ApplicationDefinitionStore 
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

        public Task UpdateEnvironment(string appId, string environmentId, EnvironmentInfo environment)
        {
            var replacement = MapEnvironment(environment);
            return _collection.FindOneAndUpdateAsync(
                c => c.ApplicationId == appId && c.Environments.Any(e => e.EnvironmentId == appId),
                Builders<ApplicationDefinition>.Update.Set(doc => doc.Environments[-1], replacement));
        }

        public Task DeleteEnvironment(string appId, string environmentId)
        {
            return _collection.FindOneAndUpdateAsync(
                c => c.ApplicationId == appId && c.Environments.Any(e => e.EnvironmentId == appId),
                Builders<ApplicationDefinition>.Update.PullFilter(doc => doc.Environments,
                    el => el.EnvironmentId == environmentId));
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

        private static Environment MapEnvironment(EnvironmentInfo info)
        {
            var environment = new Environment
            {
                Name = info.Name,
                Description = info.Description,
                RequiredPermission = info.RequiredPermission,
                Metadata = info.Metadata
            };
            return environment;
        }
    }
}
