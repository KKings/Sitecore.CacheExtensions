
namespace KKings.Foundation.Caching.Caches
{
    using System;

    public abstract class BaseSessionCache : ICache
    {
        public virtual T Get<T>(string key) where T : class, new()
        {
            throw new NotImplementedException();
        }

        public virtual void Set<T>(string key, T value) where T : class, new()
        {
            throw new NotImplementedException();
        }

        public virtual void Evict(string key)
        {
            throw new NotImplementedException();
        }

        public virtual void EvictAll()
        {
            throw new NotImplementedException();
        }
    }
}