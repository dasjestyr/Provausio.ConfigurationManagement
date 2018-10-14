using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver.Core.Events;

namespace Provausio.ConfigurationManagement.Api.DependencyInjection
{
    using System;
    using Data;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using MongoDB.Driver;

    public static class MongoDbInstaller
    {
        public static void AddMongoDb(this IServiceCollection services, IConfiguration configuration, IHostingEnvironment environment)
        {
            services.AddSingleton(p =>
            {
                var connectionString = Environment.GetEnvironmentVariable("MONGODB_CONN");
                
                var logger = p.GetRequiredService<ILogger<Startup>>();
                var mongoConnectionUrl = new MongoUrl(connectionString);
                var mongoClientSettings = MongoClientSettings.FromUrl(mongoConnectionUrl);
                mongoClientSettings.ClusterConfigurator = cb => {
                    cb.Subscribe<CommandStartedEvent>(e => {
                        logger.LogInformation($"{e.CommandName} - {e.Command.ToJson()}");
                    });
                };
                
                Console.Out.WriteLine($"Using connection string {connectionString}");
                var client = new MongoClient(mongoClientSettings);

                var database = environment.IsDevelopment() ? "provausio_test_1" : "configurationManagement";       
                
                return client.GetDatabase(database);
            });

            services.AddTransient<IApplicationDefinitionStore, ApplicationDefinitionStore>();
        }
    }
}