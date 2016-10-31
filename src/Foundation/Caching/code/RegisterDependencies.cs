
namespace KKings.Foundation.Caching
{
    using Caches;
    using Configuration;
    using Factories;
    using Microsoft.Extensions.DependencyInjection;
    using Sitecore.DependencyInjection;

    public class RegisterDependencies : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IConfigurationReader, ConfigurationReader>();
            serviceCollection.AddSingleton<ICacheManager, SitecoreCacheManager>();
            serviceCollection.AddSingleton<ISitecoreCacheFactory, SitecoreCacheFactory>();
            serviceCollection.AddScoped<BaseTransientCache, TransientCache>();
        }
    }
}