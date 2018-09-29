namespace Provausio.ConfigurationManagement.Api.DependencyInjection
{
    using Data;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using MongoDB.Driver;

    public static class MongoDbInstaller
    {
        public static void AddMongoDb(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(p => 
            {
                var connectionString = configuration["MongoDb:ConnectionString"];
                var client = new MongoClient(connectionString);
            
                return client.GetDatabase("configurationManagement");
            });

            services.AddTransient<ApplicationDefinitionStore>();
        }
    }
}