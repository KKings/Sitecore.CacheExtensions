
namespace KKings.Foundation.Caching.Factories
{
    using Caches;
    using Sitecore.Abstractions;

    public interface ISitecoreCacheFactory
    {
        ICache Create(CacheConfig config, BaseLog logger);
    }
}