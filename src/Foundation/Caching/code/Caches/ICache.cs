
namespace KKings.Foundation.Caching.Caches
{
    public interface ICache
    {
        /// <summary>
        /// Gets a value based on a Key
        /// </summary>
        /// <typeparam name="T">Type of Value</typeparam>
        /// <param name="key">Key</param>
        /// <returns>Value of K with Type <typeparamref name="T"/></returns>
        T Get<T>(string key) where T : class, new();

        /// <summary>
        /// Sets a value based on Key
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        void Set<T>(string key, T value) where T : class, new();

        /// <summary>
        /// Removes a Cache Entry if it exists by key (if exists)
        /// </summary>
        /// <param name="key">Key</param>
        void Evict(string key);

        /// <summary>
        /// Clears the Cache
        /// </summary>
        void EvictAll();
    }
}