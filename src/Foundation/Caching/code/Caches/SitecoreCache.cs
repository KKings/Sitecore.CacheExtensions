
namespace KKings.Foundation.Caching.Caches
{
    using System;
    using Sitecore.Abstractions;
    using Sitecore.Caching;

    public class SitecoreCache : CustomCache, ICache
    {
        /// <summary>
        /// Cache Configuration
        /// </summary>
        public CacheConfig CacheConfig { get; }

        /// <summary>
        /// Base Log Implementation 
        /// </summary>
        private readonly BaseLog logger;

        public SitecoreCache(CacheConfig cacheConfig, BaseLog logger) : base(cacheConfig.Name, cacheConfig.MaxSize)
        {
            this.CacheConfig = cacheConfig;
            this.logger = logger;
        }

        /// <summary>
        /// Gets a Cache Entry by Cache Key
        /// </summary>
        /// <typeparam name="T">Typeof Cache Entry</typeparam>
        /// <param name="key">Cache Key</param>
        /// <returns>Cache Entry</returns>
        public virtual T Get<T>(string key) where T : class, new()
        {
            if (String.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key), $"{nameof(key)} is null or empty.");
            }

            return this.GetObject(key) as T;
        }

        /// <summary>
        /// Sets a Cache Entry by a Cache Key
        /// </summary>
        /// <typeparam name="T">Tyepof Cache Entry</typeparam>
        /// <param name="key">Cache Key</param>
        /// <param name="value">Cache Entry</param>
        public virtual void Set<T>(string key, T value) where T : class, new()
        {
            if (value == null)
            {
                this.logger.Warn($"Setting {key} with null value {this.CacheConfig.Name}", this);
                return;
            }

            if (this.InnerCache.MaxSize == 0)
            {
                return;
            }

            if (this.CacheConfig.Lifespan <= 0)
            {
                this.Insert(key, value);
            }

            switch (this.CacheConfig.ExpirationType)
            {
                case ExpirationType.Absolute:
                    this.Insert(key, value, DateTime.UtcNow.AddSeconds(this.CacheConfig.Lifespan));
                    break;
                case ExpirationType.Sliding:
                    this.Insert(key, value, TimeSpan.FromSeconds(this.CacheConfig.Lifespan));
                    break;
                case ExpirationType.Sticky:
                    break;
                default:
                    throw new Exception($"Expiration Type, {this.CacheConfig.ExpirationType}, has not been configured");
            }
        }

        /// <summary>
        /// Evicts a single entry
        /// </summary>
        /// <param name="key">Cache Key</param>
        public virtual void Evict(string key)
        {
            if (!this.InnerCache.ContainsKey(key) || this.InnerCache.MaxSize == 0)
            {
                return;
            }

            this.InnerCache.Remove(key);
        }

        /// <summary>
        /// Evict all Entries
        /// </summary>
        public virtual void EvictAll()
        {
            this.InnerCache.Clear();
        }

        /// <summary>
        /// Insert a Cache Entry using the Sliding Expiration
        /// </summary>
        /// <param name="key">Cache Key</param>
        /// <param name="value">Cache Entry</param>
        /// <param name="slidingExpiration">Sliding Expiration</param>
        public virtual void Insert(string key, object value, TimeSpan slidingExpiration)
        {
            this.InnerCache.Add(key, value, slidingExpiration);
        }

        /// <summary>
        /// Insert a Cache Entry using the Absolute Expiration 
        /// </summary>
        /// <param name="key">Cache Key</param>
        /// <param name="value">Cache Entry</param>
        /// <param name="absoluteExpiration">Absolute Expiration</param>
        public virtual void Insert(string key, object value, DateTime absoluteExpiration)
        {
            this.InnerCache.Add(key, value, absoluteExpiration);
        }
        
        /// <summary>
        /// Insert a Cache Entry without an Expiration
        /// </summary>
        /// <param name="key">Cache Key</param>
        /// <param name="value">Cache Entry</param>
        public virtual void Insert(string key, object value)
        {
            this.InnerCache.Add(key, value);
        }
    }
}
