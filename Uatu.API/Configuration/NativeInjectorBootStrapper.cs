using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Uatu.Core.Interfaces;
using Uatu.DbConnectors;

namespace Uatu.API.Configuration
{
    public class NativeInjectorBootStrapper
    {
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddSingleton(ResolveConfigurationImplementation<DbConnectorSettings>);
            services.AddScoped<IDbConnector>(provider =>
            {
                var config = provider.GetService<IConfiguration>();
                switch (config["DbConnectorSettings:Provider"])
                {
                    case "MongoDb":
                        return new MongoDbConnector(config["DbConnectorSettings:ConnectionString"]);
                    default:
                        throw new Exception("DbConnectorSettings.Provider not recognized!");
                }
            });
        }

        private static T ResolveConfigurationImplementation<T>(IServiceProvider provider)
        {
            var configuration = provider.GetService<IConfiguration>();
            var sectionName = typeof(T).Name;

            return configuration.GetSection(sectionName).Get<T>();
        }
    }
}
