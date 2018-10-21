using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Provausio.ConfigurationManagement.Api.Auth;

namespace Provausio.ConfigurationManagement.Api
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using DependencyInjection;

    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IHostingEnvironment Environment { get; }

        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }               

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMongoDb(Configuration, Environment);
            services.AddLogging(config => config.AddDebug());
            services.AddAuth(Configuration);
            services.AddCors();
            services.AddMvc(options => options.AllowCombiningAuthorizeFilters = false).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }
        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (!env.IsProduction())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            //app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
