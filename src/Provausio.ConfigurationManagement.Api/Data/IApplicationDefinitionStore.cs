namespace Provausio.ConfigurationManagement.Api.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Model;

    public interface IApplicationDefinitionStore
    {
        Task CreateApplication(string appId, ApplicationInfo application);
        Task<IEnumerable<ApplicationInfo>> GetApplications();
        Task<ApplicationInfo> GetApplication(string id);
        Task UpdateApplication(string appId, ApplicationInfo application);
        Task DeleteApplication(string appId);
        Task CreateEnvironment(string appId, string environmentId, EnvironmentInfo environment);
        Task<IEnumerable<EnvironmentInfo>> GetEnvironments(string appId);
        Task UpdateEnvironment(string appId, string environmentId, EnvironmentInfo environment);
        Task DeleteEnvironment(string appId, string environmentId);
        Task SaveConfiguration(string appId, string environmentId, ConfigurationInfo info);
        Task<ConfigurationInfo> GetConfiguration(string appId, string environmentId);
    }
}