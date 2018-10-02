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
                //var connectionString = configuration["MongoDb:ConnectionString"];
                var connectionString = Environment.GetEnvironmentVariable("MONGODB_CONN");
                Console.Out.WriteLine($"Using connection string {connectionString}");
                var client = new MongoClient(connectionString);

                var database = environment.IsDevelopment() ? "provausio_test_1" : "configurationManagement";       
                
                return client.GetDatabase(database);
            });

            services.AddTransient<IApplicationDefinitionStore, ApplicationDefinitionStore>();
        }
    }
}