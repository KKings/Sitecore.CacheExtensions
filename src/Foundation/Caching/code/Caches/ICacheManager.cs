
namespace KKings.Foundation.Caching.Caches
{
    using System.Collections.Generic;

    public interface ICacheManager
    {
        /// <summary>
        /// Determines if the cache exists by Name
        /// </summary>
        /// <param name="cacheName">Cache Name</param>
        bool Exists(string cacheName);

        /// <summary>
        /// Adds a new cache by <see cref="CacheConfig"/>
        /// </summary>
        /// <param name="config">Cache Configuration</param>
        void Add(CacheConfig config);

        /// <summary>
        /// Gets a cache by Name
        /// <exception cref="KeyNotFoundException"></exception>
        /// </summary>
        /// <param name="cacheName">Cache Name</param>
        /// <param name="silent">Throw exception if not found</param>
        ICache Get(string cacheName, bool silent);
    }
}