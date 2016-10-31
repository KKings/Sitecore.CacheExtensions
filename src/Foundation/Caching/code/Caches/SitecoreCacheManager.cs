
namespace KKings.Foundation.Caching.Caches
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using Factories;
    using Sitecore.Abstractions;

    public class SitecoreCacheManager : ICacheManager
    {
        /// <summary>
        /// Implementation for Logging
        /// </summary>
        private readonly BaseLog logger;

        /// <summary>
        /// Implementation for Creating Sitecore Caches
        /// </summary>
        private readonly ISitecoreCacheFactory cacheFactory;

        /// <summary>
        /// Internal Mapping of Instantiated Caches
        /// </summary>
        private IDictionary<string, ICache> Caches { get; }

        public SitecoreCacheManager(BaseLog logger, 
            ISitecoreCacheFactory cacheFactory)
        {
            this.logger = logger;
            this.cacheFactory = cacheFactory;

            this.Caches = new ConcurrentDictionary<string, ICache>();
        }

        /// <summary>
        /// Determines if the cache exists by Name
        /// </summary>
        /// <param name="cacheName">Cache Name</param>
        public bool Exists(string cacheName)
        {
            return !String.IsNullOrEmpty(cacheName) && this.Caches.ContainsKey(cacheName);
        }

        /// <summary>
        /// Adds a new cache by <see cref="CacheConfig"/> Configuration
        /// </summary>
        /// <param name="config">Configuration to create the cache</param>
        public void Add(CacheConfig config)
        {
            if (this.Exists(config.Name))
            {
                throw new Exception($"A cache by the name of, {config.Name}, has already been initialized.");
            }

            var cache = this.cacheFactory.Create(config, this.logger);

            this.Caches[config.Name] = cache;
        }

        /// <summary>
        /// Gets a cache by Name
        /// <exception cref="KeyNotFoundException"></exception>
        /// </summary>
        /// <param name="cacheName">Cache Name</param>
        /// <param name="silent">If <c>false</c> throws exception if cache not found</param>
        public ICache Get(string cacheName, bool silent = true)
        {
            if (this.Exists(cacheName))
            {
                return this.Caches[cacheName];
            }

            this.logger.Error($"The cache, {cacheName}, was not found. Please configure the cache before accessing", this);

            if (!silent)
            {
                throw new KeyNotFoundException($"The cache, {cacheName}, was not found. Please configure the cache before accessing.");
            }

            return null;
        }
    }
}