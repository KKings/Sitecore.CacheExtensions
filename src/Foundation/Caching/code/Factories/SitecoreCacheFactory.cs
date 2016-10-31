
namespace KKings.Foundation.Caching.Factories
{
    using Caches;
    using Sitecore.Abstractions;

    public class SitecoreCacheFactory : ISitecoreCacheFactory
    {
        /// <summary>
        /// Creates the Sitecore Cache
        /// </summary>
        /// <param name="config">Cache Configuration</param>
        /// <param name="logger">Logging Implementation</param>
        /// <returns><c>SitecoreCache</c></returns>
        public ICache Create(CacheConfig config, BaseLog logger)
        {
            return new SitecoreCache(config, logger);
        }
    }
}