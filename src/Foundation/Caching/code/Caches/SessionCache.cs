
namespace KKings.Foundation.Caching.Caches
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Web;

    public class SessionCache : ICache
    {        
        /// <summary>
        /// Base Implementation of the HttpContext
        /// </summary>
        private readonly HttpContextBase httpContextBase;

        /// <summary>
        /// Items Key within the HttpContext
        /// </summary>
        public string ItemsKey { get; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Local Dictionary Cache
        /// <para>Takes into account if the items are cleared</para>
        /// </summary>
        internal ConcurrentDictionary<string, object> _localCache
        {
            get
            {
                var httpSessionStateBase = this.httpContextBase.Session;

                if (httpSessionStateBase == null)
                {
                    return new ConcurrentDictionary<string, object>();
                }

                return httpSessionStateBase[this.ItemsKey] as ConcurrentDictionary<string, object> ?? new ConcurrentDictionary<string, object>();
            }
        }

        public SessionCache(HttpContextBase httpContextBase) : this(httpContextBase, new List<KeyValuePair<string, object>>()) { }

        public SessionCache(HttpContextBase httpContextBase, IList<KeyValuePair<string, object>> collection)
        {

            if (httpContextBase?.Session == null)
            {
                throw new ArgumentNullException(nameof(httpContextBase));
            }

            this.httpContextBase = httpContextBase;

            var httpSessionStateBase = this.httpContextBase.Session;

            if (httpSessionStateBase != null)
            {
                httpSessionStateBase[this.ItemsKey] = new ConcurrentDictionary<string, object>(collection);
            }
        }


        /// <summary>
        /// Gets a cache entry by a cache key
        /// </summary>
        /// <param name="key">Cache Key</param>
        /// <returns>Cache entry or <c>default</c></returns>
        public virtual T Get<T>(String key) where T : class, new()
        {
            if (String.IsNullOrEmpty(key))
            {
                throw new ArgumentException($"{nameof(key)}, is null or empty.");
            }

            return this._localCache.ContainsKey(key) ? this._localCache[key] as T : default(T);
        }

        /// <summary>
        /// Sets a cache entry by a cache key
        /// </summary>
        /// <typeparam name="T">Typeof of Cache Entry</typeparam>
        /// <param name="key">Cache Key</param>
        /// <param name="value">Cache Entry</param>
        public virtual void Set<T>(String key, T value) where T : class, new()
        {
            if (String.IsNullOrEmpty(key))
            {
                throw new ArgumentException($"{nameof(key)}, is null or empty.");
            }

            this._localCache.AddOrUpdate(key, value, (k, old) => value);

            var httpSessionStateBase = this.httpContextBase.Session;

            if (httpSessionStateBase != null)
            {
                httpSessionStateBase[this.ItemsKey] = this._localCache;
            }
        }

        /// <summary>
        /// Removes a Cache Entry if it exists by key (if exists)
        /// </summary>
        /// <param name="key">Key</param>
        public virtual void Evict(String key)
        {
            if (!this.Contains(key))
            {
                return;
            }

            object obj;
            this._localCache.TryRemove(key, out obj);

            var httpSessionStateBase = this.httpContextBase.Session;

            if (httpSessionStateBase != null)
            {
                httpSessionStateBase[this.ItemsKey] = this._localCache;
            }
        }


        /// <summary>
        /// Removes all Cache Entries
        /// </summary>
        public virtual void EvictAll()
        {
            if (!this._localCache.IsEmpty)
            {
                this._localCache.Clear();
            }
        }

        /// <summary>
        /// Determines whether the Cache contains the key
        /// </summary>
        /// <param name="key">Cache Key</param>
        /// <returns><c>True</c> if the cache contains the key</returns>
        public virtual bool Contains(string key)
        {
            return this._localCache.ContainsKey(key);
        }
    }
}