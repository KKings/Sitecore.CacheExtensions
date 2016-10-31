
namespace KKings.Foundation.Caching
{
    using System;

    public sealed class CacheConfig
    {
        /// <summary>
        /// Cache Name
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Cache Max Size
        /// </summary>
        public long MaxSize { get; private set; }

        /// <summary>
        /// Cache Lifespan
        /// <para>Lifespan is in seconds, I.E. 60 for 1 minute</para>
        /// </summary>
        public int Lifespan { get; private set; }

        /// <summary>
        /// Cache Expiration Type
        /// </summary>
        public ExpirationType ExpirationType { get; private set; }

        public CacheConfig(string name, 
            long maxSize, 
            int lifespan,
            ExpirationType expirationType = ExpirationType.Absolute)
        {
            if (String.IsNullOrEmpty(name))
            {
                throw new ArgumentException($"{nameof(name)} cannot be null or empty.", nameof(name));
            }

            this.Name = name;
            this.MaxSize = maxSize;
            this.Lifespan = lifespan;
            this.ExpirationType = expirationType;
        }
    }
}